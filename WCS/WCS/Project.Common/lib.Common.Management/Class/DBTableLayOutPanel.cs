﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lib.Common.Management
{
    public class DBufferTableLayoutPanel : TableLayoutPanel
    {

        public DBufferTableLayoutPanel()
        {

            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);




        }
        public DBufferTableLayoutPanel(IContainer container)
        {

            container.Add(this);

            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);

        }

    }

}
