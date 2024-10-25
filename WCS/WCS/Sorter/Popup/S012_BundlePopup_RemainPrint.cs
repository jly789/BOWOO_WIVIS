using bowoo.Framework.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

using lib.Common;
using lib.Common.Management;

namespace Sorter.Popup
{
    public partial class S012_BundlePopup_RemainPrint : lib.Common.Management.BaseForm
    {
        //public delegate void select_print(string print);
        //public event select_print print_name_send;
        Sorter.Pages.S012_BundelList form = null;
        string seleted_print = string.Empty;

        public S012_BundlePopup_RemainPrint(Sorter.Pages.S012_BundelList form)
        {
            InitializeComponent();
            this.CenterToParent();
            this.form = form;
        }

        private void ManageLocationPopup_Initialized(object sender, EventArgs e)
        {
            //타이머 및 사용여부 가져오기
            //사용여부 로드
            menuId = "S012";
            menuTitle = "프린트 선택";

            radTitleBarPopup.Text = LanguagePack.Translate(radTitleBarPopup.Text);


            radBtnSave.Text = LanguagePack.Translate(radBtnSave.Text);
            radBtnCancel.Text = LanguagePack.Translate(radBtnCancel.Text);

        }

        /// <summary>
        /// 명세서 재발행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton2_Click(object sender, EventArgs e)
        {
            form.seleted_print = "LASER";
            this.Close();
        }

        /// <summary>
        /// 라벨 재발행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnSave_Click(object sender, EventArgs e)
        {
            form.seleted_print = "LABEL";
            this.Close();
        }
    }
}