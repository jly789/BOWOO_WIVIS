using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Telerik.WinControls.UI;

namespace lib.Common.Management
{
    public partial class Calendar : BaseControl
    {
        //public event EventHandler radDatePickerCal_ValueChanged;

        /// <summary>
        /// Set Width
        /// </summary>
        [ComVisible(true)]
        [DefaultValue(90)]
        public int CalWidth
        {
            set
            {
                this.radDatePickerCal.Width = value;
            }
            get { return this.radDatePickerCal.Width; }
        }

        /// <summary>
        /// Set Height
        /// </summary>
        [ComVisible(true)]
        [DefaultValue(20)]
        public int CalHeight
        {
            set
            {
                this.radDatePickerCal.Height = value;
            }
            get { return this.radDatePickerCal.Height; } 
        }

        /// <summary>
        /// Set Date
        /// </summary>
        [ComVisible(true)]
        public String DefaultDate
        {
            set
            {
                this.radDatePickerCal.Value = Convert.ToDateTime(value);
                this.radDatePickerCal.Text = value;
            }
            get { return this.radDatePickerCal.Text; }
        }


        public Calendar()
        {
            InitializeComponent();
            makeControl();
        }

        private void makeControl()
        {
            this.radDatePickerCal.CustomFormat = "yyyy-MM-dd";
            this.radDatePickerCal.DateTimePickerElement.TextBoxElement.EnableMouseWheel = false;

            //init Value
            this.radDatePickerCal.Width = this.CalWidth == 0 ? 90 : this.CalWidth;
            this.radDatePickerCal.Height = this.CalHeight == 0 ? 20 : this.CalHeight;
            this.radDatePickerCal.Text = this.DefaultDate == string.Empty ? DateTime.Now.ToShortDateString() : this.DefaultDate;
            this.radDatePickerCal.Value = this.DefaultDate == string.Empty ? Convert.ToDateTime(DateTime.Now.ToShortDateString()) : Convert.ToDateTime(this.DefaultDate);
        }

        /// <summary>
        /// Block User Input Value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radDatePickerCal_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
