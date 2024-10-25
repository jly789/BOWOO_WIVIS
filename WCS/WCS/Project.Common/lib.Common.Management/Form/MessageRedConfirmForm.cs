using bowoo.Framework.common;
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
    public partial class MessageRedConfirmForm : BaseForm
    {
        public MessageRedConfirmForm()
        {
            InitializeComponent();
        }
        public bool IsPlaySound = false;
        public void Show(string Message)
        {
            this.radButtonYes.Select();


            if(IsPlaySound) SoundMain.PlayMessageBox();
            this.StartPosition = FormStartPosition.CenterParent;
            this.radTBAlert.Text = Message.Replace(@"\r\n", Environment.NewLine);


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

        private void radButtonYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void radButtonNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radTilteBar_Close(object sender, EventArgs args)
        {
        }
    }
}
