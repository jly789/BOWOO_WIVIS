/*****************************************************************************
 * 역할 : Object의 확장 클래스
 *****************************************************************************/
 
using System.Text;
using System.Reflection;
using System;
using System.Text.RegularExpressions;


namespace lib.Common
{
    static public class ObjectEx
    {
        /// <summary>
        /// Object.ToString() 의 null 에러를 방지하기 위한 확장 메서드. 
        /// object x  = null;
        /// x.ToString();  <---- null 에러 발생. 
        /// 
        /// 응용 :
        /// 1. System.Data.DBNull 의 경우에 사용 가능.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public string ToStringSafe(this object obj)
        {
            return (obj ?? string.Empty).ToString();
        } 
                      
        
        /// <summary>
        /// Object를 Xml로 변환
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sb"></param>
        /// <remarks>
        /// </remarks>
        static public void ConvertClassToXml(this object obj, string rootTag, ref StringBuilder sb)
        {
            sb.Append(string.Format("<{0}>", rootTag));

            PropertyInfo[] piAry = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in piAry)
            {
                sb.Append(string.Format("<{0}>{1}</{0}>", pi.Name, pi.GetValue(obj, null)));
            }

            sb.Append(string.Format("</{0}>", rootTag));
        }


        /// <summary>
        /// Converts an Object to it's integer value
        /// </summary>
        /// <param name="ObjectToConvert"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public int ToInteger(this object ObjectToConvert)
        {
            try
            {
                return Convert.ToInt32(ObjectToConvert.ToString());
            }
            catch
            {
                throw new Exception("Object cannot be converted to Integer");
            }
        }

        /// <summary>
        /// Converts an Object to it's integer value
        /// </summary>
        /// <param name="ObjectToConvert"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public long ToLong(this object ObjectToConvert)
        {
            try
            {
                return Convert.ToInt64(ObjectToConvert.ToString());
            }
            catch
            {
                throw new Exception("Object cannot be converted to Long");
            }
        }

        /// <summary>
        /// Converts an Object to it's double value
        /// </summary>
        /// <param name="ObjectToConvert"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public double ToDouble(this object ObjectToConvert)
        {
            try
            {
                return Convert.ToDouble(ObjectToConvert.ToString());
            }
            catch
            {
                throw new Exception("Object cannot be converted to double");
            }
        }

        /// <summary>
        /// Converts an Object to it's decimal value
        /// </summary>
        /// <param name="ObjectToConvert"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public decimal ToDecimal(this object ObjectToConvert)
        {
            try
            {
                return Convert.ToDecimal(ObjectToConvert.ToString());
            }
            catch
            {
                throw new Exception("Object cannot be converted to decimal");
            }
        }

        /// <summary>
        /// Converts a string to a bool case
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public bool? ToBool(this object obj)
        {
            try
            {
                if(string.IsNullOrEmpty(obj.ToString()))
                    return null;
                else
                    return Convert.ToBoolean(obj.ToString());
            }
            catch(Exception ex)
            {
                throw new Exception("Object cannot be converted to Boolean" + ex.InnerException);
            }

        }

        /// <summary>
        /// Converts a Object to a DateTime case
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public DateTime ToDateTime(this object Object)
        {
            try
            {
                if( string.IsNullOrEmpty(Convert.ToString(Object)) )
                    return new DateTime();
                else
                    return Convert.ToDateTime(Convert.ToString(Object));
            }
            catch (Exception ex)
            {
                throw new Exception("Object cannot be converted to DateTime. Object: " + ex.InnerException);
            }
        }

        


        /// <summary>
        /// 문자열이 널인지 체크하고 널이면 빈문자열을 리턴합니다.
        /// </summary>
        /// <param name="obj">체크할 object</param>
        /// <returns>object.문자열 / string.Empty</returns>
        static public string NullCheck(object obj)
        {
            return ((obj == null) ? string.Empty : obj.ToString());
        }

    }
}
