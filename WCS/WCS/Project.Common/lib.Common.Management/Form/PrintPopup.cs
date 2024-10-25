using bowoo.Framework.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace lib.Common.Management
{
    public partial class PrintPopup : lib.Common.Management.BaseForm
    {
        public RadGridView sGrid = null;
        public RadLabel sLabel = null;

        public PrintPopup()
        {
            BaseEntity.IsFiredPrint = false;

            InitializeComponent();
            this.BringToFront();
        }

        /// <summary>
        /// Close Print Popup   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonPrintClose_Click(object sender, EventArgs e)
        {
            ClosePopup();
        }

        /// <summary>
        /// Set Public Function for calling in other class
        /// </summary>
        public void ClosePrintPopupByPosition(int XPosition,int YPosition)
        {
            ClosePopup();
        }

        /// <summary>
        /// Set this PopupClose
        /// </summary>
        public void ClosePopup()
        {
            this.Dispose(true);
            this.Close();
        }

        /// <summary>
        /// Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radListViewExport_ItemMouseClick(object sender, Telerik.WinControls.UI.ListViewItemEventArgs e)
        {
            this.Visible = false;

            if (sGrid != null)
            {
                BaseEntity.IsFiredPrint = true;

                if (sGrid.RowCount == 0)
                {
                    bowooMessageBox.Show(string.Format("{0} 데이터가 존재하지 않습니다.\r\n 데이터를 확인하세요.", e.Item.Key.ToString().Equals("Print") ? "프린트" : "엑셀 파일로 변환할"));
                    return;
                }

                switch (e.Item.Key.ToString())
                {
                    case "Save":
                        ExcelForm docExcel = new ExcelForm(this,sGrid, sLabel);
                        break;
                    case "Print":
                        PrintForm docPrint = new PrintForm(this,sGrid, sLabel);
                        break;
                }

                this.Close();
            }
            else
            {
                bowooMessageBox.Show(string.Format("{0} 데이터 테이블을 선택하세요.", e.Item.Key.ToString().Equals("Print") ? "프린트" : "엑셀 파일로 변환할"));
            } 
        }
    }
}
