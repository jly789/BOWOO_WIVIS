/* *
 * 
 * http://www.hoons.kr/board.aspx?Name=cshaptip&Mode=2&BoardIdx=33089&Key=Content&Value=excel+export
 * http://msdn.microsoft.com/ko-kr/library/system.web.ui.page_members.aspx 
 * http://blogs.msdn.com/b/erikaehrli/archive/2009/01/30/how-to-export-data-to-excel-from-an-asp-net-application-avoid-the-file-format-differ-prompt.aspx
 * http://www.hoons.kr/board.aspx?Name=qaaspnet&Mode=2&BoardIdx=25091&Key=Content&Value=excel+export
 * http://www.hoons.kr/board.aspx?Name=qaaspnet&Mode=2&BoardIdx=21142&Key=Content&Value=excel+export
 * 
 * */

using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace lib.Common
{
    public enum ContentType 
    {
        xls,
        xlsx,
        doc,
        docx,
        pdf,
        ppt,
        pptx,
        txt,
        csv,
        octet
    }

    public class Export : Object
    {            

        /// <summary>
        /// DataTable에서 엑셀 변환 또는 엑셀에서 DataTable 변환 가능 함수
        /// 테이블 테이타를 Import하여 엑셀파일로 변환
        /// Export data to an Excel spreadsheet via ADO.NET
        /// </summary>
        /// <param name="DiskFileNmae"></param>
        /// <param name="dt"></param>
        /// <param name="version"></param>
        public void ToExcel(string DiskFileNmae, DataTable dt, ContentType? version)
        {
            string strExcelConn = string.Empty;
            
            switch(version)
            {
                case ContentType.xls:
                    strExcelConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="  + DiskFileNmae + ";Extended Properties='Excel 8.0;HDR=Yes'";
                    break;
                default:
                    strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DiskFileNmae + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'";
                    break;
            }

            using (OleDbConnection conn = new OleDbConnection(strExcelConn))
            {
                // 아래는 주소관련 테이블 생성예시
                // Create a new sheet in the Excel spreadsheet.
                OleDbCommand cmd = new OleDbCommand("create table ZipCode(ZipCode varchar(50), SIDO varchar(50), GUGUN varchar(50), DONG varchar(50), BUNJI varchar(50))", conn);

                // Open the connection.
                conn.Open();

                // Execute the OleDbCommand.
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO ZipCode (ZipCode, SIDO, GUGUN, DONG, BUNJI) values (?,?,?,?,?)";

                // Add the parameters.

                cmd.Parameters.Add("ZipCode", OleDbType.VarChar, 50, "ZipCode");
                cmd.Parameters.Add("SIDO", OleDbType.VarChar, 50, "SIDO");
                cmd.Parameters.Add("GUGUN"  , OleDbType.VarChar, 50, "GUGUN"  );
                cmd.Parameters.Add("DONG"   , OleDbType.VarChar, 50, "DONG"   );
                cmd.Parameters.Add("BUNJI"  , OleDbType.VarChar, 50, "BUNJI"  );

                // Initialize an SqlDataAdapter object.
                OleDbDataAdapter da = new OleDbDataAdapter("select * from ZipCode", conn);

                // Set the InsertCommand of SqlDataAdapter, 
                // which is used to insert data.
                da.InsertCommand = cmd;

                // Changes the Rowstate()of each DataRow to Added,
                // so that SqlDataAdapter will insert the rows.
                foreach (DataRow dr in dt.Rows)
                {
                    dr.SetAdded();
                }

                // Insert the data into the Excel spreadsheet.
                da.Update(dt);
            }
        }

        /// <summary>
        /// Export data to an Excel spreadsheet via ADO.NET
        /// </summary>
        /// <param name="dt"></param>
        public void ToXml( DataTable dt)
        {
        }
    }
}