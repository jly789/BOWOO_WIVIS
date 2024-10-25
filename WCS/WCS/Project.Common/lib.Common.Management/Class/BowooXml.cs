using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;
using System.Data;
using Telerik.WinControls.UI;

namespace lib.Common.Management
{
    public class BowooXml
    {
        static object xmlMakeLock = new object();

        public XmlDocument xmlMake(List<object> objectClass)
        {
            lock (xmlMakeLock)
            {
                //---------------------------------------------------
                XmlDocument xdoc = new XmlDocument();

                // 루트노드
                XmlNode root = xdoc.CreateElement("root");
                xdoc.AppendChild(root);

                //데이터 노드
                XmlNode[] childNode = new XmlNode[objectClass.Count];

                string columnName = string.Empty;
                string columnValue = string.Empty;

                for (int i = 0; i < objectClass.Count; i++)
                {
                    //데이터 집합 노드
                    XmlNode data = xdoc.CreateElement("data");
                    root.AppendChild(data);
                    foreach (FieldInfo r in objectClass[i].GetType().GetFields())
                    {
                        columnName = r.Name;
                        columnValue = r.GetValue(objectClass[i]) != null ? r.GetValue(objectClass[i]).ToString() : "";
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

        public XmlDocument xmlMake(List<GridViewRowInfo> tempTable)
        {
            lock (xmlMakeLock)
            {
                //---------------------------------------------------
                XmlDocument xdoc = new XmlDocument();

                // 루트노드
                XmlNode root = xdoc.CreateElement("root");
                xdoc.AppendChild(root);

                //데이터 노드
                //XmlNode[] childNode = new XmlNode[tempTable.Columns.Count];
                XmlNode childNode = null;

                string columnName = string.Empty;
                string columnValue = string.Empty;

                ////데이터 집합 노드
                //XmlNode data = xdoc.CreateElement("data");
                //root.AppendChild(data);






                for (int i = 0; i < tempTable.Count; i++)
                {
                    //데이터 집합 노드
                    XmlNode data = xdoc.CreateElement("data");
                    root.AppendChild(data);
                    for (int j = 0; j < tempTable[i].Cells.Count; j++)
                    {


                        columnName = tempTable[i].Cells[j].ColumnInfo.Name;
                        childNode = xdoc.CreateElement(columnName);

                        //자식 노드 이너 값 설정
                        columnValue = tempTable[i].Cells[j].Value != null ? tempTable[i].Cells[j].Value.ToString() : "";

                        childNode.InnerText = columnValue;
                        data.AppendChild(childNode);


                    }
                    root.AppendChild(data);
                }


                xdoc.Save(@"d:\Emp.xml");
                return xdoc;
            }
        }
    }
}
