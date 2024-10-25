using Telerik.WinForms.Documents.Layout;
namespace DPS_B2B.Popup
{
    partial class DPS001_CheckBoxSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DPS001_CheckBoxSelect));
            this.radPanelContents = new Telerik.WinControls.UI.RadPanel();
            this.radButtonNo = new Telerik.WinControls.UI.RadButton();
            this.radButtonYes = new Telerik.WinControls.UI.RadButton();
            this.radTilteBar = new Telerik.WinControls.UI.RadTitleBar();
            this.object_37002063_d7d0_4d02_87f3_f5a39b8d26c6 = new Telerik.WinControls.RootRadElement();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContents)).BeginInit();
            this.radPanelContents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonYes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTilteBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelContents
            // 
            this.radPanelContents.Controls.Add(this.radButtonNo);
            this.radPanelContents.Controls.Add(this.radButtonYes);
            this.radPanelContents.Controls.Add(this.radTilteBar);
            this.radPanelContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanelContents.Location = new System.Drawing.Point(0, 0);
            this.radPanelContents.Name = "radPanelContents";
            this.radPanelContents.Size = new System.Drawing.Size(364, 176);
            this.radPanelContents.TabIndex = 0;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).LeftWidth = 2F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).TopWidth = 2F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).RightWidth = 2F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BottomWidth = 2F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).TopColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radButtonNo
            // 
            this.radButtonNo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.radButtonNo.BackColor = System.Drawing.Color.Transparent;
            this.radButtonNo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radButtonNo.BackgroundImage")));
            this.radButtonNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radButtonNo.Location = new System.Drawing.Point(210, 85);
            this.radButtonNo.Name = "radButtonNo";
            this.radButtonNo.Size = new System.Drawing.Size(113, 41);
            this.radButtonNo.TabIndex = 2;
            this.radButtonNo.Text = "선택 해제";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonNo.GetChildAt(0))).Text = "선택 해제";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonYes
            // 
            this.radButtonYes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.radButtonYes.BackColor = System.Drawing.Color.Transparent;
            this.radButtonYes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radButtonYes.BackgroundImage")));
            this.radButtonYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radButtonYes.Location = new System.Drawing.Point(47, 85);
            this.radButtonYes.Name = "radButtonYes";
            this.radButtonYes.Size = new System.Drawing.Size(113, 41);
            this.radButtonYes.TabIndex = 1;
            this.radButtonYes.Text = "선택 체크";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonYes.GetChildAt(0))).Text = "선택 체크";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radTilteBar
            // 
            this.radTilteBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTilteBar.LeftImage = ((System.Drawing.Image)(resources.GetObject("radTilteBar.LeftImage")));
            this.radTilteBar.Location = new System.Drawing.Point(0, 2);
            this.radTilteBar.Name = "radTilteBar";
            this.radTilteBar.Size = new System.Drawing.Size(364, 42);
            this.radTilteBar.TabIndex = 0;
            this.radTilteBar.TabStop = false;
            ((Telerik.WinControls.UI.RadTitleBarElement)(this.radTilteBar.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radTilteBar.GetChildAt(0).GetChildAt(0))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.ImagePrimitive)(this.radTilteBar.GetChildAt(0).GetChildAt(2).GetChildAt(0))).ImageLayout = System.Windows.Forms.ImageLayout.Center;
            ((Telerik.WinControls.Primitives.ImagePrimitive)(this.radTilteBar.GetChildAt(0).GetChildAt(2).GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.Primitives.ImagePrimitive)(this.radTilteBar.GetChildAt(0).GetChildAt(2).GetChildAt(0))).ImageScaling = Telerik.WinControls.Enumerations.ImageScaling.SizeToFit;
            ((Telerik.WinControls.Primitives.ImagePrimitive)(this.radTilteBar.GetChildAt(0).GetChildAt(2).GetChildAt(0))).Margin = new System.Windows.Forms.Padding(5, 10, 0, 0);
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.radTilteBar.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.UI.RadImageButtonElement)(this.radTilteBar.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.Primitives.ImagePrimitive)(this.radTilteBar.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(3).GetChildAt(1).GetChildAt(0))).Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            // 
            // object_37002063_d7d0_4d02_87f3_f5a39b8d26c6
            // 
            this.object_37002063_d7d0_4d02_87f3_f5a39b8d26c6.Name = "object_37002063_d7d0_4d02_87f3_f5a39b8d26c6";
            this.object_37002063_d7d0_4d02_87f3_f5a39b8d26c6.StretchHorizontally = true;
            this.object_37002063_d7d0_4d02_87f3_f5a39b8d26c6.StretchVertically = true;
            // 
            // DPS001_CheckBoxSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(364, 176);
            this.Controls.Add(this.radPanelContents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DPS001_CheckBoxSelect";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(0, 0);
            this.Text = "존 번호 입력";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContents)).EndInit();
            this.radPanelContents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonYes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTilteBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelContents;
        private Telerik.WinControls.UI.RadTitleBar radTilteBar;
        private Telerik.WinControls.UI.RadButton radButtonYes;
        private Telerik.WinControls.RootRadElement object_37002063_d7d0_4d02_87f3_f5a39b8d26c6;
        private Telerik.WinControls.UI.RadButton radButtonNo;
    }
}
