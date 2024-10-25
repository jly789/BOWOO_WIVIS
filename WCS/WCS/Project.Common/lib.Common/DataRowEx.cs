using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime;
using System.Xml.Linq;
using System.Xml.XPath;

namespace lib.Common
{
    /*****************************************************************************
     * 역할 : DataTable 조작과 관련된 함수 모음.
     *****************************************************************************/
    static public class DataRowEx
    {                
		/// <summary>
		/// DataRow값 변환
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <param name="columnName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
        /// <remarks>
        /// </remarks>
		static public T Field<T>(this DataRow row, string columnName, T defaultValue)
		{
			if (row[columnName] is DBNull) return defaultValue;
			return GenericEx.ConvertTypeEx<T>(row[columnName], defaultValue);
		}

		/// <summary>
		/// DataRow값 변환
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <param name="columnName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
        /// <remarks>
        /// </remarks>
		static public T Field<T>(this DataRow row, int columnIndex, T defaultValue)
		{
			if (row[columnIndex] is DBNull) return defaultValue;
			return GenericEx.ConvertTypeEx<T>(row[columnIndex], defaultValue);
		}     

		/// <summary>
		/// DataRow값 변환
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <param name="columnName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		static public T Field<T>(this DataRowView row, string columnName, T defaultValue)
		{
			if (row[columnName] is DBNull) return defaultValue;
			return GenericEx.ConvertTypeEx<T>(row[columnName], defaultValue);
		}

		/// <summary>
		/// DataRow값 변환
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <param name="columnName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		static public T Field<T>(this DataRowView row, int columnIndex, T defaultValue)
		{
			if (row[columnIndex] is DBNull) return defaultValue;
			return GenericEx.ConvertTypeEx<T>(row[columnIndex], defaultValue);
		}
    }
}
