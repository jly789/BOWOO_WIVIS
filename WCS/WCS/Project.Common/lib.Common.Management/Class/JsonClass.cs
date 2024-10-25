using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace lib.Common.Management
{
    public class JsonClass
    {
        private static JsonClass _Instance;

        static public JsonClass getInstance()
        {
            if (_Instance == null)
                _Instance = new JsonClass();
            return _Instance;
        }

        public JObject HttpCall(string web_Addr,Dictionary<string,string> param)
        {
            //DB에 접속하여 박스 정보를 가져온다.


            foreach (KeyValuePair<string, string> r in param)
            {
                if(web_Addr.Substring(web_Addr.Length - 1,1) == "?")
                {
                    web_Addr += r.Key + "=" + r.Value;
                }
                else
                {
                    web_Addr += "&" + r.Key + "=" + r.Value;
                }
            }

            //데이터를 전송할 서버와 연결(URL)
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(web_Addr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            // 응답 값 확인
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
            string result = streamReader.ReadToEnd();
            Console.WriteLine(result);
            Debug.WriteLine(result.ToString());
            JObject jo = JObject.Parse(result);

            return jo;
        }

    }
}
