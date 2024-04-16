using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicTools
{
    public class JsonUtils
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonText)
        {
            string txt = jsonText;
            if (jsonText == null || jsonText.Length < 2)
                txt = "{}";
            var jSetting = new JsonSerializerSettings
            {
                //忽略为NULL的值
                NullValueHandling = NullValueHandling.Ignore
            };

            return (T)JsonConvert.DeserializeObject<T>(jsonText, jSetting);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerializer(object obj)
        {
            var jSetting = new JsonSerializerSettings();
            //忽略为NULL的值
            jSetting.NullValueHandling = NullValueHandling.Ignore;

            return JsonConvert.SerializeObject(obj, jSetting);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerializer(object obj, string strDateFormat)
        {
            var jSetting = new JsonSerializerSettings();
            //忽略为NULL的值
            jSetting.NullValueHandling = NullValueHandling.Ignore;

            //时间格式
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = strDateFormat;

            return JsonConvert.SerializeObject(obj, Formatting.Indented, timeConverter);
        }

    }
}
