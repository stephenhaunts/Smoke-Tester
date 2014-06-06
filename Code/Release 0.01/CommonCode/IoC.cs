/**
* Smoke Tester Tool : Post deployment smoke testing tool.
* 
* http://www.stephenhaunts.com
* 
* This file is part of Smoke Tester Tool.
* 
* Smoke Tester Tool is free software: you can redistribute it and/or modify it under the terms of the
* GNU General Public License as published by the Free Software Foundation, either version 2 of the
* License, or (at your option) any later version.
* 
* Smoke Tester Tool is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* 
* See the GNU General Public License for more details <http://www.gnu.org/licenses/>.
* 
* Curator: Stephen Haunts
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Collections;
using Common.Reflection;

namespace InversionOfControl
{    
    public static class IoC
    {
        private static readonly AutoDictionary<Type, Type> resolutionCache = new AutoDictionary<Type, Type>();
        private static IEnumerable<Type> knownTypes;
      
        public static Type ResolveType(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new InvalidOperationException("You must pass an Interface to Resolve");
            }

            if (!resolutionCache.ContainsKey(interfaceType))
            {
                knownTypes = knownTypes ?? Assembly.GetCallingAssembly().GetAllTypes();

                List<Type> implementations = new List<Type>(knownTypes.Where(t => !t.IsInterface && !t.IsAbstract && t.Implements(interfaceType)));

                if (implementations.Count > 1)
                {
                    throw new InvalidOperationException(string.Format(
                        "Multiple implementations of {0} were found: {1}", interfaceType.Name,
                        string.Join(", ",
                            implementations.Select(
                                t => string.Format("{0} in {1}", t.Name, Path.GetFileName(t.Assembly.Location))))));
                }

                if (implementations.Count < 1)
                {
                    throw new InvalidOperationException(
                        string.Format("No implementations of {0} were found in the assemblies nearby.",
                            interfaceType.Name));
                }

                resolutionCache.Add(interfaceType, implementations[0]);
            }

            return resolutionCache[interfaceType];
        }
     
        public static Type ResolveType<TInterface>() where TInterface : class
        {
            Type interfaceType = typeof(TInterface);

            return ResolveType(interfaceType);
        }
      
        public static void AddResolutionShortcut<TInterface, TImplementation>() where TImplementation : TInterface
        {
            AddResolutionShortcut(typeof(TInterface), typeof(TImplementation));
        }
       
        public static void AddResolutionShortcut(Type interfaceType, Type implementationType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException(string.Format("Supplied type {0} is not an interface.", interfaceType.Name));
            }

            if (implementationType.IsInterface || implementationType.IsAbstract)
            {
                throw new ArgumentException(string.Format("Supplied type {0} is not a concrete type.",
                    implementationType.Name));
            }

            resolutionCache[interfaceType] = implementationType;
        }
  
        public static TInterface Resolve<TInterface>(params object[] args) where TInterface : class
        {
            Type resolvedType = ResolveType<TInterface>();
            ValidateConstructor(typeof(TInterface), resolvedType, args);

            return (TInterface)Activator.CreateInstance(resolvedType, args);
        }

        private static readonly Func<Type, Type, bool>[] parameterValidators
            =
        {
            (tArg,tParam) => tArg == tParam,
            (tArg,tParam) => tParam.IsInterface && tArg.Implements(tParam),
            (tArg,tParam) => tArg != tParam && tArg.IsSubclassOf(tParam),
            (tArg,tParam) => tArg != tParam && tParam.IsAssignableFrom(tArg)
        };

        private static void ValidateConstructor(Type interfaceType, Type resolvedType, IList<object> args)
        {
            ConstructorInfo[] allConstructors = resolvedType.GetConstructors();
            ConstructorInfo[] semiFinalists = allConstructors.Where(c => c.GetParameters().Length == args.Count).ToArray();

            if (semiFinalists == null || semiFinalists.Length == 0)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "{2} resolved to {1} which was passed {0} arguments, but no suitable constructors were found",
                        args.Count,
                        resolvedType.Name, interfaceType.Name));
            }

            int finalists = 0;

            foreach (ConstructorInfo constructor in semiFinalists)
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                int i = 0;
                int parameterMatches = 0;

                foreach (ParameterInfo parameter in parameters)
                {
                    int validationsPassed = parameterValidators.Count(v => v(args[i].GetType(), parameter.ParameterType));

                    if (validationsPassed >= 1)
                    {
                        parameterMatches++;
                    }

                    i++;
                }

                if (parameterMatches == parameters.Length && finalists == 1)
                {
                    throw new InvalidOperationException(
                        string.Format("{1} resolved to {0} but multiple constructors matched the requested signature",
                            resolvedType.Name, interfaceType.Name));
                }

                finalists++;
            }

            if (finalists == 0)
            {
                throw new InvalidOperationException(
                    string.Format("{1} resolved to {0} but no constructors match the requested signature",
                        resolvedType.Name, interfaceType.Name));
            }
        }
    }
}
