using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib.Common
{
    static public class CodeConvert
    {
        /// <summary>
        /// http://msdn.microsoft.com/ko-kr/library/bb311038.aspx
        /// </summary>
        /// <param name="inputHexa"></param>
        /// <returns></returns>
        public static string EncodeHexa(this string input)
        {
            string reslut = string.Empty;

            char[] values = input.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);

                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{0:X}", value);
                
                reslut += hexOutput+":";
            }

            return reslut;
        }    

        /// <summary>
        /// http://msdn.microsoft.com/ko-kr/library/bb311038.aspx
        /// </summary>
        /// <param name="hexValues"></param>
        /// <returns></returns>
        public static string DecodeHexa(this string hexValues)
        {
            string reslut = string.Empty;

            string[] hexValuesSplit = hexValues.Split(':');
            foreach (String hex in hexValuesSplit)
            {
                if(!string.IsNullOrEmpty(hex))
                {
                    // Convert the number expressed in base-16 to an integer.
                    int value = Convert.ToInt32(hex, 16);
                    // Get the character corresponding to the integral value.
                    string stringValue = Char.ConvertFromUtf32(value);
                    char charValue = (char)value;

                    reslut += stringValue;

                    //Console.WriteLine("hexadecimal value = {0}, int value = {1}, char value = {2} or {3}", hex, value, stringValue, charValue);
                }
            }

            return reslut;
        }    

        public static string EncodeAscii(this string input)
        {
            string reslut = string.Empty;

            // Convert the string into a byte[].
            byte[] asciiBytes = Encoding.ASCII.GetBytes(input);

            char[] values = input.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                                
                reslut += value+":";

                //Console.WriteLine("Hexadecimal value of {0} is {1}", letter, hexOutput);
            }

            return reslut;
        }    

        public static string DecodeAscii(this string asciiValues)
        {
            string reslut = string.Empty;

            ASCIIEncoding ascii = new ASCIIEncoding();
                        
            string[] hexValuesSplit = asciiValues.Split(':');
            foreach (String hex in hexValuesSplit)
            {
                if(!string.IsNullOrEmpty(hex))
                {                    
                    
                    int value = Convert.ToInt32(hex);
                    //byte[] asciiStringArray = (char)value;
                    //string binaryStringArray = Encoding.ASCII.GetString(asciiStringArray,0,asciiStringArray.Length);

                    reslut += (char)value;

                    //Console.WriteLine("hexadecimal value = {0}, int value = {1}, char value = {2} or {3}", hex, value, stringValue, charValue);
                }
            }

            return reslut;
        }   
    }
}
