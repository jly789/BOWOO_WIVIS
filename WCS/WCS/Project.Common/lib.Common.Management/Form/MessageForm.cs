using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace lib.Common.Management
{
    public partial class MessageForm : BaseForm
    {
        public bool IsPlaySound = false;
        public MessageForm()
        {
            InitializeComponent();

            this.MaximumSize = new Size(this.Width, 221);
            this.MinimumSize = new Size(this.Width, 221);
        }

        private void radButtonConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        public void Show(string Message)
        {

            if (IsPlaySound) SoundMain.PlayMessageBox();
            this.StartPosition = FormStartPosition.CenterParent;
            this.radTextBoxAlert.Text = Message.Replace(@"\r\n", Environment.NewLine);
            if (this.radTextBoxAlert.Lines.Length > 4)
            {
                this.MaximumSize = new Size(this.Width, 350);
                this.MinimumSize = new Size(this.Width, 350);
            }
            else
            {
                this.MaximumSize = new Size(this.Width, 221);
                this.MinimumSize = new Size(this.Width, 221);
            }

            
            if (LoadingClass.sf != null && !LoadingClass.sf.IsDisposed)
            {
                if (LoadingClass.sf.InvokeRequired)
                {
                    IAsyncResult sfAr = LoadingClass.sf.BeginInvoke(new MethodInvoker(delegate
                    {
                        this.ShowDialog(LoadingClass.sf);
                    }));
                    LoadingClass.sf.EndInvoke(sfAr);
                }
                else
                {
                    this.ShowDialog(LoadingClass.sf);
                }
            }
            else
            {
                this.ShowDialog();
            }
            
        }
        

    }
}
