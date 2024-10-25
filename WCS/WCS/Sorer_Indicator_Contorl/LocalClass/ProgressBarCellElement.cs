using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace Sorer_Indicator_Contorl
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
                string[] temp = value.ToString().Split('/');
                int max_value = Convert.ToInt32(temp[1]);
                int work_value = Convert.ToInt32(temp[0]);
                this.radProgressBarElement.Maximum = max_value;
                this.radProgressBarElement.Value1 =  work_value > max_value ? max_value : work_value;
                this.radProgressBarElement.Text = value.ToString();

                this.radProgressBarElement.Value1 = Convert.ToInt16(work_value);                
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
