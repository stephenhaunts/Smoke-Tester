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
using System.Reflection;
using System.Threading.Tasks;

namespace Common.Reflection
{
    public class PropertyAccessor
    {
        private PropertyAccessor() { }

        static readonly SortedList<PropertyInfo, PropertyAccessor> accessorCache = new SortedList<PropertyInfo, PropertyAccessor>(new GenericComparer<PropertyInfo>());

        public static PropertyAccessor Create(PropertyInfo propertyInfo)
        {
            PropertyAccessor accessor;
            lock (accessorCache)
            {
                if (!accessorCache.TryGetValue(propertyInfo, out accessor))
                {
                    accessorCache.Add(propertyInfo, accessor = new PropertyAccessor(propertyInfo));
                }
            }

            return accessor;
        }

        private enum CallTypes
        {
            ViaDelegate, ReflectedMethod, ClassMethod
        }

        private readonly PropertyInfo propertyInfo;
        private Func<object, object> fastGetter;
        private Action<object, object> fastSetter;
        private MethodInfo getterMethod;
        private MethodInfo setterMethod;
        private CallTypes preferredGetterCall;
        private CallTypes preferredSetterCall;

        private readonly object getterLock = new object();
        private readonly object setterLock = new object();

        public object Get(object target)
        {
            lock (getterLock)
            {
            tryAgain:
                switch (preferredGetterCall)
                {
                    case CallTypes.ViaDelegate:
                        try
                        {
                            return fastGetter(target);
                        }
                        catch (Exception)
                        {
                            preferredGetterCall = CallTypes.ReflectedMethod;
                            goto tryAgain;
                        }
                    case CallTypes.ReflectedMethod:
                        try
                        {
                            return getterMethod.Invoke(target, null);
                        }
                        catch (Exception)
                        {
                            preferredGetterCall = CallTypes.ClassMethod;
                            goto tryAgain;
                        }
                    case CallTypes.ClassMethod:
                        return propertyInfo.GetValue(target, null);
                }
            }

            return null;
        }


        public void Set(object target, object value)
        {
            lock (setterLock)
            {
            tryAgain:
                switch (preferredSetterCall)
                {
                    case CallTypes.ViaDelegate:
                        try
                        {
                            fastSetter(target, value);
                        }
                        catch (Exception)
                        {
                            preferredSetterCall = CallTypes.ReflectedMethod;
                            goto tryAgain;
                        }
                        break;
                    case CallTypes.ReflectedMethod:
                        try
                        {
                            setterMethod.Invoke(target, new[] { value });
                        }
                        catch (Exception)
                        {
                            preferredSetterCall = CallTypes.ClassMethod;
                            goto tryAgain;
                        }
                        break;
                    case CallTypes.ClassMethod:
                        propertyInfo.SetValue(target, value, null);
                        break;
                }
            }
        }

        private void InitialiseSetter()
        {
            setterMethod = propertyInfo.GetSetMethod() ?? propertyInfo.GetSetMethod(true);
            try
            {
                fastSetter = FastMethodReflection.VoidCaller1Param(setterMethod);
                preferredSetterCall = fastSetter != null
                                              ? CallTypes.ViaDelegate
                                              : CallTypes.ReflectedMethod;
            }
            catch (Exception)
            {
                preferredSetterCall = CallTypes.ReflectedMethod;
            }
        }

        private void InitialiseGetter()
        {
            getterMethod = propertyInfo.GetGetMethod() ?? propertyInfo.GetGetMethod(true);
            try
            {
                fastGetter = FastMethodReflection.FuncCaller0Params(getterMethod);
                preferredGetterCall = fastGetter != null
                                              ? CallTypes.ViaDelegate
                                              : CallTypes.ReflectedMethod;
            }
            catch (Exception)
            {
                preferredGetterCall = CallTypes.ReflectedMethod;
            }
        }

        private PropertyAccessor(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;

            Task[] initTasks =
            {
                Task.Factory.StartNew(InitialiseSetter),
                Task.Factory.StartNew(InitialiseGetter)
            };

            Task.WaitAll(initTasks);
        }
    }
}
