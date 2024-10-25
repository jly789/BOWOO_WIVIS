namespace Sorter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.radTextBoxPW = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBoxID = new Telerik.WinControls.UI.RadTextBox();
            this.radButtonLogin = new Telerik.WinControls.UI.RadButton();
            this.radPanelLoginMain = new Telerik.WinControls.UI.RadPanel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radDropDownListLanguage = new Telerik.WinControls.UI.RadDropDownList();
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.object_6e2f56a7_5855_4c0d_b832_c30cedfd630b = new Telerik.WinControls.RootRadElement();
            this.office2013DarkTheme1 = new Telerik.WinControls.Themes.Office2013DarkTheme();
            this.office2010BlackTheme1 = new Telerik.WinControls.Themes.Office2010BlackTheme();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxPW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelLoginMain)).BeginInit();
            this.radPanelLoginMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radTextBoxPW
            // 
            this.radTextBoxPW.AllowShowFocusCues = true;
            this.radTextBoxPW.AutoSize = false;
            this.radTextBoxPW.Location = new System.Drawing.Point(317, 274);
            this.radTextBoxPW.MaxLength = 30;
            this.radTextBoxPW.Name = "radTextBoxPW";
            this.radTextBoxPW.NullText = "Password";
            this.radTextBoxPW.PasswordChar = '*';
            this.radTextBoxPW.Size = new System.Drawing.Size(310, 44);
            this.radTextBoxPW.TabIndex = 2;
            this.radTextBoxPW.ThemeName = "Office2013Dark";
            this.radTextBoxPW.Leave += new System.EventHandler(this.radTextBox_Leave);
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.radTextBoxPW.GetChildAt(0))).ForeColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.radTextBoxPW.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxPW.GetChildAt(0).GetChildAt(0))).NullText = "Password";
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxPW.GetChildAt(0).GetChildAt(0))).ToolTipText = "패스워드를 입력하세요";
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxPW.GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.Black;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxPW.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxPW.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            // 
            // radTextBoxID
            // 
            this.radTextBoxID.AutoSize = false;
            this.radTextBoxID.BackColor = System.Drawing.Color.Transparent;
            this.radTextBoxID.ForeColor = System.Drawing.Color.Transparent;
            this.radTextBoxID.Location = new System.Drawing.Point(317, 229);
            this.radTextBoxID.MaxLength = 30;
            this.radTextBoxID.Name = "radTextBoxID";
            this.radTextBoxID.NullText = "User ID";
            this.radTextBoxID.Size = new System.Drawing.Size(310, 44);
            this.radTextBoxID.TabIndex = 1;
            this.radTextBoxID.ThemeName = "Office2013Dark";
            this.radTextBoxID.TextChanged += new System.EventHandler(this.radTextBoxID_TextChanged);
            this.radTextBoxID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radTextBox_KeyPress);
            this.radTextBoxID.Leave += new System.EventHandler(this.radTextBox_Leave);
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.radTextBoxID.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.radTextBoxID.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).NullText = "User ID";
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).StretchVertically = false;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).ToolTipText = "ID를 입력하세요";
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.Black;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextBoxID.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
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
            this.radButtonLogin.Location = new System.Drawing.Point(317, 319);
            this.radButtonLogin.Name = "radButtonLogin";
            this.radButtonLogin.Size = new System.Drawing.Size(310, 42);
            this.radButtonLogin.TabIndex = 0;
            this.radButtonLogin.TabStop = false;
            this.radButtonLogin.ThemeName = "Office2013Dark";
            this.radButtonLogin.Click += new System.EventHandler(this.radButtonLogin_Click);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonLogin.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radPanelLoginMain
            // 
            this.radPanelLoginMain.BackColor = System.Drawing.Color.Transparent;
            this.radPanelLoginMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radPanelLoginMain.BackgroundImage")));
            this.radPanelLoginMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radPanelLoginMain.Controls.Add(this.radLabel1);
            this.radPanelLoginMain.Controls.Add(this.radButtonLogin);
            this.radPanelLoginMain.Controls.Add(this.radTextBoxPW);
            this.radPanelLoginMain.Controls.Add(this.radTextBoxID);
            this.radPanelLoginMain.Controls.Add(this.radDropDownListLanguage);
            this.radPanelLoginMain.Controls.Add(this.radButtonExit);
            this.radPanelLoginMain.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.radPanelLoginMain.ForeColor = System.Drawing.Color.Transparent;
            this.radPanelLoginMain.Location = new System.Drawing.Point(-6, 3);
            this.radPanelLoginMain.Name = "radPanelLoginMain";
            this.radPanelLoginMain.Size = new System.Drawing.Size(926, 487);
            this.radPanelLoginMain.TabIndex = 0;
            this.radPanelLoginMain.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radPanelLoginMain.ThemeName = "Office2013Dark";
            this.radPanelLoginMain.Initialized += new System.EventHandler(this.radPanelLoginMain_Initialized);
            this.radPanelLoginMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(0))).Padding = new System.Windows.Forms.Padding(0);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(0))).Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(1))).Width = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(1))).LeftWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(1))).TopWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(1))).RightWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(1))).BottomWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanelLoginMain.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(740, 440);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(83, 25);
            this.radLabel1.TabIndex = 4;
            this.radLabel1.Text = "Language";
            ((Telerik.WinControls.UI.RadLabelElement)(this.radLabel1.GetChildAt(0))).Text = "Language";
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabel1.GetChildAt(0).GetChildAt(2).GetChildAt(1))).TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabel1.GetChildAt(0).GetChildAt(2).GetChildAt(1))).ForeColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radLabel1.GetChildAt(0).GetChildAt(2).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            // 
            // radDropDownListLanguage
            // 
            this.radDropDownListLanguage.Location = new System.Drawing.Point(829, 442);
            this.radDropDownListLanguage.Name = "radDropDownListLanguage";
            this.radDropDownListLanguage.Size = new System.Drawing.Size(76, 19);
            this.radDropDownListLanguage.TabIndex = 3;
            this.radDropDownListLanguage.Text = "radDropDownListLanguage";
            this.radDropDownListLanguage.ThemeName = "Office2013Dark";
            ((Telerik.WinControls.UI.RadDropDownListElement)(this.radDropDownListLanguage.GetChildAt(0))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(0))).Width = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(0))).LeftWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(0))).TopWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(0))).RightWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(0))).BottomWidth = 0F;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.UI.StackLayoutElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderWidth = 0F;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderLeftWidth = 0F;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderTopWidth = 0F;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderRightWidth = 0F;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderBottomWidth = 0F;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderInnerColor = System.Drawing.Color.CornflowerBlue;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderInnerColor2 = System.Drawing.Color.CornflowerBlue;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderInnerColor3 = System.Drawing.Color.CornflowerBlue;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BorderInnerColor4 = System.Drawing.Color.CornflowerBlue;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.UI.RadDropDownListEditableAreaElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadDropDownTextBoxElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(0))).Text = "radDropDownListLanguage";
            ((Telerik.WinControls.UI.RadDropDownTextBoxElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(0))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.UI.RadDropDownListArrowButtonElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(1))).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadDropDownListArrowButtonElement)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(1))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).Width = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).TopWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).RightWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).BottomWidth = 0F;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radDropDownListLanguage.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            // 
            // radButtonExit
            // 
            this.radButtonExit.BackColor = System.Drawing.Color.Transparent;
            this.radButtonExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radButtonExit.BackgroundImage")));
            this.radButtonExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radButtonExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radButtonExit.Location = new System.Drawing.Point(859, 9);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(46, 46);
            this.radButtonExit.TabIndex = 2;
            this.radButtonExit.Click += new System.EventHandler(this.radButtonExit_Click);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radButtonExit.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
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
            this.Controls.Add(this.radPanelLoginMain);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LoginForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "LoginForm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxPW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelLoginMain)).EndInit();
            this.radPanelLoginMain.ResumeLayout(false);
            this.radPanelLoginMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanelLoginMain;
        private Telerik.WinControls.UI.RadButton radButtonLogin;
        private Telerik.WinControls.UI.RadButton radButtonExit;
        private Telerik.WinControls.UI.RadTextBox radTextBoxID;
        private Telerik.WinControls.UI.RadTextBox radTextBoxPW;
        private Telerik.WinControls.RootRadElement object_6e2f56a7_5855_4c0d_b832_c30cedfd630b;
        private Telerik.WinControls.UI.RadDropDownList radDropDownListLanguage;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.Themes.Office2013DarkTheme office2013DarkTheme1;
        private Telerik.WinControls.Themes.Office2010BlackTheme office2010BlackTheme1;
    }
}
