using Telerik.WinForms.Documents.Layout;
namespace lib.Common.Management
{
    partial class MessageConfirmForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageConfirmForm));
            this.radPanelContents = new Telerik.WinControls.UI.RadPanel();
            this.radButtonNo = new Telerik.WinControls.UI.RadButton();
            this.radButtonYes = new Telerik.WinControls.UI.RadButton();
            this.radTilteBar = new Telerik.WinControls.UI.RadTitleBar();
            this.radPanelMessage = new Telerik.WinControls.UI.RadPanel();
            this.radTBAlert = new Telerik.WinControls.UI.RadTextBox();
            this.object_37002063_d7d0_4d02_87f3_f5a39b8d26c6 = new Telerik.WinControls.RootRadElement();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContents)).BeginInit();
            this.radPanelContents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonYes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTilteBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessage)).BeginInit();
            this.radPanelMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTBAlert)).BeginInit();
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
            this.radPanelContents.MaximumSize = new System.Drawing.Size(342, 221);
            this.radPanelContents.MinimumSize = new System.Drawing.Size(342, 221);
            this.radPanelContents.Name = "radPanelContents";
            // 
            // 
            // 
            this.radPanelContents.RootElement.MaxSize = new System.Drawing.Size(342, 221);
            this.radPanelContents.RootElement.MinSize = new System.Drawing.Size(342, 221);
            this.radPanelContents.Size = new System.Drawing.Size(342, 221);
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
            this.radButtonNo.BackColor = System.Drawing.Color.Transparent;
            this.radButtonNo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radButtonNo.BackgroundImage")));
            this.radButtonNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radButtonNo.Location = new System.Drawing.Point(173, 177);
            this.radButtonNo.Name = "radButtonNo";
            this.radButtonNo.Size = new System.Drawing.Size(113, 41);
            this.radButtonNo.TabIndex = 2;
            this.radButtonNo.Text = "아니오";
            this.radButtonNo.Click += new System.EventHandler(this.radButtonNo_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonNo.GetChildAt(0))).Text = "아니오";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonNo.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonYes
            // 
            this.radButtonYes.BackColor = System.Drawing.Color.Transparent;
            this.radButtonYes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radButtonYes.BackgroundImage")));
            this.radButtonYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radButtonYes.Location = new System.Drawing.Point(63, 177);
            this.radButtonYes.Name = "radButtonYes";
            this.radButtonYes.Size = new System.Drawing.Size(113, 41);
            this.radButtonYes.TabIndex = 1;
            this.radButtonYes.Text = "예";
            this.radButtonYes.Click += new System.EventHandler(this.radButtonYes_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonYes.GetChildAt(0))).Text = "예";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonYes.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radTilteBar
            // 
            this.radTilteBar.LeftImage = ((System.Drawing.Image)(resources.GetObject("radTilteBar.LeftImage")));
            this.radTilteBar.Location = new System.Drawing.Point(1, 1);
            this.radTilteBar.Name = "radTilteBar";
            this.radTilteBar.Size = new System.Drawing.Size(340, 42);
            this.radTilteBar.TabIndex = 0;
            this.radTilteBar.TabStop = false;
            this.radTilteBar.Close += new Telerik.WinControls.UI.TitleBarSystemEventHandler(this.radTilteBar_Close);
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
            // radPanelMessage
            // 
            this.radPanelMessage.Controls.Add(this.radTBAlert);
            this.radPanelMessage.Location = new System.Drawing.Point(7, 48);
            this.radPanelMessage.Name = "radPanelMessage";
            this.radPanelMessage.Size = new System.Drawing.Size(327, 123);
            this.radPanelMessage.TabIndex = 1;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).BoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).BorderDrawMode = Telerik.WinControls.BorderDrawModes.LeftOverBottom;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).LeftWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).TopWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).RightWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).BottomWidth = 10F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).TopColor = System.Drawing.Color.CornflowerBlue;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).BottomColor = System.Drawing.Color.CornflowerBlue;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelMessage.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            // 
            // radTBAlert
            // 
            this.radTBAlert.AcceptsReturn = true;
            this.radTBAlert.AutoSize = false;
            this.radTBAlert.BackColor = System.Drawing.Color.Transparent;
            this.radTBAlert.Location = new System.Drawing.Point(6, 0);
            this.radTBAlert.Multiline = true;
            this.radTBAlert.Name = "radTBAlert";
            this.radTBAlert.Size = new System.Drawing.Size(315, 110);
            this.radTBAlert.TabIndex = 0;
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.radTBAlert.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTBAlert.GetChildAt(0).GetChildAt(0))).StretchVertically = true;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTBAlert.GetChildAt(0).GetChildAt(0))).Enabled = false;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTBAlert.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 10F);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTBAlert.GetChildAt(0).GetChildAt(0))).AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTBAlert.GetChildAt(0).GetChildAt(0))).CanFocus = false;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTBAlert.GetChildAt(0).GetChildAt(0))).MinSize = new System.Drawing.Size(310, 18);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTBAlert.GetChildAt(0).GetChildAt(0))).MaxSize = new System.Drawing.Size(310, 80);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTBAlert.GetChildAt(0).GetChildAt(0))).StretchHorizontally = true;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radTBAlert.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // object_37002063_d7d0_4d02_87f3_f5a39b8d26c6
            // 
            this.object_37002063_d7d0_4d02_87f3_f5a39b8d26c6.Name = "object_37002063_d7d0_4d02_87f3_f5a39b8d26c6";
            this.object_37002063_d7d0_4d02_87f3_f5a39b8d26c6.StretchHorizontally = true;
            this.object_37002063_d7d0_4d02_87f3_f5a39b8d26c6.StretchVertically = true;
            // 
            // MessageConfirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(342, 221);
            this.Controls.Add(this.radPanelMessage);
            this.Controls.Add(this.radPanelContents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(342, 221);
            this.MinimumSize = new System.Drawing.Size(342, 221);
            this.Name = "MessageConfirmForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(342, 221);
            this.Text = "MessageForm";
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContents)).EndInit();
            this.radPanelContents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonYes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTilteBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessage)).EndInit();
            this.radPanelMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTBAlert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelContents;
        private Telerik.WinControls.UI.RadTitleBar radTilteBar;
        private Telerik.WinControls.UI.RadPanel radPanelMessage;
        private Telerik.WinControls.UI.RadButton radButtonYes;
        private Telerik.WinControls.UI.RadTextBox radTBAlert;
        private Telerik.WinControls.RootRadElement object_37002063_d7d0_4d02_87f3_f5a39b8d26c6;
        private Telerik.WinControls.UI.RadButton radButtonNo;
    }
}
