using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public static class TypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// 判断当前Type是否继承自 <typeparamref name="TTarget"></typeparamref>.
        /// 内部使用 <see cref="Type.IsAssignableFrom"/>.
        /// </summary>
        /// <typeparam name="TTarget">Target type</typeparam>
        public static bool IsAssignableTo<TTarget>(this Type type)
        {
            if (type == null)
            {
                return false;
            }
            return type.IsAssignableTo(typeof(TTarget));
        }

        /// <summary>
        /// 判断当前Type是否继承自 <paramref name="targetType"></paramref>.
        /// 内部使用 <see cref="Type.IsAssignableFrom"/>
        /// </summary>
        /// <param name="type">this type</param>
        /// <param name="targetType">Target type</param>
        public static bool IsAssignableTo(this Type type, Type targetType)
        {
            if (type == null || targetType == null)
            {
                return false;
            }
            return targetType.IsAssignableFrom(type);
        }
    }
}