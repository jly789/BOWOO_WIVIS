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
    public partial class ProgressFormW : lib.Common.Management.BaseForm
    {
        public ProgressFormW()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        public void SetLocation(Point ParentPosition)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = ParentPosition.X;
            this.Top = ParentPosition.Y;
        }
    }
}
