using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorer_Indicator_Contorl.LocalClass
{
    public class Barcode
    {
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
                string strSUM = "";

                int intSTA = 203; //START Code A : 203  208  248   : 103
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
                        intSTA = 209; //209
                        intEND = 211; //211
                        break;
                    case "C":
                        intSTA = 210;
                        intEND = 211;
                        break;
                    default:
                        intSTA = 208;
                        intEND = 211;
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
                    Console.WriteLine(pstrBARCODE[i].ToString());

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
                strSUM = Char.ToString((char)(intMMM + 32));

                //Console.WriteLine(string.Format("CHECK SUM  :  {0} % {1} = {2}  --> [ {3} ]", intSUM.ToString(), intMOD.ToString(), intMMM.ToString(),strSUM));
                #endregion //체크썸





                #region "START / STOP"

                strrb = string.Format("{0}{1}{2}{3}", Char.ToString((char)intSTA), pstrBARCODE, strSUM, Char.ToString((char)intEND));
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
        #endregion
    }
}
