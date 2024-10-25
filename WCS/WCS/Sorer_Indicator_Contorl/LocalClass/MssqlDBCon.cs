using System.Collections.Generic;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using bowoo.Framework.common;
using System.Data;
using System;


namespace Sorer_Indicator_Contorl
{
    class MssqlDBCon
    {
        
        // bowoo db 접속을 위한 정보
        string ip = null;
        string id = null;
        string password = null;
        string database = null;
        string connectionstring = string.Empty;
        SqlConnection con;

        private static MssqlDBCon _Instance;

        //static public MssqlDBConnect getInstance()
        //{
        //    if (_Instance == null)
        //        _Instance = new MssqlDBConnect();
        //    return _Instance;
        //}

        public MssqlDBCon()
        {
            if (connectionstring == "")
            {
                connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["DataBaseInformation"].ToString();
                TripleDESCrypto.CryptoUtil ENC = new TripleDESCrypto.CryptoUtil();
            }
            con = new SqlConnection(connectionstring);
        }

        public SqlConnection connection_open()
        {
            //string con_status = string.Format("server={0};uid={1};pwd={2};database={3};Min Pool Size=20;",ip,id,password,database);
            try
            {
                int wewqe = con.ConnectionTimeout;
                con.Open();
                return con;
            }
            catch(SqlException sqlex)
            {
                return con;

            }
            catch (Exception ex)
            {
                return con;
            }
        }

        public void connection_close(SqlConnection con)
        {
            try
            {
                con.Close();
            }
            catch (Exception ex)
            {

            }
            
        }






    }
}
