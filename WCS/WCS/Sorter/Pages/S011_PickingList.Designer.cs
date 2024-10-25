namespace Sorter.Pages
{
    partial class S011_PickingList
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.UC01_SMSGridViewLeft = new lib.Common.Management.UC01_GridView();
            this.radCheckedDropDownList1 = new Telerik.WinControls.UI.RadCheckedDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckedDropDownList1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("굴림", 15F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(552, 33);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 28);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged_1);
            // 
            // UC01_SMSGridViewLeft
            // 
            this.UC01_SMSGridViewLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UC01_SMSGridViewLeft.BackColor = System.Drawing.Color.White;
            this.UC01_SMSGridViewLeft.GridAllowAddNewRow = false;
            this.UC01_SMSGridViewLeft.GridAllowEditRow = true;
            this.UC01_SMSGridViewLeft.GridMultiSelect = true;
            this.UC01_SMSGridViewLeft.GridShowGroupPanel = false;
            this.UC01_SMSGridViewLeft.GridTitleText = "";
            this.UC01_SMSGridViewLeft.Location = new System.Drawing.Point(3, 7);
            this.UC01_SMSGridViewLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.UC01_SMSGridViewLeft.Name = "UC01_SMSGridViewLeft";
            this.UC01_SMSGridViewLeft.Size = new System.Drawing.Size(1104, 621);
            this.UC01_SMSGridViewLeft.TabIndex = 1;
            // 
            // radCheckedDropDownList1
            // 
            this.radCheckedDropDownList1.Location = new System.Drawing.Point(678, 33);
            this.radCheckedDropDownList1.Name = "radCheckedDropDownList1";
            this.radCheckedDropDownList1.Size = new System.Drawing.Size(133, 20);
            this.radCheckedDropDownList1.TabIndex = 8;
            // 
            // S011_PickingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.radCheckedDropDownList1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.UC01_SMSGridViewLeft);
            this.Name = "S011_PickingList";
            this.Size = new System.Drawing.Size(1110, 635);
            ((System.ComponentModel.ISupportInitialize)(this.radCheckedDropDownList1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private lib.Common.Management.UC01_GridView UC01_SMSGridViewLeft;
        private System.Windows.Forms.ComboBox comboBox1;
        private Telerik.WinControls.UI.RadCheckedDropDownList radCheckedDropDownList1;
    }
}
