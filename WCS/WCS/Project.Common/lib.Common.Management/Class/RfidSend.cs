using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace lib.Common.Management.Class
{
    /// <summary>
    ///RFID(유성)으로 박스정보 전송 후 프린트 하는 클래스 (위비스에서만 사용)
    /// </summary>
    public class RfidSend
    {
        public void rfid_send(DataSet tempSet, string print_msg, int label_pos)
        {
            bowoo.Lib.DataBase.SqlQueryExecuter DBUtil = bowoo.Lib.DataBase.SqlQueryExecuter.getInstance();
            if (tempSet.Tables[0] != null && Convert.ToInt32(tempSet.Tables[1].Rows[0]["work_type"]) == 2)
            {
                try
                {
                    //커넥션 나중에 오픈해야함. (실제 오픈시 주석풀어야함.)
                    lib.Common.Management.WivisOracle wivisOracle = new lib.Common.Management.WivisOracle(System.Configuration.ConfigurationManager.AppSettings["OracleDatabaseip"].ToString());
                    //lib.Common.Management.WivisOracle wivisOracle = null;
                    wivisOracle.rfidInsert(tempSet.Tables[0]);
                    StringBuilder sb = new StringBuilder("update if_box_list set status = 1 where biz_day = '");
                    sb.Append(tempSet.Tables[0].Rows[0]["biz_day"].ToString());
                    sb.Append("' and box_no = ");
                    sb.Append(tempSet.Tables[0].Rows[0]["box_no"].ToString());
                    sb.Append(" and chute_no = ");
                    sb.Append(tempSet.Tables[0].Rows[0]["chute_no"].ToString());
                    sb.Append(" and batch = '");
                    sb.Append(tempSet.Tables[0].Rows[0]["batch"].ToString() + "'");

                    try
                    {
                        DBUtil.ExecuteNonQuery(sb.ToString()); //오라클 데이터 전송후 에러가 없을 경우 box_stauts 를 1로 바꿈.

                    }
                    catch (Exception ex)
                    {
                        lib.Common.Log.LogFile.WriteError(ex, "Query: " + "update if_box_list" + "\n" + ex.Message + ex.StackTrace);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {

                }

                printMethode(print_msg, label_pos);
                //if (print_msg.Substring(0, 1) != "M") //미할당 매장이 아닐경우
                //{
                //    printMethode(print_msg, label_pos);
                //}
                //else //미할당 매장
                //{
                //    Task.Run(() =>
                //    {
                //        TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //        p.print(tempSet.Tables[1], "LABEL_" + label_pos.ToString().PadLeft(2, '0'), "unassigned_label");
                //    });
                //}

            }
            //번들 작업 박스 마감시.
            else if (tempSet.Tables[1] != null && Convert.ToInt32(tempSet.Tables[1].Rows[0]["work_type"]) != 2)
            {
                Task.Run(() =>
                {
                    TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                    p.print(tempSet.Tables[1], "LABEL_" + label_pos.ToString().PadLeft(2, '0'), "bundle_label");
                });
            }
        }


        /// <summary>
        /// 프린트 정보를 파싱해서 프린트 클래스에 넘김.
        /// </summary>
        /// <param name="printList"></param>
        public void printMethode(string printList, int label_no)
        {

            Task.Run(() =>
            {
                string[] printString_arr = null; //라벨 출력할 데이터를 담는 변수.
                printString_arr = printList.ToString().Split('/');
                //앞뒤 공백제거
                for (int i = 0; i < printString_arr.Length; i++)
                {
                    printString_arr[i] = printString_arr[i].TrimEnd().TrimStart();
                }
                TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //p.print(printString_arr, "Intermec PC43d (203 dpi)");
                p.print(printString_arr, "LABEL_" + label_no.ToString().PadLeft(2, '0'));
            });
        }



        public object[] invoiceSerch(int chute_no)
        {
            try
            {
                object[] returnvalue = null;
                bowoo.Lib.DataBase.SqlQueryExecuter DBUtil = bowoo.Lib.DataBase.SqlQueryExecuter.getInstance();
                SqlParameter[] parmData = null;

                parmData = new SqlParameter[5];
                parmData[0] = new SqlParameter();
                parmData[0].ParameterName = "@V_CHUTE_NO"; //슈트번호
                parmData[0].DbType = DbType.Int32;
                parmData[0].Direction = ParameterDirection.Input;
                parmData[0].Value = chute_no;

                parmData[1] = new SqlParameter();
                parmData[1].ParameterName = "@O_GUBUN_CD"; //택배사 구분 코드
                parmData[1].Size = 8000;
                parmData[1].Value = "";
                parmData[1].Direction = ParameterDirection.Output;

                parmData[2] = new SqlParameter();
                parmData[2].ParameterName = "@O_SHOP_CD"; //매장코드
                parmData[2].Size = 8000;
                parmData[2].Value = "";
                parmData[2].Direction = ParameterDirection.Output;

                parmData[3] = new SqlParameter();
                parmData[3].ParameterName = "@O_CUST_CD"; //고정값1 화주사?
                parmData[3].Size = 8000;
                parmData[3].Value = "";
                parmData[3].Direction = ParameterDirection.Output;

                parmData[4] = new SqlParameter();
                parmData[4].ParameterName = "@O_LC_CD"; //고정값2 센터코드?
                parmData[4].Size = 8000;
                parmData[4].Value = "";
                parmData[4].Direction = ParameterDirection.Output;

                DBUtil.ExecuteDataSetSqlParam("IF_SP_GET_HANJIN_INVOICE_VALUE", parmData);
                
                if (parmData[1].Value.ToString() == "" || parmData[2].Value.ToString() == "" || parmData[3].Value.ToString() == "")
                {
                    if (parmData[1].Value.ToString() == "")
                    {
                        throw new Exception("배송사 구분 코드가 검색 되지 않아 박스 마감불가");
                    }
                    else if (parmData[2].Value.ToString() == "")
                    {
                        throw new Exception("매장 코드가 검색 되지 않아 박스 마감불가");
                    }
                }

                //한진 택배일 경우만 (위비스)ERP에서 송장 정보 검색
                if (parmData[1].Value.ToString() == "C")
                {
                    //returnvalue = new object[5];
                    //returnvalue[0] = "1234567890"; // 송장 번호
                    //returnvalue[1] = "1564"; // 터미널 코드
                    //returnvalue[2] = "테스트"; // 터미널 명

                    lib.Common.Management.WivisOracle wivisOracle = new lib.Common.Management.WivisOracle(System.Configuration.ConfigurationManager.AppSettings["OracleDatabaseip"].ToString());
                    returnvalue = wivisOracle.invoiceSerch(parmData[2].Value.ToString()
                                                    , parmData[3].Value.ToString()
                                                    , parmData[4].Value.ToString()); //송장 번호 검색
                }
                return returnvalue;

            }
            catch (Exception ex)
            {
                lib.Common.Log.LogFile.WriteError(ex, "Query: " + $"BOX_END(슈트마감)\r\n슈트번호{chute_no}" + "\n" + ex.Message + ex.StackTrace);
                throw ex;
                //return null;
            }
        }
    }
}
