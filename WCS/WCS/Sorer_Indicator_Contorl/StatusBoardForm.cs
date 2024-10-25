using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using lib.Common.Management;
using bowoo.Framework.common;
using bowoo.Lib.DataBase;
using System.Threading;

namespace Sorer_Indicator_Contorl
{
    public partial class StatusBoardForm : Telerik.WinControls.UI.RadForm
    {
        static public bowoo.Lib.DataBase.SqlQueryExecuter DBUtil = bowoo.Lib.DataBase.SqlQueryExecuter.getInstance();

        //크로스쓰레드 방지.
        delegate void crossTheread_statusUpdate(DataTable workDT); // 작업현황 그리드 업데이트 처음 로딩시
        delegate void crossTheread_statusUpdate1(int rowNumber, string ord_no, int waitCount, string progress); // 작업현황 그리드 업데이트 //bcr,zone표시기 작업시
        delegate void crossTheread_statusUpdate2(int rowNumber, string progress); // 작업현황 그리드 업데이트 cell작업
        delegate void crossTheread_gridoff(); //작업 종료.
        delegate void crossTheread_progressUpdate(int ord_qty, int work_qty, int remain_qty, string percent); //작업 현황 업데이트
        delegate void crossTheread_timer(bool bit); //타이머 온오프
        delegate void crossTheread_test(DataTable dt);


        Thread sacnDataThread = null;

        DataTable workDT = new DataTable();
        Scaner_dataSelectDAO scaner_select = new Scaner_dataSelectDAO();

        float headerFontSize = 30f;
        bowoo.Lib.DataBase.SqlQueryExecuter dbutill = SqlQueryExecuter.getInstance();

        public StatusBoardForm()
        {
            InitializeComponent();
            Screen[] sc;
            sc = Screen.AllScreens;
            Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
            this.Location = p;
            this.WindowState = FormWindowState.Maximized;
        }

        private void StatusBoard_Load(object sender, EventArgs e)
        {

            ProgressBarColumn customColumn = new ProgressBarColumn("progress");
            

            timer1.Interval = 3000;
            timer1.Start();
        }

       
        public void reciveClear()
        {
            timer1.Stop();
            Clear();
        }


        //작업 상태 업데이트. 크로스쓰레드 방지
        private void Clear()
        {
            if (this.InvokeRequired)
            {
                crossTheread_gridoff d = new crossTheread_gridoff(Clear);
                this.Invoke(d, new object[] { });
            }
            else
            {
                //radGridView1.Rows.Clear();
                //radGridView2.Rows.Clear();
            }
        }

        


        //작업현황 업데이트
        public void reciveProgressUpdate(int ord_qty, int work_qty, int remain_qty, string percent)
        {
            progressUpdate(ord_qty, work_qty, remain_qty, percent);
        }

        private void progressUpdate(int ord_qty, int work_qty, int remain_qty, string percent)
        {
            if (this.workOrder.InvokeRequired)
            {
                crossTheread_progressUpdate d = new crossTheread_progressUpdate(progressUpdate);
                this.Invoke(d, new object[] { ord_qty, work_qty, remain_qty, percent });
            }
            else
            {
                this.workOrder.Text = work_qty.ToString();
                this.remainOrder.Text = remain_qty.ToString();
                this.totalOrder.Text = ord_qty.ToString();
                this.totalProgressBar.Maximum = ord_qty;
                this.totalProgressBar.Value1 = work_qty;
                this.totalProgressBar.Text = percent;
            }
        }


        //public void timerHandle(bool bit)
        //{
        //    if (this.workOrder.InvokeRequired)
        //    {
        //        crossTheread_timer d = new crossTheread_timer(timerHandle);
        //        this.Invoke(d, new object[] { bit });
        //    }
        //    else
        //    {
        //        if(bit == true)
        //        {
        //            sacnDataThread = new Thread(new ThreadStart(scanThread));
        //            sacnDataThread.Start();
        //            //this.timer1.Start();
        //        }
        //        else
        //        {
        //            if(sacnDataThread != null)
        //            sacnDataThread.Abort();
        //            //this.timer1.Stop();
        //        }
        //    }
        //}


        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

       
        
    }
}
