using System;

namespace lib.Common
{
    /*****************************************************************************
     * 역할 : 컨트롤 관련 함수 모음.
     *****************************************************************************/ 
    static public class ClsGenericEx
    {
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
