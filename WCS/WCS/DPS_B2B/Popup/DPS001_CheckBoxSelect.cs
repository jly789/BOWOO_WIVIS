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
    public partial class DPS001_CheckBoxSelect : lib.Common.Management.BaseForm
    {
        public string inputZoneNo = "";

        public DPS001_CheckBoxSelect()
        {
            InitializeComponent();
            this.CenterToParent();

            this.radButtonYes.Click += radButtonYes_Click;
            this.radButtonNo.Click += radButtonNo_Click;
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        //아니오
        void radButtonNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        //예
        void radButtonYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


      
    }
}
