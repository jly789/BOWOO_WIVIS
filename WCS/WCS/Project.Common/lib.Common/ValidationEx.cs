using System;
using System.Text.RegularExpressions;

namespace lib.Common
{
    /*****************************************************************************
     * 역할 : 유효성 관련 함수 모음.
     *****************************************************************************/ 
    static public class ValidationEx
    {
        #region 유효성 확인

        /// <summary>
        /// 우편번호 유효성 검사
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsZipCode(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"(^[0-9]{3})-([0-9]{3})$");
        }

        /// <summary>
	    /// 주민등록번호 유효성 검사 메서드
        /// </summary>
	    /// <param name = "str">검사할 주민번호</param>
        /// <returns></returns>
	    static public bool IsJuminNumber(this string str)
	    {
		    if (str.IsNumber()) {
			    return false;
		    }

		    // 숫자만 입력했는지 체크
		    if (str.Length != 13) {
			    return false;
		    }
		    // 13자리가 맞는지 체크
		    int sum = 0;
		    // 유효성 검사
		    int temp = 0;
		    // 주민번호를 배열에 저장

		    int[] num = new int[14];

		    // 가중치 값 저장

		    int[] digit = {
			    2,
			    3,
			    4,
			    5,
			    6,
			    7,
			    8,
			    9,
			    2,
			    3,
			    4,
			    5
		    };

		    // 입력된 문자를 숫자로 변환
		    int i = 0;
		    while (i < 13) {
			    //Parse()함수는 string형을 받아서 형변환
			    //Convert는 object 형을 받아서 형변환
			    num[i] = int.Parse(str[i].ToString());
			    System.Math.Max(System.Threading.Interlocked.Increment(ref i), i - 1);
		    }


		    //for(i)
		    // 주민번호 유효성 검사
		    int j = 0;
		    while (j < 12) {
			    sum += digit[j] * num[j];
			    System.Math.Max(System.Threading.Interlocked.Increment(ref j), j - 1);
		    }
		    //for(i)
		    // 유효성 검사

		    temp = sum % 11;

		    int check_digit_num1 = temp;
		    // 총합을 11로 나눈 나머지
		    int check_digit_num2 = num[12];
		    //주민번호 제일 끝자리
		    if (check_digit_num1 == 0) {
			    if (check_digit_num2 == 1) {
				    return true;
			    } else {
				    return false;
			    }
		    //if
		    } else if (check_digit_num1 == 1) {
			    if (check_digit_num2 == 0) {
				    return true;
			    } else {
				    return false;
			    }
		    //else if
		    } else if ((11 - check_digit_num1) == check_digit_num2) {
			    return true;
		    } else {
			    return false;
		    }
	    }
        
        /// <summary>
        /// 주민번호 앞자리 유효성 검사
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsValidIDNumber(string input)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^[0-9][0-9][0-1][0-9][0-3][0-9]$")) return false;

            int month = Convert.ToInt32(input.Substring(2, 2));
            int day = Convert.ToInt32(input.Substring(4, 2));

            if (month < 1 || month > 12) return false;
            if (day < 1 || day > 31) return false;

