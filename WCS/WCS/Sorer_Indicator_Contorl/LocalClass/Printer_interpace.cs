using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;
using System.Data;

namespace Sorer_Indicator_Contorl
{
    class Printer_interpace
    {
        lib.Common.Management.Barcode barcode = lib.Common.Management.Barcode.getInstance();
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
        DataTable tempTable = null;
        string[] tempString = null;
        //인쇄시 종이에 마진주기.
        Margins margins = new Margins(20, 20, 20, 20);
        //code 39 바코드
        Font barcode_font = new Font("3 of 9 Barcode", 80);
        Font FtBT1 = null;


        //int startWidth = 10;//시작 x좌표
        //int startHeight = 20;//시작 y좌표
        string err_cdoe;
        int pageNo = 1;//페이지 넘버
        int maxPage = 1;
        int cnt = 1;//row_Nomber
        int datalenght = 0;
        string work_seq = string.Empty; //이전 시퀀스번호 저장. 시퀀스번호가 다를경우 페이지 넘기기위함.
        int printSeq = 1;
        int sumqty = 0; //시퀀스별 오더 수량 합계/
        int laserMaxRow = 27; //레이저 프린트 한페이지 최대 로우 수량.

        Pen pen = null;

        public Printer_interpace()
        {
            foreach (string r in PrinterSettings.InstalledPrinters)
            {
                print_list.Add(r);
                if (pr_setting.IsDefaultPrinter)
                {
                    defaultPrinter = r;
                }
            }
            //인쇄시 종이에 마진주기.
            //pd.DefaultPageSettings.Margins = margins;
            //새로운 페이지 인쇄시 발생하는 이벤트.test112
            //pd.PrintPage += new PrintPageEventHandler(test112);
            //pd.PrintPage += new PrintPageEventHandler(test112);
            //pd.PrinterSettings.PrinterName = defaultPrinter;
            pd.PrinterSettings.PrinterName = "LABEL_01";
        }



        public List<string> get_print()
        {
            return print_list;
        }

        public string get_defaultPrinter()
        {
            return defaultPrinter;
        }

        public void testprint(string print_name, DataTable dt)
        {
            cnt = 1;
            //인쇄를 할 프린트 설정.
            pd.PrinterSettings.PrinterName = print_name;
            itemCd_list = new List<string>();
            //아이템 코드 얻어오기. 코드39는 앞뒤로 *을 찍어줘야 해서 별을 찍음.
            //foreach (DataRow r in dt.Select("column1 = 1"))
            //{
            //    itemCd_list.Add("*" + r["dataGridViewTextBoxColumn5"].ToString() + "*");
            //}
            maxPage = (itemCd_list.Count / 25) + (itemCd_list.Count % 25 == 0 ? 0 : 1);
            //미리보기 테스트
            test();
            dt.AsEnumerable();
            //인쇄시작.
            //pd.Print();
        }

        public void print(string[] tempString, string print_name)
        {
            lock (lock_object)
            {

                cnt = 0;
                sumqty = 0;
                printSeq = 1;
                pd.PrinterSettings.PrinterName = print_name;

                this.tempString = tempString;
                work_seq = string.Empty;
                //인쇄를 할 프린트 설정.
                //if (print_name.Contains("LASER_01")) //레이저 프린트 처리.

                if (tempString[0] == "C") // cj택배
                {
                    pd.PrintPage += new PrintPageEventHandler(cj_Label);

                }
                else if (tempString[0] == "T")//탑코리아 택배
                {
                    pd.PrintPage += new PrintPageEventHandler(top_Label);
                }
                else if (tempString[0] == "R")//정리작업
                {
                    pd.PrintPage += new PrintPageEventHandler(clean_Label);
                }

                //test();
                //인쇄시작.
                pd.Print();
            }
        }

        public void print(DataTable tempTable, string print_name)
        {
            lock (lock_object)
            {

                cnt = 0;
                sumqty = 0;
                printSeq = 1;
                datalenght = tempTable.Rows.Count;
                pd.PrinterSettings.PrinterName = print_name;

                this.tempTable = tempTable;
                work_seq = string.Empty;
                //인쇄를 할 프린트 설정.
                //if (print_name.Contains("LASER_01")) //레이저 프린트 처리.
                if (print_name.Contains("LASER")) //레이저 프린트 처리.
                {
                    pd.PrintPage += new PrintPageEventHandler(laserPrintPage);
                    //test();
                }
                else if (print_name.Contains("LABEL")) //라벨 프린터 처리
                {
                    pd.PrintPage += new PrintPageEventHandler(statementPrint);
                }
                //test();
                //인쇄시작.
                pd.Print();
            }
        }



        public void print()
        {
            lock (lock_object)
            {

                cnt = 1;
                printSeq = 1;
                //인쇄를 할 프린트 설정.
                pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                //pd.PrinterSettings.PrinterName = "DocuCentre-IV C2263";
                //this.dtarow = dtarow;
                //미리보기 테스트
                // test();
                //인쇄시작.
                pd.PrintPage += new PrintPageEventHandler(statementPrint);
                pd.Print();

            }
        }

