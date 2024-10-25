using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;

namespace lib.Common.Management
{
    public class WivisOracle
    {
        //private static WivisOracle _Instance;

        //static public WivisOracle getInstance()
        //{
        //    if (_Instance == null)
        //        _Instance = new WivisOracle();
        //    return _Instance;
        //}
        bowoo.Lib.DataBase.OracleDBConnection oracle = null;

        public WivisOracle(string con_str)
        {
            oracle = new bowoo.Lib.DataBase.OracleDBConnection(con_str);
        }

        public void rfidInsert(DataTable data)
        {

            //ssql += "into IF_PASDELIVERY_RECV values ('WIVIS'     ,'W'    ,'" + data.Rows[i]["wh_Cd"].ToString() + "' ,'W'     ,'A'    ,'" + data.Rows[i]["outdate"].ToString() + "' ";
            //ssql += ",'" + data.Rows[i]["batch"].ToString() + "', '" + data.Rows[i]["shop_cd"].ToString() + "', '" + data.Rows[i]["item_cd"].ToString() + "' ";
            //ssql += ",'" + data.Rows[i]["box_inv"].ToString() + "', '1' , 'N' ,'" + data.Rows[i]["chute_no"].ToString() + "' ";
            //ssql += ",'" + data.Rows[i]["ord_no"].ToString() + "', '" + data.Rows[i]["ord_seq"].ToString() + "','" + data.Rows[i]["item_cd"].ToString() + "' ";
            //ssql += ",0  , " + data.Rows[i]["box_no"].ToString() + ", '" + data.Rows[i]["box_end"].ToString() + "', '" + data.Rows[i]["asort_status"].ToString() + "' ";
            //ssql += ",'" + data.Rows[i]["assort_cd"].ToString() + "', '' , '" + data.Rows[i]["ind_no"].ToString() + "', '" + data.Rows[i]["tray_no"].ToString() + "' ";
            //ssql += ", SYSDATE, '0', SYSDATE , '')";

            

            try
            {
                string ssql = string.Empty;
                ssql = "insert into IF_PASDELIVERY_RECV";
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    ssql += " SELECT 'WIVIS'  ,'W','" + data.Rows[i]["WH_CD"].ToString() + "' ,'W' ,'A' ,'" + data.Rows[i]["biz_day"].ToString() + "' ";
                    ssql += ",'" + data.Rows[i]["box_inv"].ToString() + "', '" + data.Rows[i]["shop_cd"].ToString() + "', '" + data.Rows[i]["item_cd"].ToString() + "' ";
                    ssql += ",'" + data.Rows[i]["box_inv"].ToString() + "', '1' , 'N' ,'" + data.Rows[i]["chute_no"].ToString() + "' ";
                    ssql += ",'', '','" + data.Rows[i]["item_cd"].ToString() + "' ";
                    ssql += ",0  , " + data.Rows[i]["wrk_qty"].ToString() + ", '" + data.Rows[i]["box_end"].ToString() + "', '' ";
                    ssql += ",'', '' , '', '' ";
                    ssql += ", SYSDATE, '0', SYSDATE , '' ";

                    //ssql += " SELECT 'WIVIS'  ,'W','" + data.Rows[i]["wh_Cd"].ToString() + "' ,'W' ,'A' ,'" + data.Rows[i]["biz_day"].ToString() + "' ";
                    //ssql += ",'" + data.Rows[i]["box_inv"].ToString() + "', '" + data.Rows[i]["shop_cd"].ToString() + "', '" + data.Rows[i]["item_cd"].ToString() + "' ";
                    //ssql += ",'" + data.Rows[i]["box_inv"].ToString() + "', '1' , 'N' ,'" + data.Rows[i]["chute_no"].ToString() + "' ";
                    //ssql += ",'" + data.Rows[i]["ord_no"].ToString() + "', '" + data.Rows[i]["ord_seq"].ToString() + "','" + data.Rows[i]["item_cd"].ToString() + "' ";
                    //ssql += ",0  , " + data.Rows[i]["wrk_qty"].ToString() + ", '" + data.Rows[i]["box_end"].ToString() + "', '" + data.Rows[i]["asort_status"].ToString() + "' ";
                    //ssql += ",'" + data.Rows[i]["assort_cd"].ToString() + "', '' , '', '' ";
                    //ssql += ", SYSDATE, '0', SYSDATE , '' ";

                    if (i + 1 == data.Rows.Count || data.Rows.Count == 1)
                    {
                        ssql += "FROM DUAL ";
                    }
                    else
                    {
                        ssql += "FROM DUAL UNION ALL ";

                    }
                }
                oracle.executeNonQuery(ssql);

            }
            catch (Exception ex)
            {
                lib.Common.Log.LogFile.WriteError(ex, "Query: " + "insert into IF_PASDELIVERY_RECV" + "\n" + ex.Message + ex.StackTrace);
                throw ex;
            }
            //oracle.executeNonQuery(data);
        }


        public object[] invoiceSerch(string shop_cd, string cust_cd, string lc_cd)
        {
            try
            {
                {
                    object[] retunvalue = oracle.executeNonQuery("SP_GET_HJ_INVOICE",shop_cd, cust_cd, lc_cd);
                    return retunvalue;
                }
            }
            catch (Exception ex)
            {
                lib.Common.Log.LogFile.WriteError(ex, "Query: " + "SP_GET_HJ_INVOICE" + "\n" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

    }
}
