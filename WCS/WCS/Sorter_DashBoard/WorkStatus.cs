using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using bowoo.Framework.common;

namespace Sorter_DashBoard
{
    public partial class WorkStatus : lib.Common.Management.BaseForm
    {
        private int SetBindCount = 0;
        public double Total_ORDQty = 0;
        public double Total_WRKQty = 0;
        public int Total_Percent = 0;

        static public string Language = "";
        public static DataTable csvDt = new DataTable();

        #region Set Value for Controls

        /// <summary>
        /// Set DashBoard All Data
        /// </summary>
        private void setAlldata()
        {
            //Initial All DataSet,DataTable
            DataSet dsAll = null;
            DataTable dtWork, dtScan, dtCfm, dtBox = null;

            //Get All Data
            Dictionary<string, object> param = new Dictionary<string, object>();
            dsAll = DBUtil.ExecuteDataSet("SP_GET_WRKSTATUS_INFO", null, CommandType.StoredProcedure);

            dtWork = dsAll.Tables[0];
            dtScan = dsAll.Tables[1];
            dtCfm = dsAll.Tables[2];
            dtBox = dsAll.Tables[3];

            
            setDataByWork(dtWork);
            setDataTotal(dtScan, dtCfm, dtBox);
        }

        /// <summary>
        /// Set Scan,Confirm,Box Data
        /// </summary>
        /// <param name="dtScan"></param>
        /// <param name="dtCfm"></param>
        /// <param name="dtBox"></param>
        private void setDataTotal(DataTable dtScan, DataTable dtCfm, DataTable dtBox)
        {
            this.radLabelScan.Text = string.Format("{0:#,##0}", double.Parse(dtScan.Rows[0]["SCAN_QTY"].ToString()));
            this.radLabelConfirm.Text = string.Format("{0:#,##0}", double.Parse(dtCfm.Rows[0]["CFM_QTY"].ToString()));
            this.radLabelBox.Text = string.Format("{0:#,##0}", double.Parse(dtBox.Rows[0]["BOX_QTY"].ToString()));
        }

        /// <summary>
        /// Set Work GridView Data
        /// </summary>
        /// <param name="dtWork"></param>
        private void setDataByWork(DataTable dtWork)
        {
            this.radGridViewLeftWork.AllowAddNewRow = true;

            this.radGridViewLeftWork.MasterTemplate.HorizontalScrollState = ScrollState.AlwaysHide;
            this.radGridViewLeftWork.MasterTemplate.VerticalScrollState = ScrollState.AlwaysHide;

            if (this.radGridViewLeftWork.SummaryRowsBottom.Count > 0) this.radGridViewLeftWork.SummaryRowsBottom.RemoveAt(0);

            GridViewSummaryRowItem summaryRowItem = new GridViewSummaryRowItem();

            foreach (GridViewColumn item in this.radGridViewLeftWork.Columns)
            {
                GridViewSummaryItem summaryItem = new GridViewSummaryItem();

                switch (item.Name)
                {
                    case "column1":
                        summaryItem = new GridViewSummaryItem("column1", Translate("합  계"), GridAggregateFunction.Var);
                        break;
                    case "column5":
                        break;
                    default:
                        summaryItem.Name = item.Name;
                        summaryItem.AggregateExpression = "Sum(" + item.Name + ")";
                        summaryItem.FormatString = "{0:#,##0}";
                        break;
                }

                summaryRowItem.Add(summaryItem);
            }

            this.radGridViewLeftWork.SummaryRowsBottom.Add(summaryRowItem);
            this.radGridViewLeftWork.MasterTemplate.ShowTotals = true;
            this.radGridViewLeftWork.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;
            this.radGridViewLeftWork.MasterView.SummaryRows[0].Height = 38;

            Total_ORDQty = dtWork.AsEnumerable().Sum(x => x.Field<int>(2));
            Total_WRKQty = dtWork.AsEnumerable().Sum(x => x.Field<int>(3));
            Total_Percent = Total_ORDQty == 0 ? 0 : int.Parse(Math.Truncate(Total_WRKQty / Total_ORDQty * 100).ToString());

            if (dtWork.Rows.Count > 6)
            {
                DataTable dtClone_VisibleData = dtWork.Clone();
                DataTable dtClone_NonVisibleData = dtWork.Clone();
                DataTable dtUnion = null;

                if (SetBindCount > 0)
                {
                    int RowCount = this.radGridViewLeftWork.Rows.Count;

                    for (int i = SetBindCount; i < dtWork.Copy().Rows.Count; i++)
                    {
                        dtClone_VisibleData.ImportRow(dtWork.Copy().Rows[i]);
                    }

                    for (int i = 0; i < SetBindCount; i++)
                    {
                        dtClone_NonVisibleData.ImportRow(dtWork.Copy().Rows[i]);
                    }

                    dtUnion = dtClone_VisibleData.AsEnumerable().Union(dtClone_NonVisibleData.AsEnumerable()).Distinct(DataRowComparer.Default).CopyToDataTable<DataRow>();
                }

                this.radGridViewLeftWork.DataSource = SetBindCount == 0 ? dtWork : dtUnion;    
            }
            else
            {
                this.radGridViewLeftWork.DataSource = dtWork;
            }
        }

