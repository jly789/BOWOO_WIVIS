using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using bowoo.Framework.common;
using lib.Common.Management;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Linq;
using System.Diagnostics;
using System.Xml;
using System.Drawing.Printing;
using Telerik.WinControls.RichTextBox.Lists;
using Telerik.Windows.Diagrams.Core;
using lib.Common;
using System.Reflection;
using System.Data.SqlClient;

namespace Sorer_Indicator_Contorl
{
    public partial class Action : lib.Common.Management.BaseForm
    {

        DataSet chute_info = null; //db에서 가지고온 슈트 정보 저장.
        //lib.Common.Management.WivisOracle wivisOracle = new lib.Common.Management.WivisOracle(System.Configuration.ConfigurationManager.AppSettings["OracleDatabaseip"].ToString());
        lib.Common.Management.WivisOracle wivisOracle = null;


        //동기화를 위한 락 변수
        static private object lock_Receive = new object();
        static private object lock_Re_rc = new object();
        static private object lock_instruction = new object();
        static private object lock_confrm_cell = new object();

        int test = 1;
        string err_cdoe = string.Empty;
        //크로스 쓰레드를 방지 하기 위함.
        delegate void SetTextCallback();
        delegate void RadGridUpdate(List<int> indicatorUpdateNeedZoneList, int zone_no); //현황판 업데이트
        delegate void RadGridUpdateConfrmCell(int row_number, string send_string); //현황판 업데이트 셀표시기에서 눌렀을때
        delegate void crossTheread_Line_Qty(int item_qty, Label la_control); // 라인별 작업수량 라벨 컨트롤러 변경.
        delegate void crossTheread_Barcode(string barcode); // 작업수량 라벨 컨트롤러 변경

        string sp_Load_indicator_Location = "IF_SP_LOCATION_SELECT";
        string sp_Load_Work_Data_Select = "IF_SP_DAS_SELECT";
        string sp_Box_End = "IF_SP_BOX_END"; //박스 마감.
        string sp_Chute_Cnt = "IF_SP_CHUTE_CNT";
        StatusBoard statusBoard = null;
        string work_type = string.Empty; // 작업타입


        Thread work_thred = null;
        string sysCD = string.Empty;


        DataSet work_dataSet = null;

        //das 작업 데이터
        string bundleCode = string.Empty; //번들 정보 저장하는 변수.
        List<Sorer_Indicator_Contorl.DPSClass.DasOrdClass> dasWorkList = null;




        //현황판에 작업 수량 표시 할떄 db를 매번 조회 하지 않게 하기위함.
        JbController[] jb_controller = null;

        string biz_day = string.Empty;
        int total_jb = 0;

        //Dictionary<string, Dictionary<int, object>> work_dictionary = new Dictionary<string, Dictionary<int, object>>();
        Dictionary<string, DPSClass.indicator_Class> serch_indicator = null; //어드래스로 해당 표시기 클래스를 바로 찾아가기 위함.

        public Action()
        {
            InitializeComponent();
            MssqlDBConnect.getInstance();

        }
        private void Action_Load(object sender, EventArgs e)
        {

            MakeSessionInq();
            this.PageSetting();
            (this.UsrCalWork.Controls.Find("radDatePickerCal", true)[0] as RadDateTimePicker).ValueChanged += Change_WorkSeq;
            stopBT.Enabled = false;


            Dictionary<string, string> type = new Dictionary<string, string>();
            type.Add("S", "S");
            type.Add("D", "D");

            comboBox1.DataSource = new BindingSource(type, null);
            comboBox1.DisplayMember = "value";
            comboBox1.ValueMember = "key";
            comboBox1.SelectedItem = "S"; //처음 로딩시 콤보박스 기본선택을 전체로 하기 위함.


        }


        private void Change_WorkSeq(object sender, EventArgs e)
        {

            string sessInq = string.Empty;
            sessInq += string.Format(";#bizday={0}", this.UsrCalWork.DefaultDate.Replace("-", string.Empty));
            BaseEntity.sessInq = sessInq;
        }


        private void Action_FormClosed(object sender, FormClosedEventArgs e)
        {

            //실행중인 쓰레들 일괄종료
            Environment.Exit(Environment.ExitCode);
        }


        //500[ms]마다 DB검색.
        private void timer_tick()
        {
            while (true)
            {
                Thread.Sleep(500);
                try
                {
                    Dictionary<string, object> paramStatusInfoYN = new Dictionary<string, object>();
                    paramStatusInfoYN["@V_BIZ_DAY"] = biz_day;
                    //paramStatusInfoYN["@V_DISP_YN"] = "Y";
                    paramStatusInfoYN["@V_DISP_YN"] = "Y";
                    work_dataSelect(paramStatusInfoYN);
                    //makeLog("타이머 오류?", false, "ㅅㄷㄴㅅ");

                }
                catch (Exception ex)
                {
                    makeLog("타이머 오류?", false, ex.Message);
                    lib.Common.Log.LogFile.WriteLog(ex.Message);
                }
            }
        }



        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                serch_indicator = new Dictionary<string, DPSClass.indicator_Class>();

                //jp 연결
                for (int i = 0; i < total_jb; i++)
                {
                    bool con_check = jb_controller[i].jb_open();
                    if (!con_check)
                    {
                        MessageBox.Show((i + 1) + "번 jb 연결이 되지 않습니다. 네트워크 상태를 확인해 주세요.");
                        return;
                    }
                    jb_controller[i].all_off();
                }

                //표시기 로케이션 가지고옴.
                DataSet tempSet = DBUtil.ExecuteDataSet(sp_Load_indicator_Location, CommandType.StoredProcedure);
                Matching matchingClass = new Matching();
                matchingClass.initmatch(tempSet, ref serch_indicator);

                biz_day = string.Format("{0}", this.UsrCalWork.DefaultDate.Replace("-", string.Empty));

                work_type = comboBox1.SelectedValue.ToString();

                //처음 시작시 전체 슈트에대한 정보 조회후 뿌리기.
                Dictionary<string, object> paramStatusInfoYN = new Dictionary<string, object>();
                paramStatusInfoYN["@V_BIZ_DAY"] = biz_day;
                paramStatusInfoYN["@V_DISP_YN"] = "Y,N";
                work_dataSelect(paramStatusInfoYN);


                if (work_type.Equals("S"))
                {
                    work_thred = new Thread(new ThreadStart(timer_tick));
                    work_thred.Start();
                }
                else
                {

                    var bcr_addr = serch_indicator.Where(r => r.Value.kit_type == "BCR").Select(r => r.Value.addr);

                    //바코드 스캐너 표시기 오픈
                    foreach (var i in bcr_addr)
                    {
                        int jb_number = Convert.ToInt32(i.Substring(0, 1)) - 1;
                        jb_controller[jb_number].bcr_open(i.Substring(1, 4));
                    }

                    dasWorkList = new List<DPSClass.DasOrdClass>();

                    //das일떄만 enable 시킴
                    textBox2.Enabled = true;
                    bu_das_cancel.Enabled = true;
                }
                statrBT.Enabled = false;
                stopBT.Enabled = true;
                comboBox1.Enabled = false;
                UsrCalWork.Enabled = false;


