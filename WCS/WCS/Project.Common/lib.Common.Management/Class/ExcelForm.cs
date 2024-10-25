using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Reporting;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.Export;
using Telerik.Data;
using System.IO;
using Telerik.WinControls.UI;
using System.Drawing;
using Telerik.WinControls.UI.Export.ExcelML;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace lib.Common.Management
{
    public class ExcelForm 
    {
        ProgressForm ProgressPopup = new ProgressForm();
        MessageForm bowooMessageBox = new MessageForm();
        SaveFileDialog sfd = new SaveFileDialog();

        public RadGridView SelectedGrid = null;
        private PrintPopup printPopup;

        /// <summary>
        /// Save Excel File
        /// </summary>
        /// <param name="ExcelGrid">Selected Grid</param>
        /// <param name="ExcelLabel">Selected Label</param>
        public ExcelForm(PrintPopup printPopup,RadGridView ExcelGrid, RadLabel ExcelLabel)
        {
            this.printPopup = printPopup;
            string PrintText = ExcelLabel.Text.IndexOf("<") > -1 ? ExcelLabel.Text.Substring(0, ExcelLabel.Text.IndexOf("<")) : ExcelLabel.Text;
            SelectedGrid = ExcelGrid;
            
            //Hide unnecessary Column
            //Setting Data Type For excel
            for (int i = 0; i < ExcelGrid.Columns.Count ; i++)
            {
                ExcelGrid.Columns[i].IsVisible = ExcelGrid.Columns[i].GetType().Name.Equals("GridViewCheckBoxColumn") ? false : ExcelGrid.Columns[i].IsVisible;
                ExcelGrid.Columns[i].ExcelExportType = DisplayFormatType.None;
            }

            bool ExistsCheckBox = false;

            //Set Grid by GridViewCheckBox Condition
            foreach (GridViewColumn item in ExcelGrid.Columns)
            {
                if (item.GetType().Name.Equals("GridViewCheckBoxColumn"))
                {
                    ExistsCheckBox = true;
                }
            }

            if (ExistsCheckBox)
            {
                int CheckCount = 0;

                for (int i = 0; i < ExcelGrid.Rows.Count; i++)
                {
                    for (int j = 0; j < ExcelGrid.Columns.Count; j++)
                    {
                        if (ExcelGrid.Rows[i].Cells[j].ColumnInfo.GetType().Name.Equals("GridViewCheckBoxColumn"))
                        {
                            if (ExcelGrid.Rows[i].Cells[j].Value.ToString().Equals("True"))
                            {
                                CheckCount++;
                                break;
                            }
                        }
                    }
                }

                if (CheckCount > 0)
                {
                    for (int i = 0; i < ExcelGrid.Rows.Count; i++)
                    {
                        for (int j = 0; j < ExcelGrid.Columns.Count; j++)
                        {
                            if (ExcelGrid.Rows[i].Cells[j].ColumnInfo.GetType().Name.Equals("GridViewCheckBoxColumn"))
                            {
                                if (ExcelGrid.Rows[i].Cells[j].Value.ToString().Equals("False"))
                                {
                                    ExcelGrid.Rows[i].IsVisible = false;
                                }
                            }
                        }
                    }
                }

                //Hide unnecessary Column
                for (int i = 0; i < ExcelGrid.Columns.Count; i++)
                {
                    ExcelGrid.Columns[i].IsVisible = ExcelGrid.Columns[i].GetType().Name.Equals("GridViewCheckBoxColumn") ? false : ExcelGrid.Columns[i].IsVisible;
                    ExcelGrid.Columns[i].ExcelExportType = DisplayFormatType.None;
                }
            }

            //sfd.Filter = String.Format("{0} (*{1})|*{1}","Excel Files",".xlsx");
            sfd.Filter = "Excel Files (.xlsx)|*.xlsx|Text Files (.txt)|*.txt|CSV Files (.csv)|*.csv";
            sfd.Title = "파일 저장";
            sfd.DefaultExt = "xlsx";
            sfd.AddExtension = true;

            string FileName = PrintText.Trim() + (BaseForm.Brand != string.Empty ? "_" + BaseForm.Brand : string.Empty) + (BaseForm.WorkSeq != string.Empty ? "_" + BaseForm.WorkSeq : string.Empty);

            sfd.FileName = string.Format("{0}_{1}", FileName, DateTime.Now.ToString("yyyyMMdd_hhmmss"));

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                switch (Path.GetExtension(sfd.FileName))
                {
                    case ".xlsx":
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();

                        Telerik.WinControls.Export.GridViewSpreadExport exporterExcel = new Telerik.WinControls.Export.GridViewSpreadExport(ExcelGrid);
                        exporterExcel.AsyncExportProgressChanged += spreadExporter_AsyncExportProgressChanged;
                        exporterExcel.AsyncExportCompleted += spreadExporter_AsyncExportCompleted;
                        exporterExcel.SheetMaxRows = ExcelMaxRows._1048576; // Max rows for Excel2007, Previous version of Excel is 65536
                        exporterExcel.ExportVisualSettings = true;
                        exporterExcel.SheetName = PrintText == string.Empty ? "Sheet" : PrintText;

                        Telerik.WinControls.Export.SpreadExportRenderer renderer = new Telerik.WinControls.Export.SpreadExportRenderer();
                        renderer.WorkbookCreated += renderer_WorkbookCreated;
                        exporterExcel.RunExportAsync(ms, renderer);
                        break;
                    case ".txt":
                        (ProgressPopup.Controls.Find("radProgressBarDownload", true)[0] as RadProgressBar).Value1 = 1;
                        ProgressPopup.SetLocation(BaseForm.MainPosition);
                        ProgressPopup.BringToFront();
                        ProgressPopup.Show();
                        StringBuilder sb = new StringBuilder();

                        IEnumerable<string> columnNames = from col in ExcelGrid.Columns
                                                          where col.IsVisible == true
                                                          select col.HeaderText;

                        sb.AppendLine(string.Join(",", columnNames));

                        for (int i = 0; i < ExcelGrid.Rows.Count; i++)
                        {
                            List<string> fields = new List<string>();
                            for (int j = 0; j < ExcelGrid.Columns.Count; j++)
                            {
                                if (ExcelGrid.Columns[j].IsVisible)
                                    fields.Add(ExcelGrid.Rows[i].Cells[j].Value.ToString().Trim().Equals(string.Empty) ? " " : ExcelGrid.Rows[i].Cells[j].Value.ToString().Trim());
                            }
                            sb.AppendLine(string.Join(",", fields));
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.Default);
                        (ProgressPopup.Controls.Find("radProgressBarDownload", true)[0] as RadProgressBar).Value1 = 100;
                        bowooMessageBox.Show("파일 생성이 완료 되었습니다.");
                        ProgressPopup.Close();
                        
                        System.Diagnostics.Process.Start(sfd.FileName);

                        break;
                    case ".csv":
                        (ProgressPopup.Controls.Find("radProgressBarDownload", true)[0] as RadProgressBar).Value1 = 1;
                        ProgressPopup.SetLocation(BaseForm.MainPosition);
                        ProgressPopup.BringToFront();
                        ProgressPopup.Show();
                        ExportToCSV exporterCSV = new ExportToCSV(ExcelGrid);
                        exporterCSV.Encoding = Encoding.Default;
                        exporterCSV.RunExport(sfd.FileName);
                        bowooMessageBox.Show("파일 생성이 완료 되었습니다.");
                        (ProgressPopup.Controls.Find("radProgressBarDownload", true)[0] as RadProgressBar).Value1 = 100;
                        ProgressPopup.Close();

                        System.Diagnostics.Process.Start(sfd.FileName);

                        break;
                    default:
                        bowooMessageBox.Show("파일 형식이 잘못되었습니다.");
                        break;
                }
            }
            else
            {
                //Show All Rows
                for (int i = 0; i < SelectedGrid.Rows.Count; i++)
                {
                    SelectedGrid.Rows[i].IsVisible = true;
                }

                //Hide unnecessary Column
                foreach (GridViewColumn item in SelectedGrid.Columns)
                {
                    item.IsVisible = item.GetType().Name.Equals("GridViewCheckBoxColumn") ? true : item.IsVisible;
                }
            }
        }

        /// <summary>
        /// Formating Excel Header Cell,Data Cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void renderer_WorkbookCreated(object sender, WorkbookCreatedEventArgs e)
        {
            var cells = e.Workbook.Worksheets[0].UsedCellRange as CellRange;
            var sheet = e.Workbook.Worksheets[0] as Worksheet;

            for (int i = 0; i < cells.RowCount; i++)
            {
                for (int j = 1; j < cells.ColumnCount; j++)
                {
                    var cellRange = new CellIndex(i,j);
                    var cellValue = sheet.Cells[cellRange].GetValue().Value.RawValue;

                    CellBorders borders = new CellBorders();
                    CellBorder border = new CellBorder(CellBorderStyle.Thin, ThemableColor.FromArgb(255, 189, 189, 189));
                    borders.Top = border;
                    borders.Bottom = border;
                    borders.Left = border;
                    borders.Right = border;
                    sheet.Cells[cellRange].SetBorders(borders);

                    if (i == 0) //Header
                    {
                        PatternFill solidPatternFill = new PatternFill(PatternType.Solid, ThemableColor.FromArgb(255, 234, 234, 234), ThemableColor.FromArgb(234, 234, 234, 234));
                        CellRange headerRange = new CellRange(0, j-1, 0, cells.ColumnCount - 1);
                        sheet.Cells[headerRange].SetFill(solidPatternFill);
                        sheet.Cells[headerRange].SetFontSize(14);
                        sheet.Cells[headerRange].SetHorizontalAlignment(Telerik.Windows.Documents.Spreadsheet.Model.RadHorizontalAlignment.Center);
                    }
                    else { //Data

                        CellRange DataRange = new CellRange(i, j, cells.RowCount-1, cells.ColumnCount - 1);
                        sheet.Cells[DataRange].SetFontSize(12);
                    }
                }
            }
        }

        /// <summary>
        /// For Hide Loading Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadExporter_AsyncExportCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            RunWorkerCompletedEventArgs args = e as RunWorkerCompletedEventArgs;

            using (System.IO.FileStream fileStream = new System.IO.FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
            {
                MemoryStream ms = args.Result as MemoryStream;
                ms.WriteTo(fileStream);
                ms.Close();
            }

            //Show All Rows
            for (int i = 0; i < SelectedGrid.Rows.Count; i++)
            {
                SelectedGrid.Rows[i].IsVisible = true;
            }

            //Hide unnecessary Column
            foreach (GridViewColumn item in SelectedGrid.Columns)
            {
                item.IsVisible = item.GetType().Name.Equals("GridViewCheckBoxColumn") ? true : item.IsVisible;
            }

            sfd.Dispose();
            ProgressPopup.Close(); 
            this.printPopup.Close();
            bowooMessageBox.Show("파일 다운로드가 완료되었습니다.");

            System.Diagnostics.Process.Start(sfd.FileName);
        }

        /// <summary>
        /// For show Loading Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadExporter_AsyncExportProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1) ProgressPopup.ShowDialog();
            (ProgressPopup.Controls.Find("radProgressBarDownload",true)[0] as RadProgressBar).Value1 = e.ProgressPercentage;
        }
    }
}
