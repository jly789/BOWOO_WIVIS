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
using System.Xml;

namespace lib.Common.Management
{
    public class Xml
    {
        private static Xml _Instance;

        static public Xml getInstance()
        {
            if (_Instance == null)
                _Instance = new Xml();
            return _Instance;
        }

        public XmlDocument xmlMake(List<Dictionary<string, string>> ParamDataList)
        {
            //---------------------------------------------------
            XmlDocument xdoc = new XmlDocument();

            // 루트노드
            XmlNode root = xdoc.CreateElement("root");
            xdoc.AppendChild(root);

            //데이터 노드
            XmlNode[] childNode = new XmlNode[ParamDataList.Count];

            string columnName = string.Empty;
            string columnValue = string.Empty;

            for (int i = 0; i < ParamDataList.Count; i++)
            {
                //데이터 집합 노드
                XmlNode data = xdoc.CreateElement("data");
                root.AppendChild(data);
                foreach (KeyValuePair<string, string> r in ParamDataList[i])
                {
                    columnName = r.Key;
                    columnValue = r.Value;
                    //자식 노드 태그명 설정
                    childNode[i] = xdoc.CreateElement(columnName);
                    //자식 노드 이너 값 설정
                    childNode[i].InnerText = columnValue;
                    data.AppendChild(childNode[i]);
                }
                root.AppendChild(data);
            }
            xdoc.Save(@"d:\Emp.xml");
            return xdoc;
        }

        public XmlDocument xmlMake(DataTable dt)
        {
            //---------------------------------------------------
            XmlDocument xdoc = new XmlDocument();

            // 루트노드
            XmlNode root = xdoc.CreateElement("root");
            xdoc.AppendChild(root);

            //데이터 노드
            XmlNode[] childNode = new XmlNode[dt.Rows.Count];

            string columnName = string.Empty;
            string columnValue = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //데이터 집합 노드
                XmlNode data = xdoc.CreateElement("data");
                root.AppendChild(data);
                for(int j = 0; j < dt.Columns.Count; j++)
                {
                    columnName = dt.Columns[j].ColumnName;
                    columnValue = dt.Rows[i][j].ToString();
                    //자식 노드 태그명 설정
                    childNode[i] = xdoc.CreateElement(columnName);
                    //자식 노드 이너 값 설정
                    childNode[i].InnerText = columnValue;
                    data.AppendChild(childNode[i]);
                }
                root.AppendChild(data);
               
            }
            xdoc.Save(@"d:\Emp.xml");
            return xdoc;
        }

    }

}
