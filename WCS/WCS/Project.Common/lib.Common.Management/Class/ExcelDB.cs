using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace lib.Common.Management
{
    public class ExcelDB
    {
        /*---------------------------------------------------------------------
         * Connection String 에는 어마어마한 옵션들이.. 주로 영문 사이트이긴
         * 하지만 좀더 상세한 제어를 원한다면 OleDB Connection String 옵션과
         * Extended Properties 에 들어가는 Excel Option 들을 살펴보는 것이 좋을
         * 듯.. HDR = 첫줄을 헤더로 사용할 것인지,IMEX = 데이터유형적용여부 등등
         *-------------------------------------------------------------------*/
        // 확장명 XLS (Excel 97~2003 용)
        private const string ConnectStrFrm_Excel97_2003 =
            "Provider=Microsoft.Jet.OLEDB.4.0;" +
            "Data Source=\"{0}\";" +
            "Mode=ReadWrite|Share Deny None;" +
            "Extended Properties='Excel 8.0; HDR={1}; IMEX={2}';" +
            "Persist Security Info=False";
        // 확장명 XLSX (Excel 2007 이상용)
        private const string ConnectStrFrm_Excel =
            "Provider=Microsoft.ACE.OLEDB.12.0;" +
            "Data Source=\"{0}\";" +
            "Mode=ReadWrite|Share Deny None;" +
            "Extended Properties='Excel 12.0 Xml; HDR={1}; IMEX={2}';" +
            "Persist Security Info=False;";
        private const string ConnectStrFrm_ExcelT =
            "Provider=Microsoft.ACE.OLEDB.12.0;" +
            "Data Source=\"{0}\";" +
            "Mode=ReadWrite;" +
            "Extended Properties='Excel 12.0 Xml; HDR={1}; IMEX={2}';" +
            "Persist Security Info=False;";

        private const string tmpConn = "Provider=Microsoft.{0}.OLEDB.{1};" +
            "Data Source=\"{2}\";" +
            "Mode=ReadWrite|Share Deny None;" +
            "Extended Properties=\"{3};IMEX=1;HDR={4};\"";

        static string[] Alpabat = new string[]{"A","B","C","D","E","F","G","H","I","J","K",
                                            "L","M","N","O","P","Q","R","S","T","U","V",
                                            "W","X","Y","Z","AA","AB","AC","AD","AE","AF","AG","AH","AI"};

        /// <summary>
        ///    Excel 파일의 형태를 반환한다.
        ///    -2 : Error 
        ///    -1 : 엑셀파일아님
        ///     0 : 97-2003 엑셀 파일 (xls)
        ///     1 : 2007 이상 파일 (xlsx)
        /// </summary>
        /// <param name="XlsFile">
        ///    Excel File 명 전체 경로입니다.
        /// </param>
        public static int ExcelFileType(string XlsFile)
        {
            byte[,] ExcelHeader = {
                { 0xD0, 0xCF, 0x11, 0xE0, 0xA1 }, // XLS  File Header
                { 0x50, 0x4B, 0x03, 0x04, 0x14 }  // XLSX File Header
            };
            // result -2=error, -1=not excel , 0=xls , 1=xlsx
            int result = -1;
            FileInfo FI = new FileInfo(XlsFile);
            FileStream FS = null;
            try
            {
                FS = FI.Open(FileMode.Open);
                byte[] FH = new byte[5];
                FS.Read(FH, 0, 5);
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (FH[j] != ExcelHeader[i, j]) break;
                        else if (j == 4) result = i;
                    }
                    if (result >= 0) break;
                }
            }
            catch (Exception ex)
            {
                result = (-2);
                //throw e;
            }
            finally
            {
                if (FS != null) FS.Close();
            }
            return result;
        }
        /// <summary>
        ///    Excel 파일을 DataSet 으로 변환하여 반환한다.
        /// </summary>
        /// <param name="FileName">
        ///    Excel File 명 PullPath
        /// </param>
        /// <param name="UseHeader">
        ///    첫번째 줄을 Field 명으로 사용할 것이지 여부
        /// </param>
        private static DataSet OpenExcel(string FileName, bool UseHeader)
        {


            string _pathOnly = Path.GetDirectoryName(FileName);
            string _fileName = Path.GetFileNameWithoutExtension(FileName);
            string _extName = Path.GetExtension(FileName);

            string copyPath = System.IO.Path.Combine(_pathOnly, _fileName + "_TEMP" + _extName);
            //string copyPath = System.IO.Path.Combine(_pathOnly, _fileName +  _extName);

            System.IO.File.Copy(FileName, copyPath, true);



            DataSet DS = null;
            string[] HDROpt = { "NO", "YES" };
            string HDR = "";
            string ConnStr = "";
            if (UseHeader)
                HDR = HDROpt[1];
            else
                HDR = HDROpt[0];
            int ExcelType = ExcelFileType(copyPath);
            switch (ExcelType)
            {
                case (-2): throw new Exception(copyPath + "의 형식검사중 오류가 발생하였습니다.");
                case (-1): throw new Exception(copyPath + "은 엑셀 파일형식이 아닙니다.");
                case (0):
                    ConnStr = string.Format(ConnectStrFrm_Excel97_2003, copyPath, HDR, "1");
                    break;
                case (1):
                    ConnStr = string.Format(ConnectStrFrm_Excel, copyPath, HDR, "1");
                    break;
            }
            OleDbConnection OleDBConn = null;
            OleDbDataAdapter OleDBAdap = null;
            DataTable Schema;

            try
            {
                OleDBConn = new OleDbConnection(ConnStr);
                OleDBConn.Open();
                Schema = OleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                DS = new DataSet();

                foreach (DataRow DR in Schema.Rows)
                {
                    //OleDBAdap = new OleDbDataAdapter("select * from[" + _fileName + "$]", OleDBConn);
                    OleDBAdap = new OleDbDataAdapter(DR["TABLE_NAME"].ToString(), OleDBConn);
                    OleDBAdap.SelectCommand.CommandType = CommandType.TableDirect;
                    OleDBAdap.AcceptChangesDuringFill = false;
                    string TableName = DR["TABLE_NAME"].ToString().Replace("$", String.Empty).Replace("'", String.Empty);
                    if (DR["TABLE_NAME"].ToString().Contains(TableName + "$")) OleDBAdap.Fill(DS, copyPath);
                }



            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {

                if (OleDBConn != null) OleDBConn.Close();

                if (System.IO.File.Exists(copyPath))
                {
                    System.IO.File.Delete(copyPath);
                }

            }
            return DS;
        }

        public static void excel_add(string dir_path,string file_name, DataTable tempTable)
        {

            if (!Directory.Exists(dir_path)) Directory.CreateDirectory(dir_path);
            file_name = dir_path + "\\" + file_name;
            
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            DataTable dt = tempTable;
            DataRow[] row = dt.Select();
            if (row.Length == 0) return;
            int columns_count = dt.Columns.Count;
            //데이터
            string[,] item = new string[row.Length, columns_count];

            //컬럼명을 가지고 오기위함.
            string[] columns_name = new string[columns_count];
            int row_no = 0;
            foreach (DataRow r in row)
            {
                for (int i = 0; i < columns_count; i++)
                {
                    item[row_no, i] = r[i].ToString();
                }
                row_no++;
            }

            excelApp = new Excel.Application();
            excelApp.DisplayAlerts = false;//ture일 경우 메시지 발생.

            wb = excelApp.Workbooks.Add();//엑셀 기본 생성
            wb.Worksheets.get_Item(1).Name = "재고 정보";
            ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

            //저장 범위를 정함.
            for (int i = 0; i < columns_count; i++)
            {
                columns_name[i] = dt.Columns[i].ColumnName;
            }
            ws.get_Range("A1", Alpabat[columns_count - 1] + "1").Value2 = columns_name;
            ws.get_Range("A2", Alpabat[columns_count - 1] + (dt.Rows.Count + 1).ToString()).Value2 = item;

            //컬럼 폭을 자동으로 맞춰주기 위함.
            for (int i = 1; i <= columns_count; i++)
            {
                ((Excel.Range)ws.Columns[i]).AutoFit();
            }

            try
            {
                wb.SaveAs(file_name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("제품정보 excel 저장 메소드 에러내용: " + ex);
            }
            finally
            {
                processKill(excelApp);
                ws = null;
                wb = null;
                excelApp = null;
            }
        }

        //백그라운드에 excel남아있지 않게 하기위해.
        private static void processKill(object obj)
        {
            Excel.Application excel = (Excel.Application)obj;
            uint processId = 0;
            Dll_import.GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out processId);
            excel.Quit();
            if (processId != 0)
            {
                System.Diagnostics.Process excelProcess = System.Diagnostics.Process.GetProcessById((int)processId);
                excelProcess.CloseMainWindow();
                excelProcess.Refresh();
                excelProcess.Kill();
            }
        }

        //private void ChangeColumnType(System.Data.DataTable dt, string p, Type type)
        //{
        //    dt.Columns.Add(p + "_new", type);
        //    foreach (System.Data.DataRow dr in dt.Rows)
        //    {   // Will need switch Case for others if Date is not the only one.
        //        dr[p + "_new"] = DateTime.FromOADate(double.Parse(dr[p].ToString())); // dr[p].ToString();
        //    }
        //    dt.Columns.Remove(p);
        //    dt.Columns[p + "_new"].ColumnName = p;
        //}


        /// <summary>
        ///    DataSet 을 Excel 파일로 저장한다.
        /// </summary>
        /// <param name="FileName">
        ///    Excel File 명 PullPath
        /// </param>
        /// <param name="DS">
        ///    Excel 로 저장할 대상 DataSet 객체.
        /// </param>
        /// <param name="ExistDel">
        ///    동일한 파일명이 있을 때 삭제 할 것인지 여부, 파일이 있고 false 면 저장안하고 그냥 false 를 리턴.
        /// </param>
        /// <param name="OldExcel">
        ///    xls 형태로 저장할 것인지 여부, false 이면 xlsx 형태로 저장함.
        /// </param>
        private static bool SaveExcel(string dir_path ,string FileName, DataSet DS, bool ExistDel, bool OldExcel)
        {
            bool result = true;

            FileName = dir_path + "\\" + FileName;
            if (File.Exists(FileName))
            {
                FileName += "_1";
            }
                //if (ExistDel) File.Delete(FileName);
                //else return result;
            string TempFile = FileName;
            // 파일 확장자가 xls 이나 xlsx 가 아니면 아예 파일을 안만들어서
            // 템프파일로 생성후 지정한 파일명으로 변경..
            OleDbConnection OleDBConn = null;
            try
            {
                OleDbCommand Cmd = null;
                string ConnStr = "";
                if (OldExcel)
                {
                    TempFile = TempFile + ".xls";
                    ConnStr = string.Format(ConnectStrFrm_Excel97_2003, TempFile, "YES", "0");
                }
                else
                {
                    TempFile = TempFile + ".xlsx";
                    ConnStr = string.Format(ConnectStrFrm_Excel, TempFile, "NO", "0");
                    //TempFile = TempFile + ".XLSB";
                    //ConnStr = string.Format(ConnectStrFrm_ExcelT, TempFile, "NO", "0");
                    //ConnStr = string.Format(ConnectStrFrm_ExcelT, TempFile);
                    //ConnStr = string.Format(tmpConn, "ACE", "12.0", TempFile, "Excel 12.0 Xml", false ? "YES" : "NO");
                }

                OleDBConn = new OleDbConnection(ConnStr);
                OleDBConn.Open();
                // Create Table(s).. : 테이블 단위 처리
                foreach (DataTable DT in DS.Tables)
                {
                    String TableName = DT.TableName;
                    StringBuilder FldsInfo = new StringBuilder();
                    StringBuilder Flds = new StringBuilder();
                    // Create Field(s) String : 현재 테이블의 Field 명 생성
                    foreach (DataColumn Column in DT.Columns)
                    {
                        if (FldsInfo.Length > 0)
                        {
                            FldsInfo.Append(",");
                            Flds.Append(",");
                        }
                        FldsInfo.Append("[" + Column.ColumnName.Replace("'", "''") + "] CHAR(255)");
                        Flds.Append(Column.ColumnName.Replace("'", "''"));
                    }

                    // Table Create
                    //Cmd = new OleDbCommand("CREATE TABLE Table ([BIZ_DAY] CHAR(255) )", OleDBConn);
                    Cmd = new OleDbCommand("CREATE TABLE " + TableName + "(" + FldsInfo.ToString() + ")", OleDBConn);
                    Cmd.ExecuteNonQuery();
                    // Insert Data
                    foreach (DataRow DR in DT.Rows)
                    {
                        StringBuilder Values = new StringBuilder();
                        foreach (DataColumn Column in DT.Columns)
                        {
                            if (Values.Length > 0) Values.Append(",");
                            Values.Append("'" + DR[Column.ColumnName].ToString().Replace("'", "''") + "'");
                        }
                        Cmd = new OleDbCommand(
                            "INSERT INTO [" + TableName + "$]" +
                            "(" + Flds.ToString() + ") " +
                            "VALUES (" + Values.ToString() + ")",
                            OleDBConn);

                        Cmd.ExecuteNonQuery();
                    }
                    //string wewe = TempFile.Insert(10, "12");
                    //int tt = TempFile.IndexOf('.');
                    //string wew = TempFile.Insert(tt, "12");
                    //File.Copy(TempFile, wew.Replace("XLSB", "XLSX"));
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (OleDBConn != null) OleDBConn.Close();
                try
                {
                    //if (File.Exists(TempFile))
                    //{
                    //    File.Move(TempFile, FileName);
                    //}
                }
                catch { }
            }
            return result;
        }



        /// <summary>
        ///    Excel 파일을 DataSet 으로 변환하여 반환한다.
        /// </summary>
        /// <param name="ExcelFile">
        ///    읽어올 Excel File 명(전체경로)입니다.
        /// </param>
        public static DataSet OpenExcelDB(string ExcelFile)
        {
            try
            {
                return OpenExcel(ExcelFile, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        ///    DataSet 을 Excel 파일로 저장한다. 동일 파일명이 있으면 Overwrite 됩니다.
        /// </summary>
        /// <param name="ExcelFile">
        ///    저장할 Excel File 명(전체경로)입니다.
        /// </param>
        /// <param name="DS">
        ///    저장할 대상 DataSet 입니다.
        /// </param>
        public static bool SaveExcelDB(string dir_path, string fileName, DataSet DS)
        {

            if (!Directory.Exists(dir_path)) Directory.CreateDirectory(dir_path);
            return SaveExcel(dir_path,fileName, DS, true, false);
        }
    }
}
