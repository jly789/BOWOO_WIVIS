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
    class OrderTable_UpdateDAO
    {

        //선택한 제품코드를 Action 폼으로 넘기기위한 델리게이트
        

        MssqlDBConnect mssql = null;

        SqlConnection con = null;

        public OrderTable_UpdateDAO()
        {
            mssql = MssqlDBConnect.getInstance();
            //mssql = new MssqlDBConnect(ip, id, password, database);
        }

        public void update_output(DataTable temp_datatable)
        {
            con = mssql.connection_open();
            string ssql = string.Empty;
            SqlBulkCopy bulkcopy = new SqlBulkCopy(con);
            bulkcopy.DestinationTableName = "TEMP_WORK_TABLE";
            bulkcopy.WriteToServer(temp_datatable);

            //ssql = @" update temp_location set group_no = b.GROUP_NO 
			         // from temp_location as a 
			         // join (select * from wr_box_information) as b on a.box_no = b.BOX_NO

		          //    update TEMP_LOCATION set group_no = 0 where box_no = 0
		          //    update TEMP_LOCATION set status = 1

            //          merge into temp_location as a
            //          using(select * from wr_box_output) as b
            //          on a.box_no = b.box_no
            //          when matched  and b.outstatus = 2 then
            //            update set a.status = 2;

            //          merge into wr_location as a
            //          using (select * from temp_location) as b 
            //          on a.row = b.row and a.field = b.field
            //          when matched then
            //            update set a.row = b.row, a.field = b.field, a.box_no = b.box_no, a.status = b.status, a.group_no = b.group_no;

            //          insert into TS_BOX_INPUT_delete select box_no,group_no,group_status,row,getdate(),getdate(),1,0 
            //                                            from wr_box_input where box_no in (select box_no from wr_location) and status = 2

            //          delete wr_box_input where box_no in (select box_no from wr_location where classification = 'line' and line_no = 1) and status = 2 
            //          delete wr_box_output where box_no not in (select box_no from wr_location where classification = 'line' and line_no = 1) and status = 2
            //          delete wr_plc_outorder where box_no not in(select box_no from wr_location where classification = 'line' and line_no = 1)

            //          select * from wr_location 
            //          select * from wr_box_output 
            //          select * from wr_box_input ";




            //using (SqlCommand cmd = new SqlCommand(ssql, con))
            //{
            //    cmd.CommandText = ssql;
            //    cmd.Connection = con;
            //    cmd.ExecuteNonQuery();
            //}

            mssql.connection_close(con);
        }

    }
}
