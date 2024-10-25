using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using System.Drawing;
using Telerik.WinControls;

namespace TelerikBarcode
{
    public class TelerikBarcode
    {
        public bool checksum = false;
        public StringAlignment stringLineAlign = StringAlignment.Far;
        public int module = 1;
        public bool showText = false;
        public bool stretch = false;

        public Image getbarcode(string value, int width, int height)
        {
            Telerik.WinControls.UI.RadBarcode barcode = new RadBarcode();
            Telerik.WinControls.UI.Barcode.Symbology.Code25Interleaved code25Standard1 = new Telerik.WinControls.UI.Barcode.Symbology.Code25Interleaved();

            code25Standard1.Checksum = this.checksum;
            code25Standard1.LineAlign = this.stringLineAlign;
            code25Standard1.Module = this.module;
            code25Standard1.ShowText = this.showText;
            code25Standard1.Stretch = this.stretch;

            barcode.Symbology = code25Standard1;
            barcode.Value = value;
            Image image = barcode.ExportToImage(width, height);


            return image;

        }




    }
}
