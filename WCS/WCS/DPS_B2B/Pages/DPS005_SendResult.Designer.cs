namespace DPS_B2B.Pages
{
    partial class DPS005_SendResult
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
            this.uC01_GridView1_pnl = new Telerik.WinControls.UI.RadPanel();
            this.tbtnDone = new Telerik.WinControls.UI.RadToggleButton();
            this.tbtnYet = new Telerik.WinControls.UI.RadToggleButton();
            this.tbtnAll = new Telerik.WinControls.UI.RadToggleButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.dBufferTableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uC01_GridView1_pnl)).BeginInit();
            this.uC01_GridView1_pnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbtnDone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbtnYet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbtnAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // dBufferTableLayoutPanel1
            // 
            this.dBufferTableLayoutPanel1.ColumnCount = 1;
            this.dBufferTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dBufferTableLayoutPanel1.Controls.Add(this.uC01_GridView1, 0, 0);
            this.dBufferTableLayoutPanel1.Controls.Add(this.uC01_GridView2, 0, 1);
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
            this.uC01_GridView1.Size = new System.Drawing.Size(1104, 245);
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
            this.uC01_GridView2.Location = new System.Drawing.Point(3, 251);
            this.uC01_GridView2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uC01_GridView2.Name = "uC01_GridView2";
            this.uC01_GridView2.Size = new System.Drawing.Size(1104, 370);
            this.uC01_GridView2.TabIndex = 1;
            // 
            // uC01_GridView1_pnl
            // 
            this.uC01_GridView1_pnl.Controls.Add(this.tbtnDone);
            this.uC01_GridView1_pnl.Controls.Add(this.tbtnYet);
            this.uC01_GridView1_pnl.Controls.Add(this.tbtnAll);
            this.uC01_GridView1_pnl.Controls.Add(this.radLabel1);
            this.uC01_GridView1_pnl.Location = new System.Drawing.Point(400, 19);
            this.uC01_GridView1_pnl.Name = "uC01_GridView1_pnl";
            this.uC01_GridView1_pnl.Size = new System.Drawing.Size(390, 38);
            this.uC01_GridView1_pnl.TabIndex = 1;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.uC01_GridView1_pnl.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // tbtnDone
            // 
            this.tbtnDone.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.tbtnDone.Location = new System.Drawing.Point(293, 8);
            this.tbtnDone.Name = "tbtnDone";
            this.tbtnDone.Size = new System.Drawing.Size(86, 24);
            this.tbtnDone.TabIndex = 1;
            this.tbtnDone.Text = "송신 완료";
            this.tbtnDone.ThemeName = "Office2010Black";
            // 
            // tbtnYet
            // 
            this.tbtnYet.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.tbtnYet.Location = new System.Drawing.Point(201, 8);
            this.tbtnYet.Name = "tbtnYet";
            this.tbtnYet.Size = new System.Drawing.Size(86, 24);
            this.tbtnYet.TabIndex = 1;
            this.tbtnYet.Text = "미 송신";
            this.tbtnYet.ThemeName = "Office2010Black";
            // 
            // tbtnAll
            // 
            this.tbtnAll.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.tbtnAll.Location = new System.Drawing.Point(109, 8);
            this.tbtnAll.Name = "tbtnAll";
            this.tbtnAll.Size = new System.Drawing.Size(86, 24);
            this.tbtnAll.TabIndex = 1;
            this.tbtnAll.Text = "전체";
            this.tbtnAll.ThemeName = "Office2010Black";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radLabel1.Location = new System.Drawing.Point(12, 8);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(78, 25);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "전송 상태";
            // 
            // DB006_SendResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.uC01_GridView1_pnl);
            this.Controls.Add(this.dBufferTableLayoutPanel1);
            this.Name = "DPS005_SendResult";
            this.Size = new System.Drawing.Size(1110, 623);
            this.Load += new System.EventHandler(this.DPS005_SendResult_Load);
            this.dBufferTableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uC01_GridView1_pnl)).EndInit();
            this.uC01_GridView1_pnl.ResumeLayout(false);
            this.uC01_GridView1_pnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbtnDone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbtnYet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbtnAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private lib.Common.Management.DBufferTableLayoutPanel dBufferTableLayoutPanel1;
        private lib.Common.Management.UC01_GridView uC01_GridView1;
        private lib.Common.Management.UC01_GridView uC01_GridView2;
        private Telerik.WinControls.UI.RadPanel uC01_GridView1_pnl;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadToggleButton tbtnDone;
        private Telerik.WinControls.UI.RadToggleButton tbtnYet;
        private Telerik.WinControls.UI.RadToggleButton tbtnAll;
    }
}
