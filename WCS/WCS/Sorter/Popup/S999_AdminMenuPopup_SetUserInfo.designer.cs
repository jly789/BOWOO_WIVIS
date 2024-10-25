namespace Sorter.Popup
{
    partial class S999_AdminMenuPopup_SetUserInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(S999_AdminMenuPopup_SetUserInfo));
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            this.radTitleBarPopup = new Telerik.WinControls.UI.RadTitleBar();
            this.radPanelContents = new Telerik.WinControls.UI.RadPanel();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radBtnCancel = new Telerik.WinControls.UI.RadButton();
            this.radBtnSave = new Telerik.WinControls.UI.RadButton();
            this.radTxtAdminCode = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.UC01_SMSGridView1 = new lib.Common.Management.UC01_GridView();
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBarPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContents)).BeginInit();
            this.radPanelContents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTxtAdminCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radTitleBarPopup
            // 
            this.radTitleBarPopup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTitleBarPopup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radTitleBarPopup.Location = new System.Drawing.Point(-1, 0);
            this.radTitleBarPopup.Name = "radTitleBarPopup";
            this.radTitleBarPopup.Size = new System.Drawing.Size(617, 38);
            this.radTitleBarPopup.TabIndex = 1;
            this.radTitleBarPopup.TabStop = false;
            this.radTitleBarPopup.Text = "사용자 등록 및 권한 설정";
            ((Telerik.WinControls.UI.RadTitleBarElement)(this.radTitleBarPopup.GetChildAt(0))).Text = "사용자 등록 및 권한 설정";
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.Layouts.ImageAndTextLayoutPanel)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3).GetChildAt(1))).Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3).GetChildAt(1).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(2))).TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(2))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radTitleBarPopup.GetChildAt(0).GetChildAt(2).GetChildAt(2))).Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            // 
            // radPanelContents
            // 
            this.radPanelContents.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radPanelContents.BackgroundImage")));
            this.radPanelContents.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radPanelContents.Controls.Add(this.radPanel1);
            this.radPanelContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelContents.Location = new System.Drawing.Point(0, 0);
            this.radPanelContents.Name = "radPanelContents";
            this.radPanelContents.Size = new System.Drawing.Size(615, 456);
            this.radPanelContents.TabIndex = 0;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).TopWidth = 0.5F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).LeftColor = System.Drawing.SystemColors.ControlLight;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).TopColor = System.Drawing.SystemColors.ControlLight;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).RightColor = System.Drawing.SystemColors.ControlLight;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BottomColor = System.Drawing.SystemColors.ControlLight;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).LeftShadowColor = System.Drawing.SystemColors.ButtonShadow;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).TopShadowColor = System.Drawing.SystemColors.ButtonShadow;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).RightShadowColor = System.Drawing.SystemColors.ButtonShadow;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BottomShadowColor = System.Drawing.SystemColors.ButtonShadow;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radBtnCancel);
            this.radPanel1.Controls.Add(this.radBtnSave);
            this.radPanel1.Controls.Add(this.radTxtAdminCode);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Controls.Add(this.UC01_SMSGridView1);
            this.radPanel1.Location = new System.Drawing.Point(12, 49);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(583, 392);
            this.radPanel1.TabIndex = 7;
            // 
            // radBtnCancel
            // 
            this.radBtnCancel.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.radBtnCancel.Location = new System.Drawing.Point(441, 335);
            this.radBtnCancel.Name = "radBtnCancel";
            this.radBtnCancel.Size = new System.Drawing.Size(116, 32);
            this.radBtnCancel.TabIndex = 3;
            this.radBtnCancel.Text = "취소";
            this.radBtnCancel.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radBtnSave
            // 
            this.radBtnSave.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.radBtnSave.Location = new System.Drawing.Point(319, 335);
            this.radBtnSave.Name = "radBtnSave";
            this.radBtnSave.Size = new System.Drawing.Size(116, 32);
            this.radBtnSave.TabIndex = 3;
            this.radBtnSave.Text = "저장";
            this.radBtnSave.Click += new System.EventHandler(this.radBtnSave_Click);
            // 
            // radTxtAdminCode
            // 
            this.radTxtAdminCode.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.radTxtAdminCode.Location = new System.Drawing.Point(166, 334);
            this.radTxtAdminCode.Name = "radTxtAdminCode";
            this.radTxtAdminCode.Size = new System.Drawing.Size(116, 32);
            this.radTxtAdminCode.TabIndex = 2;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.radLabel1.Location = new System.Drawing.Point(16, 335);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(144, 31);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "관리 코드 설정";
            // 
            // UC01_SMSGridView1
            // 
            this.UC01_SMSGridView1.GridAllowAddNewRow = false;
            this.UC01_SMSGridView1.GridAllowEditRow = true;
            this.UC01_SMSGridView1.GridMultiSelect = true;
            this.UC01_SMSGridView1.GridShowGroupPanel = false;
            this.UC01_SMSGridView1.GridTitleText = "";
            this.UC01_SMSGridView1.Location = new System.Drawing.Point(16, 11);
            this.UC01_SMSGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.UC01_SMSGridView1.Name = "UC01_SMSGridView1";
            this.UC01_SMSGridView1.Size = new System.Drawing.Size(541, 318);
            this.UC01_SMSGridView1.TabIndex = 0;
            // 
            // S999_AdminMenuPopup_SetUserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(615, 456);
            this.Controls.Add(this.radTitleBarPopup);
            this.Controls.Add(this.radPanelContents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "S999_AdminMenuPopup_SetUserInfo";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "사용자 등록 및 권한 설정";
            this.Initialized += new System.EventHandler(this.ManageLocationPopup_Initialized);
            ((System.ComponentModel.ISupportInitialize)(this.radTitleBarPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContents)).EndInit();
            this.radPanelContents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTxtAdminCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelContents;
        private Telerik.WinControls.UI.RadTitleBar radTitleBarPopup;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton radBtnCancel;
        private Telerik.WinControls.UI.RadButton radBtnSave;
        private Telerik.WinControls.UI.RadTextBox radTxtAdminCode;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private lib.Common.Management.UC01_GridView UC01_SMSGridView1;

    }
}