        /// <summary>
        /// Set TImer Rela time 
        /// </summary>
        private void setTime()
        {
            string TimeText = string.Empty;

            foreach (System.Diagnostics.Process procName in System.Diagnostics.Process.GetProcesses())
            {
                string CurrentDate = DateTime.Now.ToString("yyyy.MM.dd");
                string CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                string CurrentDay = DateTime.Now.ToString("ddd", CultureInfo.CreateSpecificCulture("en-KR"));

                TimeText = string.Format("{0} {1} {2}", CurrentDate, CurrentDay, CurrentTime);
            }

            this.radLabelRightClock.Text = TimeText;
        }

        #endregion

        #region Set Event for Controls

        /// <summary>
        /// Define Constructor
        /// </summary>
        public WorkStatus()
        {
            foreach (string item in Environment.GetCommandLineArgs())
            {
                if (item.IndexOf("Language=") > -1)
                {
                    Language = string.Format(item.Replace("Language=", string.Empty));
                }
            }

            getLanguage();
            InitializeComponent();
        }

        /// <summary>
        /// For Set Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkStatus_Load(object sender, EventArgs e)
        {
            
            if (Screen.AllScreens.Length > 1)
            {
                Screen[] sc;
                sc = Screen.AllScreens;
                int scindex = 0;

                foreach (string item in Environment.GetCommandLineArgs())
                {
                    
                    if (item.IndexOf("UseScreenSeq=") > -1)
                    {
                        scindex = int.Parse(item.Replace("UseScreenSeq=", string.Empty)) - 1;
                    }
                    //if (item.IndexOf("Language=") > -1)
                    //{
                    //    Language = string.Format(item.Replace("Language=", string.Empty));
                    //}
                }

                this.StartPosition = FormStartPosition.Manual;
                //this.WindowState = FormWindowState.Maximized;
                this.Left = sc[scindex].Bounds.Left;
                this.Top = sc[scindex].Bounds.Top;
                
                this.Show();

                //getLanguage();
                setLanguage();
                setTime();
                setAlldata();
                radGridViewLeftWork.HideSelection = true;
            }
            else
            {
                bowooMessageBox.Show("연동된 모니터가 없으므로, 작업 현황판을 실행할 수 없습니다.\r\n디스플레이 연동 후 다시 실행하세요.");
                this.Close();
            }

            //Make Multithread for quickly response
            Dictionary<string, object> paramStatusInfoDelay = new Dictionary<string, object>();
            paramStatusInfoDelay["@GET_SET_TYPE"] = "GET";
            paramStatusInfoDelay["@CODE_NM"] = "WRK_STATUS_DELAY";

            DataSet dsWrkStatusInfoDelay = DBUtil.ExecuteDataSet("DBO.SP_CODE", paramStatusInfoDelay, CommandType.StoredProcedure);
            BaseEntity.sessWrkDelay = int.Parse(dsWrkStatusInfoDelay.Tables[0].Rows[0]["CODE_VAL"].ToString());

            timerWrkStatusData.Interval = BaseEntity.sessWrkDelay * 1000;
            timerWrkStatusData.Start();

            Thread threadData = new Thread(
                () =>
                {
                    timerWrkStatusData.Tick += new EventHandler(WrkStatusData_Tick);
                });

            threadData.SetApartmentState(ApartmentState.STA);
            threadData.Start();

            timerWrkStatusWatch.Interval = 1000;
            timerWrkStatusWatch.Start();

            Thread threadWatch = new Thread(
                () =>
                {
                    timerWrkStatusWatch.Tick += new EventHandler(timerWrkStatusWatch_Tick);
                });

            threadWatch.SetApartmentState(ApartmentState.STA);
            threadWatch.Start();
        }

        private void getLanguage()
        {
            getFilePath("Sorter", "Language_Pack.csv");

            csvDt = LanguagePack.GetDataTableFromCsvStream(ExePath);
        }

