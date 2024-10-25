using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Collections;

namespace Sorer_Indicator_Contorl
{
    class JbController
    {
        //lock을 위한 변수
        static private object lock_forwarding = new object();
        static private object lock_seq = new object();
        static private object lock_sendMsg = new object();
        static private object lock_LD = new object();
        static private object lock_display16 = new object();

        //표시기 작업 수량을 Action 폼으로 넘기기위한 델리게이트
        public delegate void sendirec(string msg);
        public event sendirec send_msg;




        string fillPath = @"C:\Users\bowoo_hyeong\Documents\Visual Studio 2013\Projects\BOWOO_DPS_2F\BOWOO_DPS_2F\bin\Debug";
        string jb_no = "0";
        LogWrite log_history = new LogWrite();
        NetworkStream net_stream = null;
        Thread th1 = null;
        string outstring = null;
        string barcodeData = string.Empty;

        TcpClient jb_client = null;
        StreamWriter sw = null;
        public int seq = 0;
        private string ip;
        private int port;

        bool thread_status = false;
        string[] teststirng = new string[500];

        string[] bcr_addr = null;

        //생성자 등록된 JB ip 셋팅
        public JbController(string ip, int jb_no)
        {
            string[] con_ip = ip.Split(new char[] { ':' });
            this.ip = con_ip[0];
            int.TryParse(con_ip[1], out this.port);
            string fmt = "00";
            this.jb_no = jb_no.ToString(fmt);
        }

