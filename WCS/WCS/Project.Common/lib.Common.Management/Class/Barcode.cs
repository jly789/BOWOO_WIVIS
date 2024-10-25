using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Common.Management
{
    public class Barcode
    {
        private static Barcode _Instance;

        static public Barcode getInstance()
        {
            if (_Instance == null)
                _Instance = new Barcode();
            return _Instance;
        }

        public Dictionary<int, char> code128_array = new Dictionary<int, char>();

        #region "CODE128"
        /// <summary>
        /// 바코드산출값
        /// </summary>
        /// <param name="pstrBARCODE">값</param>
        /// <param name="pstrCOD">A / B / C</param>
        /// <param name="pstrERR">에러내용</param>
        /// <returns>바코드산출값(ttf에 적용할것)</returns>
        public string CODE128(string pstrBARCODE, string pstrCOD, ref string pstrERR)
        {
            string strrb = "";
            try
            {
                if (code128_array.Count == 0)
                {
                    code128ArrayInsert(); // 코드 딕셔너리에 등록
                }
                string strSUM = "";

                int intSTA = 204;
                //START Code A : 203  208  248   : 103
                //START Code B : 204 [209] 249   : 104
                //START Code C : 205  210  250   : 105
                int intEND = 211; // STOP Pattern: 206 [211] 251

                int intSUM = 0;
                int intMMM = 0;
                int intpBARCODE = 0;
                int intSOD = 0;
                int intMOD = 0;

                #region "A,B,C 바코드분류"

                // START / END FONTCODE
                switch (pstrCOD)
                {
                    case "B":
                        intSTA = 204; //209
                        intEND = 206; //211
                        break;
                    case "C":
                        intSTA = 205;
                        intEND = 206;
                        break;
                    default:
                        intSTA = 203;
                        intEND = 206;
                        break;
                }
                #endregion //A,B,C 바코드분류



                // CHECK시작값
                #region "MOD VALUES"
                intMOD = 103;
                switch (intSTA)
                {
                    case 203:
                    case 208:
                    case 248:
                        intSOD = 103;
                        break;

                    case 204:
                    case 209:
                    case 249:
                        intSOD = 104;
                        break;

                    case 205:
                    case 210:
                    case 250:
                        intSOD = 105;
                        break;

                }
                #endregion // MOD VALUES



                #region "바코드데이터 유효값 확인"
                intpBARCODE = pstrBARCODE.Length;
                if (intpBARCODE < 1) { pstrERR = "데이터영역이 없습니다."; return strrb; }


                for (int i = 0; i < intpBARCODE; i++)
                {
                    //Console.WriteLine(pstrBARCODE[i].ToString());

                    if ((pstrBARCODE[i] < 32) || (pstrBARCODE[i] > 126))
                    {
                        pstrERR = "데이터로 쓰지 못하는값이 포함되어있습니다 [ " + pstrBARCODE[i].ToString() + " ]";
                        return strrb;
                    }

                }
                #endregion //바코드데이터 유효값 확인


                #region "체크썸"
                intSUM = intSOD;
                for (int i = 0; i < intpBARCODE; i++)
                {
                    int intVAL = 0;
                    intVAL = pstrBARCODE[i];
                    intVAL = intVAL - 32;    //Code -> Value 변경
                    //Console.WriteLine(intVAL.ToString());

                    intSUM = intSUM + intVAL * (i + 1);
                }
                intMMM = intSUM % intMOD;
                strSUM = code128_array[intMMM].ToString();
                //strSUM = Char.ToString((char)(intMMM));


                //byte[] test12 = new byte[] { (byte)(intMMM + 32) };
                //strSUM = Encoding.UTF8.GetString(test12);

                #endregion //체크썸



                #region "START / STOP"
                
                strrb = string.Format("{0}{1}{2}{3}", code128_array[intSTA].ToString(), pstrBARCODE, strSUM, code128_array[intEND].ToString());
                //strrb = string.Format("{0}{1}{2}{3}", Char.ToString((char)intSTA), pstrBARCODE, strSUM, Char.ToString((char)intEND));
                //Console.WriteLine(strrb);

                //strrb = Char.ToString((char)intSTA);
                #endregion //START / STOP

                return strrb;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        #endregion




        #region "CODE39"

        public string CODE39(string pstrBARCODE, ref string pstrERR)
        {
            string strrb = "";

            try
            {
                strrb = "*" + pstrBARCODE + "*";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strrb;
        }

        #endregion



        #region "EAN13"


        public string EAN13(string pstrBARCODE, ref string pstrERR)
        {
            string strrb = "";

            try
            {
                string strBAR = "";
                string strTMP = "";
                int intpBARCODE = 0;
                string strBAR1 = "";
                string strBAR2 = "";


                string strFirstNUM = "";
                string strTABLENUM_LST = "";
                string strTABLENUM = "";

                string strSUM = "";
                int intSUMT = 0;
                int intSUM1 = 0; //홀수합계
                int intSUM2 = 0; //짝수합계


                #region "바코드데이터 유효값 확인"
                intpBARCODE = pstrBARCODE.Length;

                if (intpBARCODE < 1) { pstrERR = "데이터영역이 없습니다."; return strrb; }
                if (intpBARCODE < 12) { pstrERR = "12자리 숫자로 입력되어야 합니다"; return strrb; }
                pstrBARCODE = pstrBARCODE.Substring(0, 12);
                intpBARCODE = pstrBARCODE.Length;

                strFirstNUM = pstrBARCODE[0].ToString(); //첫글자추출
                strTABLENUM_LST = lfn_SEQ_TABLE_NUM(strFirstNUM);


                /*
                for (int i = 0; i < intpBARCODE; i++)
                {
                    //Console.WriteLine(pstrBARCODE[i].ToString());
                    if ((pstrBARCODE[i] < 32) || (pstrBARCODE[i] > 126))
                    {
                        pstrERR = "데이터로 쓰지 못하는값이 포함되어있습니다 [ " + pstrBARCODE[i].ToString() + " ]";
                        return strrb;
                    }
                }
                */
                #endregion //바코드데이터 유효값 확인





                #region "바코드값변환"

                strBAR = "";
                intSUM1 = 0;
                intSUM2 = 0;
                for (int i = 0; i < intpBARCODE; i++)
                {
                    strTMP = pstrBARCODE[i].ToString();
                    int intTMP = 0;
                    int.TryParse(strTMP, out intTMP);

                    if (((i + 1) % 2) == 0) //짝수
                    {
                        intSUM2 += intTMP;
                    }
                    else //홀수
                    {
                        intSUM1 += intTMP;
                    }

                    if ((i + 1) == 1)
                    {
                        strBAR += intTMP.ToString();
                    }
                    else if ((i + 1) > 1 && (i + 1) <= 7) //2~7자리 테이블
                    {
                        strTABLENUM = strTABLENUM_LST[i - 1].ToString();
                        strBAR += lfn_TABLE_COD(strTABLENUM, intTMP);
                    }
                    else
                    {
                        strBAR += lfn_TABLE_COD("", intTMP);
                    }
                }

                //공식 : {10 - ( 홀수자리숫자합 + 3 * 짝수자리숫자합) % 10 } % 10
                intSUMT = intSUM1 + (3 * intSUM2);
                intSUMT = 10 - (intSUMT % 10);
                intSUMT = intSUMT % 10;             //체크자리산출
                strSUM = lfn_TABLE_COD("", intSUMT);         //체크자리결과
                strBAR += strSUM;                   //체크자리 추가
                #endregion //체크썸



                #region "START / STOP"
                strBAR1 = strBAR.Substring(0, 7).ToString();
                strBAR2 = strBAR.Substring(7, 6).ToString();



                strrb = string.Format("{0}{1}{2}{3}", strBAR1, "*", strBAR2, "+");
                //Console.WriteLine(strrb);

                //strrb = Char.ToString((char)intSTA);
                #endregion //START / STOP






            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strrb;
        }

        //테이블 참조번호
        private string lfn_SEQ_TABLE_NUM(string pFirstNUM)
        {
            string retb = "";
            try
            {

                switch (pFirstNUM)
                {
                    case "1":
                        retb = "001011";
                        break;
                    case "2":
                        retb = "001101";
                        break;
                    case "3":
                        retb = "001110";
                        break;
                    case "4":
                        retb = "010011";
                        break;
                    case "5":
                        retb = "011001";
                        break;
                    case "6":
                        retb = "011100";
                        break;
                    case "7":
                        retb = "010101";
                        break;
                    case "8":
                        retb = "010110";
                        break;
                    case "9":
                        retb = "011010";
                        break;
                    default: // 0
                        retb = "000000";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retb;
        }


        private string lfn_TABLE_COD(string TABLE_NUM, int VAL)
        {
            string strrb = "";

            #region "0"
            if (TABLE_NUM.Equals("0"))
            {
                switch (VAL)
                {
                    case 0:
                        strrb = "A";
                        break;
                    case 1:
                        strrb = "B";
                        break;
                    case 2:
                        strrb = "C";
                        break;
                    case 3:
                        strrb = "D";
                        break;
                    case 4:
                        strrb = "E";
                        break;
                    case 5:
                        strrb = "F";
                        break;
                    case 6:
                        strrb = "G";
                        break;
                    case 7:
                        strrb = "H";
                        break;
                    case 8:
                        strrb = "I";
                        break;
                    case 9:
                        strrb = "J";
                        break;
                }
            }
            #endregion //0


            #region "1"
            if (TABLE_NUM.Equals("1"))
            {
                switch (VAL)
                {
                    case 0:
                        strrb = "K";
                        break;
                    case 1:
                        strrb = "L";
                        break;
                    case 2:
                        strrb = "M";
                        break;
                    case 3:
                        strrb = "N";
                        break;
                    case 4:
                        strrb = "O";
                        break;
                    case 5:
                        strrb = "P";
                        break;
                    case 6:
                        strrb = "Q";
                        break;
                    case 7:
                        strrb = "R";
                        break;
                    case 8:
                        strrb = "S";
                        break;
                    case 9:
                        strrb = "T";
                        break;
                }
            }
            #endregion //1



            #region "-1"
            if (TABLE_NUM.Equals(""))
            {
                switch (VAL)
                {
                    case 0:
                        strrb = "a";
                        break;
                    case 1:
                        strrb = "b";
                        break;
                    case 2:
                        strrb = "c";
                        break;
                    case 3:
                        strrb = "d";
                        break;
                    case 4:
                        strrb = "e";
                        break;
                    case 5:
                        strrb = "f";
                        break;
                    case 6:
                        strrb = "g";
                        break;
                    case 7:
                        strrb = "h";
                        break;
                    case 8:
                        strrb = "i";
                        break;
                    case 9:
                        strrb = "j";
                        break;
                }
            }
            #endregion //1


            return strrb;
        }


        #endregion //EAN13



        #region "codeArray"

        private void code128ArrayInsert()
        {

            code128_array.Add(0, ' ');
            code128_array.Add(1, '!');
            code128_array.Add(2, '"');
            code128_array.Add(3, '#');
            code128_array.Add(4, '$');
            code128_array.Add(5, '%');
            code128_array.Add(6, '&');
            code128_array.Add(7, '\'');
            code128_array.Add(8, '(');
            code128_array.Add(9, ')');
            code128_array.Add(10, '*');
            code128_array.Add(11, '+');
            code128_array.Add(12, ',');
            code128_array.Add(13, '-');
            code128_array.Add(14, '.');
            code128_array.Add(15, '/');
            code128_array.Add(16, '0');
            code128_array.Add(17, '1');
            code128_array.Add(18, '2');
            code128_array.Add(19, '3');
            code128_array.Add(20, '4');
            code128_array.Add(21, '5');
            code128_array.Add(22, '6');
            code128_array.Add(23, '7');
            code128_array.Add(24, '8');
            code128_array.Add(25, '9');
            code128_array.Add(26, ':');
            code128_array.Add(27, ';');
            code128_array.Add(28, '<');
            code128_array.Add(29, '=');
            code128_array.Add(30, '>');
            code128_array.Add(31, '?');
            code128_array.Add(32, '@');
            code128_array.Add(33, 'A');
            code128_array.Add(34, 'B');
            code128_array.Add(35, 'C');
            code128_array.Add(36, 'D');
            code128_array.Add(37, 'E');
            code128_array.Add(38, 'F');
            code128_array.Add(39, 'G');
            code128_array.Add(40, 'H');
            code128_array.Add(41, 'I');
            code128_array.Add(42, 'J');
            code128_array.Add(43, 'K');
            code128_array.Add(44, 'L');
            code128_array.Add(45, 'M');
            code128_array.Add(46, 'N');
            code128_array.Add(47, 'O');
            code128_array.Add(48, 'P');
            code128_array.Add(49, 'Q');
            code128_array.Add(50, 'R');
            code128_array.Add(51, 'S');
            code128_array.Add(52, 'T');
            code128_array.Add(53, 'U');
            code128_array.Add(54, 'V');
            code128_array.Add(55, 'W');
            code128_array.Add(56, 'X');
            code128_array.Add(57, 'Y');
            code128_array.Add(58, 'Z');
            code128_array.Add(59, '[');
            code128_array.Add(60, '\\');
            code128_array.Add(61, ']');
            code128_array.Add(62, '^');
            code128_array.Add(63, '_');
            code128_array.Add(64, '`');
            code128_array.Add(65, 'a');
            code128_array.Add(66, 'b');
            code128_array.Add(67, 'c');
            code128_array.Add(68, 'd');
            code128_array.Add(69, 'e');
            code128_array.Add(70, 'f');
            code128_array.Add(71, 'g');
            code128_array.Add(72, 'h');
            code128_array.Add(73, 'i');
            code128_array.Add(74, 'j');
            code128_array.Add(75, 'k');
            code128_array.Add(76, 'l');
            code128_array.Add(77, 'm');
            code128_array.Add(78, 'n');
            code128_array.Add(79, 'o');
            code128_array.Add(80, 'p');
            code128_array.Add(81, 'q');
            code128_array.Add(82, 'r');
            code128_array.Add(83, 's');
            code128_array.Add(84, 't');
            code128_array.Add(85, 'u');
            code128_array.Add(86, 'v');
            code128_array.Add(87, 'w');
            code128_array.Add(88, 'x');
            code128_array.Add(89, 'y');
            code128_array.Add(90, 'z');
            code128_array.Add(91, '{');
            code128_array.Add(92, '|');
            code128_array.Add(93, '}');
            code128_array.Add(94, '~');
            code128_array.Add(95, 'Ã');
            code128_array.Add(96, 'Ä');
            code128_array.Add(97, 'Å');
            code128_array.Add(98, 'Æ');
            code128_array.Add(99, 'Ç');
            code128_array.Add(100, 'È');
            code128_array.Add(101, 'É');
            code128_array.Add(102, 'Ê');
            code128_array.Add(103, 'Å');    //code128A
            code128_array.Add(104, 'Ë');    //code128B
            code128_array.Add(105, 'Ì');    //code128C
            code128_array.Add(106, 'É');
            code128_array.Add(107, 'Í');
            code128_array.Add(204, 'Ì');    
            code128_array.Add(206, 'Î');    //stop bit
            code128_array.Add(209, 'Ñ');    
            code128_array.Add(211, 'Ó');    


        }
    }
    #endregion
}
