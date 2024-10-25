using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Text;
using System.ComponentModel;


namespace lib.Common
{
    /*****************************************************************************
     * 역할 : DataTable 조작과 관련된 함수 모음.
     *****************************************************************************/ 

    static public class DataTableEx
    {        	
        #region DataTable 관련 Extension Method
        
        /// <summary>
        /// 응용: ComboBox의 첫번재 값을 Blank로 표시할 때 사용.
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public DataTable InsertBlankRow(this DataTable dtSource, int pos, string ColumName = null, string columnValue = null )
        {
                DataRow dRow      = dtSource.NewRow();

                foreach( DataColumn dCol in dtSource.Columns)
                {
                    if(dCol.DataType == System.Type.GetType("System.String") )
                        if(!string.IsNullOrEmpty(ColumName))
                            if( dCol.ColumnName == ColumName ) 
                                dRow[dCol.ColumnName] = columnValue;
                        else
                            dRow[dCol.ColumnName] = string.Empty;
                }

                if(dtSource.Rows.Count == 0)
                    return dtSource.InsertRow(dRow, pos); 
                else if( dtSource.Rows[0][0] == dRow[0] ) 
                        return dtSource;
                else
                    return dtSource.InsertRow(dRow, pos); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="pos"></param>
        /// <param name="hst"></param>
        /// <returns></returns>
        static public DataTable InsertBlankRow(this DataTable dtSource, int pos, Hashtable hst )
        {
                DataRow dRow      = dtSource.NewRow();
                foreach( DataColumn dCol in dtSource.Columns)
                {
                    if( hst.ContainsKey(dCol.ColumnName)) 
                        dRow[dCol.ColumnName] = hst[dCol.ColumnName];
                    else
                        dRow[dCol.ColumnName] = string.Empty;

                }

                if(dtSource.Rows.Count == 0)
                    return dtSource.InsertRow(dRow, pos); 
                else if( dtSource.Rows[0][0] == dRow[0] ) 
                        return dtSource;
                else
                    return dtSource.InsertRow(dRow, pos); 
        }
        
       
        /// <summary>
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="RemoveColumnName"></param>
        /// <returns>DataTable</returns>
        /// <remarks>
        /// </remarks>
        static public DataTable RemoveColumns(this DataTable dtSource,  string [] RemoveColumnName)
        {
            try
            {
                //뷰 생성
                for(int i =0 ; i<RemoveColumnName.Length; i++)
                {
                    if( dtSource.Columns.Contains(RemoveColumnName[i]) ) 
                    {
                        dtSource.Columns.Remove(RemoveColumnName[i]);
                    }
                } 
            }
            catch(Exception ex)
            {
                throw ex;
            }   
            
            return dtSource;
        } 
        
        /// <summary>
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="ExtractColumnName"></param>
        /// <returns>DataTable</returns>
        static public DataTable ExtractColumns(this DataTable dtSource,  string [] ExtractColumnName)
        {
            try
            {
                string [,] aColumnInfo = new string [ExtractColumnName.Length,2];
                
                for(int i =0; i<ExtractColumnName.Length; i++)
                {   
                    aColumnInfo[i,0] = ExtractColumnName[i].Trim();
                    aColumnInfo[i,1] = ExtractColumnName[i].Trim();                             
                }      
                         
                return  ExtractColumns(dtSource,  aColumnInfo);   
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        } 
        
        
        /// <summary>
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="aExtractColumnName"></param>
        /// <returns>DataTable</returns>
        /// <remarks>
        /// </remarks>
        static public DataTable ExtractColumns(this DataTable dtSource,  string [,] aExtractColumnName)
        {
            try
            {
                for(int j=0; j <  dtSource.Columns.Count; j++) 
                {
                    DataColumn dCol = dtSource.Columns[j];
                    
                    bool bRemove = true;
                    
                    //뷰 생성
                    for( int i =0  ; i<aExtractColumnName.Length/2; i++)
                    {
                        if( dCol.ColumnName.ToString().ToUpper() == aExtractColumnName[i,0].ToString().ToUpper() ) 
                        {
                            dCol.Caption = aExtractColumnName[i, 1];
                            bRemove = false;
                            break;
                        }
                    }
                    
                    if(bRemove) 
                    {
                        dtSource.Columns.Remove(dCol.ColumnName);
                        j--;
                    }
                }
                 
                for ( int i =0  ; i<aExtractColumnName.Length/2; i++)
                {
                    dtSource.Columns[aExtractColumnName[i,0]].SetOrdinal(i);
                }                     
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            //원하는 칼럼을 지정하는 겁니다.
            return dtSource;
        } 
        
        
        /// <summary>
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="aExtractColumnName"></param>
        /// <returns>DataTable</returns>
        /// <remarks>
        /// </remarks>
        static public DataTable DataColumnRemoveAndRename(this DataTable dtSource, string[,] aExtractColumnName)
        {
            try
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    DataColumn dCol = dtSource.Columns[j];

                    bool bRemove = true;

                    //뷰 생성
                    for (int i = 0; i < aExtractColumnName.Length / 2; i++)
                    {
                        if (dCol.ColumnName.ToString().ToUpper() == aExtractColumnName[i, 0].ToString().ToUpper())
                        {
                            dCol.ColumnName = aExtractColumnName[i, 1];
                            bRemove = false;
                            break;
                        }
                    }

                    if (bRemove)
                    {
                        dtSource.Columns.Remove(dCol.ColumnName);
                        j--;
                    }
                }

                for (int i = 0; i < aExtractColumnName.Length / 2; i++)
                {
                    dtSource.Columns[aExtractColumnName[i, 1]].SetOrdinal(i);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //원하는 칼럼을 지정하는 겁니다.
            return dtSource;
        }         
        
        /// <summary>
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public DataTable InsertRow(this DataTable dtSource, DataRow dRow , int pos )
        {
            try
            {
                dtSource.Rows.InsertAt(dRow, pos);
            }
            catch(Exception ex)
            {
                throw ex;
            }
   
            return dtSource;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        /// </remarks>
        static public DataTable InsertRow(this DataTable dtSource, string [] aColumns, int pos )
        {
            try
            {
                DataRow dRow      = dtSource.NewRow();
                
                for(int i =0; i< aColumns.Length ; i++)
                {
                    dRow[i]    = aColumns[i];
                }   
                
                dtSource.Rows.InsertAt(dRow, pos);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return dtSource;
        }
        
        /// <summary>
        /// DataTable에서 조건에 해당되는 데이터만으로 구성된 DataTable 만드는 함수.
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="RowFilter"></param>
        /// <param name="Sort"></param>
        /// <param name="ColumnNames">새로 만들어지는 DataTable에 들어가야할 DataColumn들.</param>
        /// <returns></returns>

        static public DataTable MakeNewDataTable(this DataTable dtSource,  string RowFilter, string Sort = null, string [] ColumnNames = null)
        {   
            //뷰 생성
            DataView view = null;
            
            try
            {
                view = dtSource.DefaultView;
        
                if(!string.IsNullOrEmpty(RowFilter)) view.RowFilter = RowFilter;
                if(!string.IsNullOrEmpty(Sort))      view.Sort      = Sort;                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            if(ColumnNames == null)
                return view.ToTable();
            else
                return view.ToTable(false, ColumnNames);
        } 

        /// <summary>
        /// 
        /// http://codecorner.galanter.net/2009/04/20/group-by-and-aggregates-in-net-datatable/
        /// </summary>
        /// <param name="i_sGroupByColumn"></param>
        /// <param name="i_sAggregateColumn"></param>
        /// <param name="i_dSourceTable"></param>
        /// <returns></returns>
        static public DataTable GroupByToTable(this DataTable SourceTable, string [] GroupByColumns, string RowFilter)
        {
	        return GroupByToTable(SourceTable, GroupByColumns, RowFilter, string.Empty);
        }

        /// <summary>
        /// 
        /// http://codecorner.galanter.net/2009/04/20/group-by-and-aggregates-in-net-datatable/
        /// </summary>
        /// <param name="i_sGroupByColumn"></param>
        /// <param name="i_sAggregateColumn"></param>
        /// <param name="i_dSourceTable"></param>
        /// <returns></returns>
        static public DataTable GroupByToTable(this DataTable SourceTable, string[] GroupByColumns, string RowFilter, string Sort)
        {
	        DataView dv = new DataView(SourceTable);
            dv.RowFilter = RowFilter;
            dv.Sort      = Sort;

	        //getting distinct values for group column  
	        DataTable dtGroup = dv.ToTable(true, GroupByColumns);

	        return dtGroup;
        }


        /// <summary>
        /// http://codecorner.galanter.net/2009/12/17/grouping-ado-net-datatable-using-linq/
        /// </summary>
        /// <param name="i_sGroupByColumn"></param>
        /// <param name="i_sAggregateColumn"></param>
        /// <param name="i_dSourceTable"></param>
        /// <returns></returns>
        static public DataTable GroupByLinq(string i_sGroupByColumn, string i_sAggregateColumn, DataTable i_dSourceTable)
        {
	        DataView dv = new DataView(i_dSourceTable);

	        //getting distinct values for group column  
	        DataTable dtGroup = dv.ToTable(true, new string[] { i_sGroupByColumn });

	        //adding column for the row count  
	        dtGroup.Columns.Add("Count", typeof(int));


	        //looping thru distinct values for the group, counting  
	        foreach (DataRow dr in dtGroup.Rows) 
	        {
		        dr["Count"] = i_dSourceTable.Compute("Count(" + i_sAggregateColumn + ")", i_sGroupByColumn + " = '" + dr[i_sGroupByColumn] + "'");

	        }

	        //returning grouped/counted result  
	        return dtGroup;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="keyColumn"></param>
        /// <returns></returns>
        static public Dictionary<string, Hashtable> DataTableToList(DataTable dt, string keyColumn)
        {
            Dictionary<string, Hashtable> rtn = new Dictionary<string, Hashtable>();

            foreach (DataRow row in dt.Rows)
            {
                rtn.Add(row[keyColumn].ToString(), DataRowToHashTable(dt.Columns, row));
            }

            return rtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static public List<Hashtable> DataTableToList(DataTable dt)
        {
            List<Hashtable> rtn = new List<Hashtable>();

            foreach (DataRow row in dt.Rows)
            {
                rtn.Add(DataRowToHashTable(dt.Columns, row));
            }

            return rtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        static public Hashtable DataRowToHashTable(DataColumnCollection cols, DataRow row)
        {
            Hashtable htRow = new Hashtable();
            foreach (DataColumn col in cols)
            {
                htRow.Add(col.ColumnName, row[col.ColumnName]);
            }
            return htRow;
        }
        #endregion        

        #region "클래스 객체를 DataTable 로 변환"

        /// <summary>
        /// Convert from Object To DataTable : 클래스 객체를 DataTable 로 변환
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        static public DataTable GetPropertiesOfClass(object obj)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Name");
            dt.Columns.Add("Value");
            dt.Columns.Add("Type");

            PropertyInfo[] piAry = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in piAry)
            {
                dt.Rows.Add(pi.Name, pi.GetValue(obj, null), pi.PropertyType.ToString().Replace("System.", string.Empty));
            }

            dt.Rows.Add("■■■■■■■■■■■■■■■■", "■■■■■■■", "■■■■■■■");

            FieldInfo[] fiAry = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo fi in fiAry)
            {
                dt.Rows.Add(fi.Name, fi.GetValue(obj), fi.FieldType.ToString().Replace("System.", string.Empty));
            }

            return dt;
        }      

        #endregion        


        #region "IEnumerable를 DataTable 로 변환"

		/// <summary>
		/// IEnumerable<T> -> DataTable
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
        /// <remarks>
        /// </remarks>
		static public DataTable GetPropertiesOfClass<T>(IEnumerable<T> objs)
		{
			IEnumerator<T> en = objs.GetEnumerator();

			if (!en.MoveNext())
				return null;

			DataTable dt = new DataTable();

			PropertyInfo[] piAry = en.Current.GetType().GetProperties();
			foreach (PropertyInfo pi in piAry)
			{
				dt.Columns.Add(string.Format("{0}[{1}]", pi.Name, pi.PropertyType.ToString().Replace("System.", string.Empty)), pi.PropertyType);
			}

			do
			{
				List<object> list = new List<object>();

				var obj = en.Current;

				piAry = obj.GetType().GetProperties();
				foreach (PropertyInfo pi in piAry)
					list.Add(pi.GetValue(obj, null));

				dt.Rows.Add(list.ToArray());

			} while (en.MoveNext());

			return dt;
		}
        #endregion


        #region "Convert DataTable"
        
        /// <summary>
        /// DataTable을 JSON String으로 변환하는 메서드
        /// </summary>
        /// <param name="Dt"></param>
        /// <returns></returns>
        static public string GetJSONString(DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "'" + StrDc[i] + "' : '" + StrDc[i] + i.ToString() + "¾" + "',";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();
            if (Dt.Rows.Count > 0)
            {
                Sb.Append("{Table:[");

                for (int i = 0; i < Dt.Rows.Count; i++)
                {

                    string TempStr = HeadStr;
                    Sb.Append("{");

                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {

                        TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
                    }

                    Sb.Append(TempStr + "},");
                }

                Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
                Sb.Append("]}");
            }

            return Sb.ToString().Replace("\n", "\\n");
        }

        /// <summary>
        /// DataTable을 JSON View으로 변환하는 메서드
        /// </summary>
        /// <param name="Dt"></param>
        /// <returns></returns>
        static public string GetJSONView(DataTable Dt)
        {

            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "'" + StrDc[i] + "' : '" + StrDc[i] + i.ToString() + "¾" + "',";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();
            if (Dt.Rows.Count > 0)
            {
                Sb.Append("{Table:");

                string TempStr = HeadStr;
                Sb.Append("{");

                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[0][j].ToString());
                }

                Sb.Append(TempStr + "},");

                Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
                Sb.Append("}");
            }

            return Sb.ToString().Replace("\n", "\\n");
        }

        /// <summary>
        /// DataTable 및 Hashtable DATA를 JSON String으로 변환하는 메서드
        /// </summary>
        /// <param name="Dt"></param>
        /// <returns></returns>
        static public string GetJSONString(DataTable Dt, Hashtable ht)
        {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += StrDc[i] + ": '" + StrDc[i] + i.ToString() + "¾" + "',";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();

            //Hashtable 사용하여 변수명/값 추가
            Sb.Append("{");

            foreach (DictionaryEntry de in ht)
            {
                Sb.Append(de.Key + " : '" + de.Value + "',");
            }

            if (Dt.Rows.Count > 0)
            {
                Sb.Append("Table : [");

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string TempStr = HeadStr;
                    Sb.Append("{");

                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
                    }

                    Sb.Append(TempStr + "},");
                }

                Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
                Sb.Append("]};");
            }
            else
            {
                Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 3));
                Sb.Append("};");
            }

            return Sb.ToString().Replace("\n", "\\n");
        }
        #endregion