        private void cj_Label(object sender, PrintPageEventArgs e)
        {

            float startWidth = 0;//시작 x좌표
            float startHeight = 1;//시작 y좌표            

            pen = new Pen(Brushes.Black, (float)0.2);
            //pen.Freeze();


            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.


            RectangleF[] drawRect = new System.Drawing.RectangleF[8];
            StringFormat[] sf = new StringFormat[10];

            sf[0] = new StringFormat();
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[1] = new StringFormat();
            //sf[0].Alignment = StringAlignment.;//가로 정렬
            //sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[2] = new StringFormat();
            sf[2].Alignment = StringAlignment.Center;//가로 정렬
            sf[2].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[3] = new StringFormat();
            sf[3].Alignment = StringAlignment.Center;//가로 정렬
            sf[3].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[4] = new StringFormat();
            sf[4].Alignment = StringAlignment.Near;//가로 정렬
            //sf[4].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[5] = new StringFormat();
            //sf[5].Alignment = StringAlignment.Center;//가로 정렬
            sf[5].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[6] = new StringFormat();
            //sf[6].Alignment = StringAlignment.Center;//가로 정렬
            sf[6].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[7] = new StringFormat();
            sf[7].Alignment = StringAlignment.Far;//가로 정렬
            sf[7].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[8] = new StringFormat();
            sf[8].Alignment = StringAlignment.Center;//가로 정렬
            sf[8].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[9] = new StringFormat();
            sf[9].Alignment = StringAlignment.Far;//가로 정렬
            sf[9].LineAlignment = StringAlignment.Center;//높이 정렬


            /* printString 인덱스별 데이터 정보.
            * [0]: OK
            * [1]: 회사 이름 (주)위비스
            * [2]: 블랜드 이름
            * [3]: 슈트번호
            * [4]: 매장명
            * [5]: 매장주소1
            * [6]: 매장주소2
            * [7]: 매장전화번호
            * [8]: 박스번호
            * [9]: 마지막 박스 인지 확인 하는 변수. 마지막 박스 Y, 아니면 N
            * [10]: 입수량
            * [11]: 집하 코드
            * [12]: 집하 장소
            * [13]: 송장 번호
            */

            if (tempString[8] == "Y")
            {
                tempString[7] = tempString[8] + "  E";
            }

            //발신.
            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight, (float)10, 20);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight, (float)10, 20);
            e.Graphics.DrawString("발신", new Font("Arial", 20), Brushes.Black, drawRect[0], sf[0]); //tns_code

