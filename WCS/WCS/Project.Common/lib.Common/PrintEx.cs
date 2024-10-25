/*****************************************************************************
 * 역할 : Telerik Grid Report를 위한 확장 클래스
 *****************************************************************************/ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls.UI;
using Telerik.WinControls.Themes;
using Telerik.Reporting;
using Telerik.ReportViewer;
using System.Drawing;

namespace lib.Common
{
    static public partial class PrintEx 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrintGrid"></param>
        /// <param name="TitleText"></param>
        static public void RadPrintGrid(RadGridView PrintGrid, string TitleText)
        {

            GridPrintStyle style = new GridPrintStyle();
            style.FitWidthMode = PrintFitWidthMode.FitPageWidth;
            PrintGrid.PrintStyle = style;

            



        }

    }
}
