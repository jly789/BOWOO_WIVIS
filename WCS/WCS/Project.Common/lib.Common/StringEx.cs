/*****************************************************************************
 * 역할 : String의 확장 클래스
 *****************************************************************************/

using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;



namespace lib.Common
{
    static public class StringEx
    {

        #region "정규식을 이용한 문자 추출"
        /// <summary>
        /// ASCII 값에서 문자를 얻는 함수.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="Quotes"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string GetChar(int intAscii)
        {
            return Encoding.ASCII.GetString(new byte[] { (byte)intAscii });
        }

        /// <summary>
        /// 입력으로 배열을 주고'A', 'S', 'C' 을 리턴
        /// ANSI SQL의 IN 문에서 사용. 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="Quotes"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string ConcateList(ArrayList arr)
        {
            return ConcateList(arr, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="Quotes"></param>
        /// <returns></returns>
        static public string ConcateList(ArrayList arr, string Quotes)
        {
            string strResult = string.Empty;

            for (int i = 0; i < arr.Count; i++)
            {
                strResult += Quotes + arr[i].ToString() + Quotes;

                if (i != arr.Count - 1) strResult += ", ";
            }

            return strResult;
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string GetNumber(this string str)
        {
            return GetNumber(str, "");
        }
        static public string GetNumber(this string str, string replace)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            else
                return Regex.Replace(str, "[^0-9]", replace);
        }

        /// <summary>
        /// 스트링에서 숫자와 영문자를 제외한 문자를 제거하는 함수. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string ReplaceSymbol(string input, string replace)
        {
            return Regex.Replace(input, "[^A-Za-z0-9]", replace);
        }

        /// <summary>
        /// 특수 기호 문자 제거. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string BlockInjection(string input, string replace)
        {
            // 한글도 입력되는 아래 정규식을 사용 . 
            return Regex.Replace(input, "[\\ \\-\\;\\!\\@\\#\\$\\%\\^\\&\\(\\)\\\\_\\/\\\\:\\*\\?\\\"\\<\\>\\|]", replace);
        }



        /// <summary>
        /// 문자열을 역순으로 바꾸어서 리턴한다.
        /// </summary>
        /// <param name="svalue"></param>
        /// <returns></returns>
        static public string Reverse(this string svalue)
        {

            int ilen = svalue.Length;
            string[] strvalue = new string[ilen];
            string strReturn = "";
            try
            {
                for (int i = 0; i < ilen; i++)
                {
                    strvalue[i] = svalue.Substring(i, 1);
                }
                for (int j = svalue.Length; j > 0; j--)
                {
                    strReturn = strReturn + strvalue[j];
                }
                return strReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region "표기 형식 변환"
        /// <summary>
        /// 출생일부터 현재까지 나이 계산 함수.
        /// 
        /// http://www.devpia.com/MAEUL/Contents/Detail.aspx?BoardID=17&MAEULNo=8&no=81845&ref=81845
        /// </summary>
        /// <param name="p_strBthdt"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2011.02.18. 용세중. 추가
        /// </remarks>
        static public string CalcAge(string p_strBthdt)
        {
            return CalcAge(p_strBthdt, DateTime.Today.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 출생일부터 현재까지 나이 계산 함수.
        /// 
        /// http://www.devpia.com/MAEUL/Contents/Detail.aspx?BoardID=17&MAEULNo=8&no=81845&ref=81845
        /// </summary>
        /// <param name="p_strBthdt"></param>
        /// <param name="p_bstBasisdt"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string CalcAge(string p_strBthdt, string p_bstBasisdt)
        {
            string sArge = string.Empty;
            try
            {
                if (p_strBthdt.Length != 8) return sArge;
                if (p_bstBasisdt.Length != 8) return sArge;

                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                dtfi.ShortDatePattern = "yyyyMMdd";
                dtfi.DateSeparator = "-";

                DateTime dtBthdt = DateTime.ParseExact(p_strBthdt, "yyyyMMdd", dtfi);
                DateTime dtBasisdt = DateTime.ParseExact(p_bstBasisdt, "yyyyMMdd", dtfi);

                TimeSpan res = dtBasisdt - dtBthdt;
                DateTime dateTimeAge = new DateTime(res.Ticks);

                sArge = dateTimeAge.ToString("yy");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sArge;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        static public DateTime? GetDateTime(this string val, string format = "")
        {
            val = val.GetNumber("");

            if (!string.IsNullOrEmpty(val) && val.Length == 8)
            {
                return DateTime.ParseExact(val, format, CultureInfo.InvariantCulture);
            }
            else
                return null;
        }

        #endregion



        #region "Type Convert "
        /// <summary>
        /// Object를 String으로 변환하는 메서드
        /// </summary>
        /// <param name="convertValue"></param>
        /// <returns></returns>
        static public string ConvertObjectToString(object convertValue)
        {
            if (convertValue == null || string.IsNullOrEmpty(convertValue.ToString()))
            {
                return string.Empty;
            }
            else
            {
                return convertValue.ToString();
            }
        }

        /// <summary>
        /// String 을 Int로 변환하는 메서드
        /// </summary>
        /// <param name="convertValue"></param>
        /// <returns></returns>
        static public int ConvertStringToInt(string convertValue)
        {
            int returnValue = 0;

            if (!string.IsNullOrEmpty(convertValue))
            {
                returnValue = int.Parse(convertValue);
            }

            return returnValue;
        }



        /// <summary>
        /// 문자열로 부터 Boolean값으로 변환합니다.
        /// </summary>
        /// <param name="strValue">변환할 문자열</param>
        /// <returns>Boolean</returns>
        static public bool ParseBoolean(string strValue)
        {
            return bool.Parse(strValue);
        }


        /// <summary>
        /// 문자열로 부터 Double값으로 변환합니다.
        /// </summary>
        /// <param name="strValue">변환할 문자열</param>
        /// <returns>Double</returns>
        static public double ParseDouble(string strValue)
        {
            return double.Parse(strValue);
        }


        /// <summary>
        /// 문자열로 부터 int값으로 변환합니다.
        /// </summary>
        /// <param name="strValue">변환할 문자열</param>
        /// <returns>int</returns>
        static public int ParseInt(string strValue)
        {
            return int.Parse(strValue);
        }


        /// <summary>
        /// 문자열로 부터 uint값으로 변환합니다.
        /// </summary>
        /// <param name="strValue">변환할 문자열</param>
        /// <returns>uint</returns>
        static public uint ParseUINT(string strValue)
        {
            return (uint)ParseInt(strValue);
        }
        #endregion

        /// <summary>
        /// 테그 지우기
        /// </summary>
        /// <param name="str">문자열</param>
        /// <returns>문자열</returns>
        static public string stripTags(this string str)
        {
            string tmpstr = string.Empty;
            if (str != null)
            {
                Regex regex = new Regex("\\<(\\/?)(\\w+)*([^<>]*)>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                tmpstr = regex.Replace(str, "");
            }
            return tmpstr;
        }



        /// <summary>
        /// 한글*영문 Word길이 제어
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="max">길이</param>
        /// <param name="suffix">붙일문자</param>
        /// <returns>문자열</returns>
        static public string strCut(string str, int max, string suffix)
        {
            string strResult = "";
            string s = str.ToString();

            if (s == "") return strResult;

            int count = 0;
            string tmpStr = str.Trim().ToString();
            char[] chrarr = tmpStr.ToCharArray();

            if (tmpStr.Length != 0)
            {
                for (int i = 0; i < chrarr.Length; i++)
                {
                    int temp = Convert.ToInt32(chrarr[i]);
                    if (temp < 0 || temp >= 128)
                    {
                        //한글 2byte
                        count = count + 2;
                    }
                    else
                    {
                        count = count + 1;
                    }

                    if (count <= max)
                    {
                        strResult = strResult + tmpStr.Substring(i, 1);
                    }
                    else
                    {
                        strResult = strResult + suffix;
                        break;
                    }
                }
            }
            return strResult;
        }


        /// <summary>
        /// 문자열이 설정한 길이보다 길다면 설정한 길이만큼 자르고 뒤에 대체 문자열을 붙여서 반환한다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length">제한길이</param>
        /// <param name="text">마지막 대체 문자열</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string LengthCut(this string value, int length, string text)
        {
            // 넘어온 문자열을 캐릭터형태로 변환한다.
            char[] chArray = value.ToCharArray();

            // 문자열 길이
            int len = 0;

            // 반환할 텍스트 저장 변수
            string retText = string.Empty;
            // 길이 확인
            foreach (char ch in chArray)
            {
                // 유니코드(한글, 특수문자 등등 2바이트 문자)
                if (char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    len += 2;
                }
                // 영숫자 (1바이트 문자)
                else
                {
                    len++;
                }

                retText += ch;

                if (len >= length)
                {
                    retText += text;
                    break;
                }
            }

            return retText;
        }

        /// <summary>
        /// 문자열이 설정한 길이보다 길다면 설정한 길이만큼 자르고 뒤에 ..을 붙여서 반환한다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length">제한길이</param>
        /// <param name="text">마지막 대체 문자열</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string LengthCut(this string value, int length)
        {
            return value.LengthCut(length, "..");
        }

        /// <summary>
        /// 문자열이 설정한 길이보다 길다면 설정한 길이만큼 자르고 뒤에 ..을 붙여서 반환한다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length">제한길이</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string LengthCutNoDot(this string value, int length)
        {
            return value.LengthCut(length, string.Empty);
        }


        #region
        /// <summary>
        /// Accepts a date time value, calculates number of days, minutes or seconds and shows 'pretty dates'
        /// like '2 days ago', '1 week ago' or '10 minutes ago'
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        static public string GetPrettyDate(this DateTime d)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return d.ToString();
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "just now";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago",
                    dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                    Math.Ceiling((double)dayDiff / 7));
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        static public void AppendFormatWithLine(this StringBuilder sb, string format, params object[] args)
        {
            sb.AppendFormat(format, args);
            sb.AppendLine();
        }

        /// <summary>
        /// 인덴트
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        static public string Indent(this string source, int count)
        {
            return source.PadLeft(count).ToString();
        }

        /// <summary>
        /// 문자열 줄바꿈 기호 <br /> 로 변환
        /// </summary>
        /// <param name="StringToReplace"></param>
        /// <returns></returns>
        static public string ToReplaceBr(this string StringToReplace)
        {
            return StringToReplace.Replace("\n", "<br />");
        }


        /// <summary>
        /// 문자열 <script></script> HTMLENCODE
        /// </summary>
        /// <param name="StringToReplace"></param>
        /// <returns></returns>
        static public string ToScriptTag(this string StringToReplace)
        {
            string tmpStr = StringToReplace;//.ToLower();

            int idx = tmpStr.ToLower().IndexOf("<script");
            if (idx > -1)
                tmpStr = tmpStr.ToLower().Replace("<script", "&lt;script");

            idx = tmpStr.ToLower().IndexOf("</script>");
            if (idx > -1)
                tmpStr = tmpStr.Replace("</script>", "&lt;/script&gt;");

            idx = tmpStr.ToLower().IndexOf("<javascript");
            if (idx > -1)
                tmpStr = tmpStr.Replace("<javascript", "&lt;javascript");

            tmpStr = tmpStr.Replace("'", "&apos;");
            tmpStr = tmpStr.Replace("&", "&amp;");

            tmpStr = tmpStr.Replace("<", "&lt;");
            tmpStr = tmpStr.Replace(">", "&gt;");
            tmpStr = tmpStr.Replace("%3c", "&lt;");
            tmpStr = tmpStr.Replace("%3C", "&lt;");
            tmpStr = tmpStr.Replace("%3e", "&gt;");
            tmpStr = tmpStr.Replace("%3E", "&gt;");

            idx = tmpStr.ToLower().IndexOf("onmouseover");
            if (idx > -1)
                tmpStr = tmpStr.ToLower().Replace("onmouseover", "tmp");
            idx = tmpStr.ToLower().IndexOf("onerror");
            if (idx > -1)
                tmpStr = tmpStr.ToLower().Replace("onerror", "tmp");

            tmpStr = tmpStr.Replace("&lt;/a&gt;", "</a>");
            tmpStr = tmpStr.Replace("&lt;/font&gt;", "</font>");
            tmpStr = tmpStr.Replace("&lt;/b&gt;", "</b>");
            tmpStr = tmpStr.Replace("&lt;img", "<img");
            tmpStr = tmpStr.Replace("&lt;b>", "<b>");
            tmpStr = tmpStr.Replace("&lt;font ", "<font ");
            tmpStr = tmpStr.Replace("&lt;a ", "<a ");

            return tmpStr;
        }

        /// <summary>
        /// Converts a string to a Sentence case
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        static public string ToSentence(this string String)
        {
            if (String.Length > 0)
                return String.Substring(0, 1).ToUpper() + String.Substring(1, String.Length - 1);

            return "";
        }


        /// <summary>
        /// Converts a valid string to integer
        /// </summary>
        /// <param name="StringToConvert"></param>
        /// <returns></returns>
        static public int ToInteger(this string StringToConvert)
        {
            try
            {
                if (string.IsNullOrEmpty(StringToConvert.ToString()))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(StringToConvert.ToString());
                }

            }
            catch (Exception ex)
            {
                throw new Exception("String could not be converted to Integer" + ex.InnerException);
            }
        }

        /// <summary>
        /// Converts a valid string to double
        /// </summary>
        /// <param name="StringToConvert"></param>
        /// <returns></returns>
        static public double ToDouble(this string StringToConvert)
        {
            return Convert.ToDouble(StringToConvert);
        }


        /// <summary>
        /// Converts an String to it's decimal value
        /// </summary>
        /// <param name="StringToConvert"></param>
        /// <returns></returns>
        static public decimal ToDecimal(this string StringToConvert)
        {
            try
            {
                return Convert.ToDecimal(StringToConvert.ToString());
            }
            catch
            {
                throw new Exception("String cannot be converted to decimal");
            }
        }

        /// <summary>
        /// Converts a string to a DateTime case
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        static public DateTime ToDateTime(this string String)
        {
            try
            {
                if (string.IsNullOrEmpty(String))
                    return DateTime.MinValue;
                else
                    return Convert.ToDateTime(String);
            }
            catch (Exception ex)
            {
                throw new Exception("Object cannot be converted to DateTime. Object: " + ex.ToString());
            }
        }


        /// <summary>
        /// 문자열이 널인지 체크하고 널이면 빈문자열을 리턴합니다.
        /// </summary>
        /// <param name="strValue">체크할 문자열</param>
        /// <returns>문자열 / string.Empty</returns>
        static public string NullCheck(string strValue)
        {
            return ((strValue == null) ? string.Empty : strValue);
        }



        /// <summary>
        /// url 절대 경로 가져오기
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public string ConvertAbsolutePath(string url)
        {
            try
            {
                if (url.Contains("http://"))
                {
                    url = url.Replace("http://", "");
                    url = url.Substring(url.IndexOf("/"), url.Length - url.IndexOf("/"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return url;
        }
        #endregion


        /// <summary>
        /// ▶ GetBodyHtml : Body
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public string GetBodyHtml(string path)
        {
            string bodyHtml = String.Empty;

            try
            {
                FileStream stream = File.OpenRead(path + "\\noname.htm");

                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                bodyHtml = Encoding.UTF8.GetString(buffer);
                stream.Close();

                File.Delete(path + "\\noname.htm");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bodyHtml;
        }

        /// <summary>
        /// ▶ GetBodyImg : Body Image
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public Stack<string[]> GetBodyImg(string path)
        {
            Stack<string[]> bodyImg = new Stack<string[]>();

            try
            {
                FileInfo[] files;
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                files = dirInfo.GetFiles();

                if (files.Length > 0)
                {
                    foreach (FileInfo file in files)
                    {
                        string[] fileInfo = new string[4];
                        fileInfo[0] = file.FullName;
                        fileInfo[1] = file.Name;
                        fileInfo[2] = file.Extension;
                        fileInfo[3] = file.Length.ToString();

                        bodyImg.Push(fileInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bodyImg;
        }

        /// <summary>
        /// 숫자만 추출하는 함수. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string GetNumber(this object obj, string replace)
        {
            if (obj == null)
                return string.Empty;
            else if (string.IsNullOrEmpty(obj.ToString()))
                return string.Empty;
            else
                return Regex.Replace(obj.ToString(), "[^0-9]", replace);
        }

        /// <summary>
        /// Right 함수. Text의 우측부터 길이만큼 자른다
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string Right(this string Text, int length)
        {
            string ConvertText = string.Empty;

            if (Text.Length < length)
            {
                length = Text.Length;
            }

            ConvertText = Text.Substring(Text.Length - length, length);

            return ConvertText;
        }

        /// <summary>
        /// Left 함수. Text의 좌측부터 길이만큼 자른다
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public string Left(this string Text, int length)
        {
            string ConvertText = string.Empty;

            if (Text.Length < length)
            {
                length = Text.Length;
            }

            ConvertText = Text.Substring(0, length);

            return ConvertText;
        }
    }
}
