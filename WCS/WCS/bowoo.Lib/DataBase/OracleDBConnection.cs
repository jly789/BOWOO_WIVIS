using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.OracleClient;
using System.Data;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;

namespace bowoo.Lib.DataBase
{
    public class OracleDBConnection
    {
        private static OracleDBConnection _Instance;
        private OracleConnection con = new OracleConnection();
        string con_str = string.Empty;


        public  OracleDBConnection(string con_str)
        {
            this.con_str = con_str;
            con.ConnectionString = con_str;
            Environment.SetEnvironmentVariable("NLS_LANG", "KOREAN_KOREA.KO16MSWIN949"); //오라클 캐릭터셋이 달라서 넣어 줘야함.
        }


        public void executeNonQuery(string cmd_Str)
        {
            try
            {
                con.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = cmd_Str;
                cmd.Connection = con;

                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(con.State.ToString() == "Open")
                con.Close();
            }
        }



        public object[] executeNonQuery(string cmd_Str, string shop_cd, string cust_cd, string lc_cd)
        {
            try
            {
                con.Open();

                OracleCommand cmd = new OracleCommand();
                cmd.Parameters.Add("I_SHIP_COMP_CD", OracleDbType.NVarchar2, 200).Value = cust_cd;
                cmd.Parameters.Add("I_CUST_ID", OracleDbType.NVarchar2, 200).Value = lc_cd;
                cmd.Parameters.Add("I_SHP_CD", OracleDbType.NVarchar2, 200).Value = shop_cd;

                cmd.Parameters.Add("O_INVOICE", OracleDbType.NVarchar2, 200).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("O_TML_CD", OracleDbType.NVarchar2, 200).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("O_TML_NM", OracleDbType.NVarchar2, 200).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("O_RTN_CD", OracleDbType.Double).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("O_RTN_MSG", OracleDbType.NVarchar2, 200).Direction = ParameterDirection.Output;

                cmd.CommandText = cmd_Str;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();

                object[] returnValue = new object[5];
                returnValue[0] = cmd.Parameters["O_INVOICE"].Value;
                returnValue[1] = cmd.Parameters["O_TML_CD"].Value;
                returnValue[2] = cmd.Parameters["O_TML_NM"].Value;
                returnValue[3] = cmd.Parameters["O_RTN_CD"].Value;
                returnValue[4] = cmd.Parameters["O_RTN_MSG"].Value;

                //double t1 = 0;
                //if(double.TryParse(returnValue[3].ToString(), out t1 ))
                //{
                //    double test1234 = t1;
                //}
                //else
                //{
                    
                //}
                //string wewewe = returnValue[3].ToString();
                //O_RTN_CD가 1이 아니면 송장 정보 가져오는데 에러 발생한 것 이므로 return
                if (Convert.ToInt32(returnValue[3].ToString()) != 1) throw new Exception(returnValue[4].ToString());


                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State.ToString() == "Open")
                    con.Close();
            }
        }


        public void executeNonQuery(DataTable data)
        {
            
            OracleCommand cmd = new OracleCommand();

            con.Open();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("창고코드", typeof(string));
            //dt.Columns.Add("창고명", typeof(string));
            //dt.Columns.Add("구분", typeof(string));

            //for(int i = 0; i< 10; i++)
            //{
            //    DataRow drow = dt.NewRow();
            //    drow["창고코드"] = "TEST" + i;
            //    drow["창고명"] = "TESTBOWOO";
            //    drow["구분"] = i.ToString();
            //    dt.Rows.Add(drow);
            //}
            for(int i = 0; i< data.Columns.Count; i++)
            {
                Console.WriteLine(data.Columns[i].ColumnName);
            }
            using (OracleBulkCopy bulkcopy = new OracleBulkCopy(con))
            {
                bulkcopy.DestinationTableName = "창고정보";
                bulkcopy.WriteToServer(data);
            }

            cmd.Connection = con;

            //cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
