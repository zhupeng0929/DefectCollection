using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;

namespace XiaoQingWa_Work_Utility
{
    public class JsonHelper
    {
        public static string Serializer<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray());
                return szJson;
            }
        }

        public static string SerializerObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Deserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        /// <summary> 
        /// Json数据绑定类 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        public class JsonBinder<T> : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                IList<T> list = new List<T>();
                //从请求中获取提交的参数数据 
                var json = controllerContext.HttpContext.Request.Form[bindingContext.ModelName] as string;
                //提交参数是对象 
                if (json.StartsWith("{") && json.EndsWith("}"))
                {
                    JObject jsonBody = JObject.Parse(json);
                    JsonSerializer js = new JsonSerializer();
                    object obj = js.Deserialize(jsonBody.CreateReader(), typeof(T));
                    list.Add((T)obj);
                    return list;
                }
                //提交参数是数组 
                if (json.StartsWith("[") && json.EndsWith("]"))
                {
                    JArray jsonRsp = JArray.Parse(json);
                    if (jsonRsp != null)
                    {
                        for (int i = 0; i < jsonRsp.Count; i++)
                        {
                            JsonSerializer js = new JsonSerializer();
                            object obj = js.Deserialize(jsonRsp[i].CreateReader(), typeof(T));
                            list.Add((T)obj);
                        }
                    }
                    return list;
                }
                return null;
            }
        }
    }
}
