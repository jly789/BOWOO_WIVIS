using bowoo.Framework.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace DPS_B2B.Popup
{
    public partial class DPS001_ZoneSettingPopup_Input : lib.Common.Management.BaseForm
    {
        public string inputZoneNo = "";

        public DPS001_ZoneSettingPopup_Input()
        {
            InitializeComponent();
            this.CenterToParent();

            this.radTextBoxZoneNo.KeyDown += radTextBoxZoneNo_KeyDown;
            this.radButtonYes.Click += radButtonYes_Click;
            this.radButtonNo.Click += radButtonNo_Click;
            this.radTextBoxZoneNo.Select();
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        //아니오
        void radButtonNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //예
        void radButtonYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            inputZoneNo = this.radTextBoxZoneNo.Text;
            this.Close();
        }

        //존 번호 입력 후 엔터
        void radTextBoxZoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                radButtonYes_Click(null, null);
            }
        }

    }
}
