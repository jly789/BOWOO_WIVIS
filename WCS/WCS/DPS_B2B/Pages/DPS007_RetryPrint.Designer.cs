namespace DPS_B2B.Pages
{
    partial class DPS007_RetryPrint
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
            this.components = new System.ComponentModel.Container();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            this.uC01_GridView_Left = new lib.Common.Management.UC01_GridView();
            this.dBufferTableLayoutPanel1 = new lib.Common.Management.DBufferTableLayoutPanel(this.components);
            this.dBufferTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uC01_GridView_Left
            // 
            this.uC01_GridView_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC01_GridView_Left.GridAllowAddNewRow = false;
            this.uC01_GridView_Left.GridAllowEditRow = true;
            this.uC01_GridView_Left.GridMultiSelect = true;
            this.uC01_GridView_Left.GridShowGroupPanel = false;
            this.uC01_GridView_Left.GridTitleText = "";
            this.uC01_GridView_Left.Location = new System.Drawing.Point(3, 2);
            this.uC01_GridView_Left.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uC01_GridView_Left.Name = "uC01_GridView_Left";
            this.uC01_GridView_Left.Size = new System.Drawing.Size(1104, 619);
            this.uC01_GridView_Left.TabIndex = 1;
            // 
            // dBufferTableLayoutPanel1
            // 
            this.dBufferTableLayoutPanel1.ColumnCount = 1;
            this.dBufferTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.dBufferTableLayoutPanel1.Controls.Add(this.uC01_GridView_Left, 0, 0);
            this.dBufferTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dBufferTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.dBufferTableLayoutPanel1.Name = "dBufferTableLayoutPanel1";
            this.dBufferTableLayoutPanel1.RowCount = 1;
            this.dBufferTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dBufferTableLayoutPanel1.Size = new System.Drawing.Size(1110, 623);
            this.dBufferTableLayoutPanel1.TabIndex = 0;
            // 
            // DPS007_RetryPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dBufferTableLayoutPanel1);
            this.Name = "DPS007_RetryPrint";
            this.Size = new System.Drawing.Size(1110, 623);
            this.Load += new System.EventHandler(this.DPS007_ManageLocation_Load);
            this.dBufferTableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private lib.Common.Management.UC01_GridView uC01_GridView_Left;
        private lib.Common.Management.DBufferTableLayoutPanel dBufferTableLayoutPanel1;
    }
}