        //작업 시작
        public bool jb_open()
        {
            try
            {
                jb_client = new TcpClient(ip, port);
                sw = new StreamWriter(jb_client.GetStream(), Encoding.GetEncoding("ks_c_5601-1987"));
                net_stream = jb_client.GetStream();
                th1 = new Thread(new ThreadStart(read_data));
                th1.Start();
                thread_status = true;
                seq = 0;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //작업 종료
        public void jb_close()
        {
            Thread.Sleep(10);
            thread_status = false;
            //th1.Join();
            //th1.Suspend();
            th1.Abort();
            if (sw != null)
            {
                sw.Close();
            }
            jb_client.Close();
        }

        //16행표시기 데이터 send
        public void Product_DisPlay16(string cell_id, string value)
        {
            lock(lock_display16)
            {
                string msg = null;
                string[] lcd_msg = new string[6];
                int length = value.Length;

                //자리수가 16자리가 안될경우 나머지 자리를 공백으로 채움
                if (value.Length < 16)
                {
                    for (int i = 0; i < 16 - length; i++)
                    {
                        value = value.Insert(value.Length, " ");
                    }
                }

                msg = seq_count(seq) + "044";
                for (int i = 0; i < 4; i++)
                {
                    if (i != 3)
                    {
                        msg += cell_id + "LF" + (i + 1).ToString() + value.Substring(i * 5, 5);
                    }
                    else
                    {
                        msg += cell_id + "LF" + (i + 1).ToString() + value.Substring(i * 5, 1);
                    }
                }
                sendMeg(msg);
            }
        }

        public void product_LD(string addr)
        {
            lock (lock_LD)
            {
                string msg = null;
                msg = seq_count(seq) + "006";
                msg += addr + "LD";
                sendMeg(msg);
            }
        }
        //표시기에 작업 데이터 뿌리기
        public void Product_forwarding
                            (string cell_Id, string display, string buzzer, string buzzer_cycle
                            , string lc_color, string lc_cycle, string lc_status
                            , string fn_color, string fn_cycle, string fn_status)
        {
            lock (lock_forwarding)
            {
                //product_LD(cell_Id);

                string msg = null;
                //string lcd_id = requirements.Sub_Addr.Substring(1, 4);
                //string[] lcd_msg = new string[6];
                //string write_qty = requirements.work_qty.ToString().PadLeft(5, '0');


                string lc_color_no = color_no(lc_color);
                string fn_color_no = color_no(fn_color);
                //buzzer = "0";
                //buzzer_cycle = "0";
                //2'3행 불뛰우기
                msg = seq_count(seq) + "036";
                msg += cell_Id + "LF" + display;
                msg += cell_Id + "LB" + buzzer + buzzer_cycle + "0000";
                msg += cell_Id + "LC" + lc_color_no + lc_cycle + lc_status + fn_color_no + fn_cycle + fn_status;
                sendMeg(msg);

                log_history.LogWriter(jb_no.ToString() + cell_Id + "표시기 수량(" + display + ") ON", Application.StartupPath, @"\Logs", @"\JB_PC통신로그");//로그 남기기위함.

            }
        }

        //표시기에 작업 데이터 뿌리기
        public void Product_forwarding(IndicatorOrderClass indicatorOrderClass)
        {
            lock (lock_forwarding)
            {
                string msg = null;

                string lc_color_no = color_no(indicatorOrderClass.lc_color);
                string fn_color_no = color_no(indicatorOrderClass.fn_color);

                //2'3행 불뛰우기
                msg = seq_count(seq) + "036";
                msg += indicatorOrderClass.cell_Id + "LF" + indicatorOrderClass.display;
                msg += indicatorOrderClass.cell_Id + "LB" + indicatorOrderClass.buzzer + indicatorOrderClass.buzzer_cycle + "0000";
                msg += indicatorOrderClass.cell_Id + "LC" + lc_color_no + indicatorOrderClass.lc_cycle + indicatorOrderClass.lc_status + fn_color_no + indicatorOrderClass.fn_cycle + indicatorOrderClass.fn_status;
                sendMeg(msg);

                log_history.LogWriter(jb_no.ToString() + indicatorOrderClass.cell_Id + "표시기 수량(" + indicatorOrderClass.display + ") ON", Application.StartupPath, @"\Logs", @"\JB_PC통신로그");//로그 남기기위함.
            }
        }

        public void bcr_open(string cell_Id)
        {
            string msg = null;
            msg = seq_count(seq) + "006";
            msg += cell_Id + "BO";
            sendMeg(msg);
        }


        public void bcr_close(string cell_Id)
        {
            string msg = null;
            msg = seq_count(seq) + "006";
            msg += cell_Id + "BC";
            sendMeg(msg);
        }

        private string color_no(string msg)
        {
            string rec_string = string.Empty;
            switch (msg)
            {
                case "RED":
                    rec_string = "1";
                    break;
                case "GREEN":
                    rec_string = "2";
                    break;
                case "YELLOW":
                    rec_string = "3";
                    break;
                default:
                    rec_string = "0";
                    break;
            }
            return rec_string;
        }

        public void lcd_off(string lcd_id)
        {
            string msg = seq_count(seq) + "006" + lcd_id + "LD";
            sendMeg(msg);
        }

        public void cancle(string[] loc_nos)
        {
            int count_no = 0;
            string msg = null;

            foreach (string r in loc_nos)
            {
                msg += seq_count(seq) + "006" + r + "LD";
                //한글 표시기도 같이 끄기위함. (한글표시기 없을경우 이부분 주석 처리해야 함.)
                msg += seq_count(seq) + "006" + (Convert.ToInt32(r) + 1).ToString() + "LD";
                if (msg.Length >= 208)
                {
                    sendMeg(msg);
                    msg = null;
                }
                else if (loc_nos.Length == count_no + 1)
                {
                    sendMeg(msg);
                }
                count_no++;
            }
        }

        //jb로 커맨드 전송
        public void sendMeg(string msg)
        {
            lock (lock_sendMsg)
            {
                Thread.Sleep(2);
                byte[] buffer = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(msg);
                net_stream.Write(buffer, 0, buffer.Length);
                net_stream.Flush();
                //sw.WriteLine(msg);
                //sw.Flush();
                //Console.WriteLine("jb로 명령 보냄 : " + msg + " [" + System.DateTime.Now + "]");
                log_history.LogWriter("표시기로 전송한 문자열 : [" +jb_no + "] " + msg, Application.StartupPath, @"\Logs", @"\JB_PC통신로그");//로그 남기기위함.
            }
        }

        //JB에서 보낸 스트림값 읽기
        public void read_data()
        {
            while (thread_status)
            {
                try
                {
                    byte[] outbuf = new byte[20];
                    Console.WriteLine("jb 통신 대기 상태로 들어감.");
                    int nbytes = net_stream.Read(outbuf, 0, outbuf.Length);
                    outstring = Encoding.ASCII.GetString(outbuf, 0, nbytes);
                    // log1.LogWriter(outstring, fillPath);
                    Console.WriteLine("컨트롤러에서 받은값 JB 번호 : " + jb_no + "  ( " + outstring + " ) ");
                    log_history.LogWriter("컨트롤러에서 받은값(" + jb_no.ToString() + outstring + ") ", Application.StartupPath, @"\Logs", @"\JB_PC통신로그");//로그 남기기위함.
                    /*
                     * RC : 표시기 CONFRM 버튼
                     * RF : FN 누룬후 표시기 CONFRM 버튼
                     * ME : 명령어에 대한 정상 상태값 반환
                     * RB : 스캐너 값 3번 나누어서 들어옴
                     */
                    string addr = outstring.Substring(0, 4);
                    if (outstring.Contains("RC"))
                    {
                        product_LD(addr);
                        string send_string = outstring.Insert(outstring.IndexOf("RC") - 4, jb_no);
                        send_msg(send_string);
                    }
                    else if (outstring.Contains("RF"))
                    {
                        product_LD(addr);
                        string send_string = outstring.Insert(outstring.IndexOf("RF") - 4, jb_no);
                        send_msg(send_string);
                    }
                    else if (outstring.Contains("RB"))
                    {
                        scanDataRead(outstring);
                        Console.WriteLine(outstring);
                        //string send_string = outstring.Insert(outstring.IndexOf("RB") - 4, jb_no);
                        //send_msg(send_string);
                    }
                }
                catch (Exception eq)
                {
                    barcodeData = string.Empty;
                    Console.WriteLine("왜에러가 났을까:" + eq + " !!!!!@@@@@ jb번호 : " + this.jb_no);
                    //MessageBox.Show(eq.Message);
                }
            }
        }

        //바코드 인터페이스에서 받은 데이터
        private void scanDataRead(string reciveData)
        {
            int dataStartIndex = reciveData.IndexOf("RB") + 2;
            int dataLength = Convert.ToInt32(reciveData.Substring(dataStartIndex, 1));
            string data = reciveData.Substring(dataStartIndex + 1, dataLength);
            //data = lib.Common.StringEx.ReplaceSymbol(data,"");
            barcodeData += data;
            if (barcodeData.Contains("\n"))
            {
                Console.WriteLine(barcodeData);
                //int test11 = barcodeData.Length;
                //int test112 = barcodeData.IndexOf('\n');
                //string t1 = barcodeData.Substring(0, barcodeData.IndexOf('\n'));
                //string t2 = barcodeData.Insert(0, reciveData.Substring(0, 4)); //표시기 주소 인서트
                //string t3 = barcodeData.Insert(0, jb_no); //jb 번호 인서트
                //barcodeData = barcodeData.Substring(0, barcodeData.IndexOf('\n') - 1);
                barcodeData = lib.Common.StringEx.ReplaceSymbol(barcodeData, "");
                barcodeData = barcodeData.Insert(0, "RB"); //jb 번호 인서트
                barcodeData = barcodeData.Insert(0, reciveData.Substring(0, 4)); //표시기 주소 인서트
                barcodeData = barcodeData.Insert(0, jb_no); //jb 번호 인서트
                send_msg(barcodeData);
                barcodeData = string.Empty;
                //scanDataRead(barcodeData);
            }

        }

        //시퀀스 카운트
        public string seq_count(int seq)
        {
            lock (lock_seq)
            {
                string fmt = "0000";
                string formatSeq = seq.ToString(fmt);
                if (seq == 9999)
                {
                    this.seq = 0;
                }
                else
                {
                    this.seq++;
                }
                return formatSeq;
            }
        }

        // 한글표시기에 글자 잘라서 보내기위한 메소드
        private string[] msg_trim1(string msg)
        {
            byte[] temp_buffer = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(msg);
            byte[] temp_test = Encoding.UTF8.GetBytes(msg);
            // byte[] trim_buffer = new byte[temp_buffer.Length];
            byte[] trim_buffer = new byte[30];
            byte[] test_buffer = new byte[6];
            string[] temp_string = new string[6];
            int copy_count = 0;
            int kr_count = 0;
            int en_count = 0;
            int up_count = 0;

            if (temp_buffer.Length > 30)
            {
                Array.Copy(temp_buffer, 0, trim_buffer, 0, 30);
                for (int j = 0; j < 6; j++)
                {
                    if (j == 2 || j == 5)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (trim_buffer[i + copy_count] > 127) kr_count += 1;
                            if (trim_buffer[i + copy_count] <= 127) en_count += 1;

                            if (i == 2)
                            {
                                int kr_count_mod = kr_count % 2;
                                if (kr_count_mod == 0) up_count = 3;
                                if (kr_count_mod != 0) up_count = 2;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (trim_buffer[i + copy_count] > 127) kr_count += 1;
                            if (trim_buffer[i + copy_count] <= 127) en_count += 1;

                            if (i == 5)
                            {
                                int kr_count_mod = kr_count % 2;
                                if (kr_count_mod == 0) up_count = 6;
                                if (kr_count_mod != 0) up_count = 5;
                            }
                        }
                    }
                    // test_buffer = new byte[up_count];
                    for (int i = 0; i < 6; i++)
                    {
                        /*if (test_buffer[i] == null)*/
                        test_buffer[i] = 32;
                    }
                    Array.Copy(trim_buffer, copy_count, test_buffer, 0, up_count);
                    copy_count += up_count;

                    temp_string[j] = Encoding.GetEncoding("ks_c_5601-1987").GetString(test_buffer);
                    kr_count = 0;
                    en_count = 0;
                }
            }
            else
            {
                Array.Copy(temp_buffer, 0, trim_buffer, 0, temp_buffer.Length);
                int lenght = temp_buffer.Length / 6;
                int lenght_mod = temp_buffer.Length % 6;
                if (lenght == 0 || lenght_mod >= 1) lenght += 1;
                if (lenght == 3) lenght += 1;

                for (int j = 0; j < lenght; j++)
                {
                    if (j == 2 || j == 5)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (trim_buffer[i + copy_count] > 127) kr_count += 1;
                            if (trim_buffer[i + copy_count] <= 127) en_count += 1;

                            if (i == 2)
                            {
                                int kr_count_mod = kr_count % 2;
                                if (kr_count_mod == 0) up_count = 3;
                                if (kr_count_mod != 0) up_count = 2;
                            }
                        }
                    }
                    else if (lenght_mod == 0 || j < lenght - 1)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (trim_buffer[i + copy_count] > 127) kr_count += 1;
                            if (trim_buffer[i + copy_count] <= 127) en_count += 1;

                            if (i == 5)
                            {
                                int kr_count_mod = kr_count % 2;
                                if (kr_count_mod == 0) up_count = 6;
                                if (kr_count_mod != 0) up_count = 5;
                            }
                        }
                    }
                    else if (lenght - 1 == j && temp_buffer.Length - copy_count > 6)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (trim_buffer[i + copy_count] > 127) kr_count += 1;
                            if (trim_buffer[i + copy_count] <= 127) en_count += 1;

