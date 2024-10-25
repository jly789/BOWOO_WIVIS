using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Sorer_Indicator_Contorl
{
    public partial class Form1 : Form
    {

        //동기화를 위한 락 변수
        static private object lock_object = new object();


        PrintDocument pd = new PrintDocument();
        PrinterSettings pr_setting = new PrinterSettings();
        PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        List<string> print_list = new List<string>();
        List<string> itemCd_list = new List<string>();
        //기본설정된 프른트 찾기
        string defaultPrinter;
        DataRow dtarow = null;
        //인쇄시 종이에 마진주기.
        Margins margins = new Margins(20, 20, 20, 20);
        //code 39 바코드
        Font barcode_font = new Font("3 of 9 Barcode", 80);
        Font FtBT1 = null;


        int startWidth = 10;//시작 x좌표
        int startHeight = 15;//시작 y좌표

        int pageNo = 1;//페이지 넘버
        int maxPage = 1;
        int cnt = 1;//row_Nomber

        public Form1()
        {
            InitializeComponent();
        }

        public void test_print(DataTable dt)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["dork_no"].ToString() == "1")
                {
                    print1(dt.Rows[i], "LABEL_01");
                }
                else
                {
                    print1(dt.Rows[i], "LABEL_02");
                }
            }
            this.Close();
        }

        public void print1(DataRow dtarow, string print_name)
        {

                cnt = 1;

                //인쇄를 할 프린트 설정.
                pd.PrinterSettings.PrinterName = print_name;
                this.dtarow = dtarow;
                //미리보기 테스트
                // test();
                //인쇄시작.
                pd.Print();
        }


        private void print_page_Event(object sender, PrintPageEventArgs e)
        {

            int hight_plus = 0;
            int rectangleHight = 120;
            RectangleF drawRect = new System.Drawing.RectangleF();

            StringFormat sf_bar = new StringFormat();//컬럼 안에있는 값들 가운데로 정렬하기위해서.
            sf_bar.Alignment = StringAlignment.Center;//가로 정렬
            sf_bar.LineAlignment = StringAlignment.Center;//높이 정렬


            StringFormat sf = new StringFormat();//컬럼 안에있는 값들 가운데로 정렬하기위해서.
            sf.Alignment = StringAlignment.Center;//가로 정렬
            sf.LineAlignment = StringAlignment.Center;//높이 정렬


            StringFormat sf1 = new StringFormat();//컬럼 안에있는 값들 가운데로 정렬하기위해서.
            sf1.Alignment = StringAlignment.Center;//가로 정렬
            sf1.LineAlignment = StringAlignment.Far;//높이 정렬

            for (int i = 0; i < dtarow.ItemArray.Length; i++)
            {
                drawRect = new RectangleF((float)(startWidth), (float)startHeight + hight_plus, (float)380, rectangleHight);
                e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth), (float)startHeight + hight_plus, (float)380, rectangleHight);
                if (i == 1)
                {
                    e.Graphics.DrawString(dtarow[i].ToString(), new Font("code 128", 32f), Brushes.Black, drawRect, sf_bar); //오더번호 바코드
                    e.Graphics.DrawString(dtarow[i + 1].ToString(), new Font("Arial", 20), Brushes.Black, drawRect, sf1); //오더번호
                }
                else
                {
                    if (i == 2)
                    {
                        continue;
                    }
                    e.Graphics.DrawString(dtarow[i].ToString(), new Font("Arial", 50), Brushes.Black, drawRect, sf); //tns_code
                }

                hight_plus += rectangleHight + 5;


            }
            e.HasMorePages = false;


            
        }
    }
}
