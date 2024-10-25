using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.Reporting;
using Telerik.ReportViewer;
using Telerik.WinControls.UI;

namespace lib.Common.Management
{
    public class PrintForm : RadPrintDocument
    {
        public string LeftUpperText = string.Empty;
        public static int currentpage = 1;
        public RadGridView SelectedGrid = null;
        public RadPrintPreviewDialog dialog = null;
        private PrintPopup printPopup;

        /// <summary>
        /// Setting Pring Data
        /// </summary>
        /// <param name="PrintGrid">Selected Grid</param>
        /// <param name="PrintLabel">Selected Label</param>
        public PrintForm(PrintPopup printPopup, RadGridView PrintGrid, RadLabel PrintLabel)
        {
            this.printPopup = printPopup;
            string PrintText = PrintLabel.Text.IndexOf("<") > -1 ? PrintLabel.Text.Substring(0, PrintLabel.Text.IndexOf("<")) : PrintLabel.Text;
            SelectedGrid = PrintGrid;

            PrintGridView(PrintGrid, PrintText);
        }

        /// <summary>
        /// Setting Print Page and PrintView Dialog form
        /// </summary>
        /// <param name="PrintGrid"></param>
        /// <param name="Title"></param>
        private void PrintGridView(RadGridView PrintGrid,string Title)
        {
            // Set Print Page
            this.HeaderHeight = 35;
            this.Margins = new System.Drawing.Printing.Margins(5, 35, 5, 10);
            this.AssociatedObject = PrintGrid;
            this.LeftUpperText = Title;

            bool ExistsCheckBox = false;

            //Set Grid by GridViewCheckBox Condition
            foreach (GridViewColumn item in PrintGrid.Columns)
            {
                if (item.GetType().Name.Equals("GridViewCheckBoxColumn"))
                {
                    ExistsCheckBox = true;
                }
            }

            if (ExistsCheckBox)
            {
                int CheckCount = 0;

                for (int i = 0; i < PrintGrid.Rows.Count; i++)
                {
                    for (int j = 0; j < PrintGrid.Columns.Count; j++)
                    {
                        if (PrintGrid.Rows[i].Cells[j].ColumnInfo.GetType().Name.Equals("GridViewCheckBoxColumn"))
                        {
                            if (PrintGrid.Rows[i].Cells[j].Value.ToString().Equals("True"))
                            {
                                CheckCount++;
                                break;
                            }
                        }
                    }
                }

                if (CheckCount > 0) {
                    for (int i = 0; i < PrintGrid.Rows.Count; i++)
                    {
                        for (int j = 0; j < PrintGrid.Columns.Count; j++)
                        {
                            if (PrintGrid.Rows[i].Cells[j].ColumnInfo.GetType().Name.Equals("GridViewCheckBoxColumn"))
                            {
                                if (PrintGrid.Rows[i].Cells[j].Value.ToString().Equals("False"))
                                {
                                    PrintGrid.Rows[i].IsVisible = false;
                                }
                            }
                        }
                    }
                }

                //Hide unnecessary Column
                foreach (GridViewColumn item in PrintGrid.Columns)
                {
                    item.IsVisible = item.GetType().Name.Equals("GridViewCheckBoxColumn") ? false : item.IsVisible;
                }

            }

            //Set LandScape
            PrintGrid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
            this.Landscape = PrintGrid.Columns.Count > 3 ? true : false;

            PrintDialogsLocalizationProvider.CurrentProvider = new DialogsLocalizationProvider();
            dialog = new RadPrintPreviewDialog(this);
            dialog.ShowDialog();
        }

        /// <summary>
        /// For Setting PageHeader 
        /// </summary>
        /// <param name="args"></param>
        protected override void PrintHeader(System.Drawing.Printing.PrintPageEventArgs args)
        {
            base.PrintHeader(args);

            Rectangle headerRect = new Rectangle(args.MarginBounds.Location, new Size(args.MarginBounds.Width, this.HeaderHeight));
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            Font UpperFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            if (currentpage == 1)
            {
                args.Graphics.DrawString(this.LeftUpperText, UpperFont, Brushes.DarkBlue, new Rectangle(headerRect.X, headerRect.Y, headerRect.Width, headerRect.Height), stringFormat);
                args.Graphics.DrawLine(new Pen(Brushes.Black), headerRect.Location, new Point(headerRect.Location.X + headerRect.Width, headerRect.Location.Y));
            }
        }

        /// <summary>
        /// For Setting PageFooter 
        /// </summary>
        /// <param name="args"></param>
        protected override void PrintFooter(System.Drawing.Printing.PrintPageEventArgs args)
        {
            base.PrintFooter(args);

            Rectangle FooterRect = new Rectangle(new Point(args.MarginBounds.X,args.MarginBounds.Bottom),new Size(args.MarginBounds.Width, this.FooterHeight));
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            Font FooterUpperFont = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            args.Graphics.DrawString(currentpage.ToString(), FooterUpperFont, Brushes.DarkBlue, new Rectangle(FooterRect.X, FooterRect.Y-20, FooterRect.Width, FooterRect.Height), stringFormat);

            currentpage = args.HasMorePages ? currentpage + 1 : 1;
        }

        /// <summary>
        /// Set rollback Selected GridView
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEndPrint(System.Drawing.Printing.PrintEventArgs e)
        {
           base.OnEndPrint(e);
            
           //Show All Rows
           for (int i = 0; i < SelectedGrid.Rows.Count; i++)
           {
               SelectedGrid.Rows[i].IsVisible = true;
           }

           //Show All Column
           foreach (GridViewColumn item in SelectedGrid.Columns)
           {
               item.IsVisible = item.GetType().Name.Equals("GridViewCheckBoxColumn") ? true : item.IsVisible;
           }
           
           //Rollback Grid properity
           SelectedGrid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill; 

           // acces the print button in the PrintGridView method
           RadCommandBar bar = dialog.Controls[1] as RadCommandBar;
           var printButton = bar.Rows[0].Strips[0].Items[0] as CommandBarButton;
           printButton.Click += PrintButton_Click;
        }

        /// <summary>
        /// For Close Print Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, EventArgs e)
        {
            dialog.Close();
        }
    }
}
