using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib.Common
{
    static public class GenericEx
    {
        #region Converting Methods

        /// <summary>
        /// 형변환
        /// </summary>
        /// <typeparam name="T">형식</typeparam>
        /// <param name="value">변환할 값</param>
        /// <returns></returns>
        static public T ConvertType<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 형변환(Nullable 처리 및 value == null 일 경우 기본값 리턴)
        /// </summary>
        /// <typeparam name="T">형식</typeparam>
        /// <param name="value">변환할 값</param>
        /// <param name="defaultValue">값이 null 일 경우 기본값</param>
        /// <returns></returns>
        static public T ConvertTypeEx<T>(object value, T defaultValue)
        {
            try
            {
                return value == null ? defaultValue : (T)Convert.ChangeType(value, GetConvertType(typeof(T)));
            }
            catch (FormatException)
            {
                // 값이 변환 불가능 할 경우 기본값을 넘김
                return defaultValue;
            }
        }

        #endregion


        #region Utility Methods

        /// <summary>
        /// type 의 Nullable 여부에 따라 Type를 리턴
        /// </summary>
        /// <param name="type">타입</param>
        /// <returns></returns>
        static public Type GetConvertType(Type type)
        {
            // Nullable 타입인 경우와 아닌 경우를 구분해서 변환
            return IsNullableType(type) ? (new System.ComponentModel.NullableConverter(type)).UnderlyingType : type;
        }

        /// <summary>
        /// Nullable 타입인지 확인
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        #endregion
    }
}
