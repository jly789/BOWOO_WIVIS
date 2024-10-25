namespace Sorer_Indicator_Contorl
{
    partial class StatusBoardForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusBoardForm));
            this.remainOrder = new Telerik.WinControls.UI.RadLabel();
            this.workOrder = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.totalOrder = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.totalProgressBar = new Telerik.WinControls.UI.RadProgressBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.text_label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.remainOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // remainOrder
            // 
            this.remainOrder.AutoSize = false;
            this.remainOrder.BackColor = System.Drawing.Color.Transparent;
            this.remainOrder.Font = new System.Drawing.Font("Segoe UI", 50F, System.Drawing.FontStyle.Bold);
            this.remainOrder.ForeColor = System.Drawing.Color.Red;
            this.remainOrder.Location = new System.Drawing.Point(1678, 898);
            this.remainOrder.Name = "remainOrder";
            this.remainOrder.Size = new System.Drawing.Size(231, 99);
            this.remainOrder.TabIndex = 110;
            this.remainOrder.Text = "0";
            this.remainOrder.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // workOrder
            // 
            this.workOrder.AutoSize = false;
            this.workOrder.BackColor = System.Drawing.Color.Transparent;
            this.workOrder.BorderVisible = true;
            this.workOrder.Font = new System.Drawing.Font("Segoe UI", 50F, System.Drawing.FontStyle.Bold);
            this.workOrder.ForeColor = System.Drawing.Color.Blue;
            this.workOrder.Location = new System.Drawing.Point(966, 186);
            this.workOrder.Name = "workOrder";
            this.workOrder.Size = new System.Drawing.Size(231, 99);
            this.workOrder.TabIndex = 108;
            this.workOrder.Text = "0";
            this.workOrder.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radLabel4
            // 
            this.radLabel4.BackColor = System.Drawing.Color.Transparent;
            this.radLabel4.Font = new System.Drawing.Font("Segoe UI", 50F);
            this.radLabel4.Location = new System.Drawing.Point(1373, 898);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(299, 99);
            this.radLabel4.TabIndex = 109;
            this.radLabel4.Text = "남은오더";
            // 
            // radLabel3
            // 
            this.radLabel3.BackColor = System.Drawing.Color.Transparent;
            this.radLabel3.Font = new System.Drawing.Font("Segoe UI", 70F);
            this.radLabel3.Location = new System.Drawing.Point(661, 188);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(418, 138);
            this.radLabel3.TabIndex = 105;
            this.radLabel3.Text = "완료수량";
            // 
            // totalOrder
            // 
            this.totalOrder.AutoSize = false;
            this.totalOrder.BackColor = System.Drawing.Color.Transparent;
            this.totalOrder.BorderVisible = true;
            this.totalOrder.Font = new System.Drawing.Font("Segoe UI", 70F, System.Drawing.FontStyle.Bold);
            this.totalOrder.Location = new System.Drawing.Point(335, 175);
            this.totalOrder.Name = "totalOrder";
            this.totalOrder.Size = new System.Drawing.Size(315, 138);
            this.totalOrder.TabIndex = 106;
            this.totalOrder.Text = "99999";
            this.totalOrder.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radLabel1
            // 
            this.radLabel1.BackColor = System.Drawing.Color.Transparent;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 70F);
            this.radLabel1.Location = new System.Drawing.Point(7, 175);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(322, 138);
            this.radLabel1.TabIndex = 107;
            this.radLabel1.Text = "총오더";
            // 
            // totalProgressBar
            // 
            this.totalProgressBar.Font = new System.Drawing.Font("Segoe UI", 35F, System.Drawing.FontStyle.Bold);
            this.totalProgressBar.Location = new System.Drawing.Point(0, 1002);
            this.totalProgressBar.Name = "totalProgressBar";
            this.totalProgressBar.Size = new System.Drawing.Size(1918, 70);
            this.totalProgressBar.TabIndex = 104;
            this.totalProgressBar.Text = "0%";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(1726, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(174, 47);
            this.pictureBox2.TabIndex = 103;
            this.pictureBox2.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // text_label2
            // 
            this.text_label2.AutoSize = true;
            this.text_label2.BackColor = System.Drawing.Color.Transparent;
            this.text_label2.Font = new System.Drawing.Font("휴먼옛체", 65.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.text_label2.ForeColor = System.Drawing.Color.Blue;
            this.text_label2.Location = new System.Drawing.Point(645, 12);
            this.text_label2.Name = "text_label2";
            this.text_label2.Size = new System.Drawing.Size(718, 92);
            this.text_label2.TabIndex = 111;
            this.text_label2.Text = "SORTER SYSTEM";
            // 
            // StatusBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Sorer_Indicator_Contorl.Properties.Resources.popupbackground2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1912, 1040);
            this.Controls.Add(this.text_label2);
            this.Controls.Add(this.remainOrder);
            this.Controls.Add(this.workOrder);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.totalOrder);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.totalProgressBar);
            this.Controls.Add(this.pictureBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StatusBoardForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StatusBoard";
            this.Load += new System.EventHandler(this.StatusBoard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.remainOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private Telerik.WinControls.UI.RadLabel workOrder;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel totalOrder;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadProgressBar totalProgressBar;
        private Telerik.WinControls.UI.RadLabel remainOrder;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label text_label2;
    }
}
