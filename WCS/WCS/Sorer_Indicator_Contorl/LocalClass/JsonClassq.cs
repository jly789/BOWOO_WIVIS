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

namespace Sorer_Indicator_Contorl
{
    class JsonClassq
    {
        private static JsonClassq _Instance;

        static public JsonClassq getInstance()
        {
            if (_Instance == null)
                _Instance = new JsonClassq();
            return _Instance;
        }

        static public void HttpCall(string web_Addr,Dictionary<string,string> param)
        {
            //DB에 접속하여 박스 정보를 가져온다.


            //가져온 박스 정보를 이용하여 JSON 데이터 생성
            //JObject json = new JObject();
            //foreach(KeyValuePair<string,string> r in param)
            //{
            //    json.Add(r.Key, r.Value);
            //}

            //json.Add("key", "DzOY4X32X9FLa4CI5eRF");
            //json.Add("WhCod", "CN01");
            //json.Add("BizDay", "20200903");

            web_Addr += "?";
            foreach (KeyValuePair<string, string> r in param)
            {
                web_Addr += "&" + r.Key + "=" + r.Value;
            }

            //데이터를 전송할 서버와 연결(URL)
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(web_Addr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://cbtdev.htns.com:8090/api/dps/RcvDataDL.do?WhCod=CN01&BizDay=20200903&key=DzOY4X32X9FLa4CI5eRF&Batch=4");
            //httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Method = "POST";

            // 데이터 전송
            //StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            //streamWriter.Write(json);
            //streamWriter.Flush();

            // 응답 값 확인
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream());
            string result = streamReader.ReadToEnd();
            Console.WriteLine(result);
            Debug.WriteLine(result.ToString());

            JObject jo = JObject.Parse(result);
            var a = jo.SelectToken("DATA_LIST");
            //var cnt = a.Count();
            DataTable dt = new DataTable();
            dt.Columns.Add("OrdQty");
            dt.Columns.Add("TnsCode");
            dt.Columns.Add("Batch");
            dt.Columns.Add("BizDay");
            dt.Columns.Add("ItemLoc");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("OrdSeq");
            dt.Columns.Add("ItemBar");
            dt.Columns.Add("OrdNo");


            foreach (var item in a)
            {
                DataRow temp_row = dt.NewRow();
                temp_row["OrdQty"] = item.SelectToken("OrdQty").ToString();
                temp_row["TnsCode"] = item.SelectToken("TnsCode").ToString();
                temp_row["Batch"] = item.SelectToken("Batch").ToString();
                temp_row["BizDay"] = item.SelectToken("BizDay").ToString();
                temp_row["ItemLoc"] = item.SelectToken("ItemLoc").ToString();
                temp_row["ItemName"] = item.SelectToken("ItemName").ToString();
                temp_row["OrdSeq"] = item.SelectToken("OrdSeq").ToString();
                temp_row["ItemBar"] = item.SelectToken("ItemBar").ToString();
                temp_row["OrdNo"] = item.SelectToken("OrdNo").ToString();
                dt.Rows.Add(temp_row);
                //if (items != null)
                //{
                //    foreach (var token in items)
                //    {
                //        var name = String.Format("{0}", token.SelectToken("name"));
                //        var value = String.Format("{0}", token.SelectToken("value"));
                //    }
                //}


            }

            //streamWriter.Close();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Debug.WriteLine(dt.Columns[i].ColumnName);
            }

            DataRow[] temp_row12 = dt.Select("");

        }

    }
}
