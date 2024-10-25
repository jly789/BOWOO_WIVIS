using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace Sorer_Indicator_Contorl
{
    class Scaner_dataSelectDAO
    {
        MssqlDBConnect mssql = null;

        SqlConnection con = null;
        MssqlDBCon conn = new MssqlDBCon();
        public Scaner_dataSelectDAO()
        {
            mssql = MssqlDBConnect.getInstance();
        }

        public DataTable select()
        {
            DataTable dt = new DataTable();
            try
            {
                con = conn.connection_open();
                //con = mssql.connection_open();
                string ssql = null;
                SqlCommand cmd = new SqlCommand();

                //SqlDataReader reader = null;
                ssql = @"select 1 as status,count(STATUS)  as count
	                from CJ_SCAN_LIST
	                where STATUS = 1
	                and SCAN_DAY = '" + DateTime.Now.ToString("yyyy-MM-dd") + @"'
	                group by STATUS
	                union(
	                select 2 as status,count(STATUS)  as count
	                from CJ_SCAN_LIST
	                where STATUS = 2
	                and SCAN_DAY = '" + DateTime.Now.ToString("yyyy-MM-dd") + @"'
	                group by STATUS
	                )";

                cmd.Connection = con;
                cmd.CommandText = ssql;


                SqlDataAdapter adpter = new SqlDataAdapter(cmd);
                adpter.Fill(dt);

                cmd.Dispose();
                //reader.Close();
                mssql.connection_close(con);

                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
            

        }
    }
}
