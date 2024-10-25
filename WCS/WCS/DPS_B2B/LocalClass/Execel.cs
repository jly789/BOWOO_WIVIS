using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using excel = Microsoft.Office.Interop.Excel;


namespace DPS_B2B
{
    class Execel
    {


        string[] Alpabat = new string[]{"A","B","C","D","E","F","G","H","I","J","K",
                                            "L","M","N","O","P","Q","R","S","T","U","V",
                                            "W","X","Y","Z"};


        
        public DataTable read(string filepath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();

                // 엑셀 파일 열기
                string ddwdwd = filepath + @"\Location";
                wb = excelApp.Workbooks.Open(filepath + @"\Location");
                // 첫번째 Worksheet
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
                // 현재 Worksheet에서 사용된 Range 전체를 선택
                Excel.Range rng = ws.UsedRange;


                // Range 데이타를 배열 (One-based array)로
                object[,] data = rng.Value;
                string[,] return_String = new string[data.GetLength(0), data.GetLength(1)];

                DataTable dt = new DataTable();

                dt.Columns.Add("LOC_NO");
                dt.Columns.Add("ITEM_CD");
                string t = dt.Columns[0].ToString();
                string t1 = dt.Columns[1].ToString();
                for (int r = 2; r <= data.GetLength(0); r++)
                {
                    DataRow dr = dt.NewRow();
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        dr[dt.Columns[c].ToString()] = data[r, c + 9] == null ? "" : data[r, c + 9].ToString();
                    }
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("엑셀 로드 메소드 에러내용 : " + ex);
                return null;
            }
            finally
            {
                processKill(excelApp);
            }

        }



        public void excel_add(string filepath, DataTable dt)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;
            //폴더 유무 확인후 없으면 생성.
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            //DataTable dt = ds.Tables[0];
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
            wb.Worksheets.get_Item(1).Name = "로케이션";
            ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

            string date = DateTime.Now.ToString("yyyy'-'MM'-'dd' _TIME 'HH'-'mm'-'ss");
            //string file_name = date + ".xlsx";
            string file_name ="로케이션 백업.xlsx";

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
                //파일명 동일한거 있을때 검사하고 뒤에 다른거를 붙이기위해 사용하려고 만들었는데 시간을 같이 찍어서 필요 없음.
                DirectoryInfo dir = new DirectoryInfo(filepath);
                FileInfo[] files = dir.GetFiles(file_name, SearchOption.AllDirectories);
                if (files.Count() == 0)
                {
                    wb.SaveAs(filepath + @"\" + file_name);
                }
                else
                {
                    string insert_string = "(" + files.Count().ToString() + ")";
                    string temp_file = file_name.Insert(7, insert_string);
                    wb.SaveAs(filepath +"\\" + temp_file);
                }

                string efegefeef = filepath + file_name;
                wb.SaveAs(filepath + file_name);

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
        private void processKill(object obj)
        {
            Excel.Application excel =  (Excel.Application)obj;
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
    }
}
