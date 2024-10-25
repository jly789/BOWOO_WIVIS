﻿namespace lib.Common.Management
{
    partial class ProgressFormW
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressFormW));
            this.radPanelContent = new Telerik.WinControls.UI.RadPanel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.radLabelTitleBottom = new Telerik.WinControls.UI.RadLabel();
            this.radLabelTtileTop = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContent)).BeginInit();
            this.radPanelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitleBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTtileTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelContent
            // 
            this.radPanelContent.BackColor = System.Drawing.Color.White;
            this.radPanelContent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radPanelContent.Controls.Add(this.progressBar1);
            this.radPanelContent.Controls.Add(this.radLabelTitleBottom);
            this.radPanelContent.Controls.Add(this.radLabelTtileTop);
            this.radPanelContent.Location = new System.Drawing.Point(465, 295);
            this.radPanelContent.Name = "radPanelContent";
            this.radPanelContent.Size = new System.Drawing.Size(350, 130);
            this.radPanelContent.TabIndex = 2;
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelContent.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelContent.GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelContent.GetChildAt(0))).Shape = null;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelContent.GetChildAt(0).GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelContent.GetChildAt(0).GetChildAt(0))).Shape = null;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContent.GetChildAt(0).GetChildAt(1))).Width = 3F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContent.GetChildAt(0).GetChildAt(1))).LeftWidth = 3F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContent.GetChildAt(0).GetChildAt(1))).TopWidth = 3F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContent.GetChildAt(0).GetChildAt(1))).RightWidth = 3F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContent.GetChildAt(0).GetChildAt(1))).BottomWidth = 3F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContent.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.progressBar1.Location = new System.Drawing.Point(45, 64);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(262, 18);
            this.progressBar1.TabIndex = 3;
            // 
            // radLabelTitleBottom
            // 
            this.radLabelTitleBottom.AutoSize = false;
            this.radLabelTitleBottom.BackColor = System.Drawing.Color.Transparent;
            this.radLabelTitleBottom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radLabelTitleBottom.BackgroundImage")));
            this.radLabelTitleBottom.Location = new System.Drawing.Point(114, 93);
            this.radLabelTitleBottom.Name = "radLabelTitleBottom";
            this.radLabelTitleBottom.Size = new System.Drawing.Size(124, 10);
            this.radLabelTitleBottom.TabIndex = 2;
            ((Telerik.WinControls.UI.RadLabelElement)(this.radLabelTitleBottom.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTitleBottom.GetChildAt(0).GetChildAt(2).GetChildAt(1))).TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTitleBottom.GetChildAt(0).GetChildAt(2).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTitleBottom.GetChildAt(0).GetChildAt(2).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // radLabelTtileTop
            // 
            this.radLabelTtileTop.AutoSize = false;
            this.radLabelTtileTop.BackColor = System.Drawing.Color.Transparent;
            this.radLabelTtileTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radLabelTtileTop.BackgroundImage")));
            this.radLabelTtileTop.Location = new System.Drawing.Point(129, 36);
            this.radLabelTtileTop.Name = "radLabelTtileTop";
            this.radLabelTtileTop.Size = new System.Drawing.Size(95, 16);
            this.radLabelTtileTop.TabIndex = 1;
            ((Telerik.WinControls.UI.RadLabelElement)(this.radLabelTtileTop.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTtileTop.GetChildAt(0).GetChildAt(2).GetChildAt(1))).TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTtileTop.GetChildAt(0).GetChildAt(2).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(105)))), ((int)(((byte)(13)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTtileTop.GetChildAt(0).GetChildAt(2).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // ProgressFormW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.radPanelContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1280, 720);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "ProgressFormW";
            this.Opacity = 0.5D;
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1280, 720);
            this.Text = "ProgressPopup";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContent)).EndInit();
            this.radPanelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitleBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTtileTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelContent;
        private Telerik.WinControls.UI.RadLabel radLabelTitleBottom;
        private Telerik.WinControls.UI.RadLabel radLabelTtileTop;
        public System.Windows.Forms.ProgressBar progressBar1;
    }
}
