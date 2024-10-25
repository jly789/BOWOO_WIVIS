using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace Sorer_Indicator_Contorl
{
    class ProgressBarColumn : GridViewDataColumn
    {
        public ProgressBarColumn(string fieldName) : base(fieldName)
        {
        }

        public override Type GetCellType(GridViewRowInfo row)
        {
            if (row is GridViewDataRowInfo)
            {
                return typeof(ProgressBarCellElement);
            }
            return base.GetCellType(row);
        }
    }
}
