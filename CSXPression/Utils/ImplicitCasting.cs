using System;
using System.Collections.Generic;
using System.Linq;

namespace CSXPression.Utils
{
    /// <summary>
    /// This class test if a type can be implicitly cast into an other type
    /// </summary>
    public static class ImplicitCasting
    {
        // Based on https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2012/y5b434w4(v=vs.110)?redirectedfrom=MSDN
        private static readonly IDictionary<Type, Type[]> implicitCastDict = new Dictionary<Type, Type[]>
        {
            { typeof(sbyte), new Type[] { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(byte), new Type[] { typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(short), new Type[] { typeof(int), typeof(long),  typeof(float), typeof(double), typeof(decimal) } },
            { typeof(ushort), new Type[] { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(int), new Type[] { typeof(long), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(uint), new Type[] {typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(long), new Type[] { typeof(float), typeof(double), typeof(decimal) } },
            { typeof(char), new Type[] { typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) } },
            { typeof(float), new Type[] { typeof(double) } },
            { typeof(ulong), new Type[] { typeof(float), typeof(double), typeof(decimal) } },
        };

        public static bool IsImplicitlyCastableTo(this Type fromType, Type toType)
        {
            return toType.IsAssignableFrom(fromType)
                || (implicitCastDict.ContainsKey(fromType) && implicitCastDict[fromType].Contains(toType));
        }
    }
}