        #region "Convert"
        /// <summary>
        /// 
        /// http://www.codeproject.com/Articles/157601/Working-with-Entities-instead-of-DataTable-Objects
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableRow"></param>
        /// <returns></returns>
        static public T ConvertToEntity<T>(this DataRow tableRow) where T : new()
        {
            // Create a new type of the entity I want
            Type t = typeof(T);
            T returnObject = new T();

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                string colName = col.ColumnName;

                // Look for the object's property with the columns name, ignore case
                PropertyInfo pInfo = t.GetProperty(colName.ToLower(),
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // did we find the property ?
                if (pInfo != null)
                {
                    object val = tableRow[colName];

                    // is this a Nullable<> type
                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            // Convert the db type into the T we have in our Nullable<T> type
                            val = Convert.ChangeType
			        (val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity
                        val = Convert.ChangeType(val, pInfo.PropertyType);
                    }
                    // Set the value of the property with the value from the db
                    pInfo.SetValue(returnObject, val, null);
                }
            }

            // return the entity object with values
            return returnObject;
        }

        /// <summary>
        /// 
        /// 
        /// DataTable dt = Dal.GetCompanies();
        /// List<Entities.Company> companyList = dt.ConvertToList<Entities.Company>();
        /// 
        /// http://www.codeproject.com/Articles/157601/Working-with-Entities-instead-of-DataTable-Objects
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        static public List<T> ConvertToList<T>(this DataTable table) where T : new()
        {
            Type t = typeof(T);

            // Create a list of the entities we want to return
            List<T> returnObject = new List<T>();

            // Iterate through the DataTable's rows
            foreach (DataRow dr in table.Rows)
            {
                // Convert each row into an entity object and add to the list
                T newRow = dr.ConvertToEntity<T>();
                returnObject.Add(newRow);
            }

            // Return the finished list
            return returnObject;
        }

