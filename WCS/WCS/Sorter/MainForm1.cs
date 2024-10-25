using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Scheduler.Dialogs;

using System.Data.SqlClient;
using bowoo.Lib;
using bowoo.Lib.DataBase;
using Telerik.WinControls.Themes;
using Sorter.Controls;
using System.Runtime.InteropServices;

namespace Sorter
{
    public partial class MainForm1 : Sorter.Master.BaseForm
    {

        /// <summary>
        /// This essentially does exactly the same as grabbing the title bar of a window, from the window manager's point of view.
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void radPanelMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public MainForm1()
        {
            InitializeComponent();
            initCommonControls();
        }

        private void initCommonControls()
        {
            this.FormElement.TitleBar.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;

            //Case1. Query 사용
            //radddlBrand DataBinding
            Dictionary<string, object> param = new Dictionary<string, object>(1);
            param["@input"] = "1";

            string query = @"SELECT [BRAND_CD]
                              ,[BRAND_NM]
                          FROM [SORTER].[dbo].[IF_BRAND_MASTER] WHERE '1'=@input";

            DataTable dtData = DBUtil.ExecuteDataSet(query, param).Tables[0];
            this.radddlBrand.DataSource = dtData;
            this.radddlBrand.DisplayMember = "BRAND_NM";
            this.radddlBrand.DataMember = "BRAND_CD";

            //Menu
            DataTable dtMenu = new DataTable();

            dtMenu.Columns.Add("NAME", typeof(string));
            dtMenu.Columns.Add("ID", typeof(string));

            DataRow row = dtMenu.NewRow();
            row["NAME"] = "menu1";
            row["ID"] = "1";

            dtMenu.Rows.Add(row);
            dtMenu.AcceptChanges();

            DataRow row2 = dtMenu.NewRow();
            row2["NAME"] = "menu2";
            row2["ID"] = "2";

            dtMenu.Rows.Add(row2);
            dtMenu.AcceptChanges();

            DataRow row3 = dtMenu.NewRow();
            row3["NAME"] = "menu3";
            row3["ID"] = "3";

            dtMenu.Rows.Add(row3);
            dtMenu.AcceptChanges();

            //Image menuImage = Image.FromFile(@"C:\Source\SMS\Sorter\Sorter\Resource\buttonMenu.png");

            foreach (DataRow itemRow in dtMenu.Rows)
	        {
                ListViewDataItem item = new ListViewDataItem();
                
                //rdGBMenu.Controls.Add(btn);

	        }


            //LbxMenu.DataSource = dtMenu;
            //LbxMenu.DisplayMember = "NAME";
            //LbxMenu.ValueMember = "ID";
            //LbxMenu.Items.
            //

            //Calendar
            this.UsrCalWork.ReadOnly = false;
            this.UsrCalWork.DefaultDate = DateTime.Now.ToShortDateString();

            //this.FormElement.TitleBar.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;

            //Sorter.Master.BaseControl ctrlCal = new Sorter.Controls.Calendar() { CalWidth = 92, CalHeight = 23, ReadOnly = false,DefaultDate = DateTime.Now.ToShortDateString() };
            //ctrlCal.Location = new Point(468, 11);
            //this.radPanelHeader.Controls.Add(ctrlCal);
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            UserControl Ctrl = new Sorter.Pages.ManageLocation();

            this.radPanelContents.Controls.Add(Ctrl);
        }

        private void radButtonLogOut_Click(object sender, EventArgs e)
        {
            //bowooMessageBox.Show("설정이 완료되었습니다. 설정이 완료되었습니다.");
            UserControl Ctrl = new Sorter.Pages.ManageLocation();

            this.radPanelContents.Controls.Add(Ctrl);
        
        }
    }
}
