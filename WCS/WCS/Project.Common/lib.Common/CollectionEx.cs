using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.Reflection;


namespace lib.Common
{
    static public class CollectionEx
    {
		/// <summary>
		/// Dictionary의 형 변환
		/// (Page.Items)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dic"></param>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		static public T Key<T>(this IDictionary dic, object key, T defaultValue)
		{
			return GenericEx.ConvertTypeEx<T>(dic[key], defaultValue);
		}

		static public T Key<T>(this IDictionary dic, object key)
		{
			return GenericEx.ConvertType<T>(dic[key]);
		}
    }
}
