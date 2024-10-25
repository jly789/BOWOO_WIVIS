using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Sorer_Indicator_Contorl
{
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this System.Windows.Forms.Control ctr, bool setting)
        {
            Type dgvType = ctr.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(ctr, setting, null);
        }
    }
}
