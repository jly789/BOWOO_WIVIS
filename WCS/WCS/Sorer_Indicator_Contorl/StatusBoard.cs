using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Threading;
using System.Data.SqlClient;

namespace Sorer_Indicator_Contorl
{
    public partial class StatusBoard : Telerik.WinControls.UI.RadForm
    {
        static public bowoo.Lib.DataBase.SqlQueryExecuter DBUtil = bowoo.Lib.DataBase.SqlQueryExecuter.getInstance();
        //크로스쓰레드 방지.
        delegate void crossTheread_NowTime(); // 현재시간,소요시간 컨트롤 text 변경
        delegate void crossTheread_statusUpdate1(int rowNumber, string ord_no, int waitCount, string progress); // 작업현황 그리드 업데이트 //bcr,zone표시기 작업시
        delegate void crossTheread_statusUpdate2(int rowNumber, string progress); // 작업현황 그리드 업데이트 cell작업
        delegate void crossTheread_gridoff(); //작업 종료.
        delegate void crossTheread_progressUpdate(int ord_qty, int work_qty, int remain_qty, string percent); //작업 현황 업데이트
        DataTable workDT = new DataTable();

        float headerFontSize = 35f;
        DateTime nowTime, startTime, endTime, requiredTime;
        Thread thread = null;
        DataSet dataset = null;
        int box_qty = 0, ord_qty = 0, wrk_qty = 0, remain_qty = 0;
        string biz_day = string.Empty;
        int tray_on_count = 0;

        public StatusBoard(string biz_day)
        {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();
            Screen[] sc;
            sc = Screen.AllScreens;
            Point p = new Point(sc[0].Bounds.Location.X, sc[0].Bounds.Location.Y);
            this.Location = p;
            this.WindowState = FormWindowState.Maximized;
            this.biz_day = biz_day;
        }

        private void StatusBoard_Load(object sender, EventArgs e)
        {
            nowTime = new DateTime();
            startTime = new DateTime();
            endTime = new DateTime();
            requiredTime = new DateTime();
            thread = new Thread(new ThreadStart(threadMethode));
            thread.Start();
            //radGridView1.Rows.AddNew();
            //radGridView1.Rows.AddNew();
            //radGridView1.Rows.AddNew();
            //radGridView1.Rows.AddNew();
            //radGridView1.Rows.AddNew();
            //radGridView1.Rows.AddNew();
        }

        //현재시간 표시하기 위함.
        private void threadMethode()
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    //param.Add("biz_day", "2021-01-19");
                    nowTime = System.DateTime.Now;


                    SqlParameter[] parmData = new SqlParameter[6];


                    parmData[0] = new SqlParameter();
                    parmData[0].ParameterName = "@V_BIZ_DAY"; //작업 날짜
                    parmData[0].DbType = DbType.String;
                    parmData[0].Direction = ParameterDirection.Input;
                    parmData[0].Value = biz_day;

                    parmData[1] = new SqlParameter();
                    parmData[1].ParameterName = "@V_BOX_COUNT"; //박스 수량
                    parmData[1].DbType = DbType.Int32;
                    parmData[1].Direction = ParameterDirection.Output;

                    parmData[2] = new SqlParameter();
                    parmData[2].ParameterName = "@V_ORD_QTY"; //오더 수량
                    parmData[2].DbType = DbType.Int32;
                    parmData[2].Direction = ParameterDirection.Output;

                    parmData[3] = new SqlParameter();
                    parmData[3].ParameterName = "@V_WRK_QTY"; //작업 수량
                    parmData[3].DbType = DbType.Int32;
                    parmData[3].Direction = ParameterDirection.Output;

                    parmData[4] = new SqlParameter();
                    parmData[4].ParameterName = "@V_RE_QTY"; //남은수량
                    parmData[4].DbType = DbType.Int32;
                    parmData[4].Direction = ParameterDirection.Output;

                    parmData[5] = new SqlParameter();
                    parmData[5].ParameterName = "@V_TRAY_ON_COUNT"; //남은수량
                    parmData[5].DbType = DbType.Int32;
                    parmData[5].Direction = ParameterDirection.Output;

