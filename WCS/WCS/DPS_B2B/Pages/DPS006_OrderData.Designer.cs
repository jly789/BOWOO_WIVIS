using lib.Common.Management;
namespace DPS_B2B.Pages
{
    partial class DPS006_OrderData
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
            this.dBufferTableLayoutPanel1 = new lib.Common.Management.DBufferTableLayoutPanel(this.components);
            this.uC01_GridView1 = new lib.Common.Management.UC01_GridView();
            this.uC01_GridView2 = new lib.Common.Management.UC01_GridView();
            this.uC01_GridView3 = new lib.Common.Management.UC01_GridView();
            this.dBufferTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dBufferTableLayoutPanel1
            // 
            this.dBufferTableLayoutPanel1.ColumnCount = 2;
            this.dBufferTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.dBufferTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.dBufferTableLayoutPanel1.Controls.Add(this.uC01_GridView1, 0, 0);
            this.dBufferTableLayoutPanel1.Controls.Add(this.uC01_GridView2, 1, 0);
            this.dBufferTableLayoutPanel1.Controls.Add(this.uC01_GridView3, 0, 1);
            this.dBufferTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dBufferTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.dBufferTableLayoutPanel1.Name = "dBufferTableLayoutPanel1";
            this.dBufferTableLayoutPanel1.RowCount = 2;
            this.dBufferTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.dBufferTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.dBufferTableLayoutPanel1.Size = new System.Drawing.Size(1110, 623);
            this.dBufferTableLayoutPanel1.TabIndex = 0;
            // 
            // uC01_GridView1
            // 
            this.uC01_GridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC01_GridView1.GridAllowAddNewRow = false;
            this.uC01_GridView1.GridAllowEditRow = true;
            this.uC01_GridView1.GridMultiSelect = true;
            this.uC01_GridView1.GridShowGroupPanel = false;
            this.uC01_GridView1.GridTitleText = "";
            this.uC01_GridView1.Location = new System.Drawing.Point(3, 2);
            this.uC01_GridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uC01_GridView1.Name = "uC01_GridView1";
            this.uC01_GridView1.Size = new System.Drawing.Size(549, 245);
            this.uC01_GridView1.TabIndex = 0;
            // 
            // uC01_GridView2
            // 
            this.uC01_GridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC01_GridView2.GridAllowAddNewRow = false;
            this.uC01_GridView2.GridAllowEditRow = true;
            this.uC01_GridView2.GridMultiSelect = true;
            this.uC01_GridView2.GridShowGroupPanel = false;
            this.uC01_GridView2.GridTitleText = "";
            this.uC01_GridView2.Location = new System.Drawing.Point(558, 2);
            this.uC01_GridView2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uC01_GridView2.Name = "uC01_GridView2";
            this.uC01_GridView2.Size = new System.Drawing.Size(549, 245);
            this.uC01_GridView2.TabIndex = 1;
            // 
            // uC01_GridView3
            // 
            this.dBufferTableLayoutPanel1.SetColumnSpan(this.uC01_GridView3, 2);
            this.uC01_GridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC01_GridView3.GridAllowAddNewRow = false;
            this.uC01_GridView3.GridAllowEditRow = true;
            this.uC01_GridView3.GridMultiSelect = true;
            this.uC01_GridView3.GridShowGroupPanel = false;
            this.uC01_GridView3.GridTitleText = "";
            this.uC01_GridView3.Location = new System.Drawing.Point(3, 251);
            this.uC01_GridView3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uC01_GridView3.Name = "uC01_GridView3";
            this.uC01_GridView3.Size = new System.Drawing.Size(1104, 370);
            this.uC01_GridView3.TabIndex = 2;
            // 
            // DPS006_OrderData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dBufferTableLayoutPanel1);
            this.Name = "DPS006_OrderData";
            this.Size = new System.Drawing.Size(1110, 623);
            this.Load += new System.EventHandler(this.DPS002_OrderData_Load);
            this.dBufferTableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private lib.Common.Management.DBufferTableLayoutPanel dBufferTableLayoutPanel1;
        private lib.Common.Management.UC01_GridView uC01_GridView1;
        private lib.Common.Management.UC01_GridView uC01_GridView2;
        private lib.Common.Management.UC01_GridView uC01_GridView3;
    }
}
