using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using System.Windows.Forms;


namespace lib.Common.Management
{
    public partial class DBradPanel : RadPanel
    {
        public DBradPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);
        }

        public DBradPanel(IContainer container)
        {
            container.Add(this);
            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);
        }
        //protected override void OnNotifyMessage(Message m)
        //{
        //    if(m.Msg != 0x14)
        //        base.OnNotifyMessage(m);
        //}
    }
}
