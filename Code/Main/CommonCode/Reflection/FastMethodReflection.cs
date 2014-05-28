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
using System.Reflection;

namespace Common.Reflection
{
    internal static class FastMethodReflection
    {
        static readonly MethodInfo voidConverter1ParamMethod = typeof(FastMethodReflection).GetMethod("VoidConverter1Param", BindingFlags.Static | BindingFlags.NonPublic);
        static readonly MethodInfo funcConverter0ParamsMethod = typeof(FastMethodReflection).GetMethod("FuncConverter0Params", BindingFlags.Static | BindingFlags.NonPublic);

        public static Action<object, object> VoidCaller1Param(MethodInfo method)
        {
            return (Action<object, object>)voidConverter1ParamMethod.MakeGenericMethod(method.DeclaringType, method.GetParameters()[0].ParameterType).Invoke(null, new object[] { method });
        }     

        public static Func<object, object> FuncCaller0Params(MethodInfo method)
        {
            return (Func<object, object>)funcConverter0ParamsMethod.MakeGenericMethod(method.DeclaringType, method.ReturnType).Invoke(null, new object[] { method });
        }
    }
}
