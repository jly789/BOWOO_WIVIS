namespace Sorter.Pages
{
    partial class S006_SearchTrayinfo
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
            this.RadPanelContents = new Telerik.WinControls.UI.RadPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.UC01_SMSGridView1 = new lib.Common.Management.UC01_GridView();
            this.UC01_SMSGridView2 = new lib.Common.Management.UC01_GridView();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            ((System.ComponentModel.ISupportInitialize)(this.RadPanelContents)).BeginInit();
            this.RadPanelContents.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RadPanelContents
            // 
            this.RadPanelContents.BackColor = System.Drawing.Color.Transparent;
            this.RadPanelContents.Controls.Add(this.tableLayoutPanel1);
            this.RadPanelContents.Location = new System.Drawing.Point(0, 0);
            this.RadPanelContents.Margin = new System.Windows.Forms.Padding(0);
            this.RadPanelContents.Name = "RadPanelContents";
            this.RadPanelContents.Size = new System.Drawing.Size(1110, 625);
            this.RadPanelContents.TabIndex = 1;
            this.RadPanelContents.Visible = false;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(229)))), ((int)(((byte)(237)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(207)))), ((int)(((byte)(221)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(0))).NumberOfColors = 4;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(0))).GradientAngle = 90F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(0))).GradientPercentage2 = 1F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.WhiteSmoke;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(0))).Shape = null;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).BoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).LeftWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).TopWidth = 8F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).RightWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).BottomWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).TopColor = System.Drawing.SystemColors.ButtonFace;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(164)))), ((int)(((byte)(188)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.RadPanelContents.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel1.Controls.Add(this.UC01_SMSGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.UC01_SMSGridView2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1110, 625);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // UC01_SMSGridView1
            // 
            this.UC01_SMSGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UC01_SMSGridView1.GridAllowAddNewRow = false;
            this.UC01_SMSGridView1.GridAllowEditRow = true;
            this.UC01_SMSGridView1.GridMultiSelect = true;
            this.UC01_SMSGridView1.GridShowGroupPanel = false;
            this.UC01_SMSGridView1.GridTitleText = "";
            this.UC01_SMSGridView1.Location = new System.Drawing.Point(3, 2);
            this.UC01_SMSGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.UC01_SMSGridView1.Name = "UC01_SMSGridView1";
            this.UC01_SMSGridView1.Size = new System.Drawing.Size(271, 621);
            this.UC01_SMSGridView1.TabIndex = 0;
            // 
            // UC01_SMSGridView2
            // 
            this.UC01_SMSGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UC01_SMSGridView2.GridAllowAddNewRow = false;
            this.UC01_SMSGridView2.GridAllowEditRow = true;
            this.UC01_SMSGridView2.GridMultiSelect = true;
            this.UC01_SMSGridView2.GridShowGroupPanel = false;
            this.UC01_SMSGridView2.GridTitleText = "";
            this.UC01_SMSGridView2.Location = new System.Drawing.Point(280, 2);
            this.UC01_SMSGridView2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.UC01_SMSGridView2.Name = "UC01_SMSGridView2";
            this.UC01_SMSGridView2.Size = new System.Drawing.Size(827, 621);
            this.UC01_SMSGridView2.TabIndex = 1;
            // 
            // S006_SearchTrayinfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.RadPanelContents);
            this.Name = "S006_SearchTrayinfo";
            this.Size = new System.Drawing.Size(1110, 635);
            ((System.ComponentModel.ISupportInitialize)(this.RadPanelContents)).EndInit();
            this.RadPanelContents.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel RadPanelContents;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private lib.Common.Management.UC01_GridView UC01_SMSGridView1;
        private lib.Common.Management.UC01_GridView UC01_SMSGridView2;
    }
}