                    dataset = DBUtil.ExecuteDataSetSqlParam("IF_SP_STATUSBOARD", parmData);
                    box_qty = Convert.ToInt32(parmData[1].Value);
                    ord_qty = Convert.ToInt32(parmData[2].Value);
                    wrk_qty = Convert.ToInt32(parmData[3].Value);
                    remain_qty = Convert.ToInt32(parmData[4].Value);
                    tray_on_count = Convert.ToInt32(parmData[5].Value);
                    workStatusUpdate();

                }
                catch (Exception ex)
                {
                    lib.Common.Log.LogFile.WriteLog(ex.Message);
                }
            }
        }

        //현재시간 , 소요시간 text 변경
        private void workStatusUpdate()
        {
            if (this.InvokeRequired)
            {
                crossTheread_NowTime d = new crossTheread_NowTime(workStatusUpdate);
                this.Invoke(d, new object[] { });
            }
            else
            {
                try
                {
                    la_CurrentTime.Text = nowTime.ToString("HH:mm:ss");
                    startTime = Convert.ToDateTime(dataset.Tables[0].Rows[0]["start_time"]);
                    endTime = Convert.ToDateTime(dataset.Tables[0].Rows[0]["end_time"]);

                    string percent = (Math.Round((float)wrk_qty / (float)ord_qty * 100, 2)).ToString() + "%";
                    la_Progress.Text = percent;

                    la_OrdQty.Text = ord_qty.ToString(); //오더수
                    la_WrkQty.Text = wrk_qty.ToString(); // 작업수
                    la_RemQty.Text = remain_qty.ToString(); //남은수
                    la_BoxQty.Text = box_qty.ToString(); //박스수

                    //progressbar업데이트
                    this.totalProgressBar.Maximum = ord_qty;
                    this.totalProgressBar.Value1 = wrk_qty;
                    this.totalProgressBar.Text = percent;

                    if (tray_on_count > 0)
                    {
                        this.totalProgressBar.ProgressBarElement.IndicatorElement1.BackColor = Color.Red;
                        this.totalProgressBar.ProgressBarElement.IndicatorElement1.BackColor2 = Color.Red;
                        this.totalProgressBar.ProgressBarElement.IndicatorElement1.BackColor3 = Color.Red;
                        this.totalProgressBar.ProgressBarElement.IndicatorElement1.BackColor4 = Color.Red;
                    }
                    else
                    {
                        this.totalProgressBar.ProgressBarElement.IndicatorElement1.BackColor = Color.Green;
                        this.totalProgressBar.ProgressBarElement.IndicatorElement1.BackColor2 = Color.Green;
                        this.totalProgressBar.ProgressBarElement.IndicatorElement1.BackColor3 = Color.Green;
                        this.totalProgressBar.ProgressBarElement.IndicatorElement1.BackColor4 = Color.Green;

                    }

                    foreach (DataRow dr in dataset.Tables[1].Rows)
                    {
                        switch (dr["input_no"].ToString())
                        {
                            case "1":
                                la_Ind1.Text = dr["count"].ToString();
                                break;
                            case "2":
                                la_Ind2.Text = dr["count"].ToString();
                                break;
                            case "3":
                                la_Ind3.Text = dr["count"].ToString();
                                break;
                        }
                    }
                    //la_Ind1.Text = dataset.Tables[1].Rows[0].ItemArray[1].ToString(); //1번 인덕션 작업수
                    //la_Ind2.Text = dataset.Tables[1].Rows[1].ItemArray[1].ToString(); //2번 인덕션 작업수
                    //la_Ind3.Text = dataset.Tables[1].Rows[2].ItemArray[1].ToString(); //3번 인덕션 작업수


                    la_StartTime.Text = startTime.ToString("HH:mm:ss"); //작업 시작 시간
                    la_EndTime.Text = endTime.ToString("HH:mm:ss"); //작업 마감 시간

                    if (startTime.ToString("HHmmss") != "000000" && endTime.ToString("HHmmss") == "000000")
                    {
                        TimeSpan timespan = nowTime - startTime;
                        la_RequiredTime.Text = (timespan.Hours + ":" + timespan.Minutes + ":" + timespan.Seconds).ToString(); //작업 소요시간
                    }
                    else
                    {
                        TimeSpan timespan = endTime - startTime;
                        la_RequiredTime.Text = (timespan.Hours + ":" + timespan.Minutes + ":" + timespan.Seconds).ToString(); //작업 소요시간

                    }
                }
                catch
                {

                }
            }
        }


        private void la_Closing_Hour_FormClosing(object sender, FormClosingEventArgs e)
        {
            thread.Abort();
        }

    }
}
