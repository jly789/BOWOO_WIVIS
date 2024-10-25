namespace lib.Common.Management
{
    partial class LoadingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingForm));
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.BackColor = System.Drawing.Color.Transparent;
            this.radLabel1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel1.ForeColor = System.Drawing.Color.Transparent;
            this.radLabel1.Image = ((System.Drawing.Image)(resources.GetObject("radLabel1.Image")));
            this.radLabel1.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radLabel1.Location = new System.Drawing.Point(0, 0);
            this.radLabel1.MaximumSize = new System.Drawing.Size(1280, 720);
            this.radLabel1.MinimumSize = new System.Drawing.Size(1280, 720);
            this.radLabel1.Name = "radLabel1";
            // 
            // 
            // 
            this.radLabel1.RootElement.MaxSize = new System.Drawing.Size(1280, 720);
            this.radLabel1.RootElement.MinSize = new System.Drawing.Size(1280, 720);
            this.radLabel1.Size = new System.Drawing.Size(1280, 720);
            this.radLabel1.TabIndex = 0;
            ((Telerik.WinControls.UI.RadLabelElement)(this.radLabel1.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadLabelElement)(this.radLabel1.GetChildAt(0))).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.ImagePrimitive)(this.radLabel1.GetChildAt(0).GetChildAt(2).GetChildAt(0))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabel1.GetChildAt(0).GetChildAt(2).GetChildAt(1))).TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.radLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1280, 720);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "LoadingForm";
            this.Opacity = 0.3D;
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1280, 720);
            this.Text = "LoadingForm";
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
