using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public static class CommandTextInfoExtensions
    {
        /// <summary>
        /// 添加where条件
        /// </summary>
        /// <param name="cti">CommandTextInfo</param>
        /// <param name="whereFormat">where语句 eg: id={0} and name={1}</param>
        /// <param name="ps">参数</param>
        public static CommandTextInfo AppendWhere(this CommandTextInfo cti, string whereFormat, params object[] ps)
        {
            var num = cti.BeginIndex;
            var paramsPlaceholder = new List<string>();
            for (int i = 0; i < ps.Length; i++)
            {
                paramsPlaceholder.Add("@" + (num + i));
            }
            cti.CommandText += string.Format(whereFormat, paramsPlaceholder.ToArray());
            cti.Parameters = cti.Parameters.Concat(ps).ToArray();
            return cti;
        }

        /// <summary>
        /// 添加in条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cti"></param>
        /// <param name="whereFormat"> id in ({0}) </param>
        /// <param name="ps">参数</param>
        /// <returns></returns>
        public static CommandTextInfo AppendWhereIn<T>(this CommandTextInfo cti, string whereFormat, IEnumerable<T> ps)
        {
            var paramList = cti.Parameters.ToList();
            cti.CommandText += SqlCommandTextInfoBuilder.BuildBufferInCommandText(whereFormat, ps, paramList);
            cti.Parameters = paramList.ToArray();
            return cti;
        }

        public static bool IsNullOrWhiteSpace(this CommandTextInfo textInfo)
        {
            return textInfo == null || string.IsNullOrWhiteSpace(textInfo.CommandText);
        }
    }
}