            //업체 명, 신용정보
            drawRect[1] = new RectangleF((float)(startWidth) + 10, (float)startHeight, 60f, 20f);
            e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);

            e.Graphics.DrawString(tempString[1] + " - " + tempString[2], new Font("Arial", 10), Brushes.Black, drawRect[1], sf[1]); //tns_code
            e.Graphics.DrawString("신용", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, 35, 16, sf[0]);

            //권역 분류 바코드
            e.Graphics.DrawString(barcode.CODE128(tempString[11], "B", ref err_cdoe), new Font("code 128", 38f), Brushes.Black, 53, 5, sf[8]);

            //슈트번호
            drawRect[2] = new RectangleF((float)(startWidth) + 70, (float)startHeight, (float)30f, 20f);
            e.Graphics.DrawRectangle(pen, drawRect[2].X, drawRect[2].Y, drawRect[2].Width, drawRect[2].Height);
            e.Graphics.DrawString(tempString[3], new Font("Arial", 35), Brushes.Black, drawRect[2], sf[2]); //tns_code

            //수신
            drawRect[3] = new RectangleF((float)(startWidth), (float)startHeight + 20, (float)10f, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            e.Graphics.DrawString("수 신", new Font("Arial", 25), Brushes.Black, drawRect[3], sf[3]); //tns_code

            //매장명
            drawRect[4] = new RectangleF((float)(startWidth) + 10, (float)startHeight + 20, (float)90f, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            e.Graphics.DrawString(tempString[4], new Font("Arial", 35), Brushes.Black, drawRect[4], sf[4]); //tns_code

            //ADDR1,ADDR2,TEL
            e.Graphics.DrawString(tempString[5], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 40, sf[4]);
            e.Graphics.DrawString(tempString[6], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 45, sf[4]);
            e.Graphics.DrawString(tempString[7], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 50, sf[4]);





            //박스 번호
            drawRect[5] = new RectangleF((float)(startWidth), (float)startHeight + 60, (float)50f, 10f);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);
            e.Graphics.DrawString("BOX", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 3, 63, sf[4]);
            e.Graphics.DrawString(tempString[8], new Font("Arial", 15), Brushes.Black, drawRect[5], sf[9]); //tns_code

            //PCS(제품 수량)
            drawRect[6] = new RectangleF((float)(startWidth) + 50, (float)startHeight + 60, (float)50f, 10f);
            e.Graphics.DrawRectangle(pen, drawRect[6].X, drawRect[6].Y, drawRect[6].Width, drawRect[6].Height);
            e.Graphics.DrawString("PCS", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 52, 63, sf[4]);
            e.Graphics.DrawString(tempString[10], new Font("Arial", 15), Brushes.Black, drawRect[6], sf[9]); //tns_code

            //집하 코드 집하 장소
            drawRect[7] = new RectangleF((float)(startWidth), (float)startHeight + 70, (float)100f, 30f);
            e.Graphics.DrawRectangle(pen, drawRect[7].X, drawRect[7].Y, drawRect[7].Width, drawRect[7].Height);
            e.Graphics.DrawString(tempString[11] + "\r\n" + tempString[12], new Font("Arial", 30), Brushes.Black, drawRect[7], sf[7]); //tns_code

            //송장 출력
            e.Graphics.DrawString(barcode.CODE128(tempString[13], "B", ref err_cdoe), new Font("Code 128", 35), Brushes.Black, 30, 73, sf[8]);
            e.Graphics.DrawString(tempString[13], new Font("Arial", 10), Brushes.Black, 30, 85, sf[8]);

            //출력일자


            e.Graphics.DrawString("출력일시 : " + DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 30, 95, sf[8]);

            pd.PrintPage -= new PrintPageEventHandler(cj_Label);
        }

        private void top_Label(object sender, PrintPageEventArgs e)
        {

            float startWidth = 0;//시작 x좌표
            float startHeight = 1;//시작 y좌표            

            pen = new Pen(Brushes.Black, (float)0.2);
            //pen.Freeze();


            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.


            RectangleF[] drawRect = new System.Drawing.RectangleF[9];
            StringFormat[] sf = new StringFormat[10];

            sf[0] = new StringFormat();
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[1] = new StringFormat();
            //sf[0].Alignment = StringAlignment.;//가로 정렬
            //sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[2] = new StringFormat();
            sf[2].Alignment = StringAlignment.Center;//가로 정렬
            sf[2].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[3] = new StringFormat();
            sf[3].Alignment = StringAlignment.Center;//가로 정렬
            sf[3].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[4] = new StringFormat();
            sf[4].Alignment = StringAlignment.Near;//가로 정렬
            //sf[4].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[5] = new StringFormat();
            //sf[5].Alignment = StringAlignment.Center;//가로 정렬
            sf[5].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[6] = new StringFormat();
            //sf[6].Alignment = StringAlignment.Center;//가로 정렬
            sf[6].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[7] = new StringFormat();
            sf[7].Alignment = StringAlignment.Far;//가로 정렬
            sf[7].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[8] = new StringFormat();
            sf[8].Alignment = StringAlignment.Center;//가로 정렬
            sf[8].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[9] = new StringFormat();
            sf[9].Alignment = StringAlignment.Far;//가로 정렬
            sf[9].LineAlignment = StringAlignment.Center;//높이 정렬


            /* printString 인덱스별 데이터 정보.
            * [0]: OK
            * [1]: 회사 이름 (주)위비스
            * [2]: 블랜드 이름
            * [3]: 슈트번호
            * [4]: 매장명
            * [5]: 매장주소1
            * [6]: 매장주소2
            * [7]: 매장전화번호
            * [8]: 박스번호
            * [9]: 마지막 박스 인지 확인 하는 변수. 마지막 박스 Y, 아니면 N
            * [10]: 입수량
            * [11]: 집하 코드
            * [12]: 집하 장소
            * [13]: 송장 번호
            */

            if (tempString[8] == "Y")
            {
                tempString[7] = tempString[8] + "  E";
            }

            //발신.
            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight + 4, (float)10, 16);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight + 4, (float)10, 16);
            e.Graphics.DrawString("발신", new Font("Arial", 20), Brushes.Black, drawRect[0], sf[0]); //tns_code

            //업체 명, 신용정보
            drawRect[1] = new RectangleF((float)(startWidth) + 10, (float)startHeight + 4, 60f, 16);
            e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);

            e.Graphics.DrawString(tempString[1] + " - " + tempString[2], new Font("Arial", 10), Brushes.Black, drawRect[1], sf[1]); //tns_code

            //슈트번호
            drawRect[2] = new RectangleF((float)(startWidth) + 70, (float)startHeight + 4, (float)30f, 16f);
            e.Graphics.DrawRectangle(pen, drawRect[2].X, drawRect[2].Y, drawRect[2].Width, drawRect[2].Height);
            e.Graphics.DrawString(tempString[3], new Font("Arial", 35), Brushes.Black, drawRect[2], sf[2]); //tns_code

            //수신
            drawRect[3] = new RectangleF((float)(startWidth), (float)startHeight + 20, (float)10f, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            e.Graphics.DrawString("수 신", new Font("Arial", 25), Brushes.Black, drawRect[3], sf[3]); //tns_code

            //매장명
            drawRect[4] = new RectangleF((float)(startWidth) + 10, (float)startHeight + 20, (float)90f, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            e.Graphics.DrawString(tempString[4], new Font("Arial", 35), Brushes.Black, drawRect[4], sf[4]); //tns_code

            //ADDR1,ADDR2,TEL
            e.Graphics.DrawString(tempString[5], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 40, sf[4]);
            e.Graphics.DrawString(tempString[6], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 45, sf[4]);
            e.Graphics.DrawString(tempString[7], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 50, sf[4]);

            //박스 번호
            drawRect[5] = new RectangleF((float)(startWidth), (float)startHeight + 60, (float)50f, 10f);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);
            e.Graphics.DrawString("BOX", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 3, 63, sf[4]);
            e.Graphics.DrawString(tempString[8], new Font("Arial", 15), Brushes.Black, drawRect[5], sf[9]); //tns_code

            //PCS(제품 수량)
            drawRect[6] = new RectangleF((float)(startWidth) + 50, (float)startHeight + 60, (float)50f, 10f);
            e.Graphics.DrawRectangle(pen, drawRect[6].X, drawRect[6].Y, drawRect[6].Width, drawRect[6].Height);
            e.Graphics.DrawString("PCS", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 52, 63, sf[4]);
            e.Graphics.DrawString(tempString[10], new Font("Arial", 15), Brushes.Black, drawRect[6], sf[9]); //tns_code

            //집하 코드 집하 장소
            drawRect[7] = new RectangleF((float)(startWidth), (float)startHeight + 70, (float)100f, 26f);
            e.Graphics.DrawRectangle(pen, drawRect[7].X, drawRect[7].Y, drawRect[7].Width, drawRect[7].Height);
            e.Graphics.DrawString(tempString[11] + "\r\n" + tempString[12], new Font("Arial", 30), Brushes.Black, drawRect[7], sf[7]); //tns_code

            //송장 출력
            e.Graphics.DrawString(barcode.CODE128(tempString[13], "B", ref err_cdoe), new Font("Code 128", 25), Brushes.Black, 50, 73, sf[8]);
            e.Graphics.DrawString(tempString[13], new Font("Arial", 10), Brushes.Black, 50, 83, sf[8]);

            //출력일자
            e.Graphics.DrawString("출력일시 : " + DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 50, 93, sf[8]);

            // topcorea 찍는것.!
            drawRect[8] = new RectangleF((float)(startWidth), (float)startHeight, (float)100, 4);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight, (float)100, 4);
            e.Graphics.DrawString("TOPKOREA TOPKREA TOPKREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA", new Font("Arial", 5), Brushes.Black, drawRect[8], sf[0]); //tns_code


            drawRect[8] = new RectangleF((float)(startWidth), (float)startHeight + 96, (float)100, 4);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight + 96, (float)100, 4);
            e.Graphics.DrawString("TOPKOREA TOPKREA TOPKREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA", new Font("Arial", 5), Brushes.Black, drawRect[8], sf[0]); //tns_code


            pd.PrintPage -= new PrintPageEventHandler(top_Label);
        }

        private void clean_Label(object sender, PrintPageEventArgs e)
        {

            float startWidth = 0;//시작 x좌표
            float startHeight = 1;//시작 y좌표            

            pen = new Pen(Brushes.Black, (float)0.2);
            //pen.Freeze();


            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.


            RectangleF[] drawRect = new System.Drawing.RectangleF[8];
            StringFormat[] sf = new StringFormat[10];

            sf[0] = new StringFormat();
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[1] = new StringFormat();
            //sf[0].Alignment = StringAlignment.;//가로 정렬
            //sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[2] = new StringFormat();
            sf[2].Alignment = StringAlignment.Center;//가로 정렬
            sf[2].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[3] = new StringFormat();
            sf[3].Alignment = StringAlignment.Center;//가로 정렬
            sf[3].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[4] = new StringFormat();
            sf[4].Alignment = StringAlignment.Near;//가로 정렬
            //sf[4].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[5] = new StringFormat();
            //sf[5].Alignment = StringAlignment.Center;//가로 정렬
            sf[5].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[6] = new StringFormat();
            //sf[6].Alignment = StringAlignment.Center;//가로 정렬
            sf[6].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[7] = new StringFormat();
            sf[7].Alignment = StringAlignment.Far;//가로 정렬
            sf[7].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[8] = new StringFormat();
            sf[8].Alignment = StringAlignment.Center;//가로 정렬
            sf[8].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[9] = new StringFormat();
            sf[9].Alignment = StringAlignment.Far;//가로 정렬
            sf[9].LineAlignment = StringAlignment.Center;//높이 정렬


            /* printString 인덱스별 데이터 정보.
            * [0]: OK
            * [1]: 회사 이름 (주)위비스
            * [2]: 블랜드 이름
            * [3]: 슈트번호
            * [4]: 매장명
            * [5]: 매장주소1
            * [6]: 매장주소2
            * [7]: 매장전화번호
            * [8]: 박스번호
            * [9]: 마지막 박스 인지 확인 하는 변수. 마지막 박스 Y, 아니면 N
            * [10]: 입수량
            * [11]: 집하 코드
            * [12]: 집하 장소
            * [13]: 송장 번호
            */

            if (tempString[8] == "Y")
            {
                tempString[7] = tempString[8] + "  E";
            }

            //발신.
            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight, (float)10, 20);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight, (float)10, 20);
            e.Graphics.DrawString("발신", new Font("Arial", 20), Brushes.Black, drawRect[0], sf[0]); //tns_code

            //업체 명, 신용정보
            drawRect[1] = new RectangleF((float)(startWidth) + 10, (float)startHeight, 60f, 20f);
            e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);

            e.Graphics.DrawString(tempString[1] + " - " + tempString[2], new Font("Arial", 10), Brushes.Black, drawRect[1], sf[1]); //tns_code
            e.Graphics.DrawString("신용", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, 35, 16, sf[0]);

            //권역 분류 바코드
            e.Graphics.DrawString(barcode.CODE128(tempString[11], "B", ref err_cdoe), new Font("code 128", 38f), Brushes.Black, 53, 5, sf[8]);

            //슈트번호
            drawRect[2] = new RectangleF((float)(startWidth) + 70, (float)startHeight, (float)30f, 20f);
            e.Graphics.DrawRectangle(pen, drawRect[2].X, drawRect[2].Y, drawRect[2].Width, drawRect[2].Height);
            e.Graphics.DrawString(tempString[3], new Font("Arial", 35), Brushes.Black, drawRect[2], sf[2]); //tns_code

            //수신
            drawRect[3] = new RectangleF((float)(startWidth), (float)startHeight + 20, (float)10f, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            e.Graphics.DrawString("수 신", new Font("Arial", 25), Brushes.Black, drawRect[3], sf[3]); //tns_code

            //매장명
            drawRect[4] = new RectangleF((float)(startWidth) + 10, (float)startHeight + 20, (float)90f, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            e.Graphics.DrawString(tempString[4], new Font("Arial", 35), Brushes.Black, drawRect[4], sf[4]); //tns_code

            //ADDR1,ADDR2,TEL
            e.Graphics.DrawString(tempString[5], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 40, sf[4]);
            e.Graphics.DrawString(tempString[6], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 45, sf[4]);
            e.Graphics.DrawString(tempString[7], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 50, sf[4]);





            //박스 번호
            drawRect[5] = new RectangleF((float)(startWidth), (float)startHeight + 60, (float)50f, 10f);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);
            e.Graphics.DrawString("BOX", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 3, 63, sf[4]);
            e.Graphics.DrawString(tempString[8], new Font("Arial", 15), Brushes.Black, drawRect[5], sf[9]); //tns_code

            //PCS(제품 수량)
            drawRect[6] = new RectangleF((float)(startWidth) + 50, (float)startHeight + 60, (float)50f, 10f);
            e.Graphics.DrawRectangle(pen, drawRect[6].X, drawRect[6].Y, drawRect[6].Width, drawRect[6].Height);
            e.Graphics.DrawString("PCS", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 55, 63, sf[4]);
            e.Graphics.DrawString(tempString[10], new Font("Arial", 15), Brushes.Black, drawRect[6], sf[9]); //tns_code

            //집하 코드 집하 장소
            drawRect[7] = new RectangleF((float)(startWidth), (float)startHeight + 70, (float)100f, 30f);
            e.Graphics.DrawRectangle(pen, drawRect[7].X, drawRect[7].Y, drawRect[7].Width, drawRect[7].Height);
            e.Graphics.DrawString(tempString[11] + "\r\n" + tempString[12], new Font("Arial", 30), Brushes.Black, drawRect[7], sf[7]); //tns_code

            //송장 출력
            e.Graphics.DrawString(barcode.CODE128(tempString[13], "B", ref err_cdoe), new Font("Code 128", 35), Brushes.Black, 30, 73, sf[8]);
            e.Graphics.DrawString(tempString[13], new Font("Arial", 10), Brushes.Black, 30, 85, sf[8]);

            //출력일자


            e.Graphics.DrawString("출력일시 : " + DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 30, 95, sf[8]);

            pd.PrintPage -= new PrintPageEventHandler(clean_Label);


        }

        private void statementPrint(object sender, PrintPageEventArgs e)
        {

            int startWidth = 0;//시작 x좌표
            int startHeight = 0;//시작 y좌표       
            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.     

            pen = new Pen(Brushes.Black, (float)0.1);

            //박스 테두리 사이즈.
            Rectangle rectangle = new Rectangle();
            Size[] recSize = new Size[50];
            Font[] font = new Font[50];
            font[0] = new Font("Arial", 8);

            StringFormat[] sf = new StringFormat[8];

            sf[0] = new StringFormat();
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[1] = new StringFormat();
            //sf[0].Alignment = StringAlignment.;//가로 정렬
            sf[1].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[2] = new StringFormat();
            sf[2].Alignment = StringAlignment.Center;//가로 정렬
            sf[2].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[3] = new StringFormat();
            sf[3].Alignment = StringAlignment.Center;//가로 정렬
            sf[3].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[4] = new StringFormat();
            //sf[4].Alignment = StringAlignment.Center;//가로 정렬
            //sf[4].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[5] = new StringFormat();
            //sf[5].Alignment = StringAlignment.Center;//가로 정렬
            sf[5].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[6] = new StringFormat();
            //sf[6].Alignment = StringAlignment.Center;//가로 정렬
            sf[6].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[7] = new StringFormat();
            sf[7].Alignment = StringAlignment.Far;//가로 정렬
            sf[7].LineAlignment = StringAlignment.Center;//높이 정렬


            e.Graphics.DrawString("거래명세서", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, 105, 15, sf[0]);
            e.Graphics.DrawString(/* 슈트 번호 들어가는 변수*/"148", new Font("Arial", 30, FontStyle.Bold), Brushes.Black, 188, 15, sf[0]);
            e.Graphics.DrawString("전표일자 : " /* +  날짜 들어가는 변수 들어가야함. */, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 10, 25);
            e.Graphics.DrawString("전표번호 : " /* +  전표번호 들어가는 변수 들어가야함. */, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, 25);

            // 공급자 ---------------------------------------------------------------------------------------------------
            //공급자.
            recSize[0] = new Size(8, 26);
            rectangle = new Rectangle(new Point(startWidth + 9, startHeight + 30), recSize[0]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("공\n\n급\n\n자", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //등록번호.
            recSize[1] = new Size(26, 8);
            rectangle = new Rectangle(new Point(startWidth + 17, startHeight + 30), recSize[1]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("등록번호", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //등록번호 value.
            recSize[3] = new Size(63, 8);
            rectangle = new Rectangle(new Point(startWidth + 43, startHeight + 30), recSize[3]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("211-87-60782", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //상호.
            rectangle = new Rectangle(new Point(startWidth + 17, startHeight + 38), recSize[1]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("상  호", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //상호명.
            recSize[4] = new Size(28, 8);
            rectangle = new Rectangle(new Point(startWidth + 43, startHeight + 38), recSize[4]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("(주)위비스", font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //성명.
            recSize[5] = new Size(15, 8);
            rectangle = new Rectangle(new Point(startWidth + 71, startHeight + 38), recSize[5]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("성 명", font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //성명value.
            recSize[6] = new Size(20, 8);
            rectangle = new Rectangle(new Point(startWidth + 86, startHeight + 38), recSize[6]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("도상현", font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //주소.
            recSize[2] = new Size(26, 10);
            rectangle = new Rectangle(new Point(startWidth + 17, startHeight + 46), recSize[2]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("주  소", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //주소value.
            recSize[7] = new Size(63, 10);
            rectangle = new Rectangle(new Point(startWidth + 43, startHeight + 46), recSize[7]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("서울 성동구 성수2가1동325-2 서울숲 한라시그마밸리 15층", font[0], Brushes.Black, rectangle, sf[1]); //tns_code
            //-----------------------------------------------------------------------------------------------------------

            //공급받는자----------------------------------------------------------------------------------------------
            //주소value.
            rectangle = new Rectangle(new Point(startWidth + 106, startHeight + 30), recSize[0]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("공\n급\n받\n는\n자", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //등록번호.
            rectangle = new Rectangle(new Point(startWidth + 114, startHeight + 30), recSize[1]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("등록번호", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //등록번호 value.
            rectangle = new Rectangle(new Point(startWidth + 140, startHeight + 30), recSize[3]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("3170236171", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //상호.
            rectangle = new Rectangle(new Point(startWidth + 114, startHeight + 38), recSize[1]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("상  호", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //상호명.
            rectangle = new Rectangle(new Point(startWidth + 140, startHeight + 38), recSize[4]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("강내상설", font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //성명.
            rectangle = new Rectangle(new Point(startWidth + 168, startHeight + 38), recSize[5]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("성 명", font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //성명value.
            rectangle = new Rectangle(new Point(startWidth + 183, startHeight + 38), recSize[6]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("소준호", font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //주소.
            rectangle = new Rectangle(new Point(startWidth + 114, startHeight + 46), recSize[2]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("주  소", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //주소value.
            rectangle = new Rectangle(new Point(startWidth + 140, startHeight + 46), recSize[7]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("충북 청원군 강내면 월곡리 64-19 298-9", font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //-----------------------------------------------------------------------------------------------------------


            //사이즈----------------------------------------------------------------------------------------------

            string[,] sizemap = new string[6, 16]
            {
                {"01","075","080","085","090","095","100","105","110","115","120","125","130","140","150","00F"}
                ,{"02","220","225","230","235","240","245","250","255","260","265","270","275","280","285","290"}
                ,{"03","061","064","067","070","073","076","079","082","085","088","090","00S","00M","00L","00F"}
                ,{"04","074","076","078","080","082","084","086","088","090","092","094","096","098","100","102"}
                ,{"07","000","018","019","020","021","022","023","024","025","00S","00M","00L","00F","ZZZ",""}
                ,{"09","075","080","085","090","095","100","105","110","0MF","00F","","","","",""}
            };

            recSize[8] = new Size(8, 30);
            rectangle = new Rectangle(new Point(startWidth + 9, startHeight + 57), recSize[8]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("Box\nNo", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            recSize[9] = new Size(21, 30);
            rectangle = new Rectangle(new Point(startWidth + 17, startHeight + 57), recSize[9]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("품  번", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            rectangle = new Rectangle(new Point(startWidth + 38, startHeight + 57), recSize[8]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("색\n\n상", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            float PointX = startWidth + 46;
            float PointY = startHeight + 57;

            RectangleF sizeRec = new RectangleF();
            SizeF sizemapRec = new SizeF(7.5f, 5);
            //사이즈 맵찍기.
            for (int i = 0; i < sizemap.GetLength(0); i++)
            {
                for (int j = 0; j < sizemap.GetLength(1); j++)
                {
                    //rectangle = new Rectangle(new Point(PointX, PointY), new SizeF(7.5,5));
                    sizeRec = new RectangleF(new PointF(PointX, PointY), sizemapRec);
                    e.Graphics.DrawRectangle(pen, PointX, PointY, 7.5f, 5);
                    e.Graphics.DrawString(sizemap[i, j], font[0], Brushes.Black, sizeRec, sf[0]); //tns_code
                    PointX += 7.5f;
                }
                PointY += 5;
                PointX = startWidth + 46;
            }


            rectangle = new Rectangle(new Point(startWidth + 166, startHeight + 57), recSize[8]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("계", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            recSize[9] = new Size(14, 30);
            rectangle = new Rectangle(new Point(startWidth + 174, startHeight + 57), recSize[9]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("공급\n단가\n\n/\n\n판매\n단가", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            recSize[10] = new Size(15, 30);
            rectangle = new Rectangle(new Point(startWidth + 188, startHeight + 57), recSize[10]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("공급\n금액\n\n/\n\n판매\n금액", font[0], Brushes.Black, rectangle, sf[0]); //tns_code




            //----------------------------------------------------------------------------------------------------------

        }




        private void labelPrintPage(object sender, PrintPageEventArgs e)
        {
            int startWidth = 10;//시작 x좌표
            int startHeight = 20;//시작 y좌표


            //e.Graphics.PageUnit = GraphicsUnit.Millimeter; mm터로 바꾸는 것.
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


            for (int j = 0; j < tempTable.Columns.Count; j++)
            {
                if (j == 1)
                {
                    //사각형그리기
                    drawRect = new RectangleF((float)(startWidth), (float)startHeight + hight_plus, (float)380, rectangleHight);
                    e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth), (float)startHeight + hight_plus, (float)380, rectangleHight);

                    e.Graphics.DrawString(tempTable.Rows[cnt][j].ToString(), new Font("code 128", 32f), Brushes.Black, drawRect, sf_bar); //오더번호 바코드
                    e.Graphics.DrawString(tempTable.Rows[cnt][j + 1].ToString(), new Font("Arial", 20), Brushes.Black, drawRect, sf1); //오더번호
                }
                else
                {
                    if (j == 2 || j == 4)
                    {
                        continue;
                    }
                    //사각형그리기
                    drawRect = new RectangleF((float)(startWidth), (float)startHeight + hight_plus, (float)380, rectangleHight);
                    e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth), (float)startHeight + hight_plus, (float)380, rectangleHight);

                    e.Graphics.DrawString(tempTable.Rows[cnt][j].ToString(), new Font("Arial", 50), Brushes.Black, drawRect, sf); //tns_code
                }

                hight_plus += rectangleHight + 5;
            }
            if (cnt < datalenght - 1)
            {
                cnt++;
                e.HasMorePages = true;
            }
            else if (cnt == datalenght - 1)
            {
                pd.PrintPage -= new PrintPageEventHandler(labelPrintPage);
            }

        }


        public void laserPrintPage(object sender, PrintPageEventArgs e)
        {
            string nowseq = string.Empty; //현재 시퀀스 번호.
            //itemCd_list.Count
            StringFormat sf = new StringFormat();//컬럼 안에있는 값들 가운데로 정렬하기위해서.
            sf.Alignment = StringAlignment.Center;//가로 정렬
            sf.LineAlignment = StringAlignment.Center;//높이 정렬
            //컬럼 가로 사이즈 ----------------------------------------------------
            //column_width 0 : row_no 사이즈, 1: barcode 사이즈 , 2: item_cd 사이즈
            int[] column_width = new int[] { 50, 100, 100, 470, 50 };
            string[] column_name = new string[] { "No", "로케이션", "제품코드", "제품명", "수량" };
            string[] font_arr = new string[] { "Arial", "Arial", "Arial" };
            Font[] fontArray = new Font[] { new Font("Arial", 13), new Font("Arial", 13), new Font("Arial", 13), new Font("Arial", 10), new Font("Arial", 13) };
            int headHeight = 40;//gridview 컬럼 하나의 높이
            //---------------------------------------------------------------------
            //데이터 높이 사이즈
            int data_hight = 35;
            //---------------------------------------------------------------------
            int startWidth = 5;//시작 x좌표
            int startHeight = 140;//시작 y좌표
            int temp = 0;//row개수 세어줄것. cnt의 역할
            string workSeq = tempTable.Rows[cnt][0].ToString();

            if (work_seq == string.Empty)
            {
                int ord_count = tempTable.Select("work_seq = '" + workSeq + "'").Length;

                work_seq = tempTable.Rows[cnt][0].ToString();
                maxPage = (ord_count / laserMaxRow) + (ord_count % laserMaxRow > 0 ? 1 : 0);
            }

            e.Graphics.DrawString("내품 리스트", new Font("Arial", 30, FontStyle.Bold), Brushes.Black, 270, 10);
            e.Graphics.DrawString("작업 일자 : " + DateTime.Now.ToString("yyyy/MM/dd"), new Font("Arial", 10), Brushes.Black, 570, 60);
            e.Graphics.DrawString("페이지번호 : " + pageNo + "/" + maxPage, new Font("Arial", 10), Brushes.Black, 570, 80);
            e.Graphics.DrawString("작업시퀀스 : " + tempTable.Rows[cnt][0].ToString(), new Font("Arial", 10), Brushes.Black, 570, 100);
            e.Graphics.DrawString("오더번호 : " + tempTable.Rows[cnt][1].ToString(), new Font("Arial", 10), Brushes.Black, 570, 120);
            if (cnt == 66)
            {

            }
            RectangleF drawRect = new System.Drawing.RectangleF();
            int width_plus = 0;

            //컬럼명 찍기.
            for (int i = 0; i < column_width.Length; i++)
            {
                drawRect = new RectangleF((float)(startWidth + width_plus), (float)startHeight, (float)column_width[i], headHeight);
                e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width_plus), (float)startHeight, (float)column_width[i], headHeight);
                e.Graphics.DrawString(column_name[i], new Font("Arial", 13, FontStyle.Bold), Brushes.Black, drawRect, sf);
                width_plus += column_width[i];
            }

            startHeight += headHeight;

            for (int i = cnt; i < tempTable.Rows.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (j == 0) width_plus = 0;
                    drawRect = new RectangleF((float)(startWidth + width_plus), (float)startHeight, (float)column_width[j], data_hight);
                    e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width_plus), (float)startHeight, (float)column_width[j], data_hight);

                    if (j == 0)
                    {
                        e.Graphics.DrawString(printSeq.ToString(), new Font(font_arr[j], 13), Brushes.Black, drawRect, sf);
                    }
                    else
                    {
                        e.Graphics.DrawString(tempTable.Rows[cnt][j + 1].ToString(), fontArray[j], Brushes.Black, drawRect, sf);
                    }

                    width_plus += column_width[j];
                }
                sumqty += Convert.ToInt32(tempTable.Rows[cnt][5]); //오더수량 합계

                if (cnt < tempTable.Rows.Count - 1)
                {
                    nowseq = tempTable.Rows[cnt + 1][0].ToString();
                }
                //nowseq = tempTable.Rows[cnt][0].ToString();
                startHeight += data_hight;
                temp++;
                printSeq++;
                cnt++;

                if (tempTable.Rows.Count == cnt)
                {
                    pd.PrintPage -= new PrintPageEventHandler(laserPrintPage);

                }

                if (temp % laserMaxRow == 0 && nowseq == work_seq && tempTable.Rows.Count != cnt)
                {
                    //work_seq = string.Empty;
                    if (tempTable.Rows.Count > cnt)
                    {
                        e.HasMorePages = true;
                    }
                    pageNo++;
                    break;
                }


                if (nowseq != work_seq || tempTable.Rows.Count == cnt)
                {
                    if (tempTable.Rows.Count > cnt)
                    {
                        e.HasMorePages = true;
                    }
                    //마지막에 합계를 넣기 위함.
                    for (int j = 0; j < 5; j++)
                    {
                        if (j == 0) width_plus = 0;
                        drawRect = new RectangleF((float)(startWidth + width_plus), (float)startHeight, (float)column_width[j], data_hight);
                        e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width_plus), (float)startHeight, (float)column_width[j], data_hight);

                        if (j == 0)
                        {
                            e.Graphics.DrawString(printSeq.ToString(), new Font(font_arr[j], 13), Brushes.Black, drawRect, sf);
                        }
                        else if (j == 1)
                        {
                            e.Graphics.DrawString("합계", fontArray[j], Brushes.Black, drawRect, sf);
                        }
                        else if (j == 4)
                        {

                            e.Graphics.DrawString(sumqty.ToString(), fontArray[j], Brushes.Black, drawRect, sf);
                        }
                        width_plus += column_width[j];
                    }
                    work_seq = string.Empty;
                    printSeq = 1;
                    sumqty = 0;
                    pageNo = 1;
                    //return;
                    //maxPage = 1;
                    break;
                }

            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("dataGridViewTextBoxColumn5", typeof(string));

            for (int i = 0; i < 10; i++)
            {
                dt.Rows.Add("ssk-1234" + i);
            }
            //print(defaultPrinter, dt);
        }

        private void test()
        {
            printPreviewDialog1.Document = pd;
            printPreviewDialog1.ShowDialog();
        }



    }
}