        public string Translate(string Korean)
        {
            string Trans = "";

            DataRow[] row = csvDt.Select(string.Format("한국어='{0}'", Korean));

            if (row.Length > 0)
            {
                Trans = row[0][Language].ToString();
            }

            else
            {
                Trans = Korean;
            }

            return Trans;
        }

        private void setLanguage()
        {
            radLabel1.Text = Translate(radLabel1.Text);
            radGroupBoxChulgo.HeaderText = Translate(radGroupBoxChulgo.HeaderText);
            radGroupAll.HeaderText = Translate(radGroupAll.HeaderText);
            radGroupBoxScan.HeaderText = Translate(radGroupBoxScan.HeaderText);
            radGroupBoxConfirm.HeaderText = Translate(radGroupBoxConfirm.HeaderText);
            radGroupBox.HeaderText = Translate(radGroupBox.HeaderText);

            ColumnGroupsViewDefinition view = radGridViewLeftWork.ViewDefinition as ColumnGroupsViewDefinition;

            view.ColumnGroups[0].Text = Translate(view.ColumnGroups[0].Text);
            view.ColumnGroups[1].Text = Translate(view.ColumnGroups[1].Text);
            view.ColumnGroups[2].Text = Translate(view.ColumnGroups[2].Text);
            view.ColumnGroups[3].Text = Translate(view.ColumnGroups[3].Text);
            radGridViewLeftWork.Columns[0].HeaderText = Translate(radGridViewLeftWork.Columns[0].HeaderText);
            radGridViewLeftWork.Columns[1].HeaderText = Translate(radGridViewLeftWork.Columns[1].HeaderText);
            radGridViewLeftWork.Columns[2].HeaderText = Translate(radGridViewLeftWork.Columns[2].HeaderText);
            radGridViewLeftWork.Columns[3].HeaderText = Translate(radGridViewLeftWork.Columns[3].HeaderText);
            radGridViewLeftWork.Columns[4].HeaderText = Translate(radGridViewLeftWork.Columns[4].HeaderText);
            radGridViewLeftWork.Columns[5].HeaderText = Translate(radGridViewLeftWork.Columns[5].HeaderText);
            radGridViewLeftWork.Columns[6].HeaderText = Translate(radGridViewLeftWork.Columns[6].HeaderText);
        }

        /// <summary>
        /// Set Timer Event - All Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WrkStatusData_Tick(object sender, EventArgs e)
        {
            if (this.radGridViewLeftWork.Rows.Count > 0)
            {
                int GridCountPortion = SetBindCount / this.radGridViewLeftWork.Rows.Count;
                int GridCountrest = SetBindCount % this.radGridViewLeftWork.Rows.Count;

                SetBindCount = GridCountPortion > 0 ? (GridCountrest + 1) : (SetBindCount + 1);
            }
            setAlldata();
        }

        /// <summary>
        /// Set Timer Event - Timer 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerWrkStatusWatch_Tick(object sender, EventArgs e)
        {
            setTime();
        }

        /// <summary>
        /// Set Grid CellFormatting Event - Font,Text Aligment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewLeftWork_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;

            switch (e.Column.Name)
            {
                case "column1":
                    e.CellElement.TextAlignment = ContentAlignment.MiddleLeft;
                    e.CellElement.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
                    break;
                case "column5":
                    if (!(e.CellElement is GridHeaderCellElement))
                    {
                        if (e.CellElement.Children.Count > 0)
                            return;

                        int Percent = int.Parse(e.CellElement.Value.ToString().Equals(string.Empty) ? "0" : e.CellElement.Value.ToString());

                        if (!e.CellElement.Value.ToString().Equals(string.Empty) && (Percent >= 0 && Percent <= 100))
                        {
                            RadProgressBarElement element = new RadProgressBarElement();
                            element.Minimum = 0;
                            element.Maximum = 100;
                            element.Value1 = Percent;
                            element.Text = Percent.ToString();
                            //((Telerik.WinControls.UI.UpperProgressIndicatorElement)element.Children[1]).BackColor = Color.Blue;
                            element.TextAlignment = ContentAlignment.MiddleCenter;
                            element.StretchHorizontally = true;
                            element.StretchVertically = true;
                            e.CellElement.Children.Add(element);
                        }
                        else
                        {
                            e.CellElement.Text = string.Empty;
                        }
                    }
                    break;
                default:
                    e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
                    break;
            }

            if (e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.TextAlignment = ContentAlignment.BottomRight;
            }
        }

        /// <summary>
        /// Set Grid RowFormatting Event - Selected or not, VisualState
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewLeftWork_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            e.RowElement.IsSelected = false;
            e.RowElement.RowVisualState = GridRowElement.RowVisualStates.None;

