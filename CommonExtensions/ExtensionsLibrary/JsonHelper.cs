using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ExtensionsLibrary
{
    public static class JsonHelper
    {
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static string ToJson(this object obj, bool ignoreError)
        {
            try
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    Error = (ob, args2) =>
                    {
                        args2.ErrorContext.Handled = true;
                    }
                };
                if (ignoreError)
                {
                    return JsonConvert.SerializeObject(obj, settings);
                }
                else
                {
                    return JsonConvert.SerializeObject(obj);
                }
            }
            catch (Exception ex)
            {
                if (!ignoreError)
                {
                    throw ex;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static T ToObject<T>(this string Json)
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }

        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }

        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }

        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }

        public static JArray ToJArray(this string Json)
        {
            return Json == null ? JArray.Parse("[]") : JArray.Parse(Json.Replace("&nbsp;", ""));
        }

        /// <summary>
        /// 获取JSON指定节点数据
        /// </summary>
        /// <param name="json">JSON数据</param>
        /// <param name="node">节点</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static JToken GetNodeDataFromJson(string json, string node, string path = "")
        {
            return GetNodeDataFromJson<JToken>(json, node, path);
        }

        /// <summary>
        /// 获取JSON指定节点数据
        /// </summary>
        /// <param name="json">JSON数据</param>
        /// <param name="node">节点</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static T GetNodeDataFromJson<T>(string json, string node, string path = "")
        {
            return GetNodeDataFromJson<T>(json, node, default(T), path);
        }

        /// <summary>
        /// 获取JSON指定节点数据
        /// </summary>
        /// <param name="json">JSON数据</param>
        /// <param name="node">节点</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static T GetNodeDataFromJson<T>(string json, string node, T @default, string path = "")
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return @default;
            }

            if (string.IsNullOrWhiteSpace(node))
            {
                return @default;
            }

            var jToken = JsonConvert.DeserializeObject<JToken>(json);
            if (jToken == null || !jToken.HasValues)
            {
                return @default;
            }

            if (!string.IsNullOrWhiteSpace(path))
            {
                jToken = jToken.SelectToken(path, false);
            }

            if (jToken == null || !jToken.HasValues)
            {
                return @default;
            }

            return jToken.Value<T>(node);
        }

        /// <summary>
        /// 获取JSON指定节点数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">JSON数据</param>
        /// <param name="node">节点</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static T TryGetNodeDataFromJson<T>(string json, string node, string path = "")
        {
            try
            {
                return GetNodeDataFromJson<T>(json, node, path);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取JSON指定节点数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">JSON数据</param>
        /// <param name="node">节点</param>
        /// <param name="@default">默认值</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static T TryGetNodeDataFromJson<T>(string json, string node, T @default, string path = "")
        {
            try
            {
                return GetNodeDataFromJson<T>(json, node, @default, path);
            }
            catch
            {
                return @default;
            }
        }

        /// <summary>
        /// 读取文件，返回相应对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>返回文件内容</returns>
        public static T ReadJsonbyPath<T>(string path)
        {
            using (var stream = new StreamReader(path, System.Text.Encoding.Default))
            {
                var json = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        #region Json压缩算法

        public static List<List<List<object>>> _cache;

        public static Dictionary<string, object> unpackCreateRow(List<string> keys, List<object> values)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            for (int i = 0, len = keys.Count; i < len; i++)
                result[keys[i]] = values[i];
            return result;
        }

        public static int best(List<Dictionary<string, object>> collection)
        {
            var json = new System.Web.Script.Serialization.JavaScriptSerializer();
            int j = 0;
            _cache = new List<List<List<object>>>();
            for (int i = 0, len = 0, length = 0; i < 4; i++)
            {
                _cache.Add(pack(collection, i));
                len = json.Serialize(_cache[i]).Length;
                if (length == 0)
                    length = len;
                else if (len < length)
                {
                    length = len;
                    j = i;
                }
            }
            return j;
        }

        public static List<List<object>> pack(List<Dictionary<string, object>> collection)
        {
            return pack(collection, 0);
        }

        public static List<List<object>> pack(List<Dictionary<string, object>> collection, int compression)
        {
            List<List<object>> r = new List<List<object>>();
            if (3 < compression)
            {
                int i = best(collection);
                r = _cache[i];
                _cache.Clear();
            }
            else
            {
                List<List<object>> result = new List<List<object>>();
                List<object> header = new List<object>();
                Dictionary<string, object> first = collection[0];
                int length = collection.Count,
                    len = first.Keys.Count,
                    index;
                r.Add(header);
                foreach (string key in first.Keys)
                    header.Add(key);
                for (int i = 0; i < length; ++i)
                {
                    Dictionary<string, object> item = collection[i];
                    List<object> row = new List<object>();
                    for (int j = 0; j < len; ++j)
                        row.Add(item[(string)header[j]]);
                    r.Add(row);
                }
                index = r.Count;
                if (0 < compression)
                {
                    List<object> row = r[1];
                    for (int j = 0; j < len; ++j)
                    {
                        if (!(row[j] is int) && !(row[j] is float) && !(row[j] is double))
                        {
                            List<object> cache = new List<object>(),
                                         current = new List<object>()
                            ;
                            current.Add(header[j]);
                            current.Add(cache);
                            header.RemoveAt(j);
                            header.Insert(j, current);
                            for (int i = 1, k = 0; i < index; ++i)
                            {
                                object value = r[i][j];
                                int l = cache.IndexOf(value);
                                if (l < 0)
                                {
                                    cache.Add(value);
                                    r[i][j] = k++;
                                }
                                else
                                    r[i][j] = l;
                            }
                        }
                    }
                }
                if (2 < compression)
                {
                    for (int j = 0; j < len; ++j)
                    {
                        if (header[j] is List<object>)
                        {
                            var json = new System.Web.Script.Serialization.JavaScriptSerializer();
                            List<object> values = new List<object>();
                            List<object> indexes = new List<object>();
                            List<object> cache = (List<object>)header[j];
                            string key = (string)cache[0];
                            cache = (List<object>)cache[1];
                            for (int i = 1; i < index; ++i)
                            {
                                object value = r[i][j];
                                indexes.Add(value);
                                values.Add(cache[(int)value]);
                            }
                            indexes.AddRange(cache);
                            if (json.Serialize(values).Length < json.Serialize(indexes).Length)
                            {
                                for (int k = 0, i = 1; i < index; ++i)
                                {
                                    r[i][j] = values[k];
                                    ++k;
                                }
                                header[j] = key;
                            }
                        }
                    }
                }
                else if (1 < compression)
                {
                    length -= (int)Math.Floor((double)(length / 2));
                    for (int j = 0; j < len; ++j)
                    {
                        if (header[j] is List<object>)
                        {
                            List<object> cache = (List<object>)header[j];
                            string key = (string)cache[0];
                            cache = (List<object>)cache[1];
                            if (length < cache.Count)
                            {
                                for (int i = 1; i < index; ++i)
                                {
                                    object value = r[i][j];
                                    r[i][j] = cache[(int)value];
                                }
                                header[j] = key;
                            }
                        }
                    }
                }
                if (0 < compression)
                {
                    for (int j = 0; j < len; ++j)
                    {
                        if (header[j] is List<object>)
                        {
                            List<object> cache = (List<object>)header[j];
                            string key = (string)cache[0];
                            header[j] = key;
                            header.Insert(j + 1, cache[1]);
                            ++len;
                            ++j;
                        }
                    }
                }
            }
            return r;
        }

        public static List<Dictionary<string, object>> unpack(List<List<object>> collection)
        {
            int length = collection.Count;
            List<object> header = collection[0];
            List<string> keys = new List<string>();
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            for (int i = 0, k = 0, l = 0, len = header.Count; i < len; ++i)
            {
                keys.Add((string)header[i]);
                k = i + 1;
                if (k < len && header[k] is object[])
                {
                    ++i;
                    for (int j = 1; j < length; ++j)
                    {
                        List<object> row = collection[j];
                        object[] head = (object[])header[k];
                        row[l] = head[(int)row[l]];
                    }
                }
                ++l;
            }
            for (int j = 1; j < length; ++j)
                result.Add(unpackCreateRow(keys, collection[j]));
            return result;
        }

        #endregion Json压缩算法

        /// <summary>
        /// 删除Json数组元素，根据Json节点的名称和值
        /// </summary>
        /// <param name="jsonArrayStr">json数组字符串</param>
        /// <param name="node">json节点名称</param>
        /// <param name="values">json节点值集合</param>
        /// <returns></returns>
        public static string DeleteJsonArrayElementByNodeVales(string jsonArrayStr, string node, List<string> values)
        {
            if (jsonArrayStr.IsNullOrEmpty() || node.IsNullOrEmpty() || values.IsNullOrEmpty())
            {
                return jsonArrayStr;
            }

            IList<JToken> delList = new List<JToken>(); //存储需要删除的项
            var jsonArray = JArray.Parse(jsonArrayStr);

            foreach (var jsonToken in jsonArray)
            {
                if (values.Contains(jsonToken.Value<string>(node)))
                {
                    delList.Add(jsonToken);
                }
            }

            foreach (var item in delList)
            {
                jsonArray.Remove(item);
            }

            return jsonArray.ToString();
        }

        public static SortedDictionary<string, object> KeySort(JObject obj)
        {
            var sortedDic = new SortedDictionary<string, object>();
            foreach (var x in obj)
            {
                if (x.Value is JValue)
                {
                    sortedDic.Add(x.Key, x.Value);
                }
                else if (x.Value is JObject)
                {
                    sortedDic.Add(x.Key, KeySort((JObject)x.Value));
                }
                else if (x.Value is JArray)
                {
                    if (x.Value.All(v => v is JValue))
                    {
                        sortedDic.Add(x.Key, x.Value);
                        continue;
                    }

                    var tmp = new SortedDictionary<string, object>[x.Value.Count()];
                    for (var i = 0; i < x.Value.Count(); i++)
                    {
                        tmp[i] = KeySort((JObject)x.Value[i]);
                    }
                    sortedDic.Add(x.Key, tmp);
                }
            }
            return sortedDic;
        }
    }
}