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
    static class Printer_Class
    {
        //동기화를 위한 락 변수
        static private object lock_object = new object();

        static PrintDocument pd = new PrintDocument();
        static PrinterSettings pr_setting = new PrinterSettings();
        static PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        static List<string> print_list = new List<string>();
        static List<string> itemCd_list = new List<string>();
        //기본설정된 프른트 찾기
        static string defaultPrinter;
        static DataRow dtarow = null;
        static DataTable tempTable = null;
        //인쇄시 종이에 마진주기.
        static Margins margins = new Margins(20, 20, 20, 20);
        //code 39 바코드
        static Font barcode_font = new Font("3 of 9 Barcode", 80);
        static Font FtBT1 = null;


        static int startWidth = 10;//시작 x좌표
        static int startHeight = 20;//시작 y좌표

        static int pageNo = 1;//페이지 넘버
        static int maxPage = 1;
        static int cnt = 1;//row_Nomber
        static int datalenght = 0;
        static string work_seq = string.Empty; //이전 시퀀스번호 저장. 시퀀스번호가 다를경우 페이지 넘기기위함.
        static int printSeq = 1;
        static int laserMaxRow = 27; //레이저 프린트 한페이지 최대 로우 수량.
        static int sumqty = 0; //시퀀스별 오더 수량 합계/


        static Printer_Class()
        {
            //pd.PrintPage += new PrintPageEventHandler(test312);

        }


        static public void print(DataTable printTable, string print_name)
        {
            lock (lock_object)
            {

                cnt = 0;
                printSeq = 1;
                datalenght = printTable.Rows.Count;
                pd.PrinterSettings.PrinterName = print_name;
                //if (print_name.Contains("LASER")) //레이저 프린트 처리.
               
                //인쇄를 할 프린트 설정.
                if (print_name.Contains("DocuCentre-IV C2263")) //레이저 프린트 처리.
                {
                    pd.PrintPage += new PrintPageEventHandler(laserPrintPage);
                    //test();
                }
                else if (print_name.Contains("LABEL")) //라벨 프린터 처리
                {
                    pd.PrintPage += new PrintPageEventHandler(labelPrintPage);
                }
                tempTable = printTable;

                work_seq = string.Empty;
                //미리보기 테스트
                test();
                //인쇄시작.
                //pd.Print();
            }
        }

        static public void print()
        {
            lock (lock_object)
            {

                pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                work_seq = string.Empty;
                //미리보기 테스트
                test();
                //인쇄시작.
                //pd.Print();
            }
        }

        static public void test312(object sender, PrintPageEventArgs e)
        {
            e.HasMorePages = true;
        }



        static private void labelPrintPage(object sender, PrintPageEventArgs e)
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


        static private void laserPrintPage(object sender, PrintPageEventArgs e)
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

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("dataGridViewTextBoxColumn5", typeof(string));

        //    for (int i = 0; i < 10; i++)
        //    {
        //        dt.Rows.Add("ssk-1234" + i);
        //    }
        //    //print(defaultPrinter, dt);
        //}

        static private void test()
        {
            printPreviewDialog1.Document = pd;
            printPreviewDialog1.ShowDialog();
        }



    }
}