            this.radGridViewLeftWork.MasterTemplate.ShowTotals = true;
        }

        /// <summary>
        /// Set Grid ViewCellFormatting Event - Font,TextAligment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewLeftWork_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            
            e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
            e.CellElement.BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            if (e.CellElement is GridColumnGroupCellElement || e.CellElement is GridHeaderCellElement)
            {
                e.CellElement.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);

                if (e.CellElement.Value.Equals("출고 작업") || e.CellElement.Value.Equals("반품 작업") || e.CellElement.Value.Equals("정리 작업"))
                {
                    e.CellElement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(0)))), ((int)(((byte)(66)))));
                    e.CellElement.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(0)))), ((int)(((byte)(84)))));
                    e.CellElement.BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(0)))), ((int)(((byte)(66)))));
                    e.CellElement.BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(48)))));
                }

            }

            else if (e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.BorderTopWidth = 6;
                e.CellElement.BorderTopColor = Color.YellowGreen;

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                        e.CellElement.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
                        break;

                    case 4:
                        e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                        e.CellElement.Font = new System.Drawing.Font("Radioland", 14F, System.Drawing.FontStyle.Bold);

                        if (e.CellElement.Children.Count > 0)
                        {
                            (e.CellElement.Children[0] as Telerik.WinControls.UI.RadProgressBarElement).Value1 = Total_Percent;
                            (e.CellElement.Children[0] as Telerik.WinControls.UI.RadProgressBarElement).Text = Total_Percent.ToString();
                            e.CellElement.Padding = new System.Windows.Forms.Padding(0, -3, 0, 0);
                        }
                        break;

                    default:
                        e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
                        e.CellElement.Font = new System.Drawing.Font("Radioland", 14F, System.Drawing.FontStyle.Bold);
                        break;
                }
            }

            else if (e.CellElement is GridDataCellElement)
            {
                if (e.ColumnIndex > 0)
                {
                    e.CellElement.Font = new System.Drawing.Font("Radioland", 14F, System.Drawing.FontStyle.Bold);
                }
            }

            else
            {
                e.CellElement.ResetValue(LightVisualElement.FontProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
            }

            switch (e.CellElement.ColumnIndex)
            {
                case 0:
                    e.CellElement.BorderRightWidth = 6;
                    e.CellElement.BorderRightColor = Color.YellowGreen;
                    e.CellElement.BorderLeftWidth = 6;
                    e.CellElement.BorderLeftColor = Color.YellowGreen;
                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;
                
                case 4:
                    e.CellElement.BorderRightWidth = 6;
                    e.CellElement.BorderRightColor = Color.YellowGreen;
                    break;

                case 5:
                    e.CellElement.BorderRightWidth = 6;
                    e.CellElement.BorderRightColor = Color.YellowGreen;
                    break;

                case 6:
                    e.CellElement.BorderRightWidth = 6;
                    e.CellElement.BorderRightColor = Color.YellowGreen;
                    break;

                default: 
                    e.CellElement.BorderRightWidth = 6;
                    e.CellElement.BorderRightColor = Color.YellowGreen;
                    break;
            }
        }

        /// <summary>
        /// Set Grid Created Row Event - Height
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewLeftWork_CreateRow(object sender, GridViewCreateRowEventArgs e)
        {
            e.RowInfo.Height = 33;
        }

        /// <summary>
        /// Set Grid Create Cell Event For Column5 ( Progressbar Column )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewLeftWork_CreateCell(object sender, GridViewCreateCellEventArgs e)
        {
            if (e.Row is GridSummaryRowElement)
            {
                if (e.Column.Name == "column5")
                {
                    e.CellElement = new CustomSummaryCellElement(e.Column, e.Row);
                }
            }
        }

        #endregion

        #region Inner Class for Progress bar Column

        public class CustomSummaryCellElement : GridSummaryCellElement
        {
            public CustomSummaryCellElement(GridViewColumn column, GridRowElement row)
                : base(column, row)
            {

            }

            protected override Type ThemeEffectiveType
            {
                get
                {
                    return typeof(GridSummaryCellElement);
                }
            }

            RadProgressBarElement progress = new RadProgressBarElement();

            protected override void CreateChildElements()
            {
                base.CreateChildElements();
                this.Children.Add(progress);
            }

            public override void SetContent()
            {
                base.SetContent();
                progress.Minimum = 0;
                progress.Maximum = 100;
                progress.TextAlignment = ContentAlignment.MiddleCenter;
                progress.StretchHorizontally = true;
                progress.StretchVertically = true;
            }

            public override bool IsCompatible(GridViewColumn data, object context)
            {
                return true;
            }

        #endregion
        }
    }
}
