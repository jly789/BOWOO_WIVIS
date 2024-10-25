using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib.Common
{
    static public class StringFormatEx
    {
        #region "표기 형식 변환"
        /// <summary>
        /// 일자를 날짜형태로("yyyy-MM-dd") 변환 : 19990101 -> 1999-01-01 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatDate(this string value)
        {
            return value.FormatDate("-");
        }

        static public string FormatDate(this string value, string barStr)
        {
            if (value == null) return string.Empty; ;

            string retStr = string.Empty;

            if (value.Length >= 8)
            {
                retStr = string.Format("{1}{0}{2}{0}{3}"
                                        , barStr
                                        , value.Substring(0, 4)
                                        , value.Substring(4, 2)
                                        , value.Substring(6, 2)
                                        );
            }

            return retStr;
        }


        /// <summary>
        /// 시간을 다음과 같은 형식으로 반환 : 121212 -> 12:12:12
        /// </summary>
        /// <param name="p_strTime"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatTime(this string p_strTime)
        {
            string strRet = string.Empty;

            if (!string.IsNullOrEmpty(p_strTime))
            {
                if (p_strTime.Length >= 2) strRet += p_strTime.Substring(0, 2);
                if (p_strTime.Length >= 4) strRet += ":" + p_strTime.Substring(2, 2);
                if (p_strTime.Length >= 6) strRet += ":" + p_strTime.Substring(4, 2);
            }

            return strRet;
        }


        /// <summary>
        /// 일자를 다음과 같은 형식으로 반환 : 19990101121212 -> 1999-01-01 12:12:12
        /// </summary>
        /// <param name="p_strDateTime"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatDateTime(this string p_strDateTime)
        {
            string strRet = string.Empty;

            if (!string.IsNullOrEmpty(p_strDateTime))
            {
                if (p_strDateTime.Length >= 4) strRet += p_strDateTime.Substring(0, 4);
                if (p_strDateTime.Length >= 6) strRet += "-" + p_strDateTime.Substring(4, 2);
                if (p_strDateTime.Length >= 8) strRet += "-" + p_strDateTime.Substring(6, 2);
                if (p_strDateTime.Length >= 10) strRet += " " + p_strDateTime.Substring(8, 2);
                if (p_strDateTime.Length >= 12) strRet += ":" + p_strDateTime.Substring(10, 2);
                if (p_strDateTime.Length >= 14) strRet += ":" + p_strDateTime.Substring(12, 2);
            }

            return strRet;
        }

        /// <summary>
        /// 주민번호를 다음과 같은 형식으로 반환 : 7001011234567 -> 700101-1234567
        /// </summary>
        /// <param name="p_strSsn"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatJumin(this string p_strSsn)
        {
            return FormatJumin(p_strSsn, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_strSsn"></param>
        /// <param name="bSecurity"></param>
        /// <returns></returns>
        static public string FormatJumin(this string p_strSsn, bool bSecurity)
        {
            string strRet = string.Empty;
            string strTmp = p_strSsn.GetNumber("");

            if (strTmp.Length == 13)
            {
                if (bSecurity) strRet = strTmp.Substring(0, 6) + "-" + strTmp.Substring(6, 1) + "******";
                else strRet = strTmp.Substring(0, 6) + "-" + strTmp.Substring(6, 7);
            }
            else
            {
                strRet = strTmp;
            }

            return strRet;
        }


        /// <summary>
        /// 사업자 번호를 다음과 같은 형식으로 반환 : 1234567890 -> 123-45-67890
        /// </summary>
        /// <param name="p_strSsn"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatBusinessNumber(this string p_strSsn)
        {
            string strRet = string.Empty;
            string strTmp = p_strSsn.GetNumber("");

            if (strTmp.Length == 10)
            {
                strRet = strTmp.Substring(0, 3) + "-" + strTmp.Substring(3, 2) + "-" + strTmp.Substring(5, 5);
            }
            else
            {
                strRet = strTmp;
            }

            return strRet;
        }



        /// <summary>
        /// Money Format으로 표시하기. 
        /// </summary>
        /// <param name="p_value"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatMoney(this string p_value)
        {
            string sTemp = p_value.GetNumber("");

            if (!string.IsNullOrEmpty(sTemp))
                return double.Parse(sTemp).ToString("C");
            else
                return sTemp;
        }


        /// <summary>
        /// </summary>
        /// <param name="p_value"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatMoney(this string p_value, string p_mode)
        {
            string strMoney = FormatMoney(p_value);

            if (strMoney == "") return "";
            if (p_mode == "1")
            {
                strMoney = strMoney.Substring(1);
                strMoney = strMoney + " 원";
            }
            else if (p_mode == "2")
            {
                strMoney = strMoney.Substring(1);
            }
            return strMoney;
        }

        /// <summary>
        /// 전화번호를 다음과 같은 형식으로 반환 : 01000000000 -> 010-0000-0000
        /// </summary>
        /// <param name="p_value"></param>
        /// <returns></returns>
        /// </remarks>
        static public string FormatPhone(this string p_value)
        {
            if (string.IsNullOrEmpty(p_value)) return string.Empty;

            string strTmpPhone = string.Empty;
            string strArea = string.Empty;
            string strPrefix = string.Empty;
            string strNumber = string.Empty;

            strTmpPhone = p_value.Replace("-", "");

            // 번호의 길이가 7보다 짧으면 그대로 반환
            if (strTmpPhone.Length < 7)
            {
                return strTmpPhone;
            }

            // 지역번호를 구한다.
            string strTmp = strTmpPhone.Substring(0, 1);
            string strTmp2 = strTmpPhone.Substring(0, 2);

            if (strTmp != "0")
            {
                // 지역번호 없음
                strArea = string.Empty;
            }
            else if (strTmp2 == "02")
            {
                // 서울지역
                strArea = "02";
                strTmpPhone = strTmpPhone.Substring(2);
            }
            else
            {
                // 서울이외지역
                strArea = strTmpPhone.Substring(0, 3);
                strTmpPhone = strTmpPhone.Substring(3);
            }

            // 지역번호를 제외한 번호를 분해한다.
            if (strTmpPhone.Length < 7)
            {
                strPrefix = string.Empty;
                strNumber = strTmpPhone;
            }
            else if (strTmpPhone.Length == 7)
            {
                strPrefix = strTmpPhone.Substring(0, 3);
                strNumber = strTmpPhone.Substring(3);
            }
            else
            {
                strPrefix = strTmpPhone.Substring(0, 4);
                strNumber = strTmpPhone.Substring(4);
            }

            // 반환할 번호를 만든다.
            string strRet = strNumber;
            if (strPrefix != "")
            {
                strRet = strPrefix + "-" + strRet;
            }
            if (strArea != "")
            {
                strRet = strArea + "-" + strRet;
            }

            return strRet;
        }


        /// <summary>
        /// 우편번호를 다음과 같은 형식으로 반환 : 000000 -> 000-000
        /// </summary>
        /// <param name="p_value"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatZipCode(this string value)
        {
            string str = value.GetNumber("");

            if (str.Length < 3)
            {
                return value;
            }
            else
            {
                return str.Substring(0, 3) + "-" + str.Substring(3);
            }
        }


        /// <summary>
        /// 유심 코드 포맷 변환 
        /// </summary>
        /// <param name="p_value"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatUSIMCode(this string p_value)
        {
            if (p_value == "") return "";

            p_value += "*******************"; // 20자리로 만듬. 에러방지용.
            return p_value.Substring(0, 4) + "-" + p_value.Substring(4, 2) + "**-****-****-***F"; // 맨 끝자리를 F로 한다.
        }


        /// <summary>
        /// 유심 코드 포맷 변환 
        /// </summary>
        /// <param name="p_value"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string FormatUSIMCode(this string p_value, Boolean bEtc)
        {
            if (bEtc == true) return FormatUSIMCode(p_value);
            if (p_value == "") return "";
            //p_value += "*******************"; // 20자리로 만듬. 에러방지용.
            return p_value.Substring(0, 4) + "-" + p_value.Substring(4, 4) + "-" + p_value.Substring(8, 4) + "-" + p_value.Substring(12, 4) + "-" + p_value.Substring(16, 3) + "F";

        }
        #endregion
    }
}