            DateTime result;
            // 2008년이 2월이 29일까지 존재하기때문에 사용.
            return (DateTime.TryParse(string.Format("{0}-{1:00}-{2:00}", 2008, month, day), out result));

        }
        

	    //URL 유효성 검사 메서드
	    //http\:\/\/[\w\-]+(\.\[\w\-]+)+ 
	    static public bool IsURL(this string str)
	    {
		    if (System.Text.RegularExpressions.Regex.IsMatch(str, "http\\:\\/\\/[\\w\\-]+(\\.\\[\\w\\-]+)+") == false) {
			    return false;
		    } else {
			    return true;
		    }
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        static public bool IsDateTime(this string date)
        {
            //DateTime tempDate;
            //return DateTime.TryParse(date, out tempDate) ? true : false;

            Regex ShortDate = new Regex(@"^\d{6}$");
            Regex LongDate = new Regex(@"^\d{8}$");

            string s = date.GetNumber().Trim();
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            else
            {
                if (ShortDate.Match(s).Success)
                {
                    s = s.Substring(0, 2) + "/" + s.Substring(2, 2) + "/" + s.Substring(4, 2);
                }
                if (LongDate.Match(s).Success)
                {
                    s = s.Substring(0, 4) + "/" + s.Substring(4, 2) + "/" + s.Substring(6, 2);
                }
                DateTime d = DateTime.MinValue;
                if (DateTime.TryParse(s, out d))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

	    //날짜 입력 체크 메서드
	    //"\{[0-9]^4}[-,.\/]\d{[0-1][0-9]}[-,.\/]\d{[0-3][0-9]}"
	    static public bool IsDateYMD(this string str)
	    {
		    try {
			    //If Regex.IsMatch(str, ("\{[0-9]^4}[-,.\/]\d{[0-1][0-9]}[-,.\/]\d{[0-3][0-9]}")) = False Then
			    //    Return False
			    //End If

			    DateTime.Parse(str);

			    return true;
		    }
            catch
            {
			    return false;
		    }
	    }

	    //날짜 입력 체크 메서드
	    //"\d{[0-1][0-9]}[-,.\/]\d{[0-3][0-9]}"
	    static public bool IsDateMD(this string str)
	    {
		    try {
			    int iMonth = Convert.ToInt32(str.Substring(0, 2));
			    int iDay = Convert.ToInt32(str.Substring(3, 2));

			    if (0 < iMonth & iMonth < 13 & 0 < iDay & iDay < 32) {
				    return true;
			    } else {
				    return false;
			    }

		    } catch (Exception ex) {
			    throw ex;
		    }
	    }
        

        /// <summary>
        /// 날자 유효성 검사
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public bool IsDate(this string input)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(input, @"^([0-9]{4})-([0-1][0-9])-([0-3][0-9])$"))
            {
                DateTime date;
                return DateTime.TryParse(input, out date);
            }
            return false;
        }

        /// <summary>
        /// 시간 유효성 검사(hh:mm)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
	    //시간 입력 체크 메서드
	    static public bool IsTime(this string input)
	    {
		    if (System.Text.RegularExpressions.Regex.IsMatch(input, "^([0-2][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$") == false) {
			    return false;
		    } else {
			    return true;
		    }
	    }
        

        /// <summary>
        /// 전화번호 유효성 검사
        /// 전화번호 입력 체크 메서드
        /// 전화번호 : "\d{2,3}[-,.\/]\d{3,4}[-,.\/]\d{4}"
        /// 국번이 없을 수도 있는 경우: ^(\d{2,3}[-,.\/]|)\d{3,4}[-,.\/]\d{4}$</summary>
        /// <param name="input"></param>
        /// <returns></returns>
	    static public bool IsTelephone(this string str)
	    {
		    if (System.Text.RegularExpressions.Regex.IsMatch(str, ("^(\\d{2,3}[-,.\\/]|)\\d{3,4}[-,.\\/]\\d{4}$")) == false) {
			    return false;
		    } else {
			    return true;
		    }
	    }
        
        
	    //
	    //
        /// <summary>
        /// 핸드폰번호 유효성 검사
        /// 핸드폰 입력 체크 메서드
        /// 핸드폰: 01[16789]\-\d{3,4}\-\d{3,4}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
	    static public bool IsHandphone(this string str)
	    {
		    if (System.Text.RegularExpressions.Regex.IsMatch(str, ("01[016789]-\\d{3,4}-\\d{3,4}")) == false) {
			    return false;
		    } else {
			    return true;
		    }
	    }

        
        /// <summary>
        /// 입력 문자의 숫자여부 체크 : Check for Positive Integers with zero inclusive  
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        static public bool IsNumber(this string strNumber)
        {
            if (string.IsNullOrEmpty(strNumber)) return false;

            Regex objNotWholePattern = new Regex("[^0-9]");
            return !objNotWholePattern.IsMatch(strNumber);
        }

        /// <summary>
        /// Check if the string is Double  
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        static public bool IsDouble(this string strNumber)
        {
            if (strNumber == "")
                return false;

            try
            {
                Convert.ToDouble(strNumber);

            }
            catch (Exception)
            {

                return false;
            }

            return true;

        }

        /// <summary>
        /// Function to Check for AlphaNumeric. 
        /// </summary>
        /// <param name="strToCheck"> String to check for alphanumeric</param>
        /// <returns>True if it is Alphanumeric</returns>
        static public bool IsAlphaNumeric(this string strToCheck)
        {
            Regex regex = new Regex(@"^[0-9a-zA-Z]{1,100}$");
            return strToCheck == string.Empty ? false : regex.IsMatch(strToCheck);
        }

        /// <summary>
        ///Function to Check for valid alphanumeric input with space chars also
        /// </summary>
        /// <param name="strToCheck"> String to check for alphanumeric</param>
        /// <returns>True if it is Alphanumeric</returns>
        static public bool IsAlphaNumericWithSpace(this string strToCheck)
        {
            bool valid = true;

            if (strToCheck == "")
                return false;

            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9\\s]");

            valid = !objAlphaNumericPattern.IsMatch(strToCheck);
            return valid;
        }

        /// <summary>
        /// Check for valid alphabet input with space chars also
        /// </summary>
        /// <param name="strToCheck"> String to check for alphanumeric</param>
        /// <returns>True if it is Alphanumeric</returns>
        static public bool IsAlphabetWithSpace(this string strToCheck)
        {
            bool valid = true;

            if (strToCheck == "")
                return false;

            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z\\s]");

            valid = !objAlphaNumericPattern.IsMatch(strToCheck);
            return valid;
        }

        /// <summary>
        /// Check for valid alphabet input with space chars also
        /// </summary>
        /// <param name="strToCheck"> String to check for alphanumeric</param>
        /// <returns>True if it is Alphanumeric</returns>
        static public bool IsAlphabetWithHyphen(this string strToCheck)
        {
            bool valid = true;

            if (strToCheck == "")
                return false;

            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z\\-]");

            valid = !objAlphaNumericPattern.IsMatch(strToCheck);
            return valid;
        }

        /// <summary>
        ///  Check for Alphabets.
        /// </summary>
        /// <param name="strToCheck">Input string to check for validity</param>
        /// <returns>True if valid alphabetic string, False otherwise</returns>
        static public bool IsAlpha(this string strToCheck)
        {
            bool valid = true;

            if (strToCheck == "")
                return false;

            Regex objAlphaPattern = new Regex("[^a-zA-Z]");

            valid = !objAlphaPattern.IsMatch(strToCheck);
            return valid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strInteger"></param>
        /// <returns></returns>
        static public bool IsInteger(this string strInteger)
        {
            try
            {
                if (string.IsNullOrEmpty(strInteger))
                    return false;

                Convert.ToInt32(strInteger);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 타당한 이메일 주소인지 체크하는 함수. 
        /// Checks whether a valid Email address was input
        /// </summary>
        /// <param name="inputEmail">Email address to validate</param>
        /// <returns>True if valid, False otherwise</returns>
        static public bool IsEmail(this string inputEmail)
        {
            if (inputEmail != null && inputEmail != "")
            {
                //string strRegex = @"(?<user>[^@]+)@(?<host>.+)";
                //string strRegex = @"^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
                //string strRegex = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                
                Regex re = new Regex(strRegex);
                
                Boolean ismatch = Regex.IsMatch(inputEmail, strRegex);

                if (ismatch)
                    return true;
                else
                    return false;
            }
            else
                return (false);
        }

        /// <summary>
        /// 사업자 번호 체크
        /// </summary>
        /// <param name="biz_no"></param>
        /// <returns></returns>
        static public bool IsBusinessNumber(this string biz_no)
        {
            biz_no = biz_no.GetNumber();
            if (biz_no.Length != 10)
                return false;
            int[] weight = {1,3,7,1,3,7,1,3,5 };
            int result = 0;
            string[] biz_id = new string[10];
            for (int i = 0; i < 10; i++)
                biz_id[i] = biz_no.Substring(i, 1);
            int total = 0;
            for (int i=0; i<9; i++)
                total += int.Parse(biz_id[i]) * weight[i];
            total += (int.Parse(biz_id[8]) * 5 ) / 10;
            int check = total % 10;
            if (check == 0) result = 0;
            else result = 10 - check;
            if (result != int.Parse(biz_id[9]))
                return false;
            else
                return true;
        }

        /// <summary>
        /// 법인번호 체크
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static public bool LegalId(string number)
        {
            number = number.GetNumber();
            if (number.Length != 13)
                return false;
            int[] arr_rule = { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int total = 0;
            for (int i = 0; i < 12; i++)
                total += int.Parse(number.Substring(i, 1)) * arr_rule[i];
            if( (10 - (total % 10) ) ==  int.Parse( number.Substring(12,1)))
                return true;
            else return false;
        }

        #endregion      
    }
}
