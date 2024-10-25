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

namespace lib.Common.Management
{
    public class Printer_interpace
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
        string[] tempString = null;
        DataTable tempTable = null;
        //인쇄시 종이에 마진주기.
        Margins margins = new Margins(20, 20, 20, 20);
        //code 39 바코드
        Font barcode_font = new Font("3 of 9 Barcode", 80);
        string err_cdoe;


        string biz_day = string.Empty;
        string batch = string.Empty;
        DataTable printTable = new DataTable();


        //int startWidth = 10;//시작 x좌표
        //int startHeight = 20;//시작 y좌표

        int pageNo = 1;//페이지 넘버
        int maxPage = 1;
        int cnt = 1;//row_Nomber
        string work_seq = string.Empty; //이전 시퀀스번호 저장. 시퀀스번호가 다를경우 페이지 넘기기위함.
        int printSeq = 1;
        int sumqty = 0; //시퀀스별 오더 수량 합계/
        int laserMaxRow = 27; //레이저 프린트 한페이지 최대 로우 수량.
        int max_value = 22; //매출전표 한페이지에 최대 뽑을 sku수

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


        public void print(string[] tempString, string print_name)
        {
            lock (lock_object)
            {
                pd = new PrintDocument();
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
                    //PaperSize[] dd = new PaperSize[] {new PaperSize("a3", 50, 50) };
                    //pd.DefaultPageSettings.PaperSize = dd[0];
                    pd.PrintPage += new PrintPageEventHandler(H_Label);

                }
                else if (tempString[0] == "T")//탑코리아 택배
                {
                    pd.PrintPage += new PrintPageEventHandler(top_Label);
                }
                else if (tempString[0] == "M")//미할당
                {
                    pd.PrintPage += new PrintPageEventHandler(Unassigned);
                }
                else if (tempString[0] == "E")//에러
                {
                    pd.PrintPage += new PrintPageEventHandler(error_Label);

                }
                
                //test();
                //인쇄시작.
                pd.Print();
            }
        }


        public void print(DataTable dt, string print_name, string value)
        {
            lock (lock_object)
            {
                pd = new PrintDocument();
                cnt = 0;
                sumqty = 0;
                printSeq = 1;
                pd.PrinterSettings.PrinterName = print_name;
                //pd.PrinterSettings.PrinterName = "LABEL_00";
                pageNo = 1;

                this.tempTable = dt.Copy();
                work_seq = string.Empty;
                //인쇄를 할 프린트 설정.
                //if (print_name.Contains("LASER_01")) //레이저 프린트 처리.

                if (value.Equals("bundle"))
                {
                    pd.DefaultPageSettings.Landscape = false; // ture = 가로 출력, false = 세로출력 
                    pd.PrintPage += new PrintPageEventHandler(bundleListPrint);

                }
                else if (value.Equals("picking"))
                {

                    biz_day = tempTable.Rows[0]["biz_day"].ToString();
                    batch = tempTable.Rows[0]["batch"].ToString();

                    tempTable.Columns.Remove("biz_day");
                    tempTable.Columns.Remove("batch");
                    tempTable.Columns.Remove("gubun");
                    //tempTable.AcceptChanges();

                    pd.DefaultPageSettings.Landscape = false; // ture = 가로 출력, false = 세로출력 
                    pd.PrintPage += new PrintPageEventHandler(pickingListPrint);
                }
                else if (value.Equals("bundle_label")) //정리작업
                {
                    pd.DefaultPageSettings.Landscape = false; // ture = 가로 출력, false = 세로출력 
                    pd.PrintPage += new PrintPageEventHandler(bundle_Label);
                }
                else if (value.Equals("das_list")) //DAS작업 리스트
                {
                    pd.DefaultPageSettings.Landscape = false; // ture = 가로 출력, false = 세로출력 
                    pd.PrintPage += new PrintPageEventHandler(DASListPrint);
                }
                //else if (value.Equals("unassigned_label")) //미할당 매장 라벨 프린트
                //{
                //    pd.DefaultPageSettings.Landscape = true; // ture = 가로 출력, false = 세로출력 
                //    pd.PrintPage += new PrintPageEventHandler(unassigned);
                //}
                else if (value.Equals("bundleA4List")) //번들 마감시 명세서 양식
                {
                    pd.DefaultPageSettings.Landscape = false; // ture = 가로 출력, false = 세로출력 
                    pd.PrintPage += new PrintPageEventHandler(bundleA4List);
                }
                else if (value.Equals("statementPrint"))
                {
                    pd.DefaultPageSettings.Landscape = false; // ture = 가로 출력, false = 세로출력 
                    pd.PrintPage += new PrintPageEventHandler(statementPrint);
                }


                //test();
                pd.Print();
            }
        }



        //private void unassigned(object sender, PrintPageEventArgs e)
        //{

        //    float startWidth = 0;//시작 x좌표
        //    float startHeight = 1;//시작 y좌표            

        //    List<string> yearList = new List<string>();
        //    List<string> seasonList = new List<string>();
        //    List<string> itemList = new List<string>();

        //    string box_inv = this.tempTable.Rows[0]["box_inv"].ToString();
        //    string box_no = this.tempTable.Rows[0]["box_no"].ToString();
        //    string wrk_qty = this.tempTable.AsEnumerable().Sum(i => i.Field<int>("wrk_qty")).ToString();
        //    string shop_nm = this.tempTable.Rows[0]["shop_nm"].ToString();

        //    pen = new Pen(Brushes.Black, (float)0.2);
        //    //pen.Freeze();


        //    e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.

        //    RectangleF[] drawRect = new System.Drawing.RectangleF[9];
        //    StringFormat[] sf = new StringFormat[10];

        //    sf[0] = new StringFormat();
        //    sf[0].Alignment = StringAlignment.Center;//가로 정렬
        //    sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

        //    sf[1] = new StringFormat();
        //    //sf[0].Alignment = StringAlignment.;//가로 정렬
        //    //sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

        //    sf[2] = new StringFormat();
        //    sf[2].Alignment = StringAlignment.Center;//가로 정렬
        //    sf[2].LineAlignment = StringAlignment.Near;//높이 정렬

        //    sf[3] = new StringFormat();
        //    sf[3].Alignment = StringAlignment.Center;//가로 정렬
        //    sf[3].LineAlignment = StringAlignment.Near;//높이 정렬


        //    sf[4] = new StringFormat();
        //    sf[4].Alignment = StringAlignment.Near;//가로 정렬
        //    sf[4].LineAlignment = StringAlignment.Near;//높이 정렬


        //    //테두리.
        //    drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight, (float)96, 97);
        //    e.Graphics.DrawRectangle(pen, drawRect[0].X, drawRect[0].Y, drawRect[0].Width, drawRect[0].Height);

        //    //슈트번호.
        //    drawRect[1] = new RectangleF((float)(startWidth) + 66, (float)startHeight, (float)30, 10);
        //    e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);
        //    e.Graphics.DrawString(tempTable.Rows[cnt]["chute_no"].ToString(), new Font("Arial", 20), Brushes.Black, drawRect[1], sf[0]);

        //    //매장명
        //    drawRect[2] = new RectangleF((float)(startWidth), (float)startHeight + 11, (float)94, 40);
        //    //e.Graphics.DrawRectangle(pen, drawRect[2].X, drawRect[2].Y, drawRect[2].Width, drawRect[2].Height);
        //    e.Graphics.DrawString(shop_nm, new Font("Arial", 25), Brushes.Black, drawRect[2], sf[2]);




        //    //년도
        //    drawRect[3] = new RectangleF((float)(startWidth) + 3, (float)startHeight + 25, (float)30, 10);
        //    e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
        //    e.Graphics.DrawString("년도", new Font("Arial", 10), Brushes.Black, drawRect[3], sf[0]);


        //    //시즌
        //    drawRect[4] = new RectangleF((float)(startWidth) + 33, (float)startHeight + 25, (float)30, 10);
        //    e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
        //    e.Graphics.DrawString("시즌", new Font("Arial", 10), Brushes.Black, drawRect[4], sf[0]);

        //    //아이템
        //    drawRect[5] = new RectangleF((float)(startWidth) + 63, (float)startHeight + 25, (float)30, 10);
        //    e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);
        //    e.Graphics.DrawString("아이템", new Font("Arial", 10), Brushes.Black, drawRect[5], sf[0]);


        //    yearList = this.tempTable.AsEnumerable().GroupBy(k => k.Field<string>("year")).Select(k => k.Key).ToList();
        //    seasonList = this.tempTable.AsEnumerable().GroupBy(k => k.Field<string>("season_name")).Select(k => k.Key).ToList();
        //    itemList = this.tempTable.AsEnumerable().GroupBy(k => k.Field<string>("item")).Select(k => k.Key).ToList();

        //    //3 : 년도, 4: 시즌 , 5:아이템
        //    drawRect[3] = new RectangleF((float)(startWidth) + 3, (float)startHeight + 35, (float)30, 25);
        //    e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
        //    drawRect[4] = new RectangleF((float)(startWidth) + 33, (float)startHeight + 35, (float)30, 25);
        //    e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
        //    drawRect[5] = new RectangleF((float)(startWidth) + 63, (float)startHeight + 35, (float)30, 25);
        //    e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);

        //    string year_string = string.Empty;
        //    string season_string = string.Empty;
        //    string item_string = string.Empty;

        //    //년도 데이터
        //    for (int i = 0; i < yearList.Count; i++)
        //    {
        //        if (i != 0)
        //        {
        //            year_string += "," + yearList[i];
        //        }
        //        else
        //        {
        //            year_string += yearList[i];
        //        }
        //    }
        //    e.Graphics.DrawString(year_string, new Font("Arial", 15), Brushes.Black, drawRect[3], sf[0]);


        //    //시즌 데이터
        //    for (int i = 0; i < seasonList.Count; i++)
        //    {
        //        if (i != 0)
        //        {
        //            season_string += "," + seasonList[i];
        //        }
        //        else
        //        {
        //            season_string += seasonList[i];
        //        }
        //    }
        //    e.Graphics.DrawString(season_string, new Font("Arial", 15), Brushes.Black, drawRect[4], sf[0]);


        //    //아이템 데이터
        //    for (int i = 0; i < itemList.Count; i++)
        //    {
        //        if (i != 0)
        //        {
        //            item_string += "," + itemList[i];
        //        }
        //        else
        //        {
        //            item_string += itemList[i];
        //        }
        //    }
        //    e.Graphics.DrawString(item_string, new Font("Arial", 15), Brushes.Black, drawRect[5], sf[0]);


        //    // 송장 하단 출력부분 ---------------------------------------------
        //    e.Graphics.DrawString("BOX    " + box_no, new Font("Arial", 15), Brushes.Black, 5, 65, sf[4]); //박스 번호
        //    e.Graphics.DrawString("PCS    " + wrk_qty, new Font("Arial", 15), Brushes.Black, 60, 65, sf[4]); //입수량 번호

        //    //송장 출력
        //    e.Graphics.DrawString(barcode.CODE128(box_inv, "B", ref err_cdoe), new Font("Code 128", 35), Brushes.Black, 48, 70, sf[3]);
        //    e.Graphics.DrawString(box_inv, new Font("Arial", 10), Brushes.Black, 48, 83, sf[3]);

        //    ////출력일자
        //    e.Graphics.DrawString("출력일시 : " + DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 48, 90, sf[3]);

        //    pd.PrintPage -= new PrintPageEventHandler(unassigned);
        //}


        private void bundle_Label(object sender, PrintPageEventArgs e)
        {

            float startWidth = 0;//시작 x좌표
            float startHeight = 1;//시작 y좌표            

            List<string> yearList = new List<string>();
            List<string> seasonList = new List<string>();
            List<string> itemList = new List<string>();

            string box_inv = this.tempTable.Rows[0]["box_inv"].ToString();
            string box_no = this.tempTable.Rows[0]["box_no"].ToString();
            string wrk_qty = this.tempTable.AsEnumerable().Sum(i => i.Field<int>("wrk_qty")).ToString();


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
            sf[2].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[3] = new StringFormat();
            sf[3].Alignment = StringAlignment.Center;//가로 정렬
            sf[3].LineAlignment = StringAlignment.Near;//높이 정렬


            sf[4] = new StringFormat();
            sf[4].Alignment = StringAlignment.Near;//가로 정렬
            sf[4].LineAlignment = StringAlignment.Near;//높이 정렬

            //테두리.
            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight, (float)96, 97);
            e.Graphics.DrawRectangle(pen, drawRect[0].X, drawRect[0].Y, drawRect[0].Width, drawRect[0].Height);


            //슈트번호.
            drawRect[1] = new RectangleF((float)(startWidth) + 25, (float)startHeight, (float)50, 25);
            //e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);
            e.Graphics.DrawString(tempTable.Rows[0]["chute_no"].ToString(), new Font("Arial", 35), Brushes.Black, drawRect[1], sf[0]);


            //번들유형
            drawRect[1] = new RectangleF((float)(startWidth) + 50, (float)startHeight, (float)50, 25);
            //e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);
            e.Graphics.DrawString(tempTable.Rows[0]["assort_cd"].ToString(), new Font("Arial", 35), Brushes.Black, drawRect[1], sf[0]);

            //년도
            drawRect[3] = new RectangleF((float)(startWidth) + 3, (float)startHeight + 18, (float)30, 10);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            e.Graphics.DrawString("년도", new Font("Arial", 10), Brushes.Black, drawRect[3], sf[0]);


            //시즌
            drawRect[4] = new RectangleF((float)(startWidth) + 33, (float)startHeight + 18, (float)30, 10);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            e.Graphics.DrawString("시즌", new Font("Arial", 10), Brushes.Black, drawRect[4], sf[0]);

            //아이템
            drawRect[5] = new RectangleF((float)(startWidth) + 63, (float)startHeight + 18, (float)30, 10);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);
            e.Graphics.DrawString("아이템", new Font("Arial", 10), Brushes.Black, drawRect[5], sf[0]);


            yearList = this.tempTable.AsEnumerable().GroupBy(k => k.Field<string>("year")).Select(k => k.Key).ToList();
            seasonList = this.tempTable.AsEnumerable().GroupBy(k => k.Field<string>("season_name")).Select(k => k.Key).ToList();
            itemList = this.tempTable.AsEnumerable().GroupBy(k => k.Field<string>("item")).Select(k => k.Key).ToList();

            //3 : 년도, 4: 시즌 , 5:아이템
            drawRect[3] = new RectangleF((float)(startWidth) + 3, (float)startHeight + 28, (float)30, 30);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            drawRect[4] = new RectangleF((float)(startWidth) + 33, (float)startHeight + 28, (float)30, 30);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            drawRect[5] = new RectangleF((float)(startWidth) + 63, (float)startHeight + 28, (float)30, 30);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);

            string year_string = string.Empty;
            string season_string = string.Empty;
            string item_string = string.Empty;

            //년도 데이터
            for (int i = 0; i < yearList.Count; i++)
            {
                if (i != 0)
                {
                    year_string += "," + yearList[i];
                }
                else
                {
                    year_string += yearList[i];
                }
            }
            e.Graphics.DrawString(year_string, new Font("Arial", 20), Brushes.Black, drawRect[3], sf[0]);


            //시즌 데이터
            for (int i = 0; i < seasonList.Count; i++)
            {
                if (i != 0)
                {
                    season_string += "," + seasonList[i];
                }
                else
                {
                    season_string += seasonList[i];
                }
            }
            e.Graphics.DrawString(season_string, new Font("Arial", 20), Brushes.Black, drawRect[4], sf[0]);


            //아이템 데이터
            for (int i = 0; i < itemList.Count; i++)
            {
                if (i != 0)
                {
                    item_string += "," + itemList[i];
                }
                else
                {
                    item_string += itemList[i];
                }
            }
            e.Graphics.DrawString(item_string, new Font("Arial", 20), Brushes.Black, drawRect[5], sf[0]);


            // 송장 하단 출력부분 ---------------------------------------------
            e.Graphics.DrawString("BOX    " + box_no, new Font("Arial", 15), Brushes.Black, 5, 65, sf[4]); //박스 번호
            e.Graphics.DrawString("PCS    " + wrk_qty, new Font("Arial", 15), Brushes.Black, 60, 65, sf[4]); //입수량 번호

            //송장 출력
            e.Graphics.DrawString(barcode.CODE128(box_inv, "B", ref err_cdoe), new Font("Code 128", 35), Brushes.Black, 48, 70, sf[3]);
            e.Graphics.DrawString(box_inv, new Font("Arial", 10), Brushes.Black, 48, 83, sf[3]);

            ////출력일자
            //e.Graphics.DrawString("출력일시 : " + DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 48, 90, sf[3]);
            e.Graphics.DrawString("출력일시 : " + tempTable.Rows[0]["biz_day"].ToString(), new Font("Arial", 10), Brushes.Black, 48, 90, sf[3]);

            
            pd.PrintPage -= new PrintPageEventHandler(bundle_Label);
        }

        private void H_Label(object sender, PrintPageEventArgs e)
        {

            float startWidth = 0;//시작 x좌표
            float startHeight = 1;//시작 y좌표            

            pen = new Pen(Brushes.Black, (float)0.2);
            //pen.Freeze();


            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.


            RectangleF[] drawRect = new System.Drawing.RectangleF[8];
            StringFormat[] sf = new StringFormat[10];

            sf[0] = new StringFormat();
            sf[0].Alignment = StringAlignment.Center;// X축 중앙 정렬
            sf[0].LineAlignment = StringAlignment.Center;//Y축 중앙 정렬

            sf[1] = new StringFormat();
            sf[1].Alignment = StringAlignment.Center;//x축 중앙
            sf[1].LineAlignment = StringAlignment.Near;//y측 상부 

            sf[2] = new StringFormat();
            sf[2].Alignment = StringAlignment.Near;//x축 좌측
            sf[2].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[3] = new StringFormat();
            sf[3].Alignment = StringAlignment.Far;//가로 정렬
            sf[3].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[4] = new StringFormat();
            sf[4].Alignment = StringAlignment.Near;//가로 정렬
            //sf[4].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[5] = new StringFormat();
            sf[5].Alignment = StringAlignment.Center;//가로 정렬
            sf[5].LineAlignment = StringAlignment.Far;//높이 정렬

            sf[6] = new StringFormat();
            sf[6].Alignment = StringAlignment.Near;//가로 정렬
            sf[6].LineAlignment = StringAlignment.Far;//높이 정렬

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
            * [0]: 배송사
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

            if (tempString[9] == "Y")
            {
                tempString[8] = tempString[8] + "  E";
            }


            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight, (float)100, 100);
            e.Graphics.DrawRectangle(pen, drawRect[0].X, drawRect[0].Y, drawRect[0].Width, drawRect[0].Height);

            //택배사명(HANJIN).
            e.Graphics.DrawString("HANJIN", new Font("HY헤드라인M", 10, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 5, (float)startHeight + 1, sf[2]);

            //택배사 전화번호
            e.Graphics.DrawString("1588-9040", new Font("HY헤드라인M", 10, FontStyle.Bold), Brushes.Black, (startWidth) + 95, (float)startHeight + 1, sf[3]);

            //수신처 ===================================================================================================================
            e.Graphics.DrawString("수신처:", new Font("HY헤드라인M", 6, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 5, (float)(startHeight) + 7, sf[2]);

            //매장명
            e.Graphics.DrawString(tempString[4], new Font("HY헤드라인M", 22, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 13, (float)(startHeight) + 7, sf[2]);

            //브랜드 명
            e.Graphics.DrawString(tempString[2], new Font("HY헤드라인M", 10), Brushes.Black, (float)(startWidth) + 13, (float)(startHeight) + 15, sf[2]);

            //매장 전화 번호
            e.Graphics.DrawString(tempString[7], new Font("HY헤드라인M", 10), Brushes.Black, (float)(startWidth) + 55, (float)(startHeight) + 15, sf[2]);

            //매장 주소
            e.Graphics.DrawString(tempString[5] + tempString[6], new Font("HY헤드라인M", 8), Brushes.Black, (float)(startWidth) + 13, (float)(startHeight) + 19, sf[2]);

            string gubunLine = "------------------------------------------------";
            //구분선
            e.Graphics.DrawString(gubunLine, new Font("HY헤드라인M", 8), Brushes.Black, (float)(startWidth) + 5, (float)(startHeight) + 21.5f, sf[2]);

            //==========================================================================================================================


            //발신처 ===================================================================================================================
            e.Graphics.DrawString("발신처:", new Font("HY헤드라인M", 6, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 5, (float)(startHeight) + 25, sf[2]);

            //발신 회사명
            e.Graphics.DrawString(tempString[1], new Font("HY헤드라인M", 17, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 13, (float)(startHeight) + 25, sf[2]);

            //발신 물류센터명
            e.Graphics.DrawString("WGL물류", new Font("HY헤드라인M", 10), Brushes.Black, (float)(startWidth) + 13, (float)(startHeight) + 31, sf[2]);

            //구분선
            e.Graphics.DrawString(gubunLine, new Font("HY헤드라인M", 8), Brushes.Black, (float)(startWidth) + 5, (float)(startHeight) + 34.5f, sf[2]);

            //==========================================================================================================================


            //한진 배송 집배점 코드
            e.Graphics.DrawString(tempString[11], new Font("HY헤드라인M", 60, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 5, (float)(startHeight) + 38, sf[2]);

            //출력일시
            e.Graphics.DrawString(DateTime.Now.ToString(), new Font("HY헤드라인M", 3.5f, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 5, (float)(startHeight) + 60, sf[2]);

            //권역 분류 바코드
            e.Graphics.DrawString(barcode.CODE128(tempString[11], "B", ref err_cdoe), new Font("code 128", 30), Brushes.Black, 80, 38, sf[1]);

            //한진 배송 집배점명
            e.Graphics.DrawString(tempString[12], new Font("HY헤드라인M", 20, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 80, (float)(startHeight) + 48, sf[1]);



            //Telerik.WinControls.UI.RadBarcode tempBarcode = new Telerik.WinControls.UI.RadBarcode();
            //Telerik.WinControls.UI.Barcode.Symbology.Code25Interleaved code25Interleaved = new Telerik.WinControls.UI.Barcode.Symbology.Code25Interleaved();
            //tempBarcode.Checksum = false;
            //tempBarcode.LineAlign = System.Drawing.StringAlignment.Far;
            //tempBarcode.Module = 2;
            //tempBarcode.ShowText = true;
            //tempBarcode.Stretch = false;
            //tempBarcode.Value = tempString[13];
            //tempBarcode.Symbology = code25Interleaved;

            TelerikBarcode.TelerikBarcode tempBarcode = new TelerikBarcode.TelerikBarcode();
            tempBarcode.checksum = false;
            tempBarcode.stringLineAlign = System.Drawing.StringAlignment.Far;
            tempBarcode.module = 2;
            tempBarcode.showText = true;
            tempBarcode.stretch = false;


            Image tempBarcodeImage = tempBarcode.getbarcode(tempString[13],200,100);
            e.Graphics.DrawImage(tempBarcodeImage, (float)(startWidth) + 50, (float)(startHeight) + 70);

            //한진 배송 집배점 코드
            e.Graphics.DrawString(tempString[13], new Font("HY헤드라인M", 10, FontStyle.Bold), Brushes.Black, (float)(startWidth) + 50, (float)(startHeight) + 65, sf[1]);
            //e.Graphics.DrawString(tempString[13], new Font("test", 30), Brushes.Black, (float)(startWidth) + 50, (float)(startHeight) + 70, sf[1]);


            //Interleaved 2of5

            pd.PrintPage -= new PrintPageEventHandler(H_Label);
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
            sf[5].Alignment = StringAlignment.Center;//가로 정렬
            sf[5].LineAlignment = StringAlignment.Far;//높이 정렬

            sf[6] = new StringFormat();
            sf[6].Alignment = StringAlignment.Near;//가로 정렬
            sf[6].LineAlignment = StringAlignment.Far;//높이 정렬

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
            * [0]: 배송사
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

            if (tempString[9] == "Y")
            {
                tempString[8] = tempString[8] + "  E";
            }

            //발신.
            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight, (float)8, 20);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight, (float)8, 20);
            e.Graphics.DrawString("발신", new Font("Arial", 20), Brushes.Black, drawRect[0], sf[0]); //tns_code

            //업체 명, 신용정보
            drawRect[1] = new RectangleF((float)(startWidth) + 8, (float)startHeight, 60f, 20f);
            e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);

            //e.Graphics.DrawString(tempString[2], new Font("Arial", 25), Brushes.Black, drawRect[1], sf[5]); //tns_code
            e.Graphics.DrawString(tempString[2], new Font("Arial", 17), Brushes.Black, 8,8, sf[1]); //tns_code
            //e.Graphics.DrawString("신용", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, 35, 16, sf[0]);

            //권역 분류 바코드
            e.Graphics.DrawString(barcode.CODE128(tempString[11], "B", ref err_cdoe), new Font("code 128", 38f), Brushes.Black, 53, 5, sf[8]);

            //슈트번호
            drawRect[2] = new RectangleF((float)(startWidth) + 68, (float)startHeight, (float)28, 20f);
            e.Graphics.DrawRectangle(pen, drawRect[2].X, drawRect[2].Y, drawRect[2].Width, drawRect[2].Height);
            e.Graphics.DrawString(tempString[3], new Font("Arial", 20), Brushes.Black, drawRect[2], sf[2]); //tns_code

            //수신
            drawRect[3] = new RectangleF((float)(startWidth), (float)startHeight + 20, (float)8, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            e.Graphics.DrawString("수 신", new Font("Arial", 25), Brushes.Black, drawRect[3], sf[3]); //tns_code

            //매장명
            drawRect[4] = new RectangleF((float)(startWidth) + 8, (float)startHeight + 20, (float)88, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            e.Graphics.DrawString(tempString[4], new Font("Arial", 30), Brushes.Black, drawRect[4], sf[4]); //tns_code

            //ADDR1,ADDR2,TEL
            e.Graphics.DrawString(tempString[5] + tempString[6] + "\n"
                                  + "TEL) " + tempString[7], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, drawRect[4], sf[6]);

            //박스 번호
            drawRect[5] = new RectangleF((float)(startWidth), (float)startHeight + 60, (float)48, 10f);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);
            e.Graphics.DrawString("BOX", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 3, 63, sf[4]);
            e.Graphics.DrawString(tempString[8], new Font("Arial", 15), Brushes.Black, drawRect[5], sf[9]); //tns_code

            //PCS(제품 수량)
            drawRect[6] = new RectangleF((float)(startWidth) + 48, (float)startHeight + 60, (float)48, 10f);
            e.Graphics.DrawRectangle(pen, drawRect[6].X, drawRect[6].Y, drawRect[6].Width, drawRect[6].Height);
            e.Graphics.DrawString("PCS", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, 50, 63, sf[4]);
            e.Graphics.DrawString(tempString[10], new Font("Arial", 15), Brushes.Black, drawRect[6], sf[9]); //tns_code

            //집하 코드 집하 장소
            drawRect[7] = new RectangleF((float)(startWidth), (float)startHeight + 70, (float)96, 30f);
            e.Graphics.DrawRectangle(pen, drawRect[7].X, drawRect[7].Y, drawRect[7].Width, drawRect[7].Height);
            e.Graphics.DrawString(tempString[11] + "\r\n" + tempString[12], new Font("Arial", 22), Brushes.Black, drawRect[7], sf[7]); //집합점 코드 ,명칭

            //송장 출력
            e.Graphics.DrawString(barcode.CODE128(tempString[13], "B", ref err_cdoe), new Font("Code 128", 35), Brushes.Black, 35, 73, sf[8]);
            e.Graphics.DrawString(tempString[13], new Font("Arial", 10), Brushes.Black, 35, 85, sf[8]);

            //출력일자
            //e.Graphics.DrawString("출력일시 : " + DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 50, 90, sf[8]);
            e.Graphics.DrawString("출력일시 : " + tempString[17], new Font("Arial", 10), Brushes.Black, 50, 90, sf[8]);

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
            sf[4].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[5] = new StringFormat();
            sf[5].Alignment = StringAlignment.Near;//가로 정렬
            sf[5].LineAlignment = StringAlignment.Far;//높이 정렬

            sf[6] = new StringFormat();
            sf[6].Alignment = StringAlignment.Near;//가로 정렬
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
            * [0]: 배송사
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

            if (tempString[9] == "Y")
            {
                tempString[8] = tempString[8] + "  E";
            }

            //발신.
            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight + 4, (float)10, 16);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight + 4, (float)10, 16);
            e.Graphics.DrawString("발신", new Font("Arial", 20), Brushes.Black, drawRect[0], sf[0]); //tns_code

            //업체 명, 신용정보
            drawRect[1] = new RectangleF((float)(startWidth) + 10, (float)startHeight + 4, 60f, 16);
            e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);

            //e.Graphics.DrawString(tempString[2], new Font("Arial", 20), Brushes.Black, drawRect[1], sf[7]); //tns_code

            e.Graphics.DrawString(tempString[2], new Font("Arial", 20), Brushes.Black, startWidth + 10, startHeight + 5, sf[1]); //tns_code


            e.Graphics.DrawString("TOPKOREA", new Font("Arial", 20), Brushes.Black, startWidth + 10, startHeight + 12, sf[1]); //tns_code

            //슈트번호
            drawRect[2] = new RectangleF((float)(startWidth) + 70, (float)startHeight + 4, (float)26f, 16f);
            e.Graphics.DrawRectangle(pen, drawRect[2].X, drawRect[2].Y, drawRect[2].Width, drawRect[2].Height);
            e.Graphics.DrawString(tempString[3], new Font("Arial", 20), Brushes.Black, drawRect[2], sf[2]); //tns_code

            //수신
            drawRect[3] = new RectangleF((float)(startWidth), (float)startHeight + 20, (float)10f, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            e.Graphics.DrawString("수 신", new Font("Arial", 25), Brushes.Black, drawRect[3], sf[3]); //tns_code

            //매장명
            drawRect[4] = new RectangleF((float)(startWidth) + 10, (float)startHeight + 20, (float)86f, 40f);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            e.Graphics.DrawString(tempString[4], new Font("Arial", 25), Brushes.Black, drawRect[4], sf[4]); //tns_code


            //ADDR1,ADDR2,TEL
            e.Graphics.DrawString(tempString[5] + " " + tempString[6] + " " + tempString[7], new Font("Arial", 10), Brushes.Black, drawRect[4], sf[5]); //tns_code

            //e.Graphics.DrawString(tempString[5], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 40, sf[4]);
            //e.Graphics.DrawString(tempString[6], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 45, sf[4]);
            //e.Graphics.DrawString(tempString[7], new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 12, 50, sf[4]);


            //drawRect[4] = new RectangleF((float)(startWidth) + 10, (float)startHeight + 20, (float)86f, 40f);
            //e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);

            //박스 번호
            drawRect[5] = new RectangleF((float)(startWidth), (float)startHeight + 60, (float)48, 6f);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);
            e.Graphics.DrawString("BOX", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, drawRect[5], sf[6]);
            e.Graphics.DrawString(tempString[8], new Font("Arial", 15), Brushes.Black, drawRect[5], sf[9]); //tns_code

            //PCS(제품 수량)
            drawRect[6] = new RectangleF((float)(startWidth) + 48, (float)startHeight + 60, (float)48, 6f);
            e.Graphics.DrawRectangle(pen, drawRect[6].X, drawRect[6].Y, drawRect[6].Width, drawRect[6].Height);
            e.Graphics.DrawString("PCS", new Font("Arial", 15, FontStyle.Bold), Brushes.Black, drawRect[6], sf[6]);
            e.Graphics.DrawString(tempString[10], new Font("Arial", 15), Brushes.Black, drawRect[6], sf[9]); //tns_code

            //집하 코드 집하 장소
            drawRect[7] = new RectangleF((float)(startWidth), (float)startHeight + 66, (float)96, 27f);
            e.Graphics.DrawRectangle(pen, drawRect[7].X, drawRect[7].Y, drawRect[7].Width, drawRect[7].Height);
            e.Graphics.DrawString(tempString[11] + "\r\n" + tempString[12], new Font("Arial", 30), Brushes.Black, drawRect[7], sf[7]); //tns_code

            //송장 출력
            e.Graphics.DrawString(barcode.CODE128(tempString[13], "B", ref err_cdoe), new Font("Code 128", 35), Brushes.Black, 48, 70, sf[8]);
            e.Graphics.DrawString(tempString[13], new Font("Arial", 10), Brushes.Black, 48, 83, sf[8]);

            //출력일자
            //e.Graphics.DrawString("출력일시 : " + DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 50, 90, sf[8]);
            e.Graphics.DrawString("출력일시 : " + tempString[17], new Font("Arial", 10), Brushes.Black, 50, 90, sf[8]);

            // topcorea 찍는것.!
            drawRect[8] = new RectangleF((float)(startWidth), (float)startHeight, (float)96, 4);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight, (float)96, 4);
            e.Graphics.DrawString("TOPKOREA TOPKREA TOPKREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA", new Font("Arial", 5), Brushes.Black, drawRect[8], sf[0]); //tns_code


            drawRect[8] = new RectangleF((float)(startWidth), (float)startHeight + 93, (float)96, 4);
            e.Graphics.DrawRectangle(pen, (float)(startWidth), (float)startHeight + 93, (float)96, 4);
            e.Graphics.DrawString("TOPKOREA TOPKREA TOPKREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA TOPKOREA", new Font("Arial", 5), Brushes.Black, drawRect[8], sf[0]); //tns_code


            pd.PrintPage -= new PrintPageEventHandler(top_Label);
        }

        private void Unassigned(object sender, PrintPageEventArgs e)
        {

            float startWidth = 0.5f;//시작 x좌표
            float startHeight = 1;//시작 y좌표            


            List<string> yearList = new List<string>();
            List<string> seasonList = new List<string>();
            List<string> itemList = new List<string>();

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
            sf[2].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[3] = new StringFormat();
            sf[3].Alignment = StringAlignment.Center;//가로 정렬
            sf[3].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[4] = new StringFormat();
            sf[4].Alignment = StringAlignment.Near;//가로 정렬
            //sf[4].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[5] = new StringFormat();
            //sf[5].Alignment = StringAlignment.Center;//가로 정렬
            sf[5].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[6] = new StringFormat();
            sf[6].Alignment = StringAlignment.Near;//가로 정렬
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
            * [0]: 배송사
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
            * [14]: 복종 코드
            * [15]: 연도
            * [16]: 시즌
            */

            if (tempString[9] == "Y")
            {
                tempString[8] = tempString[8] + "  E";
            }

            //테두리.
            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight, (float)96, 97);
            e.Graphics.DrawRectangle(pen, drawRect[0].X, drawRect[0].Y, drawRect[0].Width, drawRect[0].Height);

            //슈트번호.
            drawRect[1] = new RectangleF((float)(startWidth) + 66, (float)startHeight, (float)30, 10);
            e.Graphics.DrawRectangle(pen, drawRect[1].X, drawRect[1].Y, drawRect[1].Width, drawRect[1].Height);
            e.Graphics.DrawString(tempString[3], new Font("Arial", 20), Brushes.Black, drawRect[1], sf[0]);

            //매장명
            drawRect[2] = new RectangleF((float)(startWidth), (float)startHeight + 11, (float)94, 40);
            //e.Graphics.DrawRectangle(pen, drawRect[2].X, drawRect[2].Y, drawRect[2].Width, drawRect[2].Height);
            e.Graphics.DrawString(tempString[4], new Font("Arial", 25), Brushes.Black, drawRect[2], sf[2]);


            //년도
            drawRect[3] = new RectangleF((float)(startWidth) + 3, (float)startHeight + 25, (float)30, 10);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            e.Graphics.DrawString("년도", new Font("Arial", 10), Brushes.Black, drawRect[3], sf[0]);


            //시즌
            drawRect[4] = new RectangleF((float)(startWidth) + 33, (float)startHeight + 25, (float)30, 10);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            e.Graphics.DrawString("시즌", new Font("Arial", 10), Brushes.Black, drawRect[4], sf[0]);

            //아이템
            drawRect[5] = new RectangleF((float)(startWidth) + 63, (float)startHeight + 25, (float)30, 10);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);
            e.Graphics.DrawString("아이템", new Font("Arial", 10), Brushes.Black, drawRect[5], sf[0]);


            yearList = tempString[15].Split(',').Distinct().ToList();
            seasonList = tempString[16].Split(',').Distinct().ToList();
            itemList = tempString[14].Split(',').Distinct().ToList();


            //3 : 년도, 4: 시즌 , 5:아이템
            drawRect[3] = new RectangleF((float)(startWidth) + 3, (float)startHeight + 35, (float)30, 25);
            e.Graphics.DrawRectangle(pen, drawRect[3].X, drawRect[3].Y, drawRect[3].Width, drawRect[3].Height);
            drawRect[4] = new RectangleF((float)(startWidth) + 33, (float)startHeight + 35, (float)30, 25);
            e.Graphics.DrawRectangle(pen, drawRect[4].X, drawRect[4].Y, drawRect[4].Width, drawRect[4].Height);
            drawRect[5] = new RectangleF((float)(startWidth) + 63, (float)startHeight + 35, (float)30, 25);
            e.Graphics.DrawRectangle(pen, drawRect[5].X, drawRect[5].Y, drawRect[5].Width, drawRect[5].Height);



            string year_string = string.Empty;
            string season_string = string.Empty;
            string item_string = string.Empty;

            //년도 데이터
            for (int i = 0; i < yearList.Count; i++)
            {
                if (i != 0)
                {
                    year_string += "," + yearList[i];
                }
                else
                {
                    year_string += yearList[i];
                }
            }
            e.Graphics.DrawString(year_string, new Font("Arial", 15), Brushes.Black, drawRect[3], sf[0]);


            //시즌 데이터
            for (int i = 0; i < seasonList.Count; i++)
            {
                if (i != 0)
                {
                    season_string += "," + seasonList[i];
                }
                else
                {
                    season_string += seasonList[i];
                }
            }
            e.Graphics.DrawString(season_string, new Font("Arial", 15), Brushes.Black, drawRect[4], sf[0]);


            //아이템 데이터
            for (int i = 0; i < itemList.Count; i++)
            {
                if (i != 0)
                {
                    item_string += "," + itemList[i];
                }
                else
                {
                    item_string += itemList[i];
                }
            }
            e.Graphics.DrawString(item_string, new Font("Arial", 15), Brushes.Black, drawRect[5], sf[0]);


            // 송장 하단 출력부분 ---------------------------------------------
            e.Graphics.DrawString("BOX    " + tempString[8], new Font("Arial", 15), Brushes.Black, 5, 65, sf[4]); //박스 번호
            e.Graphics.DrawString("PCS    " + tempString[10], new Font("Arial", 15), Brushes.Black, 60, 65, sf[4]); //입수량 번호

            //송장 출력
            e.Graphics.DrawString(barcode.CODE128(tempString[13], "B", ref err_cdoe), new Font("Code 128", 35), Brushes.Black, 48, 70, sf[3]);
            e.Graphics.DrawString(tempString[13], new Font("Arial", 10), Brushes.Black, 48, 83, sf[3]);

            ////출력일자
            //e.Graphics.DrawString("출력일시 : " + DateTime.Now.ToString(), new Font("Arial", 10), Brushes.Black, 50, 90, sf[8]);
            e.Graphics.DrawString("출력일시 : " + tempString[17], new Font("Arial", 10), Brushes.Black, 50, 90, sf[3]);

            pd.PrintPage -= new PrintPageEventHandler(Unassigned);
        }

        private void error_Label(object sender, PrintPageEventArgs e)
        {

            float startWidth = 0;//시작 x좌표
            float startHeight = 1;//시작 y좌표            

            pen = new Pen(Brushes.Black, (float)0.2);
            //pen.Freeze();


            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.


            RectangleF[] drawRect = new System.Drawing.RectangleF[1];
            StringFormat[] sf = new StringFormat[1];

            sf[0] = new StringFormat();
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬


            //테두리.
            drawRect[0] = new RectangleF((float)(startWidth), (float)startHeight, (float)96, 97);
            e.Graphics.DrawRectangle(pen, drawRect[0].X, drawRect[0].Y, drawRect[0].Width, drawRect[0].Height);
            e.Graphics.DrawString(tempString[1] + "/" + tempString[2], new Font("Arial", 20), Brushes.Black, drawRect[0], sf[0]);


            pd.PrintPage -= new PrintPageEventHandler(error_Label);


        }


        private void pickingListPrint(object sender, PrintPageEventArgs e)
        {

            int startWidth = 0;//시작 x좌표
            int startHeight = 0;//시작 y좌표            


            pen = new Pen(Brushes.Black, (float)0.2);
            int[] boxWight = new int[8]; //박스 두께
            string[] headText = new string[8]; //컬럼 명
            boxWight[0] = 20;
            boxWight[1] = 20;
            boxWight[2] = 45;
            boxWight[3] = 20;
            boxWight[4] = 20;
            boxWight[5] = 25;
            boxWight[6] = 25;
            boxWight[7] = 25;

            headText[0] = "보관";
            headText[1] = "복종";
            headText[2] = "품번";
            headText[3] = "컬러";
            headText[4] = "규격";
            headText[5] = "전재고";
            headText[6] = "지시수량";
            headText[7] = "후재고";


            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.


            StringFormat[] sf = new StringFormat[10];

            sf[0] = new StringFormat(); //세로, 가로 가운데정렬
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[1] = new StringFormat();

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
            sf[6].Alignment = StringAlignment.Near;//가로 정렬
            sf[6].LineAlignment = StringAlignment.Far;//높이 정렬

            sf[7] = new StringFormat();
            sf[7].Alignment = StringAlignment.Far;//가로 정렬
            sf[7].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[8] = new StringFormat();
            sf[8].Alignment = StringAlignment.Center;//가로 정렬
            sf[8].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[9] = new StringFormat();
            sf[9].Alignment = StringAlignment.Far;//가로 정렬
            sf[9].LineAlignment = StringAlignment.Center;//높이 정렬

            startHeight += 3;
            e.Graphics.DrawString("일자 : " + biz_day, new Font("Arial", 10), Brushes.Black, startWidth + 5, startHeight); //작업 날짜
            e.Graphics.DrawString("차수 : " + batch, new Font("Arial", 10), Brushes.Black, startWidth + 70, startHeight); //작업 차수

            // -------------------------------------------------- 컬럼명 찍기 -------------------------------------------
            Font headFont = new Font("Arial", 10);
            int headPointX = 5;
            int headRecSizeH = 10;

            startHeight += 5;

            for (int i = 0; i < boxWight.Length; i++)
            {
                Rectangle Rect = new Rectangle(new Point(startWidth + headPointX, startHeight), new Size(boxWight[i], headRecSizeH));
                e.Graphics.DrawRectangle(pen, Rect);
                e.Graphics.DrawString(headText[i], headFont, Brushes.Black, Rect, sf[0]);
                headPointX += boxWight[i];
            }
            // ---------------------------------------------------------------------------------------------------


            Font rowFont = new Font("Arial", 6);
            int rowRecY = startHeight + headRecSizeH;
            int dataRecSizeH = 6;


            Rectangle[] drawRect = new System.Drawing.Rectangle[8];
            Rectangle rowRectangle = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(5, headRecSizeH)); // row 박스
            string[] check = new string[8];

            List<string> itemCd_List = tempTable.AsEnumerable().Select(k => k.Field<string>("variety")).Distinct().ToList();

            //한개 품목이 아이템이 많아서 페이지가 넘어갈 경우 테이블 복사를 하지않음.
            if (printTable.Rows.Count == 0)
            {
                printTable = tempTable.AsEnumerable().Where(k => k.Field<string>("variety") == itemCd_List[0]).CopyToDataTable();//프린트할 제품만 복사
                tempTable.AsEnumerable().Where(k => k.Field<string>("variety") == itemCd_List[0]).ToList().ForEach(k => k.Delete());//출력한 아이템 정보 삭제
                tempTable.AcceptChanges();//커밋
            }

            int data_rowCount = printTable.Rows.Count > 45 ? 45 : printTable.Rows.Count; //한페이지에 최대 뽑을수 잇는 데이터 수량

            for (int i = 0; i < data_rowCount; i++)
            {
                headPointX = 5;

                //데이터 표 그리기
                for (int k = 0; k < drawRect.Count(); k++)
                {
                    drawRect[k] = new Rectangle(new Point(startWidth + headPointX, rowRecY), new Size(boxWight[k], dataRecSizeH));
                    e.Graphics.DrawRectangle(pen, drawRect[k]);
                    headPointX += boxWight[k];

                }

                if (i == 0 || check[0] != printTable.Rows[i]["ablad"].ToString())
                {
                    e.Graphics.DrawString(printTable.Rows[i]["ablad"].ToString(), headFont, Brushes.Black, drawRect[0], sf[0]);
                }

                //전에 로우와 같은 품목은 아이템을 다시 쓰지않기위함. --------------------------------------------------------
                if (check[1] != printTable.Rows[i]["variety"].ToString() || i == 0)
                {
                    e.Graphics.DrawString(printTable.Rows[i]["variety"].ToString(), headFont, Brushes.Black, drawRect[1], sf[0]);
                }
                else
                {
                    e.Graphics.DrawString("", headFont, Brushes.Black, drawRect[1], sf[0]);
                }

                if (check[2] != printTable.Rows[i]["ITEM_STYLE"].ToString() || i == 0)
                {
                    e.Graphics.DrawString(printTable.Rows[i]["ITEM_STYLE"].ToString(), headFont, Brushes.Black, drawRect[2], sf[0]);
                }
                else
                {
                    e.Graphics.DrawString("", headFont, Brushes.Black, drawRect[2], sf[0]);
                }


                if (check[3] != printTable.Rows[i]["ITEM_COLOR"].ToString() || check[2] != printTable.Rows[i]["ITEM_STYLE"].ToString() || i == 0)
                {
                    e.Graphics.DrawString(printTable.Rows[i]["ITEM_COLOR"].ToString(), headFont, Brushes.Black, drawRect[3], sf[0]);

                }
                else
                {
                    e.Graphics.DrawString("", headFont, Brushes.Black, drawRect[3], sf[0]);

                }

                if (check[3] != printTable.Rows[i]["ITEM_COLOR"].ToString() || check[2] != printTable.Rows[i]["ITEM_STYLE"].ToString()
                    || check[4] != printTable.Rows[i]["ITEM_SIZE"].ToString() || i == 0)
                {
                    e.Graphics.DrawString(printTable.Rows[i]["ITEM_SIZE"].ToString(), headFont, Brushes.Black, drawRect[4], sf[0]);

                }
                else
                {
                    e.Graphics.DrawString("", headFont, Brushes.Black, drawRect[4], sf[0]);

                }
                //-------------------------------------------------------------------------------------------------------------
                //e.Graphics.DrawString(printTable.Rows[i]["ITEM_COLOR"].ToString(), headFont, Brushes.Black, drawRect[2], sf[0]);
                //e.Graphics.DrawString(printTable.Rows[i]["ITEM_SIZE"].ToString(), headFont, Brushes.Black, drawRect[3], sf[0]);
                e.Graphics.DrawString(printTable.Rows[i]["STOCKF"].ToString(), headFont, Brushes.Black, drawRect[5], sf[0]);
                e.Graphics.DrawString(printTable.Rows[i]["ORD_QTY"].ToString(), headFont, Brushes.Black, drawRect[6], sf[0]);
                e.Graphics.DrawString(printTable.Rows[i]["STOCKR"].ToString(), headFont, Brushes.Black, drawRect[7], sf[0]);

                check[0] = printTable.Rows[i]["ablad"].ToString();
                check[1] = printTable.Rows[i]["variety"].ToString();
                check[2] = printTable.Rows[i]["ITEM_STYLE"].ToString();
                check[3] = printTable.Rows[i]["ITEM_COLOR"].ToString();
                check[4] = printTable.Rows[i]["ITEM_SIZE"].ToString();
                rowRecY += dataRecSizeH;

            }

            cnt++;
            e.Graphics.DrawString(cnt.ToString(), headFont, Brushes.Black, new Point(105, 291));

            //출력한 데이터는 삭제
            for (int i = 0; i < data_rowCount; i++)
            {
                printTable.Rows.RemoveAt(0);
            }

            if (tempTable.Rows.Count > 0)
            {

                e.HasMorePages = true;
            }
            else if (printTable.Rows.Count > 0)
            {
                //printTable.AsEnumerable().Where(k => k.Field<string>("variety") == itemCd_List[0]).ToList().ForEach(k => k.Delete());//출력한 아이템 정보 삭제
                //printTable.AcceptChanges();
                e.HasMorePages = true;
            }
            else
            {
                pd.PrintPage -= new PrintPageEventHandler(pickingListPrint);

            }

        }




        private void bundleA4List(object sender, PrintPageEventArgs e)
        {

            int startWidth = -4;//시작 x좌표
            int startHeight = 0;//시작 y좌표            


            pen = new Pen(Brushes.Black, (float)0.2);
            int[] boxWight = new int[6]; //박스 두께
            string[] headText = new string[6]; //컬럼 명
            boxWight[0] = 50;
            boxWight[1] = 30;
            boxWight[2] = 30;
            boxWight[3] = 30;
            boxWight[4] = 30;
            boxWight[5] = 30;
  
            headText[0] = "스타일";
            headText[1] = "번들";
            headText[2] = "컬러";
            headText[3] = "사이즈";
            headText[4] = "제품수량";
            headText[5] = "박스수";


            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.


            StringFormat[] sf = new StringFormat[10];

            sf[0] = new StringFormat(); //세로, 가로 가운데정렬
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[1] = new StringFormat();

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
            sf[6].Alignment = StringAlignment.Near;//가로 정렬
            sf[6].LineAlignment = StringAlignment.Far;//높이 정렬

            sf[7] = new StringFormat();
            sf[7].Alignment = StringAlignment.Far;//가로 정렬
            sf[7].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[8] = new StringFormat();
            sf[8].Alignment = StringAlignment.Center;//가로 정렬
            sf[8].LineAlignment = StringAlignment.Near;//높이 정렬

            sf[9] = new StringFormat();
            sf[9].Alignment = StringAlignment.Far;//가로 정렬
            sf[9].LineAlignment = StringAlignment.Center;//높이 정렬

            startHeight += 3;
            e.Graphics.DrawString("일자 : " + tempTable.Rows[0]["biz_day"].ToString(), new Font("Arial", 10), Brushes.Black, startWidth + 5, startHeight); //작업 날짜
            e.Graphics.DrawString("차수 : " + tempTable.Rows[0]["batch"].ToString(), new Font("Arial", 10), Brushes.Black, startWidth + 70, startHeight); //작업 차수
            e.Graphics.DrawString("슈트 : " + tempTable.Rows[0]["chute_no"].ToString(), new Font("Arial", 10), Brushes.Black, startWidth + 100, startHeight); //작업 차수

            // -------------------------------------------------- 컬럼명 찍기 -------------------------------------------
            Font headFont = new Font("Arial", 10);
            int headPointX = 5;
            int headRecSizeH = 10;

            startHeight += 5;

            for (int i = 0; i < boxWight.Length; i++)
            {
                Rectangle Rect = new Rectangle(new Point(startWidth + headPointX, startHeight), new Size(boxWight[i], headRecSizeH));
                e.Graphics.DrawRectangle(pen, Rect);
                e.Graphics.DrawString(headText[i], headFont, Brushes.Black, Rect, sf[0]);
                headPointX += boxWight[i];
            }
            // ---------------------------------------------------------------------------------------------------


            Font rowFont = new Font("Arial", 6);
            int rowRecY = startHeight + headRecSizeH;
            int dataRecSizeH = 15;


            Rectangle[] drawRect = new System.Drawing.Rectangle[8];
            Rectangle rowRectangle = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(5, headRecSizeH)); // row 박스
            string[] check = new string[8];

            List<int> itemCd_List = tempTable.AsEnumerable().Select(k => k.Field<int>("chute_no")).Distinct().ToList();

            //한개 품목이 아이템이 많아서 페이지가 넘어갈 경우 테이블 복사를 하지않음.
            if (printTable.Rows.Count == 0)
            {
                printTable = tempTable.AsEnumerable().Where(k => k.Field<int>("chute_no") == itemCd_List[0]).CopyToDataTable();//프린트할 제품만 복사
                tempTable.AsEnumerable().Where(k => k.Field<int>("chute_no") == itemCd_List[0]).ToList().ForEach(k => k.Delete());//출력한 아이템 정보 삭제
                tempTable.AcceptChanges();//커밋
            }

            //스타일,번들코드,컬러,박스 수량 사각 박스 높이 사이즈
            int boxH_Size = printTable.Rows.Count * dataRecSizeH;

            for (int i = 0; i < printTable.Rows.Count; i++)
            {
                headPointX = 5;




                //스타일,번들코드,컬러,박스수는 사각 테두리를 한번만 그림
                if (i == 0)
                {
                    //boxWight[0] = 50; 스타일
                    //boxWight[1] = 30; 번들코드
                    //boxWight[2] = 30; 컬러
                    //boxWight[3] = 30; 아이템 사이즈
                    //boxWight[4] = 30; 제품수량
                    //boxWight[5] = 30; 박스수량
                    drawRect[0] = new Rectangle(new Point(startWidth + headPointX, rowRecY), new Size(boxWight[0], boxH_Size));
                    e.Graphics.DrawRectangle(pen, drawRect[0]);
                    e.Graphics.DrawString(printTable.Rows[i]["item_style"].ToString(), headFont, Brushes.Black, drawRect[0], sf[0]);
                    headPointX += boxWight[0];

                    drawRect[1] = new Rectangle(new Point(startWidth + headPointX, rowRecY), new Size(boxWight[1], boxH_Size));
                    e.Graphics.DrawRectangle(pen, drawRect[1]);
                    e.Graphics.DrawString(printTable.Rows[i]["assort_cd"].ToString(), headFont, Brushes.Black, drawRect[1], sf[0]);
                    headPointX += boxWight[1];

                    drawRect[2] = new Rectangle(new Point(startWidth + headPointX, rowRecY), new Size(boxWight[2], boxH_Size));
                    e.Graphics.DrawRectangle(pen, drawRect[2]);
                    e.Graphics.DrawString(printTable.Rows[i]["item_color"].ToString(), headFont, Brushes.Black, drawRect[2], sf[0]);
                    headPointX += boxWight[2] + boxWight[3] + boxWight[4];

                    drawRect[5] = new Rectangle(new Point(startWidth + headPointX, rowRecY), new Size(boxWight[5], boxH_Size));
                    e.Graphics.DrawRectangle(pen, drawRect[5]);
                    e.Graphics.DrawString(printTable.Rows[i]["box_count"].ToString(), headFont, Brushes.Black, drawRect[5], sf[0]);
                }

                drawRect[3] = new Rectangle(new Point(startWidth + 115, rowRecY), new Size(boxWight[3], dataRecSizeH));
                e.Graphics.DrawRectangle(pen, drawRect[3]);
                e.Graphics.DrawString(printTable.Rows[i]["item_size"].ToString(), headFont, Brushes.Black, drawRect[3], sf[0]);

                drawRect[4] = new Rectangle(new Point(startWidth + 145, rowRecY), new Size(boxWight[4], dataRecSizeH));
                e.Graphics.DrawRectangle(pen, drawRect[4]);
                e.Graphics.DrawString(printTable.Rows[i]["ord_qty"].ToString(), headFont, Brushes.Black, drawRect[4], sf[0]);
                //e.Graphics.DrawString("123456", headFont, Brushes.Black, drawRect[3], sf[0]);

                //Console.WriteLine(printTable.Rows[i]["item_size"].ToString());
                rowRecY += dataRecSizeH;

            }

            cnt++;
            e.Graphics.DrawString(cnt.ToString(), headFont, Brushes.Black, new Point(105, 291));

            //출력한 데이터는 삭제
            //for (int i = 0; i < printTable.Rows.Count; i++)
            //{
            //    printTable.Rows.RemoveAt(0);
            //}

            //tempTable.AsEnumerable().Where(k => k.Field<int>("chute_no") == itemCd_List[0]).ToList().ForEach(k => k.Delete());//출력한 아이템 정보 삭제
            printTable.AsEnumerable().Where(k => k.Field<int>("chute_no") == itemCd_List[0]).ToList().ForEach(k => k.Delete());//출력한 아이템 정보 삭제
            printTable.AcceptChanges();

            if (tempTable.Rows.Count > 0)
            {

                e.HasMorePages = true;
            }
            else
            {
                pd.PrintPage -= new PrintPageEventHandler(bundleA4List);

            }

        }

        private void DASListPrint(object sender, PrintPageEventArgs e)
        {

            int startWidth = 0;//시작 x좌표
            int startHeight = 0;//시작 y좌표            

            pen = new Pen(Brushes.Black, (float)0.2);
            //pen.Freeze();

            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.

            StringFormat[] sf = new StringFormat[10];

            sf[0] = new StringFormat(); //세로, 가로 가운데정렬
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[1] = new StringFormat();
            //sf[0].Alignment = StringAlignment.;//가로 정렬
            //sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[2] = new StringFormat();
            sf[2].Alignment = StringAlignment.Center;//가로 정렬
            sf[2].LineAlignment = StringAlignment.Center;//높이 정렬



            //e.Graphics.DrawString("일자 : " + tempTable.Rows[0]["biz_day"].ToString(), new Font("Arial", 10), Brushes.Black, startWidth + 5, startHeight + 3); //작업 날짜
            //e.Graphics.DrawString("차수 : " + tempTable.Rows[0]["batch"].ToString(), new Font("Arial", 10), Brushes.Black, startWidth + 60, startHeight + 3); //작업 차수


            // -------------------------------------------------- 제목1 -------------------------------------------

            Font headFont = new Font("Arial", 10);
            int headPointX = 4;
            int headRecSizeH = 7;


            Rectangle Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(50, headRecSizeH)); //품번
            e.Graphics.DrawRectangle(pen, Rect);
            e.Graphics.DrawString("품번", headFont, Brushes.Black, Rect, sf[0]);
            headPointX += 50;

            Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(25, headRecSizeH)); //컬러
            e.Graphics.DrawRectangle(pen, Rect);
            e.Graphics.DrawString("컬러", headFont, Brushes.Black, Rect, sf[0]);
            headPointX += 25;


            Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(25, headRecSizeH)); //번들
            e.Graphics.DrawRectangle(pen, Rect);
            e.Graphics.DrawString("규격", headFont, Brushes.Black, Rect, sf[0]);
            headPointX += 25;


            Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(20, headRecSizeH)); //규격
            e.Graphics.DrawRectangle(pen, Rect);
            e.Graphics.DrawString("A", headFont, Brushes.Black, Rect, sf[0]);
            headPointX += 20;


            Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(20, headRecSizeH)); //sku수
            e.Graphics.DrawRectangle(pen, Rect);
            e.Graphics.DrawString("B", headFont, Brushes.Black, Rect, sf[0]);
            headPointX += 20;


            Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(20, headRecSizeH)); //sku수
            e.Graphics.DrawRectangle(pen, Rect);
            e.Graphics.DrawString("C", headFont, Brushes.Black, Rect, sf[0]);
            headPointX += 20;


            Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(20, headRecSizeH)); //sku수
            e.Graphics.DrawRectangle(pen, Rect);
            e.Graphics.DrawString("D", headFont, Brushes.Black, Rect, sf[0]);
            headPointX += 20;


            Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(20, headRecSizeH)); //번들수
            e.Graphics.DrawRectangle(pen, Rect);
            e.Graphics.DrawString("합계", headFont, Brushes.Black, Rect, sf[0]);

            // ---------------------------------------------------------------------------------------------------



            Font rowFont = new Font("Arial", 6);
            int rowRecY = 0;

            int[] dataRecPointX = new int[] { startWidth + 4 };

            Rectangle[] drawRect = new System.Drawing.Rectangle[8];
            rowRecY = startHeight + 17;
            for (int j = 0; j < 43; j++)
            {

                drawRect[0] = new Rectangle(new Point(dataRecPointX[0], rowRecY), new Size(50, 6));
                drawRect[1] = new Rectangle(new Point(dataRecPointX[0] + 50, rowRecY), new Size(25, 6));
                drawRect[2] = new Rectangle(new Point(dataRecPointX[0] + 75, rowRecY), new Size(25, 6));
                drawRect[3] = new Rectangle(new Point(dataRecPointX[0] + 100, rowRecY), new Size(20, 6));
                drawRect[4] = new Rectangle(new Point(dataRecPointX[0] + 120, rowRecY), new Size(20, 6));
                drawRect[5] = new Rectangle(new Point(dataRecPointX[0] + 140, rowRecY), new Size(20, 6));
                drawRect[6] = new Rectangle(new Point(dataRecPointX[0] + 160, rowRecY), new Size(20, 6));
                drawRect[7] = new Rectangle(new Point(dataRecPointX[0] + 180, rowRecY), new Size(20, 6));


                for (int k = 0; k < drawRect.Count(); k++)
                {
                    e.Graphics.DrawRectangle(pen, drawRect[k]);
                }

                e.Graphics.DrawString(tempTable.Rows[cnt]["ITEM_STYLE"].ToString(), headFont, Brushes.Black, drawRect[0], sf[0]);
                e.Graphics.DrawString(tempTable.Rows[cnt]["ITEM_COLOR"].ToString(), headFont, Brushes.Black, drawRect[1], sf[0]);
                e.Graphics.DrawString(tempTable.Rows[cnt]["ITEM_SIZE"].ToString(), headFont, Brushes.Black, drawRect[2], sf[0]);
                e.Graphics.DrawString(tempTable.Rows[cnt]["A"].ToString(), headFont, Brushes.Black, drawRect[3], sf[0]);
                e.Graphics.DrawString(tempTable.Rows[cnt]["B"].ToString(), headFont, Brushes.Black, drawRect[4], sf[0]);
                e.Graphics.DrawString(tempTable.Rows[cnt]["C"].ToString(), headFont, Brushes.Black, drawRect[5], sf[0]);
                e.Graphics.DrawString(tempTable.Rows[cnt]["D"].ToString(), headFont, Brushes.Black, drawRect[6], sf[0]);
                e.Graphics.DrawString(tempTable.Rows[cnt]["SUM"].ToString(), headFont, Brushes.Black, drawRect[7], sf[0]);




                rowRecY += 6;

                if (++cnt == tempTable.Rows.Count)
                {
                    break;
                }
                //cnt++;


                if (cnt == tempTable.Rows.Count)
                {
                    break;
                }
            }
            e.Graphics.DrawString(pageNo.ToString(), headFont, Brushes.Black, 105, 280);
            //pageNo
            if (cnt < tempTable.Rows.Count)
            {
                pageNo++;
                e.HasMorePages = true;
            }
            else
            {
                pd.PrintPage -= new PrintPageEventHandler(DASListPrint);
            }

        }

        private void bundleListPrint(object sender, PrintPageEventArgs e)
        {

            int startWidth = 0;//시작 x좌표
            int startHeight = 0;//시작 y좌표            

            pen = new Pen(Brushes.Black, (float)0.2);
            //pen.Freeze();


            e.Graphics.PageUnit = GraphicsUnit.Millimeter; //mm터로 바꾸는 것.


            StringFormat[] sf = new StringFormat[10];

            sf[0] = new StringFormat(); //세로, 가로 가운데정렬
            sf[0].Alignment = StringAlignment.Center;//가로 정렬
            sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[1] = new StringFormat();
            //sf[0].Alignment = StringAlignment.;//가로 정렬
            //sf[0].LineAlignment = StringAlignment.Center;//높이 정렬

            sf[2] = new StringFormat();
            sf[2].Alignment = StringAlignment.Center;//가로 정렬
            sf[2].LineAlignment = StringAlignment.Center;//높이 정렬



            e.Graphics.DrawString("일자 : " + tempTable.Rows[0]["biz_day"].ToString(), new Font("Arial", 10), Brushes.Black, startWidth + 5, startHeight + 3); //작업 날짜
            e.Graphics.DrawString("차수 : " + tempTable.Rows[0]["batch"].ToString(), new Font("Arial", 10), Brushes.Black, startWidth + 60, startHeight + 3); //작업 차수


            // -------------------------------------------------- 제목1 -------------------------------------------

            Font headFont = new Font("Arial", 10);
            int headPointX = 4;
            int headRecSizeH = 10;
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    headPointX = 4 + 102;
                }
                Rectangle Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(30, headRecSizeH)); //품번
                e.Graphics.DrawRectangle(pen, Rect);
                e.Graphics.DrawString("품번", headFont, Brushes.Black, Rect, sf[0]);
                headPointX += 30;

                Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(10, headRecSizeH)); //컬러
                e.Graphics.DrawRectangle(pen, Rect);
                e.Graphics.DrawString("컬러", headFont, Brushes.Black, Rect, sf[0]);
                headPointX += 10;


                Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(10, headRecSizeH)); //번들
                e.Graphics.DrawRectangle(pen, Rect);
                e.Graphics.DrawString("번들", headFont, Brushes.Black, Rect, sf[0]);
                headPointX += 10;


                Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(10, headRecSizeH)); //규격
                e.Graphics.DrawRectangle(pen, Rect);
                e.Graphics.DrawString("규격", headFont, Brushes.Black, Rect, sf[0]);
                headPointX += 10;


                Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(20, headRecSizeH)); //sku수
                e.Graphics.DrawRectangle(pen, Rect);
                e.Graphics.DrawString("sku수", headFont, Brushes.Black, Rect, sf[0]);
                headPointX += 20;


                Rect = new Rectangle(new Point(startWidth + headPointX, startHeight + 10), new Size(20, headRecSizeH)); //번들수
                e.Graphics.DrawRectangle(pen, Rect);
                e.Graphics.DrawString("번들수", headFont, Brushes.Black, Rect, sf[0]);
                headPointX += 20;
            }

            // ---------------------------------------------------------------------------------------------------



            Font rowFont = new Font("Arial", 6);
            int rowRecY = 0;

            int[] dataRecPointX = new int[] { startWidth + 4, startWidth + 106 };

            Rectangle[] drawRect = new System.Drawing.Rectangle[6];
            string[] check = new string[6];
            for (int i = 0; i < 2; i++)
            {
                rowRecY = startHeight + 20;
                for (int j = 0; j < 43; j++)
                {

                    drawRect[0] = new Rectangle(new Point(dataRecPointX[i], rowRecY), new Size(30, 6));
                    drawRect[1] = new Rectangle(new Point(dataRecPointX[i] + 30, rowRecY), new Size(10, 6));
                    drawRect[2] = new Rectangle(new Point(dataRecPointX[i] + 40, rowRecY), new Size(10, 6));
                    drawRect[3] = new Rectangle(new Point(dataRecPointX[i] + 50, rowRecY), new Size(10, 6));
                    drawRect[4] = new Rectangle(new Point(dataRecPointX[i] + 60, rowRecY), new Size(20, 6));
                    drawRect[5] = new Rectangle(new Point(dataRecPointX[i] + 80, rowRecY), new Size(20, 6));

                    if (Convert.ToInt32(tempTable.Rows[cnt]["row_no"]) == 1 && Convert.ToInt32(tempTable.Rows[cnt]["bundle_count"]) > 43 - j)
                    {
                        break; //남은행보다 번들의 sku 수량이 많을경우 다음 페이지로 넘기기위함. 
                    }

                    for (int k = 0; k < drawRect.Count(); k++)
                    {
                        e.Graphics.DrawRectangle(pen, drawRect[k]);
                    }
                    //biz_day	batch	GUBUN	ITEM_STYLE	ITEM_COLOR	ITEM_SIZE	STOCKF	ORD_QTY	STOCKR
                    if (j != 0)
                    {
                        e.Graphics.DrawString(tempTable.Rows[cnt]["ITEM_STYLE"].ToString(), headFont, Brushes.Black, drawRect[0], sf[0]);
                    }
                    else
                    {
                        e.Graphics.DrawString(tempTable.Rows[cnt]["BARCODE"].ToString().Substring(0, 8), headFont, Brushes.Black, drawRect[0], sf[0]);

                    }

                    //전에 로우와 같은 품목은 아이템을 다시 쓰지않기위함. --------------------------------------------------------
                    if (j != 0)
                    {
                        e.Graphics.DrawString(tempTable.Rows[cnt]["ITEM_COLOR"].ToString(), headFont, Brushes.Black, drawRect[1], sf[0]);
                    }
                    else
                    {
                        e.Graphics.DrawString(tempTable.Rows[cnt]["BARCODE"].ToString().Substring(8, 2), headFont, Brushes.Black, drawRect[1], sf[0]);
                        //e.Graphics.DrawString("", headFont, Brushes.Black, drawRect[1], sf[0]);
                    }


                    if (j == 0 || check[0] != tempTable.Rows[cnt]["ASSORT_CD"].ToString())
                    {
                        e.Graphics.DrawString(tempTable.Rows[cnt]["ASSORT_CD"].ToString(), headFont, Brushes.Black, drawRect[2], sf[0]);

                    }
                    else
                    {
                        e.Graphics.DrawString("", headFont, Brushes.Black, drawRect[2], sf[0]);
                        //e.Graphics.DrawString("", headFont, Brushes.Black, drawRect[2], sf[0]);

                    }


                    e.Graphics.DrawString(tempTable.Rows[cnt]["ITEM_SIZE"].ToString(), headFont, Brushes.Black, drawRect[3], sf[0]);
                    e.Graphics.DrawString(tempTable.Rows[cnt]["SKU_QTY"].ToString(), headFont, Brushes.Black, drawRect[4], sf[0]);
                    if (tempTable.Rows[cnt]["BUNDEL_QTY"].ToString().Equals("0"))
                    {
                        e.Graphics.DrawString("", headFont, Brushes.Black, drawRect[5], sf[0]);
                    }
                    else
                    {
                        e.Graphics.DrawString(tempTable.Rows[cnt]["BUNDEL_QTY"].ToString(), headFont, Brushes.Black, drawRect[5], sf[0]);
                    }

                    check[0] = tempTable.Rows[cnt]["ASSORT_CD"].ToString();
                    //check[1] = tempTable.Rows[cnt]["ITEM_STYLE"].ToString();
                    //check[2] = tempTable.Rows[cnt]["ITEM_COLOR"].ToString();
                    //check[3] = tempTable.Rows[cnt]["ITEM_SIZE"].ToString();


                    rowRecY += 6;

                    if (++cnt == tempTable.Rows.Count)
                    {
                        break;
                    }
                    //cnt++;
                }

                if (cnt == tempTable.Rows.Count)
                {
                    break;
                }
            }
            if (cnt < tempTable.Rows.Count)
            {
                e.HasMorePages = true;
            }
            else
            {
                pd.PrintPage -= new PrintPageEventHandler(bundleListPrint);

            }

        }


        private void statementPrint(object sender, PrintPageEventArgs e)
        {
            DataTable dt = new DataTable();
            if (tempTable == null && tempTable.Rows.Count < 1)
            {
                return;
            }
            //int max_value = 10; // 한페이지 최대 출력 가능 sku
            //int max = Convert.ToInt32(tempTable.Compute("max(row_no)", ""));// 스타일 수량 구하기. v\
            int max = max_value; //한페이지 최대 출력 가능 sku
            int strat_index = 0; //현재 페이지 스타트 row_index
            int max_row = Convert.ToInt32(tempTable.AsEnumerable().Max(row => row["row_no"]));



            if (max_row - cnt > max_value)
            {
                strat_index = cnt;
            }
            else
            {
                max = max_row - cnt;
            }


            dt = tempTable.Select("row_no  <= " + (max + cnt) + " and row_no > " + cnt).CopyToDataTable();

            //var test = tempTable.Select("row_no  <= " + 10 + " and row_no > " + 0);
            Boolean NewPage = false;
            int Pages = 0;
            int Pages_End = 0;

            int row_add = 0;
            SizeF sizemapRec1 = new SizeF(7.5f, 8);
            //int max = Convert.ToInt32(tempTable.Compute("max(row_no)", ""));// 스타일 수량 구하기. 
            float[] add_pointY = new float[max];
            int[] item_sum = new int[max];
            int[] sales_price_sum = new int[max];
            int[] forwarding_price_sum = new int[max];
            int[] sales_price = new int[max];
            int[] forwarding_price = new int[max];
            //for (int nRow = Pages; nRow < Pages_End + 1; nRow++)
            //{
            //    if (int i = 0; i < max; i++)
            //    {
            int startWidth = -7;//시작 x좌표
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
            e.Graphics.DrawString(tempTable.Rows[0]["CHUTE_NO"].ToString(), new Font("Arial", 30, FontStyle.Bold), Brushes.Black, 188, 15, sf[0]);
            e.Graphics.DrawString("전표일자 : " + tempTable.Rows[0]["biz_day"].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 10, 25);
            e.Graphics.DrawString("송장번호 : " + tempTable.Rows[0]["box_inv"].ToString(), new Font("Arial", 10, FontStyle.Regular), Brushes.Black, 60, 25);

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
            e.Graphics.DrawString(tempTable.Rows[0]["VRKME"].ToString(), font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //상호.
            rectangle = new Rectangle(new Point(startWidth + 114, startHeight + 38), recSize[1]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("상  호", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //상호명.
            rectangle = new Rectangle(new Point(startWidth + 140, startHeight + 38), recSize[4]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString(tempTable.Rows[0]["SHOP_NM"].ToString(), font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //성명.
            rectangle = new Rectangle(new Point(startWidth + 168, startHeight + 38), recSize[5]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("성 명", font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //성명value.
            rectangle = new Rectangle(new Point(startWidth + 183, startHeight + 38), recSize[6]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString(tempTable.Rows[0]["REPRESENTATIVE"].ToString(), font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //주소.
            rectangle = new Rectangle(new Point(startWidth + 114, startHeight + 46), recSize[2]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("주  소", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            //주소value.
            rectangle = new Rectangle(new Point(startWidth + 140, startHeight + 46), recSize[7]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString(tempTable.Rows[0]["ADDR"].ToString(), font[0], Brushes.Black, rectangle, sf[1]); //tns_code

            //-----------------------------------------------------------------------------------------------------------


            //사이즈----------------------------------------------------------------------------------------------
            List<string[]> size_list = new List<string[]>();
            string[] sizemap1 = new string[] { "UP01", "075", "080", "085", "090", "095", "100", "105", "110", "115", "120", "125", "130", "140", "150", "00F" };
            string[] sizemap2 = new string[] { "SH01", "220", "225", "230", "235", "240", "245", "250", "255", "260", "265", "270", "275", "280", "285", "290" };
            string[] sizemap3 = new string[] { "BT01", "061", "064", "067", "070", "073", "076", "079", "082", "085", "088", "090", "00S", "00M", "00L", "00F" };
            string[] sizemap4 = new string[] { "BT02", "074", "076", "078", "080", "082", "084", "086", "088", "090", "092", "094", "096", "098", "100", "102" };
            string[] sizemap5 = new string[] { "AC01", "000", "018", "019", "020", "021", "022", "023", "024", "025", "00S", "00M", "00L", "00F", "ZZZ", "" };
            string[] sizemap6 = new string[] { "UP04", "075", "080", "085", "090", "095", "100", "105", "110", "0MF", "00F", "", "", "", "", "" };
            string[] sizemap7 = new string[] { "UP10", "", "00S", "00M", "00L", "0XL", "XXL", "00F", "", "", "", "", "", "", "", "" };
            string[] sizemap8 = new string[] { "UP09", "", "00M", "00L", "0XL", "XXL", "", "", "", "", "", "", "", "", "", "" };
            size_list.Add(sizemap1);
            size_list.Add(sizemap2);
            size_list.Add(sizemap3);
            size_list.Add(sizemap4);
            size_list.Add(sizemap5);
            size_list.Add(sizemap6);
            size_list.Add(sizemap7);
            size_list.Add(sizemap8);
            string[,] sizemap = new string[8, 16]
            {
                {"UP01","075","080","085","090","095","100","105","110","115","120","125","130","140","150","00F"}
                ,{"SH01","220","225","230","235","240","245","250","255","260","265","270","275","280","285","290"}
                ,{"BT01","061","064","067","070","073","076","079","082","085","088","090","00S","00M","00L","00F"}
                ,{"BT02","074","076","078","080","082","084","086","088","090","092","094","096","098","100","102"}
                ,{"AC01","018","019","020","021","022","023","024","025","00S","00M","00L","0XL","XXL","00F","ZZZ"}
                ,{"UP04","075","080","085","090","095","100","105","110","0MF","00F","","","","",""}
                ,{"UP10",""   ,"00S","00M","00L","0XL","XXL","00F",""   ,""   ,""   ,"","","","",""}
                ,{ "UP09", "", "00M", "00L", "0XL", "XXL", "", "", "", "", "", "", "", "", "", ""}
        };

            recSize[8] = new Size(8, 40);
            rectangle = new Rectangle(new Point(startWidth + 9, startHeight + 57), recSize[8]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("Box\nNo", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            recSize[9] = new Size(21, 40);
            rectangle = new Rectangle(new Point(startWidth + 17, startHeight + 57), recSize[9]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("품  번", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            rectangle = new Rectangle(new Point(startWidth + 38, startHeight + 57), recSize[8]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("색\n\n상", font[0], Brushes.Black, rectangle, sf[0]); //tns_code


            rectangle = new Rectangle(new Point(startWidth + 166, startHeight + 57), recSize[8]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("계", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            recSize[9] = new Size(14, 40);
            rectangle = new Rectangle(new Point(startWidth + 174, startHeight + 57), recSize[9]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("공급\n단가\n\n/\n\n판매\n단가", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            recSize[10] = new Size(15, 40);
            rectangle = new Rectangle(new Point(startWidth + 188, startHeight + 57), recSize[10]);
            e.Graphics.DrawRectangle(pen, rectangle);
            e.Graphics.DrawString("공급\n금액\n\n/\n\n판매\n금액", font[0], Brushes.Black, rectangle, sf[0]); //tns_code

            float PointX = startWidth + 46;
            float PointY = startHeight + 57;

            RectangleF sizeRec = new RectangleF();
            SizeF sizemapRec = new SizeF(7.5f, 5);
            //사이즈 맵찍기.
            for (int i = 0; i < size_list.Count; i++)
            {
                for (int j = 0; j < size_list[0].Length; j++)
                {
                    //rectangle = new Rectangle(new Point(PointX, PointY), new SizeF(7.5,5));
                    sizeRec = new RectangleF(new PointF(PointX, PointY), sizemapRec);
                    e.Graphics.DrawRectangle(pen, PointX, PointY, 7.5f, 5);
                    //e.Graphics.DrawString(sizemap[i, j], new Font("Arial", 7), Brushes.Black, sizeRec, sf[0]); //tns_code
                    e.Graphics.DrawString(size_list[i][j], new Font("Arial", 7), Brushes.Black, sizeRec, sf[0]); //tns_code
                    PointX += 7.5f;
                }
                PointY += 5;
                PointX = startWidth + 46;
            }

            //box_no, 품번,색상,인덱스 그룹 찍기
            for (int i = 0; i < max; i++)
            {
                if (i == 0)
                {
                    add_pointY[0] = startHeight + 97; // 데이터의 처음 시작 y축값
                }
                else
                {
                    add_pointY[i] = add_pointY[i - 1] + 8;
                }
                DataRow[] temp_row = dt.Select("row_no = " + (i + 1 + cnt));
                Rectangle box_no = new Rectangle(new Point(startWidth + 9, Convert.ToInt32(add_pointY[i])), new Size(8, 8)); //박스번호
                e.Graphics.DrawRectangle(pen, box_no);
                e.Graphics.DrawString(temp_row[0]["box_no"].ToString(), font[0], Brushes.Black, box_no, sf[2]);


                RectangleF index_group = new RectangleF(new Point(startWidth + 46, Convert.ToInt32(add_pointY[i])), new Size(8, 8)); //인덱스 그룹
                e.Graphics.DrawString(temp_row[0]["index_group"].ToString(), new Font("Arial", 7), Brushes.Black, index_group, sf[2]);

                Rectangle style = new Rectangle(new Point(startWidth + 17, Convert.ToInt32(add_pointY[i])), new Size(21, 8)); //스타일
                e.Graphics.DrawRectangle(pen, style);
                e.Graphics.DrawString(temp_row[0]["style"].ToString(), font[0], Brushes.Black, style, sf[2]);

                Rectangle color = new Rectangle(new Point(startWidth + 38, Convert.ToInt32(add_pointY[i])), new Size(8, 8)); //컬러
                e.Graphics.DrawRectangle(pen, color);
                e.Graphics.DrawString(temp_row[0]["item_color"].ToString(), font[0], Brushes.Black, color, sf[2]);

                sales_price[i] = Convert.ToInt32(temp_row[0]["SALES_AMOUNT"]);
                forwarding_price[i] = Convert.ToInt32(temp_row[0]["FORWARDING_AMOUNT"]);

            }
            //for (int i = 0 + strat_index; i < tempTable.Rows.Count + strat_index; i++)

            //foreach(DataRow dr in tempTable.Rows)
            //{
            //    if(Convert.ToInt32(tempTable.Rows[i]["row_no"]) > cnt)
            //}

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int indexpoint = Convert.ToInt32(dt.Rows[i]["row_no"]) - 1 - cnt;

                int array1_index = Convert.ToInt32(dt.Rows[i]["index_no"]);
                int size_index = Array.IndexOf(size_list[array1_index], dt.Rows[i]["item_size"].ToString());
                float size_point = (startWidth + 46) + (size_index * 7.5f);
                for (int j = 0; j < size_list[0].Length; j++)
                {
                    //rectangle = new Rectangle(new Point(PointX, PointY), new SizeF(7.5,5));
                    sizeRec = new RectangleF(new PointF(PointX, add_pointY[indexpoint]), sizemapRec1);
                    e.Graphics.DrawRectangle(pen, PointX, add_pointY[indexpoint], 7.5f, 8);
                    e.Graphics.DrawString(dt.Rows[i]["wrk_qty"].ToString(), new Font("Arial", 7), Brushes.Black, size_point + 3.25f, add_pointY[indexpoint] + 4, sf[0]); //tns_code
                    PointX += 7.5f;
                }
                PointX = startWidth + 46;

                int wrk_qty = Convert.ToInt32(dt.Rows[i]["wrk_qty"]);
                item_sum[indexpoint] += wrk_qty; //라인별 아이템 수량 합산
                forwarding_price_sum[indexpoint] += Convert.ToInt32(dt.Rows[i]["FORWARDING_AMOUNT"]) * wrk_qty;//공급단가 합산
                sales_price_sum[indexpoint] += Convert.ToInt32(dt.Rows[i]["SALES_AMOUNT"]) * wrk_qty; //판매단가 합산

            }

            //for (int i = 0 + strat_index; i < max + strat_index; i++)
            //{
            //    int indexpoint = Convert.ToInt32(tempTable.Rows[i]["row_no"]) - 1;

            //    int array1_index = Convert.ToInt32(tempTable.Rows[i]["index_no"]);
            //    int size_index = Array.IndexOf(size_list[array1_index], tempTable.Rows[i]["item_size"].ToString());
            //    float size_point = (startWidth + 46) + (size_index * 7.5f);
            //    for (int j = 0; j < size_list[0].Length; j++)
            //    {
            //        //rectangle = new Rectangle(new Point(PointX, PointY), new SizeF(7.5,5));
            //        sizeRec = new RectangleF(new PointF(PointX, add_pointY[indexpoint]), sizemapRec1);
            //        e.Graphics.DrawRectangle(pen, PointX, add_pointY[indexpoint], 7.5f, 8);
            //        e.Graphics.DrawString(tempTable.Rows[i]["wrk_qty"].ToString(), new Font("Arial", 7), Brushes.Black, size_point + 3.25f, add_pointY[indexpoint] + 4, sf[0]); //tns_code
            //        PointX += 7.5f;
            //    }
            //    PointX = startWidth + 46;

            //    int wrk_qty = Convert.ToInt32(tempTable.Rows[i]["wrk_qty"]);
            //    item_sum[indexpoint] += wrk_qty; //라인별 아이템 수량 합산
            //    forwarding_price_sum[indexpoint] += Convert.ToInt32(tempTable.Rows[i]["FORWARDING_AMOUNT"]) * wrk_qty;//공급단가 합산
            //    sales_price_sum[indexpoint] += Convert.ToInt32(tempTable.Rows[i]["SALES_AMOUNT"]) * wrk_qty; //판매단가 합산

            //}

            int item_sum_value = 0;
            int forwarding_price_value = 0;
            int forwarding_price_sum_value = 0;

            for (int i = 0; i < max; i++)
            {
                Rectangle sum_qty = new Rectangle(new Point(startWidth + 166, Convert.ToInt32(add_pointY[i])), new Size(8, 8)); //박스번호
                e.Graphics.DrawRectangle(pen, sum_qty);
                e.Graphics.DrawString(item_sum[i].ToString(), font[0], Brushes.Black, sum_qty, sf[2]);

                Rectangle price = new Rectangle(new Point(startWidth + 174, Convert.ToInt32(add_pointY[i])), new Size(14, 8)); //박스번호
                e.Graphics.DrawRectangle(pen, price);
                e.Graphics.DrawString(forwarding_price[i].ToString("#,##0"), font[0], Brushes.Black, price, sf[2]);

                Rectangle price_sum = new Rectangle(new Point(startWidth + 188, Convert.ToInt32(add_pointY[i])), new Size(15, 8)); //박스번호
                e.Graphics.DrawRectangle(pen, price_sum);
                e.Graphics.DrawString(forwarding_price_sum[i].ToString("#,##0"), font[0], Brushes.Black, price_sum, sf[2]);

                item_sum_value += item_sum[i];
                forwarding_price_value += forwarding_price[i];
                forwarding_price_sum_value += forwarding_price_sum[i];
            }
            RectangleF summary = new RectangleF(new Point(startWidth + 9, Convert.ToInt32(add_pointY[max - 1]) + 8), new SizeF(37f, 8)); //박스번호
            e.Graphics.DrawRectangle(pen, startWidth + 9, Convert.ToInt32(add_pointY[max - 1]) + 8, 37f, 8);
            e.Graphics.DrawString("소   계", font[0], Brushes.Black, summary, sf[2]);

            RectangleF recNull = new RectangleF(new PointF(startWidth + 46, Convert.ToInt32(add_pointY[max - 1]) + 8), new SizeF(120f, 8)); //박스번호
            e.Graphics.DrawRectangle(pen, startWidth + 46, Convert.ToInt32(add_pointY[max - 1]) + 8, 120f, 8);
            //e.Graphics.DrawString("", font[0], Brushes.Black, summary, sf[2]);

            Rectangle item_summary = new Rectangle(new Point(startWidth + 166, Convert.ToInt32(add_pointY[max - 1]) + 8), new Size(8, 8)); //박스번호
            e.Graphics.DrawRectangle(pen, item_summary);
            e.Graphics.DrawString(item_sum_value.ToString(), font[0], Brushes.Black, item_summary, sf[2]);

            Rectangle price_summary = new Rectangle(new Point(startWidth + 174, Convert.ToInt32(add_pointY[max - 1]) + 8), new Size(29, 8)); //박스번호
            e.Graphics.DrawRectangle(pen, price_summary);
            e.Graphics.DrawString(forwarding_price_sum_value.ToString("#,##0"), font[0], Brushes.Black, price_summary, sf[2]);

            cnt = cnt + max_value;

            e.Graphics.DrawString(pageNo + "/" + maxPage, new Font("Arial", 10), Brushes.Black, 105, 292, sf[2]);

            if (cnt < max_row && max_row > max_value)
            {
                pageNo++;
                e.HasMorePages = true;
            }
            else
            {
                pd.PrintPage -= new PrintPageEventHandler(statementPrint);
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
