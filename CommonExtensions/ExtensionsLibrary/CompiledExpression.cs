using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public static class CompiledExpression
    {
        /// <summary>
        /// Create setter by property or field
        /// </summary>
        /// <typeparam name="TEntity">属性的类</typeparam>
        /// <typeparam name="TMember">属性本身的类型</typeparam>
        /// <param name="info"> property or field</param>
        /// <returns></returns>
        public static Action<TEntity, object> CreateSetter<TEntity>(this MemberInfo info)
        {
            Type memberType;
            if (info.MemberType == MemberTypes.Property)
            {
                var prop = (PropertyInfo)info;
                //如果没有Setter则返回空
                if (!prop.CanWrite)
                {
                    return null;
                }
                memberType = prop.PropertyType;
            }
            else if (info.MemberType == MemberTypes.Field)
            {
                memberType = ((FieldInfo)info).FieldType;
            }
            else
            {
                return null;
            }

            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");
            ParameterExpression propertyValue = Expression.Parameter(typeof(object), "propertyValue");
            var body = Expression.Assign(Expression.PropertyOrField(instance, info.Name), Expression.Convert(propertyValue, memberType));

            return Expression.Lambda<Action<TEntity, object>>(body, instance, propertyValue).Compile();
        }

        public static Func<TEntity, object> CreateGettter<TEntity>(this MemberInfo info)
        {
            if (info.MemberType != MemberTypes.Property
                && info.MemberType != MemberTypes.Field)
            {
                return null;
            }

            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");
            var body = Expression.Convert(Expression.PropertyOrField(instance, info.Name), typeof(object));
            return Expression.Lambda<Func<TEntity, object>>(body, instance).Compile();
        }
    }
}