                            if (i == 5)
                            {
                                int kr_count_mod = kr_count % 2;
                                if (kr_count_mod == 0) up_count = 6;
                                if (kr_count_mod != 0) up_count = 5;
                                lenght++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < temp_buffer.Length - copy_count; i++)
                        {
                            if (trim_buffer[i + copy_count] > 127) kr_count += 1;
                            if (trim_buffer[i + copy_count] <= 127) en_count += 1;

                            if (i == temp_buffer.Length - copy_count - 1)
                            {
                                up_count = kr_count + en_count;
                                //int kr_count_mod = kr_count % 2;
                                //if (kr_count_mod == 0) up_count = 3;
                                //if (kr_count_mod != 0) up_count = 2;
                            }
                        }

                    }
                    test_buffer = new byte[6];
                    for (int k = 0; k < 6; k++)
                    {
                        test_buffer[k] = 32;
                    }
                    Array.Copy(trim_buffer, copy_count, test_buffer, 0, up_count);
                    copy_count += up_count;

                    temp_string[j] = Encoding.GetEncoding("ks_c_5601-1987").GetString(test_buffer);
                    kr_count = 0;
                    en_count = 0;
                }

            }
            return temp_string;
        }


        // 전체 표시기 종료
        public void all_off()
        {
            string can1 = "0061000MZ";
            string can2 = "0062000MZ";
            string can3 = "0063000MZ";
            string msg;

            msg = seq_count(seq) + can1;
            msg += seq_count(seq) + can2;
            msg += seq_count(seq) + can3;

            sendMeg(msg);
        }

    }
}
