namespace DPS_B2B
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.radPanelContents = new Telerik.WinControls.UI.RadPanel();
            this.radTextBoxPW = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBoxID = new Telerik.WinControls.UI.RadTextBox();
            this.radButtonLogin = new Telerik.WinControls.UI.RadButton();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            this.radPanelLogo = new Telerik.WinControls.UI.RadPanel();
            this.radPanelLoginMain = new Telerik.WinControls.UI.RadPanel();
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.radLabelTitleDetail = new Telerik.WinControls.UI.RadLabel();
            this.RadMessage = new Telerik.WinControls.UI.RadLabel();
            this.object_6e2f56a7_5855_4c0d_b832_c30cedfd630b = new Telerik.WinControls.RootRadElement();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContents)).BeginInit();
            this.radPanelContents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxPW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelLoginMain)).BeginInit();
            this.radPanelLoginMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitleDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RadMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanelContents
            // 
            this.radPanelContents.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radPanelContents.BackgroundImage")));
            this.radPanelContents.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radPanelContents.Controls.Add(this.radTextBoxPW);
            this.radPanelContents.Controls.Add(this.radTextBoxID);
            this.radPanelContents.Controls.Add(this.radButtonLogin);
            this.radPanelContents.Location = new System.Drawing.Point(205, 176);
            this.radPanelContents.Margin = new System.Windows.Forms.Padding(0);
            this.radPanelContents.Name = "radPanelContents";
            // 
            // 
            // 
            this.radPanelContents.RootElement.Shape = this.roundRectShape1;
            this.radPanelContents.Size = new System.Drawing.Size(506, 223);
            this.radPanelContents.TabIndex = 1;
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelContents.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanelContents.GetChildAt(0))).ZIndex = 100;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(0))).Shape = this.roundRectShape1;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BottomWidth = 8F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(148)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).BottomShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(53)))), ((int)(((byte)(180)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelContents.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radTextBoxPW
            // 
            this.radTextBoxPW.AllowShowFocusCues = true;
            this.radTextBoxPW.AutoSize = false;
            this.radTextBoxPW.Location = new System.Drawing.Point(66, 90);
            this.radTextBoxPW.MaxLength = 30;
            this.radTextBoxPW.Name = "radTextBoxPW";
            this.radTextBoxPW.PasswordChar = '*';
            this.radTextBoxPW.Size = new System.Drawing.Size(380, 50);
            this.radTextBoxPW.TabIndex = 2;
            this.radTextBoxPW.Click += new System.EventHandler(this.radTextBox_Click);
            this.radTextBoxPW.Leave += new System.EventHandler(this.radTextBox_Leave);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxPW.GetChildAt(0).GetChildAt(0))).ToolTipText = "패스워드를 입력하세요";
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxPW.GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxPW.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 15F);
            // 
            // radTextBoxID
            // 
            this.radTextBoxID.AutoSize = false;
            this.radTextBoxID.BackColor = System.Drawing.Color.Transparent;
            this.radTextBoxID.Location = new System.Drawing.Point(66, 30);
            this.radTextBoxID.MaxLength = 30;
            this.radTextBoxID.Name = "radTextBoxID";
            this.radTextBoxID.Size = new System.Drawing.Size(380, 50);
            this.radTextBoxID.TabIndex = 1;
            this.radTextBoxID.TextChanged += new System.EventHandler(this.radTextBoxID_TextChanged);
            this.radTextBoxID.Click += new System.EventHandler(this.radTextBox_Click);
            this.radTextBoxID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radTextBox_KeyPress);
            this.radTextBoxID.Leave += new System.EventHandler(this.radTextBox_Leave);
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.radTextBoxID.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).StretchVertically = false;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).ToolTipText = "ID를 입력하세요";
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 15F);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).AutoSize = true;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radTextBoxID.GetChildAt(0).GetChildAt(1))).BackColor2 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radTextBoxID.GetChildAt(0).GetChildAt(1))).BackColor3 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radTextBoxID.GetChildAt(0).GetChildAt(1))).BackColor4 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radTextBoxID.GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.White;
            // 
            // radButtonLogin
            // 
            this.radButtonLogin.BackColor = System.Drawing.Color.Transparent;
            this.radButtonLogin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radButtonLogin.BackgroundImage")));
            this.radButtonLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radButtonLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radButtonLogin.Location = new System.Drawing.Point(67, 159);
            this.radButtonLogin.Name = "radButtonLogin";
            this.radButtonLogin.Size = new System.Drawing.Size(384, 51);
            this.radButtonLogin.TabIndex = 0;
            this.radButtonLogin.TabStop = false;
            this.radButtonLogin.Click += new System.EventHandler(this.radButtonLogin_Click);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonLogin.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // roundRectShape1
            // 
            this.roundRectShape1.Radius = 25;
            // 
            // radPanelLogo
            // 
            this.radPanelLogo.BackColor = System.Drawing.Color.Transparent;
            this.radPanelLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radPanelLogo.BackgroundImage")));
            this.radPanelLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radPanelLogo.ForeColor = System.Drawing.Color.Transparent;
            this.radPanelLogo.Location = new System.Drawing.Point(315, 427);
            this.radPanelLogo.Name = "radPanelLogo";
            this.radPanelLogo.Size = new System.Drawing.Size(298, 41);
            this.radPanelLogo.TabIndex = 0;
            this.radPanelLogo.TabStop = false;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(0))).PaintUsingParentShape = false;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(0))).Enabled = false;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(1))).Width = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(1))).LeftWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(1))).TopWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(1))).RightWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(1))).BottomWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(1))).ShouldPaint = false;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLogo.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radPanelLoginMain
            // 
            this.radPanelLoginMain.BackColor = System.Drawing.Color.Transparent;
            this.radPanelLoginMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radPanelLoginMain.BackgroundImage")));
            this.radPanelLoginMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radPanelLoginMain.Controls.Add(this.radPanelLogo);
            this.radPanelLoginMain.Controls.Add(this.radButtonExit);
            this.radPanelLoginMain.Controls.Add(this.radLabelTitleDetail);
            this.radPanelLoginMain.Controls.Add(this.RadMessage);
            this.radPanelLoginMain.ForeColor = System.Drawing.Color.Transparent;
            this.radPanelLoginMain.Location = new System.Drawing.Point(-6, 3);
            this.radPanelLoginMain.Name = "radPanelLoginMain";
            this.radPanelLoginMain.Size = new System.Drawing.Size(926, 487);
            this.radPanelLoginMain.TabIndex = 0;
            this.radPanelLoginMain.Initialized += new System.EventHandler(this.radPanelLoginMain_Initialized);
            this.radPanelLoginMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(0))).Padding = new System.Windows.Forms.Padding(0);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(0))).Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(0))).Shape = this.roundRectShape1;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radButtonExit
            // 
            this.radButtonExit.BackColor = System.Drawing.Color.Transparent;
            this.radButtonExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radButtonExit.BackgroundImage")));
            this.radButtonExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radButtonExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radButtonExit.Location = new System.Drawing.Point(870, 7);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(40, 40);
            this.radButtonExit.TabIndex = 2;
            this.radButtonExit.Click += new System.EventHandler(this.radButtonExit_Click);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabelTitleDetail
            // 
            this.radLabelTitleDetail.BackColor = System.Drawing.Color.Transparent;
            this.radLabelTitleDetail.Location = new System.Drawing.Point(291, 122);
            this.radLabelTitleDetail.Name = "radLabelTitleDetail";
            this.radLabelTitleDetail.Size = new System.Drawing.Size(345, 47);
            this.radLabelTitleDetail.TabIndex = 1;
            this.radLabelTitleDetail.Text = "Digital Picking System";
            ((Telerik.WinControls.UI.RadLabelElement)(this.radLabelTitleDetail.GetChildAt(0))).Text = "Digital Picking System";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTitleDetail.GetChildAt(0).GetChildAt(2).GetChildAt(1))).TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTitleDetail.GetChildAt(0).GetChildAt(2).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabelTitleDetail.GetChildAt(0).GetChildAt(2).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold);
            // 
            // RadMessage
            // 
            this.RadMessage.BackColor = System.Drawing.Color.Transparent;
            this.RadMessage.Location = new System.Drawing.Point(393, 50);
            this.RadMessage.Name = "RadMessage";
            this.RadMessage.Size = new System.Drawing.Size(141, 89);
            this.RadMessage.TabIndex = 0;
            this.RadMessage.Text = "DPS";
            ((Telerik.WinControls.UI.RadLabelElement)(this.RadMessage.GetChildAt(0))).Text = "DPS";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.RadMessage.GetChildAt(0).GetChildAt(2).GetChildAt(1))).TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.RadMessage.GetChildAt(0).GetChildAt(2).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.RadMessage.GetChildAt(0).GetChildAt(2).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 45F, System.Drawing.FontStyle.Bold);
            // 
            // object_6e2f56a7_5855_4c0d_b832_c30cedfd630b
            // 
            this.object_6e2f56a7_5855_4c0d_b832_c30cedfd630b.Name = "object_6e2f56a7_5855_4c0d_b832_c30cedfd630b";
            this.object_6e2f56a7_5855_4c0d_b832_c30cedfd630b.StretchHorizontally = true;
            this.object_6e2f56a7_5855_4c0d_b832_c30cedfd630b.StretchVertically = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(911, 480);
            this.Controls.Add(this.radPanelContents);
            this.Controls.Add(this.radPanelLoginMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LoginForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "LoginForm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelContents)).EndInit();
            this.radPanelContents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxPW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelLoginMain)).EndInit();
            this.radPanelLoginMain.ResumeLayout(false);
            this.radPanelLoginMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabelTitleDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RadMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelLoginMain;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
        private Telerik.WinControls.UI.RadPanel radPanelContents;
        private Telerik.WinControls.UI.RadLabel RadMessage;
        private Telerik.WinControls.UI.RadLabel radLabelTitleDetail;
        private Telerik.WinControls.UI.RadButton radButtonLogin;
        private Telerik.WinControls.UI.RadButton radButtonExit;
        private Telerik.WinControls.UI.RadTextBox radTextBoxID;
        private Telerik.WinControls.UI.RadTextBox radTextBoxPW;
        private Telerik.WinControls.UI.RadPanel radPanelLogo;
        private Telerik.WinControls.RootRadElement object_6e2f56a7_5855_4c0d_b832_c30cedfd630b;
    }
}
