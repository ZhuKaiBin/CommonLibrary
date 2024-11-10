using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExtensionsLibrary
{
    public static class EnumExtension
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="en"></param>
        /// <param name="value"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static bool Equals(this Enum en, string value, StringComparison stringComparison)
        {
            return en.ToString().Equals(value, stringComparison);
        }

        /// <summary>
        /// 返回枚举类型DescriptionAttribute的Description
        /// 如果不存在会抛空指针异常
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>
        /// DescriptionAttribute的Description
        /// </returns>
        public static string GetCustomAttributeDescription(this System.Enum value) =>
             (value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault<object>() as DescriptionAttribute).Description;

        /// <summary>
        /// 获取XML枚举Name值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetXmlEnumAttributeValueFromEnum<TEnum>(this TEnum value) where TEnum : struct, IConvertible
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
                return null;//or string.Empty, or throw exception

            var member = enumType.GetMember(value.ToString()).FirstOrDefault();
            if (member == null)
                return null;//or string.Empty, or throw exception

            var attribute = member.GetCustomAttributes(false).OfType<XmlEnumAttribute>().FirstOrDefault();
            if (attribute == null)
                return null;//or string.Empty, or throw exception
            return attribute.Name;
        }
    }
}