                statusBoard = new StatusBoard(biz_day);
                statusBoard.Show();
            }
            catch (Exception ex)
            {
                //monitoring.reciveClear();
                //monitoring.timerHandle(false);
            }
        }

        /// <summary>
        /// DB에서 데이터 조회후 표시기에 지시.
        /// </summary>
        /// <param name="pram"></param> 날짜, das_update에서 where 조건.
        private void work_dataSelect(Dictionary<string, object> pram)
        {
            //db에서 조회.
            chute_info = DBUtil.ExecuteDataSet(sp_Load_Work_Data_Select, pram, CommandType.StoredProcedure);

            if (work_type == "D")
            {
                foreach (DataRow dr in chute_info.Tables[0].Rows)
                {
                    int jb_no = Convert.ToInt32(dr["addr"].ToString().Substring(0, 1)) - 1;
                    string addr = dr["addr"].ToString().Substring(1, 4);
                    string addr_full = dr["addr"].ToString();
                    //int status = Convert.ToInt32(dr["status"]); //작업 구분 1 : 다스 작업지시, 2 : 검수 모드

                    DPSClass.indicator_Class indicatorClass = serch_indicator[addr_full];

                    //셀표시기
                    if (indicatorClass is DPSClass.CellBtn_Class cell)
                    {
                        if (Convert.ToInt32(dr["WRK_QTY"]) + Convert.ToInt32(dr["ORDQTY"]) + Convert.ToInt32(dr["ONQTY"]) == 0) //작업이 완료된 슈트는 패스
                        {
                            continue;
                        }
                        cell.remain_qty = Convert.ToInt32(dr["ordqty"]); //남은 작업 수량
                        cell.box_no = Convert.ToInt32(dr["box_no"]);

                        cell.display = cell.box_no.ToString().Right(1)
                                         + "00"
                                         + cell.remain_qty.ToString().PadLeft(3, '0');

                        instruction_das(cell, 0);
                    }

                }
            }
            else
            {
                instruction_soter(chute_info.Tables[0]);
            }

        }

        private void stopBT_Click(object sender, EventArgs e)
        {
            //jp 연결
            for (int i = 0; i < total_jb; i++)
            {
                jb_controller[i].all_off();
            }

            //바코드 인터페이스 클로즈
            var bcr_addr = serch_indicator.Where(r => r.Value.kit_type == "BCR").Select(r => r.Value.addr);
            foreach (var i in bcr_addr)
            {
                int jb_number = Convert.ToInt32(i.Substring(0, 1)) - 1;
                jb_controller[jb_number].bcr_close(i.Substring(1, 4));
            }


            for (int i = 0; i < total_jb; i++)
            {
                jb_controller[i].all_off();
                jb_controller[i].jb_close();
            }

            crossThread_Line_qty(0, la_das_D);//D라인 작업수량
            crossThread_Line_qty(0, la_das_C);//C라인 작업수량
            crossThread_Line_qty(0, la_das_A);//A라인 작업수량
            crossThread_Line_qty(0, la_das_B);//B라인 작업수량
            crossThread_Barcode("");

            statusBoard.Close();
            statrBT.Enabled = true;
            stopBT.Enabled = false;
            comboBox1.Enabled = true;

            textBox2.Enabled = false;
            bu_das_cancel.Enabled = false;
            UsrCalWork.Enabled = true;

            if (work_type.Equals("S"))
            {
                work_thred.Abort();
            }
            else
            {

                dasWorkList.Clear();
            }
        }


        private void instruction_soter(DataTable workTable)
        {
            lock (lock_instruction)
            {
                int jb_no = 0;
                int work_qty = 0;
                string cell_Id, display, addr;
                string buzzer = string.Empty;
                string buzzer_cycle = string.Empty;
                string lc_color = string.Empty;
                string lc_cycle = string.Empty;
                string lc_status = string.Empty;
                string fn_color = string.Empty;
                string fn_cycle = string.Empty;
                string fn_status = string.Empty;
                string ord_no = string.Empty;

                DataRow[] tempRow = workTable.Select();
                IndicatorOrderClass indicatorOrderClass = null;

                //검색된 데이터가 있을때. 없으면 바로 매소드를 빠져나감.
                if (tempRow.Length > 0)
                {
                    for (int i = 0; i < tempRow.Length; i++)
                    {
                        indicatorOrderClass = new IndicatorOrderClass();

                        //work_ord,work_on,work_cfm에 데이터가 없을 경우 표시기를 다시 키지 않기 위해
                        //현재 row데이터는 continue시킴
                        //if (Convert.ToInt32(tempRow[i]["WRK_QTY"]) + Convert.ToInt32(tempRow[i]["ORDQTY"]) + Convert.ToInt32(tempRow[i]["ONQTY"]) == 0)
                        //{
                        //    continue;
                        //}
                        indicatorOrderClass.cell_Id = tempRow[i]["addr"].ToString().Substring(1, 4);
                        jb_no = Convert.ToInt32(tempRow[i]["jb_no"]) - 1;
                        //슈트 맥스수량에서 work_cfm에 있는 수량의 차를 구함.
                        int remain_qty = Convert.ToInt32(tempRow[i]["MAXQTY"]) - Convert.ToInt32(tempRow[i]["WRK_QTY"]);
                        int summary = Convert.ToInt32(tempRow[i]["ORDQTY"]) + Convert.ToInt32(tempRow[i]["ONQTY"]);

                        //해당 아이템 작업을 안했을때 buzeer on, 버튼 점멸,응답모드
                        indicatorOrderClass.buzzer = "0";
                        indicatorOrderClass.buzzer_cycle = "0";
                        string box_no = tempRow[i]["box_no"].ToString().Length >= 3 ? tempRow[i]["box_no"].ToString().Right(2) : tempRow[i]["box_no"].ToString().PadLeft(2, '0');

                        if (tempRow[i]["work_type"].ToString() == "4") // 자체 정리
                        {
                            //if (Convert.ToInt32(tempRow[i]["WRK_QTY"]) + Convert.ToInt32(tempRow[i]["ORDQTY"]) + Convert.ToInt32(tempRow[i]["ONQTY"]) == 0)
                            //{
                            //    indicatorOrderClass.display = "      ";
                            //}
                            //else
                            //{

                            //    //indicatorOrderClass.display = " "
                            //    //                        + Convert.ToInt32(tempRow[i]["box_no"]).ToString().PadLeft(2, '0')
                            //    //                        + (Convert.ToInt32(tempRow[i]["WRK_QTY"]) > 999 ? "---" : tempRow[i]["WRK_QTY"].ToString().PadLeft(3, '0'));


                            //    indicatorOrderClass.display = " "
                            //                            + box_no
                            //                            + (Convert.ToInt32(tempRow[i]["WRK_QTY"]) > 999 ? "---" : tempRow[i]["WRK_QTY"].ToString().PadLeft(3, '0'));
                            //}

                            indicatorOrderClass.display = " "
                                                    + box_no
                                                    + (Convert.ToInt32(tempRow[i]["WRK_QTY"]) > 999 ? "---" : tempRow[i]["WRK_QTY"].ToString().PadLeft(3, '0'));
                        }
                        else if (tempRow[i]["work_type"].ToString() == "2" || tempRow[i]["work_type"].ToString() == "5") //주문출고
                        {

                            if (Convert.ToInt32(tempRow[i]["WRK_QTY"]) + Convert.ToInt32(tempRow[i]["ORDQTY"]) + Convert.ToInt32(tempRow[i]["ONQTY"]) == 0)
                            {
                                indicatorOrderClass.display = "      ";
                            }
                            else
                            {

                                //indicatorOrderClass.display = " "
                                //                            + Convert.ToInt32(tempRow[i]["box_no"]).ToString().PadLeft(2, '0')
                                //                            + (summary > 999 ? "---" : summary.ToString().PadLeft(3, '0'));



                                indicatorOrderClass.display = " "
                                                            + box_no
                                                            + (summary > 999 ? "---" : summary.ToString().PadLeft(3, '0'));

                            }
                        }
                        else
                        {
                            continue;
                        }

                        if (remain_qty <= 0 && tempRow[i]["work_type"].ToString() != "4") //설정된 만재 수량과 work_cfm떨어진 수량이 같을때.
                        {
                            indicatorOrderClass.lc_color = "GREEN";
                            indicatorOrderClass.lc_cycle = "1";
                            indicatorOrderClass.lc_status = "1";

                            indicatorOrderClass.fn_color = "GREEN";
                            indicatorOrderClass.fn_cycle = "3";
                            indicatorOrderClass.fn_status = "0";

                            indicatorOrderClass.buzzer = "1";
                            indicatorOrderClass.buzzer_cycle = "1";
                        }
                        else if (tempRow[i]["FULL_YN"].ToString() == "Y") //만재 센서에의해 슈트가 풀이 되었을 경우. 녹색으로 표시
                        {
                            indicatorOrderClass.lc_color = "RED";
                            indicatorOrderClass.lc_cycle = "1";
                            indicatorOrderClass.lc_status = "1";

                            indicatorOrderClass.fn_color = "RED";
                            indicatorOrderClass.fn_cycle = "3";
                            indicatorOrderClass.fn_status = "0";
                        }
                        else if (tempRow[i]["END_BOX"].ToString() == "Y" && summary != 0) //만재 센서에의해 슈트가 풀이 되었을 경우. 녹색으로 표시
                        {
                            indicatorOrderClass.lc_color = "GREEN";
                            indicatorOrderClass.lc_cycle = "1";
                            indicatorOrderClass.lc_status = "1";

                            indicatorOrderClass.fn_color = "GREEN";
                            indicatorOrderClass.fn_cycle = "3";
                            indicatorOrderClass.fn_status = "0";

                            //indicatorOrderClass.buzzer = "1";
                            //indicatorOrderClass.buzzer_cycle = "1";
                        }
                        //else if (summary == 0 && Convert.ToInt32(tempRow[i]["WRK_QTY"]) != 0 && tempRow[i]["work_type"].ToString() != "4")
                        else if ((tempRow[i]["END_BOX"].ToString() == "Y" || summary == 0) && Convert.ToInt32(tempRow[i]["WRK_QTY"]) != 0 && tempRow[i]["work_type"].ToString() != "4")
                        {
                            indicatorOrderClass.lc_color = "GREEN";
                            indicatorOrderClass.lc_cycle = "1";
                            indicatorOrderClass.lc_status = "1";

                            indicatorOrderClass.fn_color = "GREEN";
                            indicatorOrderClass.fn_cycle = "3";
                            indicatorOrderClass.fn_status = "0";

                            indicatorOrderClass.buzzer = "1";
                            indicatorOrderClass.buzzer_cycle = "1";

                        }
                        else //슈트풀도 아니고 풀수량만큼 작업이 안되었을때.
                        {
                            indicatorOrderClass.lc_color = "0";
                            indicatorOrderClass.lc_cycle = "0";
                            indicatorOrderClass.lc_status = "1";

                            indicatorOrderClass.fn_color = "GREEN";
                            indicatorOrderClass.fn_cycle = "0";
                            indicatorOrderClass.fn_status = "0";
                        }
                        jb_controller[jb_no].Product_forwarding(indicatorOrderClass);
                    }
                }

            }

        }

        /// <summary>
        /// das 표시기 제어
        /// </summary>
        /// <param name="indicatorClass"> 표시기 정보</param>
        /// <param name="status"> 0이면 잔량 표시, 1이면 작업 지시</param>
        private void instruction_das(DPSClass.indicator_Class indicatorClass, int status)
        {
            lock (lock_instruction)
            {
                int jb_no = 0;
                string buzzer = string.Empty;
                string buzzer_cycle = string.Empty;
                string lc_color = string.Empty;
                string lc_cycle = string.Empty;
                string lc_status = string.Empty;
                string fn_color = string.Empty;
                string fn_cycle = string.Empty;
                string fn_status = string.Empty;
                string ord_no = string.Empty;

                IndicatorOrderClass indicatorOrderClass = null;

                indicatorOrderClass = new IndicatorOrderClass();



                indicatorOrderClass.cell_Id = indicatorClass.addr.Substring(1, 4);
                jb_no = Convert.ToInt32(indicatorClass.addr.Substring(0, 1)) - 1;



                //해당 아이템 작업을 안했을때 buzeer on, 버튼 점멸,응답모드
                indicatorOrderClass.buzzer = "0";
                indicatorOrderClass.buzzer_cycle = "0";

                if (indicatorClass.kit_type == "CELL")
                {
                    //DPSClass.CellBtn_Class indcator = indicatorClass as DPSClass.CellBtn_Class;


                    //indicatorOrderClass.display = " "
                    //                            + indcator.box_no.ToString().PadLeft(2, '0')
                    //                            + indcator.item_qty.ToString().PadLeft(3, '0');

                    indicatorOrderClass.display = indicatorClass.display;
                    if (status == 1) //das 작업 지시
                    {
                        indicatorOrderClass.lc_color = "GREEN";
                        indicatorOrderClass.lc_cycle = "1";
                        indicatorOrderClass.lc_status = "0";

                        indicatorOrderClass.fn_color = "GREEN";
                        indicatorOrderClass.fn_cycle = "3";
                        indicatorOrderClass.fn_status = "0";

                        indicatorOrderClass.buzzer = "1";
                        indicatorOrderClass.buzzer_cycle = "1";
                    }
                    else if (status == 2) //검수모드
                    {

                        indicatorOrderClass.lc_color = "GREEN";
                        indicatorOrderClass.lc_cycle = "3";
                        indicatorOrderClass.lc_status = "0";

                        indicatorOrderClass.fn_color = "GREEN";
                        indicatorOrderClass.fn_cycle = "3";
                        indicatorOrderClass.fn_status = "1";
                    }
                    else //das 잔량 표시
                    {

                        indicatorOrderClass.lc_color = "0";
                        indicatorOrderClass.lc_cycle = "0";
                        indicatorOrderClass.lc_status = "1";

                        indicatorOrderClass.fn_color = "GREEN";
                        indicatorOrderClass.fn_cycle = "0";
                        indicatorOrderClass.fn_status = "0";
                    }
                }
                else if (indicatorClass.kit_type == "ZONE")
                {
                    if (status == 1) // 작업지시
                    {

                        indicatorOrderClass.display = indicatorClass.display;

                        indicatorOrderClass.lc_color = "GREEN";
                        indicatorOrderClass.lc_cycle = "3";
                        indicatorOrderClass.lc_status = "0";

                        indicatorOrderClass.fn_color = "GREEN";
                        indicatorOrderClass.fn_cycle = "3";
                        indicatorOrderClass.fn_status = "1";

                        indicatorOrderClass.buzzer = "0";
                        indicatorOrderClass.buzzer_cycle = "0";
                    }
                    else if (status == 0) // 작업 완료
                    {
                        indicatorOrderClass.display = indicatorClass.display;

                        indicatorOrderClass.lc_color = "0";
                        indicatorOrderClass.lc_cycle = "0";
                        indicatorOrderClass.lc_status = "1";

                        indicatorOrderClass.fn_color = "GREEN";
                        indicatorOrderClass.fn_cycle = "3";
                        indicatorOrderClass.fn_status = "1";

                        indicatorOrderClass.buzzer = "0";
                        indicatorOrderClass.buzzer_cycle = "0";
                    }

                }
                else if (indicatorClass.kit_type == "DIS16")
                {
                    jb_controller[jb_no].Product_DisPlay16(indicatorOrderClass.cell_Id, indicatorClass.display);
                }


                if (indicatorClass.kit_type != "DIS16")
                {
                    jb_controller[jb_no].Product_forwarding(indicatorOrderClass);
                }




            }

        }


        private void jbMsg_Receive(string msg)
        {
            lock (lock_Receive)
            {

                string command = msg.Substring(6, 2);
                if (command.Equals("RC"))
                {

                    confrmRC(msg);
                }
                else if (command.Equals("RF"))
                {

                    //receveData(msg, "RF");
                    confrmRF(msg);
                }
                else if (work_type == "D" && command.Equals("RB"))
                {

                    recieveRB(msg);
                    //dasWorkList
                }
            }
        }

        private void recieveRB(string msg)
        {
            //이전 작업이 남아 있을경우 스캐너 값 무시.
            if (dasWorkList.Count != 0)
            {
                return;
            }

            try
            {
                int index = msg.IndexOf("RB");
                string barcode = msg.Substring(index + 2, msg.Length - index - 2);

                if (barcode.Length == 13 && bundleCode == string.Empty) //번들 코드
                {
                    barcode = barcode;
                }
                else if (barcode.Length == 15 && bundleCode == string.Empty)
                {
                    barcode = barcode.Substring(0, 13);
                }
                else if (barcode.Length == 1)
                {
                    bundleCode = barcode;
                    return;
                    //barcode = barcode.Substring(0, 10) + bundleCode;
                }
                else if (barcode.Length != 1 && bundleCode != string.Empty)
                {

                    barcode = barcode.Substring(0, 10) + bundleCode;
                }


                SqlParameter[] parmData = new SqlParameter[2];
                parmData[0] = new SqlParameter();
                parmData[0].ParameterName = "@BIZ_DAY"; //날짜
                parmData[0].DbType = DbType.Int32;
                parmData[0].Direction = ParameterDirection.Input;
                parmData[0].Value = biz_day;

                parmData[1] = new SqlParameter();
                parmData[1].ParameterName = "@barcode"; //바코드
                parmData[1].DbType = DbType.String;
                parmData[1].Value = barcode;

                DataSet tempDataSet = DBUtil.ExecuteDataSetSqlParam("DAS_SP_WorkDATA_SELECT", parmData);
                DPSClass.DasOrdClass work_Data = new DPSClass.DasOrdClass();
                List<string> zoneIndicatorList = new List<string>(); //존별로 표시기가 몇개 있는지 확인 하는 변수.
                                                                     //int work_count = 0; //해당 제품 총 수량.

                DataTable tempTable = tempDataSet.Tables[0].Rows.Count != 0 ? tempDataSet.Tables[0] : tempDataSet.Tables[1];

                foreach (DataRow dr in tempTable.Rows)
                {
                    int jb_no = Convert.ToInt32(dr["addr"].ToString().Substring(0, 1)) - 1;
                    string addr = dr["addr"].ToString().Substring(1, 4);
                    string addr_full = dr["addr"].ToString();
                    int status = Convert.ToInt32(dr["status"]); //작업 구분 1 : 다스 작업지시, 2 : 검수 모드

                    DPSClass.indicator_Class indicatorClass = serch_indicator[addr_full];

                    //셀표시기
                    if (indicatorClass is DPSClass.CellBtn_Class cell)
                    {
                        cell.item_qty = Convert.ToInt32(dr["ord_qty"]);

                        cell.display = " "
                                    + "  "
                                    + cell.item_qty.ToString().PadLeft(3, '0');

                        zoneIndicatorList.Add(dr["line_no"].ToString());
                        cell.status = status;
                        cell.barocde = barcode;

                        instruction_das(cell, status);
                    }

                    work_Data.remainOrder.Add(addr_full);
                }

                zoneIndicatorList = zoneIndicatorList.Distinct().ToList();

                if (zoneIndicatorList.Count > 0)
                {
                    foreach (string data in zoneIndicatorList)
                    {
                        //통로 표시기 주소 검색.
                        var temp = from indicator in serch_indicator
                                   where indicator.Value.line_no == Convert.ToInt32(data)
                                   && (indicator.Value.kit_type == "ZONE" || indicator.Value.kit_type == "DIS16")
                                   select indicator.Value.addr;

                        foreach (var addr in temp)
                        {
                            //통로 표시기.
                            if (serch_indicator[addr] is DPSClass.ZoneBtn_Class zone)
                            {
                                int count = 0;
                                var result = from data1 in serch_indicator
                                             where data1.Value.line_no == Convert.ToInt32(data) && data1.Value.kit_type == "CELL"
                                             select data1.Value;

                                foreach (var cellIndicator in result)
                                {
                                    if (cellIndicator is DPSClass.CellBtn_Class cellInd)
                                    {
                                        count += cellInd.item_qty;//라인별 작업수량
                                    }

                                }

                                //표시기 제어 프로그램 작업 수량 표시
                                switch (serch_indicator[addr].line_no)
                                {
                                    case 1:
                                        crossThread_Line_qty(count, la_das_D);//D라인 작업수량
                                        break;
                                    case 2:
                                        crossThread_Line_qty(count, la_das_C);//C라인 작업수량
                                        break;
                                    case 3:
                                        crossThread_Line_qty(count, la_das_A);//A라인 작업수량
                                        break;
                                    case 4:
                                        crossThread_Line_qty(count, la_das_B);//B라인 작업수량
                                        break;
                                }
                                zone.display = count.ToString().PadLeft(5, '0') + " ";
                                zone.status = 1;
                                instruction_das(zone, zone.status);
                            }
                            else if (serch_indicator[addr] is DPSClass.Display16_Class DIS16)
                            {
                                DIS16.display = barcode;
                                DIS16.status = 1;
                                instruction_das(DIS16, DIS16.status);
                            }
                        }
                    }
                    crossThread_Barcode(barcode);
                    //int work_count = tempDataSet.Tables[0].Rows.Count; //작업 슈트 수량.
                    dasWorkList.Add(work_Data);
                }

                if (barcode.Length != 1)
                {
                    bundleCode = string.Empty;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        /// <summary>
        /// das작업시 라인별 작업수량 표시 라벨 제어
        /// </summary>
        /// <param name="item_qty"></param>
        /// <param name="la_control"></param>
        private void crossThread_Line_qty(int item_qty, Label la_control)
        {
            if (this.InvokeRequired)
            {

                crossTheread_Line_Qty d = new crossTheread_Line_Qty(crossThread_Line_qty);
                this.Invoke(d, new object[] { item_qty, la_control });
            }
            else
            {
                if (item_qty != 0)
                {
                    la_control.Text = item_qty.ToString(); //D라인 작업수량
                    la_control.BackColor = Color.Green;
                }
                else
                {
                    la_control.Text = ""; //D라인 작업수량
                    la_control.BackColor = Color.White;
                }
            }

        }
        /// <summary>
        /// das 작업시 바코드 라벨 제어
        /// </summary>
        /// <param name="barcode"></param>
        private void crossThread_Barcode(string barcode)
        {
            if (this.InvokeRequired)
            {

                crossTheread_Barcode d = new crossTheread_Barcode(crossThread_Barcode);
                this.Invoke(d, new object[] { barcode });
            }
            else
            {
                la_das_barcode.Text = barcode;
                la_das_barcode.BackColor = barcode != "" ? Color.Green : Color.White;
            }

        }

        //다스 컨펌 터치시 작업 확정.
        private void confrmRC(string msg)
        {
            lock (lock_Re_rc)
            {
                int wrk_qty = 0;
                DPSClass.indicator_Class indicatorClass = new DPSClass.indicator_Class();
                msg = msg.Trim(); //공백제거
                string addr = msg.Substring(1, 5);
                indicatorClass = serch_indicator[addr];

                if (indicatorClass is DPSClass.CellBtn_Class cell)
                {
                    DataTable tempTable = null;
                    if (cell.status == 1) //작업지시
                    {
                        wrk_qty = Convert.ToInt32(msg.Substring(11, 3)); //작업수량

                        /*20210621추가
                        수량 조절 하다가 앞에꺼 딸려서 수량조정 되어 작업 수량이 커질경우 리턴
                        원래 수량으로 다시 불을 뛰움*/
                        if (wrk_qty > cell.item_qty)
                        {
                            instruction_das(cell, cell.status);
                            return;
                        }

                        SqlParameter[] parmData = new SqlParameter[4];
                        parmData[0] = new SqlParameter();
                        parmData[0].ParameterName = "@BIZ_DAY"; //작업 날짜
                        parmData[0].DbType = DbType.Int32;
                        parmData[0].Direction = ParameterDirection.Input;
                        parmData[0].Value = biz_day;

                        parmData[1] = new SqlParameter();
                        parmData[1].ParameterName = "@BARCODE"; //바코드
                        parmData[1].Size = 20;
                        parmData[1].DbType = DbType.String;
                        parmData[1].Direction = ParameterDirection.Input;
                        parmData[1].Value = cell.barocde;

                        parmData[2] = new SqlParameter();
                        parmData[2].ParameterName = "@CHUTE_NO"; // 슈트번호
                        parmData[2].DbType = DbType.Int32;
                        parmData[2].Direction = ParameterDirection.Input;
                        parmData[2].Value = cell.loc_no;

                        parmData[3] = new SqlParameter();
                        parmData[3].ParameterName = "@WRK_QTY"; //작업 갯수
                        parmData[3].DbType = DbType.Int32;
                        parmData[3].Value = wrk_qty;
                        parmData[3].Direction = ParameterDirection.Input;

                        tempTable = DBUtil.ExecuteDataSetSqlParam("DAS_SP_WorkDATA_confrm", parmData).Tables[0];
                        cell.remain_qty = Convert.ToInt32(tempTable.Rows[0]["ordqty"]); //남은 수량 업데이트
                    }

                    /*20210621 추가
                     수량 조정 해서 컨펌 눌렀을 경우*/
                    if (wrk_qty < cell.item_qty)
                    {
                        cell.item_qty = cell.item_qty - wrk_qty;

                        cell.display = " "
                                    + "  "
                                    + cell.item_qty.ToString().PadLeft(3, '0');

                        instruction_das(cell, cell.status);
                    }
                    else // 수량 조정 안하고 보냈을 경우.
                    {
                        string remain_qty = cell.remain_qty > 999 ? "999" : cell.remain_qty.ToString().PadLeft(3, '0');
                        string work_qty = cell.item_qty > 99 ? "--" : cell.item_qty.ToString().PadLeft(2, '0');

                        cell.display = cell.box_no.ToString().Right(1) + work_qty + remain_qty;
                        cell.status = 0;

                        instruction_das(cell, cell.status);
                        //cell.display = "";
                        cell.barocde = "";
                        cell.item_qty = 0;
                        dasWorkList[0].remainOrder.Remove(addr);
                        dasWorkList[0].indicateOrder.Add(addr);
                    }

                    //instruction_das(cell, cell.status);

                    if (dasWorkList[0].remainOrder.Count == 0) // 지시한 작업이 다 끝났을때.
                    {
                        //통로 표시기 주소 셀렉트, 작업표시를한 통로 표시기 주소만 가지고옴.
                        var result = from data in serch_indicator
                                     where (data.Value.kit_type == "ZONE" || data.Value.kit_type == "DIS16") && data.Value.status != 0
                                     select data.Value.addr;

                        //통로 표시기 불끔.
                        foreach (var data in result)
                        {
                            if (serch_indicator[data] is DPSClass.ZoneBtn_Class zone)
                            {
                                zone.display = "      ";
                                zone.status = 0;
                                instruction_das(zone, zone.status);
                            }
                            else if (serch_indicator[data] is DPSClass.Display16_Class DIS16)
                            {
                                DIS16.display = "";
                                DIS16.status = 0;
                                instruction_das(DIS16, DIS16.status);
                            }
                        }

                        for (int i = 0; i < dasWorkList[0].indicateOrder.Count; i++)
                        {
                            string tempCellAddr = dasWorkList[0].indicateOrder[i];
                            if (serch_indicator[tempCellAddr] is DPSClass.CellBtn_Class tempcell)
                            {
                                tempcell.display = tempcell.box_no.ToString().Right(1) + "  " + tempcell.remain_qty;
                                instruction_das(tempcell, 0);
                            }
                        }
                        dasWorkList.Clear();

                        crossThread_Line_qty(0, la_das_D);//D라인 작업수량
                        crossThread_Line_qty(0, la_das_C);//C라인 작업수량
                        crossThread_Line_qty(0, la_das_A);//A라인 작업수량
                        crossThread_Line_qty(0, la_das_B);//B라인 작업수량
                        crossThread_Barcode("");
                    }
                }
                else if (indicatorClass is DPSClass.ZoneBtn_Class zone)
                {
                    das_work_cancel();
                }

            }

        }


        /// <summary>
        /// das작업 취소
        /// </summary>
        private void das_work_cancel()
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            if (dasWorkList.Count > 0)
            {

                //통로 표시기 주소 셀렉트, 작업표시를한 통로 표시기 주소만 가지고옴.
                var result = from data in serch_indicator
                             where (data.Value.kit_type == "ZONE" || data.Value.kit_type == "DIS16") && data.Value.status != 0
                             select data.Value.addr;

                // 슈트 표시기 종료
                List<string> list = dasWorkList[0].remainOrder.ToList();
                foreach (string data in list)
                {
                    if (serch_indicator[data] is DPSClass.CellBtn_Class cellClass)
                    {
                        string loc_no = cellClass.loc_no;
                        string remain_qty = string.Empty; // 작업 잔량
                        string work_qty = string.Empty; //작업수량


                        remain_qty = cellClass.remain_qty > 999 ? "999" : cellClass.remain_qty.ToString().PadLeft(3, '0');
                        work_qty = "00";

                        cellClass.display = cellClass.box_no.ToString().Right(1) + work_qty + remain_qty;

                        instruction_das(cellClass, 0);

                        cellClass.status = 0;
                        //cellClass.display = "";
                        cellClass.barocde = "";
                        cellClass.item_qty = 0;
                    }
                }

                //통로 표시기 종료.
                foreach (var data in result)
                {
                    if (serch_indicator[data] is DPSClass.ZoneBtn_Class zone1)
                    {
                        zone1.display = "      ";
                        zone1.status = 0;
                        instruction_das(zone1, zone1.status);
                    }
                    else if (serch_indicator[data] is DPSClass.Display16_Class DIS16)
                    {
                        DIS16.display = "";
                        DIS16.status = 0;
                        instruction_das(DIS16, DIS16.status);
                    }
                }

                crossThread_Line_qty(0, la_das_D);//D라인 작업수량
                crossThread_Line_qty(0, la_das_C);//C라인 작업수량
                crossThread_Line_qty(0, la_das_A);//A라인 작업수량
                crossThread_Line_qty(0, la_das_B);//B라인 작업수량
                crossThread_Barcode("");

                //작업이 완료되지 않은 슈트 잔량 표시 하기위함.


                dasWorkList.Clear();
            }
            //sw.Stop();
            //Console.WriteLine("ㅈㅏㄱㅇㅓㅂ ㄱㅓㄹㄹㅣㄴ ㅅㅣㄱㅏㄴ : " + sw.ElapsedMilliseconds);
        }

        private void confrmRF(string msg)
        {
            lock (lock_confrm_cell)
            {
                try
                {
                    DPSClass.indicator_Class indicatorClass = new DPSClass.indicator_Class();
                    msg = msg.Trim(); //공백제거
                    indicatorClass = serch_indicator[msg.Substring(1, 5)];
                    SqlParameter[] parmData = null;

                    switch (msg.Right(1))
                    {
                        case "-": //박스 마감
                            string[] printString = null; //라벨 출력할 데이터를 담는 변수.


                            lib.Common.Management.Class.RfidSend wivis = new lib.Common.Management.Class.RfidSend();
                            object[] returnvalue = wivis.invoiceSerch(Convert.ToInt32(indicatorClass.loc_no));

                            //값이 null 일경우는 한진이 아닌 경우
                            if (returnvalue == null)
                            {
                                returnvalue = new object[5];
                                returnvalue[0] = ""; // 송장 번호
                                returnvalue[1] = ""; // 터미널 코드
                                returnvalue[2] = ""; // 터미널 명
                            }

                            parmData = new SqlParameter[10];
                            parmData[0] = new SqlParameter();
                            parmData[0].ParameterName = "@V_CHUTE_NO"; //슈트번호
                            parmData[0].DbType = DbType.Int32;
                            parmData[0].Direction = ParameterDirection.Input;
                            parmData[0].Value = indicatorClass.loc_no;

                            parmData[1] = new SqlParameter();
                            parmData[1].ParameterName = "@V_WORK_DAY"; //작업 날짜
                            parmData[1].DbType = DbType.String;
                            parmData[1].Value = biz_day;

                            parmData[2] = new SqlParameter();
                            parmData[2].ParameterName = "@V_SYSCODE";
                            parmData[2].Size = 10;
                            parmData[2].DbType = DbType.String;
                            parmData[2].Value = "ST";

                            parmData[3] = new SqlParameter();
                            parmData[3].ParameterName = "@V_INVOICE";
                            parmData[3].Size = 50;
                            parmData[3].DbType = DbType.String;
                            parmData[3].Value = returnvalue[0].ToString();

                            parmData[4] = new SqlParameter();
                            parmData[4].ParameterName = "@V_TML_CD";
                            parmData[4].Size = 20;
                            parmData[4].DbType = DbType.String;
                            parmData[4].Value = returnvalue[1].ToString();

                            parmData[5] = new SqlParameter();
                            parmData[5].ParameterName = "@V_TML_NM";
                            parmData[5].Size = 50;
                            parmData[5].DbType = DbType.String;
                            parmData[5].Value = returnvalue[2].ToString();

                            parmData[6] = new SqlParameter();
                            parmData[6].ParameterName = "@O_MSG"; //정상 유무
                            parmData[6].Size = 8000;
                            parmData[6].Value = "";
                            parmData[6].Direction = ParameterDirection.Output;

                            parmData[7] = new SqlParameter();
                            parmData[7].ParameterName = "@O_TYPE"; //헤더파라미터
                            parmData[7].DbType = DbType.Int32;
                            parmData[7].Value = "";
                            parmData[7].Direction = ParameterDirection.Output;

                            parmData[8] = new SqlParameter();
                            parmData[8].ParameterName = "@O_LABEL_POS"; //헤더파라미터
                            parmData[8].DbType = DbType.Int32;
                            parmData[8].Value = "";
                            parmData[8].Direction = ParameterDirection.Output;

                            parmData[9] = new SqlParameter();
                            parmData[9].ParameterName = "@O_PrintMsg"; //프린트 출력 데이터.
                            parmData[9].Size = 8000;
                            parmData[9].Value = "";
                            parmData[9].Direction = ParameterDirection.Output;

                            DataSet tempSet = DBUtil.ExecuteDataSetSqlParam(sp_Box_End, parmData);


                            string work_type = parmData[6].Value.ToString().Substring(0, 2);

                            //ok가 있으면 정상, 없으면 에러로 롤백됨
                            if (parmData[6].Value.ToString().Contains("OK")) //CO이면 정리 작업이므로 rfid전송을 하지 않음.
                            {
                                //RFID 전송및 프린트
                                lib.Common.Management.Class.RfidSend wivisRfid = new lib.Common.Management.Class.RfidSend();
                                wivisRfid.rfid_send(tempSet, parmData[9].Value.ToString(), Convert.ToInt32(parmData[8].Value));
                            }

                            //das 작업일때만 표시기 상태 업데이트
                            if (this.work_type == "D")
                            {
                                if (indicatorClass is DPSClass.CellBtn_Class cell)
                                {
                                    cell.box_no++;
                                    cell.display = cell.box_no.ToString() + cell.display.PadLeft(5).Right(5);
                                    instruction_das(cell, cell.status);
                                }
                            }
                            break;
                        case "1": //마지막 박스 재발행.

                            parmData = new SqlParameter[2];
                            parmData[0] = new SqlParameter();
                            parmData[0].ParameterName = "@V_CHUTE_NO"; //슈트번호
                            parmData[0].DbType = DbType.Int32;
                            parmData[0].Direction = ParameterDirection.Input;
                            parmData[0].Value = indicatorClass.loc_no;

                            parmData[1] = new SqlParameter();
                            parmData[1].ParameterName = "@V_BIZ_DAY"; //작업 날짜
                            parmData[1].DbType = DbType.String;
                            parmData[1].Value = biz_day;

                            DataSet tempDataSet = DBUtil.ExecuteDataSetSqlParam("SP_INDICATOR_REMAIN_PRINT", parmData);


                            foreach (DataRow dr in tempDataSet.Tables[0].Rows)
                            {
                                string data = dr.ItemArray[0].ToString();
                                int last_index = data.LastIndexOf('/');
                                int label_pos = Convert.ToInt32(data.Substring(last_index + 1, data.Length - 1 - last_index));
                                data = data.Substring(0, last_index);

                                printMethode(data, label_pos);
                            }


                            //das 작업일때만 표시기 상태 업데이트
                            if (this.work_type == "D")
                            {
                                if (indicatorClass is DPSClass.CellBtn_Class cell)
                                {
                                    cell.display = cell.box_no.ToString().Right(1)
                                                 + "00"
                                                 + cell.remain_qty.ToString().PadLeft(3, '0');

                                    instruction_das(cell, 0);
                                }
                            }

                            break;
                        case "2":
                            break;
                        case "3": //풀수량 증가


                            break;

                        case "4": //현재 슈트 if_Work_cfm 초기화

                            //parmData = new SqlParameter[3];
                            //parmData[0] = new SqlParameter();
                            //parmData[0].ParameterName = "@V_CHUTE_NO"; //슈트번호
                            //parmData[0].DbType = DbType.Int32;
                            //parmData[0].Direction = ParameterDirection.Input;
                            //parmData[0].Value = indicatorClass.loc_no;

                            //parmData[1] = new SqlParameter();
                            //parmData[1].ParameterName = "@V_BIZ_DAY"; //작업 날짜
                            //parmData[1].DbType = DbType.String;
                            //parmData[1].Value = biz_day;

                            //parmData[2] = new SqlParameter();
                            //parmData[2].ParameterName = "@O_PrintMsg"; //프린트 출력 데이터.
                            //parmData[2].Size = 8000;
                            //parmData[2].Value = "";
                            //parmData[2].Direction = ParameterDirection.Output;

                            //DBUtil.ExecuteDataSetSqlParam("IF_SP_WORK_CFM_RESET", parmData);

                            //if (parmData[2].Value.ToString() != "" && parmData[2].Value.ToString().Substring(0, 1) == "E")
                            //{
                            //    string data = parmData[2].Value.ToString();
                            //    int last_index = data.LastIndexOf('/');
                            //    int label_pos = Convert.ToInt32(data.Substring(last_index + 1, data.Length - 1 - last_index));
                            //    data = data.Substring(0, last_index);

                            //    printMethode(data, label_pos);
                            //}
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                
            }
        }


        private XmlDocument xmlMake(List<DPSClass.Ord_Class> objectClass)
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

        private XmlDocument xmlMake(List<Dictionary<string, string>> ParamDataList)
        {
            //---------------------------------------------------
            XmlDocument xdoc = new XmlDocument();

            // 루트노드
            XmlNode root = xdoc.CreateElement("root");
            xdoc.AppendChild(root);

            //데이터 노드
            XmlNode[] childNode = new XmlNode[ParamDataList.Count];

            string columnName = string.Empty;
            string columnValue = string.Empty;

            for (int i = 0; i < ParamDataList.Count; i++)
            {
                //데이터 집합 노드
                XmlNode data = xdoc.CreateElement("data");
                root.AppendChild(data);
                foreach (KeyValuePair<string, string> r in ParamDataList[i])
                {
                    columnName = r.Key;
                    columnValue = r.Value;
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


        /// <summary>
        /// Setting Session Value
        /// </summary>
        public void MakeSessionInq()
        {
            setCalendar();
            string sessInq = string.Empty;

            //sessInq += string.Format("user={0}", BaseEntity.sessSID);
            //sessInq += string.Format(";#auth={0}", BaseEntity.sessLv);
            sessInq += string.Format(";#bizday={0}", this.UsrCalWork.DefaultDate.Replace("-", string.Empty));
            //sessInq += string.Format(";#PCNO={0}", BaseEntity.sessPc);
            BaseEntity.sessInq = sessInq;

            BaseEntity.sessPort = System.Configuration.ConfigurationManager.AppSettings["Port"].ToString();
            BaseEntity.sessJBCOUNT = int.Parse(System.Configuration.ConfigurationManager.AppSettings["JBCOUNT"].ToString());
            BaseEntity.sessPc = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Pc"].ToString());
            total_jb = BaseEntity.sessJBCOUNT;
            biz_day = this.UsrCalWork.DefaultDate.Replace("-", string.Empty);
        }



        /// <summary>
        /// Set Calendar - Main Top
        /// </summary>
        private void setCalendar()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.UsrCalWork.DefaultDate = "2022-04-28";
            }
            else
            {
                this.UsrCalWork.DefaultDate = DateTime.Now.ToShortDateString();
            }
        }

        /// <summary>
        /// Change Wrk_Delay
        /// </summary>
        private void setWrkStatusDelay()
        {
            Dictionary<string, object> ParamDelay = new Dictionary<string, object>();
            ParamDelay.Add("@BIZ_DAY", this.UsrCalWork.DefaultDate.Replace("-", string.Empty));
            DBUtil.ExecuteNonQuery("SP_SET_WRK_DELAY", ParamDelay, CommandType.StoredProcedure);
        }





        //페이지 셋팅
        public void PageSetting()
        {
            try
            {
                string[] jb_ip = BaseEntity.sessPort.Split(new char[] { ',' });

                //JB 객수만큼 JB 생성
                jb_controller = new JbController[total_jb];
                for (int i = 1; i <= total_jb; i++)
                {
                    jb_controller[i - 1] = new JbController(jb_ip[i - 1], i);
                    jb_controller[i - 1].send_msg += new JbController.sendirec(jbMsg_Receive);
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("매장 별 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("조회에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //recieveRB("0000RBNOREAD");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Dictionary<string, object> ParamDelay = new Dictionary<string, object>();
            //DBUtil.ExecuteNonQuery("USP_CTR001_01_WORK_reset", ParamDelay, CommandType.StoredProcedure);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("눌렸다 그만눌러라");
        }

        /// <summary>
        /// 프린트 정보를 파싱해서 프린트 클래스에 넘김.
        /// </summary>
        /// <param name="printList"></param>
        private void printMethode(string printList, int label_no)
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
                //p.print(printList, label_no);
                p.print(printString_arr, "Label_" + label_no.ToString().PadLeft(2, '0'));
                //p.print(printString_arr, "LABEL_00");
                //p.print(printString_arr, "Microsoft Print to PDF");
                //p.print(printString_arr, "TOSHIBA B-FV4 (203 dpi) (1 복사)");
            });
        }



        /// <summary>
        /// 정리작업 라벨
        /// </summary>
        /// <param name="printList"></param>
        private void printMethode(DataTable tmepDT, int label_no)
        {
            Task.Run(() =>
            {
                TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //p.print(printList, label_no);
                p.print(tmepDT, "Label_" + label_no.ToString().PadLeft(2, '0'), "cleen");
                //p.print(printString_arr, "Intermec PC43d");
            });
        }



        private void button5_Click(object sender, EventArgs e)
        {

            List<string> testlist = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                int count1 = 0;
                if (i == 0) count1 = 30;
                if (i == 1) count1 = 31;
                if (i == 2) count1 = 32;
                if (i == 3) count1 = 33;
                if (i == 4) count1 = 34;
                for (int j = 0; j < count1; j++)
                {
                    testlist.Add(i.ToString());
                }
            }

            var ttt = from data in testlist
                      group data by data into g
                      select new
                      {
                          line = g.Key,
                          count = g.Count()
                      };


            foreach (var data in ttt)
            {
                Console.WriteLine("라인 : " + data.line + "  카운트 : " + data.count);
            }

            Console.WriteLine("끝");

            //var value = from data in zoneIndicatorList
            //            group data by data into g
            //            select new
            //            {
            //                line = g.Key,
            //                count = g.Key.Count()
            //            };
            string printString = null; //라벨 출력할 데이터를 담는 변수.




            printString = "M / (주)위비스 / 지센 / 185 / 오픈예정(행사 100) / 강원 강릉시 율곡로 3206(죽헌동) / 1층 1,2호//1/Y/4///20201030A001MW002800" + total_jb;
            ////total_jb = total_jb - 4;

            //printString = "C/(주) 위비스/지센/207/가야(H)/부산광역시 부산진구 가야동/624-7외 2필지 홈플러스 가야점 1층/0518927111/1/N/4/B26/광명/LFBLL203NA100" + test;
            //printString = "T/(주)위비스 / 지센 / 185 / 강릉죽헌 / 강원 강릉시 율곡로 3206(죽헌동) / 1층 1,2호//1/Y/4///20201030A001MW002800" + test;
            //test++;

            ////for (int i = 0;i< printString.Length; i++ )
            ////{
            ////    printString[i] = printString[i].TrimEnd().TrimStart();
            ////}

            ////printString = "OK/(주) 위비스/지센/150/강릉죽헌/강원 강릉시 율곡로 3206 (죽헌동)/1층 1,2호//1/Y/6/B26/광명/392106155181".Split('/');
            printMethode(printString, 0);




            //print.print();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //string value = " ";
            //string msg = null;
            //string[] lcd_msg = new string[6];
            //int length = value.Length;
            ////자리수가 16자리가 안될경우 나머지 자리를 공백으로 채움
            //if (value.Length < 16)
            //{
            //    for (int i = 0; i < 16 - length; i++)
            //    {
            //        value = value.Insert(value.Length, i.ToString());
            //    }
            //}
            //Console.WriteLine(value);

            confrmRC("011022RC000003");
            //List<string> zoneIndicatorList = new List<string>();
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 30; j++)
            //    {
            //        zoneIndicatorList.Add(i.ToString());
            //    }
            //}

            //var value = from data in zoneIndicatorList
            //                //group data by data into g
            //            select new
            //            {
            //                line = data,
            //                count = data.Count()
            //            };


            //foreach (var data in value)
            //{
            //    Console.WriteLine(data.line + "     " + data.count);

            //}



            //var value1 = from data in zoneIndicatorList
            //             group data by data into g
            //             select new
            //             {
            //                 line = g.Key,
            //                 count = g.Count()
            //             };


            //foreach (var data in value1)
            //{
            //    Console.WriteLine(data.line + "     " + data.count);

            //}

            //Console.WriteLine("-------------------");
            //        TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();



            //        string sql = @"

            //select  BIZ_DAY,BATCH,CHUTE_NO,ITEM_BAR,sum(wrk_qty) wrk_qty,box_no
            //from IF_BOX_LIST
            //where CHUTE_NO = 200
            //and BIZ_DAY = '20210122'
            //AND WORK_TYPE = 2
            //AND BOX_NO = 2
            //and BATCH = '001'
            //group by BIZ_DAY,BATCH,CHUTE_NO,ITEM_BAR,box_no";

            //        DataSet tmepDT = DBUtil.ExecuteDataSet(sql);

            //        //int max = Convert.ToInt32(tempSet.Tables[0].AsEnumerable().Max(row => row["row_no"]));


            //        p.print(tmepDT.Tables[0], "Label_01", "cleen");

            //p.print(tempSet.Tables[0], "Microsoft Print to PDF");
        }

        /// <summary>
        /// das 작업 취소 버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bu_das_cancel_Click(object sender, EventArgs e)
        {
            das_work_cancel();
        }

        /// <summary>
        /// das key_in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                //"043025RBLAABL205PK00F001";
                recieveRB("043025RB" + textBox2.Text);
                textBox2.Text = "";
            }
        }
    }


}
