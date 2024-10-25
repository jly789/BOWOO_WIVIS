using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace lib.Common.Management
{
    public partial class LoadingForm : BaseForm
    {
        delegate void StringParameterDelegate(string text);
        //delegate void StringParameterWithStatusDelegate(string text, TypeOfMessage status);
        delegate void LoadingShowCloseDelegate();


        bool CloseloadingScreenFlag = false;

        public LoadingForm()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

            InitializeComponent();
            
        }

        public void SetLocation(Point ParentPosition) {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = ParentPosition.X;
            this.Top = ParentPosition.Y;
        }


        public void ShowLoadingScreen()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new LoadingShowCloseDelegate(LoadingClass.ShowSplashScreen));
                
                return;
            }
            //this.TopMost = true;
            
            
            Application.Run(this);
            this.BringToFront();
            
        }

        public void CloseLoadingScreen()
        {
            if (InvokeRequired)
            {
                IAsyncResult result = BeginInvoke(new LoadingShowCloseDelegate(LoadingClass.CloseSplashScreen));
                EndInvoke(result);
                return;
            }
            CloseloadingScreenFlag = true;
            this.Close();

        }

        private void LoadingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseloadingScreenFlag)
            {
                e.Cancel = true;
            }
        }

    }
}
