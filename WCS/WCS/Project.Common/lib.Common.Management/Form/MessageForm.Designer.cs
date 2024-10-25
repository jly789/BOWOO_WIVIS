using Telerik.WinForms.Documents.Layout;
namespace lib.Common.Management
{
    partial class MessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageForm));
            this.radButtonConfirm = new Telerik.WinControls.UI.RadButton();
            this.radTilteBar = new Telerik.WinControls.UI.RadTitleBar();
            this.radPanelMessage = new Telerik.WinControls.UI.RadPanel();
            this.radTextBoxAlert = new Telerik.WinControls.UI.RadTextBox();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTilteBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessage)).BeginInit();
            this.radPanelMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxAlert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonConfirm
            // 
            this.radButtonConfirm.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.radButtonConfirm.BackColor = System.Drawing.Color.Transparent;
            this.radButtonConfirm.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radButtonConfirm.BackgroundImage")));
            this.radButtonConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radButtonConfirm.Location = new System.Drawing.Point(115, 177);
            this.radButtonConfirm.Name = "radButtonConfirm";
            this.radButtonConfirm.Size = new System.Drawing.Size(113, 41);
            this.radButtonConfirm.TabIndex = 1;
            this.radButtonConfirm.Text = "확 인";
            this.radButtonConfirm.Click += new System.EventHandler(this.radButtonConfirm_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radButtonConfirm.GetChildAt(0))).Text = "확 인";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonConfirm.GetChildAt(0).GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonConfirm.GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonConfirm.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radButtonConfirm.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonConfirm.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radTilteBar
            // 
            this.radTilteBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTilteBar.LeftImage = ((System.Drawing.Image)(resources.GetObject("radTilteBar.LeftImage")));
            this.radTilteBar.Location = new System.Drawing.Point(1, 1);
            this.radTilteBar.Name = "radTilteBar";
            this.radTilteBar.Size = new System.Drawing.Size(340, 42);
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
            // radPanelMessage
            // 
            this.radPanelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanelMessage.Controls.Add(this.radTextBoxAlert);
            this.radPanelMessage.Location = new System.Drawing.Point(7, 48);
            this.radPanelMessage.MaximumSize = new System.Drawing.Size(327, 0);
            this.radPanelMessage.MinimumSize = new System.Drawing.Size(327, 123);
            this.radPanelMessage.Name = "radPanelMessage";
            // 
            // 
            // 
            this.radPanelMessage.RootElement.MaxSize = new System.Drawing.Size(327, 0);
            this.radPanelMessage.RootElement.MinSize = new System.Drawing.Size(327, 123);
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
            // radTextBoxAlert
            // 
            this.radTextBoxAlert.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTextBoxAlert.AutoSize = false;
            this.radTextBoxAlert.BackColor = System.Drawing.Color.Transparent;
            this.radTextBoxAlert.Enabled = false;
            this.radTextBoxAlert.Location = new System.Drawing.Point(6, 0);
            this.radTextBoxAlert.MaximumSize = new System.Drawing.Size(315, 0);
            this.radTextBoxAlert.MinimumSize = new System.Drawing.Size(315, 110);
            this.radTextBoxAlert.Multiline = true;
            this.radTextBoxAlert.Name = "radTextBoxAlert";
            // 
            // 
            // 
            this.radTextBoxAlert.RootElement.MaxSize = new System.Drawing.Size(315, 0);
            this.radTextBoxAlert.RootElement.MinSize = new System.Drawing.Size(315, 110);
            this.radTextBoxAlert.Size = new System.Drawing.Size(315, 110);
            this.radTextBoxAlert.TabIndex = 0;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxAlert.GetChildAt(0).GetChildAt(0))).StretchVertically = true;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxAlert.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 10F);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radTextBoxAlert.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radTilteBar);
            this.radPanel1.Controls.Add(this.radButtonConfirm);
            this.radPanel1.Controls.Add(this.radPanelMessage);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.MaximumSize = new System.Drawing.Size(342, 0);
            this.radPanel1.MinimumSize = new System.Drawing.Size(342, 221);
            this.radPanel1.Name = "radPanel1";
            // 
            // 
            // 
            this.radPanel1.RootElement.MaxSize = new System.Drawing.Size(342, 0);
            this.radPanel1.RootElement.MinSize = new System.Drawing.Size(342, 221);
            this.radPanel1.Size = new System.Drawing.Size(342, 221);
            this.radPanel1.TabIndex = 2;
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(342, 221);
            this.Controls.Add(this.radPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(342, 221);
            this.MinimumSize = new System.Drawing.Size(342, 221);
            this.Name = "MessageForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(342, 221);
            this.Text = "";
            ((System.ComponentModel.ISupportInitialize)(this.radButtonConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTilteBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessage)).EndInit();
            this.radPanelMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxAlert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTitleBar radTilteBar;
        private Telerik.WinControls.UI.RadPanel radPanelMessage;
        private Telerik.WinControls.UI.RadButton radButtonConfirm;
        private Telerik.WinControls.UI.RadTextBox radTextBoxAlert;
        private Telerik.WinControls.UI.RadPanel radPanel1;
    }
}