        /// <summary>
        /// 
        /// 
        /// http://www.codeproject.com/Articles/157601/Working-with-Entities-instead-of-DataTable-Objects
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public DataTable ConvertToDataTable(this object obj)
        {
            // Retrieve the entities property info of all the properties
            PropertyInfo[] pInfos = obj.GetType().GetProperties();

            // Create the new DataTable
            var table = new DataTable();

            // Iterate on all the entities' properties
            foreach (PropertyInfo pInfo in pInfos)
            {
                // Create a column in the DataTable for the property
                table.Columns.Add(pInfo.Name, pInfo.GetType());
            }

            // Create a new row of values for this entity
            DataRow row = table.NewRow();
            // Iterate again on all the entities' properties
            foreach (PropertyInfo pInfo in pInfos)
            {
                // Copy the entities' property value into the DataRow
                row[pInfo.Name] = pInfo.GetValue(obj, null);
            }

            // Return the finished DataTable
            return table;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/9937573/convert-select-new-to-datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        static public DataTable ToDataTable<T>(IEnumerable<T> values)
        {
            DataTable table = new DataTable();

            foreach (T value in values)
            {
                if (table.Columns.Count == 0)
                {
                    foreach (var p in value.GetType().GetProperties())
                    {
                        table.Columns.Add(p.Name);
                    }
                }

                DataRow dr = table.NewRow();
                foreach (var p in value.GetType().GetProperties())
                {
                    dr[p.Name] = p.GetValue(value, null) + "";

                }
                table.Rows.Add(dr);
            }

            return table;
        }


        /// <summary>
        /// Converts a List to a datatable.
        /// http://lozanotek.com/blog/archive/2007/05/09/Converting_Custom_Collections_To_and_From_DataTable.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        static public DataTable ConvertTo<T>(IList<T> list)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = CreateTable<T>();

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// http://lozanotek.com/blog/archive/2007/05/09/Converting_Custom_Collections_To_and_From_DataTable.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        static public IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        /// <summary>
        /// http://lozanotek.com/blog/archive/2007/05/09/Converting_Custom_Collections_To_and_From_DataTable.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        static public IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        /// <summary>
        /// http://lozanotek.com/blog/archive/2007/05/09/Converting_Custom_Collections_To_and_From_DataTable.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        static public T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {
                        // You can log something here
                        throw;
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// http://lozanotek.com/blog/archive/2007/05/09/Converting_Custom_Collections_To_and_From_DataTable.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
        #endregion

        /// <summary>
        /// Selects specific number of rows from a datatable
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        static public DataTable SelectRows(this DataTable dataTable, int rowCount)
        {
            try
            {
                DataTable myTable = dataTable.Clone();
                DataRow[] myRows = dataTable.Select();
                for (int i = 0; i < rowCount; i++)
                {
                    if (i < myRows.Length)
                    {
                        myTable.ImportRow(myRows[i]);
                        myTable.AcceptChanges();
                    }
                }

                return myTable;

            }
            catch (Exception)
            {
                return new DataTable();
            }
        }
    }
}
