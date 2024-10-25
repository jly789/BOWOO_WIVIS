using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace lib.Common.Management
{
    class ProgressBarCellElement : GridDataCellElement
    {
        private RadProgressBarElement radProgressBarElement;

        protected override void CreateChildElements()
        {
            base.CreateChildElements();
            radProgressBarElement = new RadProgressBarElement();
            this.Children.Add(radProgressBarElement);
        }

        public ProgressBarCellElement(GridViewColumn column, GridRowElement row) : base(column, row)
        {
        }

        protected override void SetContentCore(object value)
        {
            if (this.Value != null && this.Value != DBNull.Value)
            {
                this.radProgressBarElement.Value1 = Convert.ToInt16(this.Value);
            }
        }


        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(GridDataCellElement);
            }
        }


        public override bool IsCompatible(GridViewColumn data, object context)
        {
            return data is ProgressBarColumn && context is GridDataRowElement;
        }
    }
}
