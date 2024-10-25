using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace lib.Common.Management
{
    public class LanguagePack
    {
        static public string Language = "";
        public static DataTable csvDt = new DataTable();

        public LanguagePack()
        {
            // TODO: Complete member initialization
        }

        /// <summary>
        /// csv 파일 -> 데이터 테이블
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromCsvStream(string path)
        {
            csvDt.Reset();

            StreamReader file = new StreamReader(path, Encoding.Default);

            string[] headers = file.ReadLine().Split(',');

            foreach (string header in headers)
            {
                csvDt.Columns.Add(header);
            }

            while (!file.EndOfStream)
            {
                string[] rows = Regex.Split(file.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow drCsv = csvDt.NewRow();

                for (int i = 0; i < headers.Length; i++)
                {
                    drCsv[i] = rows[i];
                }

                csvDt.Rows.Add(drCsv);
            }

            file.Close();

            return csvDt;
        }

        public string Translate(string Korean)
        {
            string Trans = Korean;

            if (csvDt.Rows.Count > 0)
            {
                DataRow[] row = csvDt.Select(string.Format("한국어='{0}'", Korean.Replace("\r\n", "\\r\\n")));

                if (row.Length > 0)
                {
                    Trans = row[0][Language].ToString();
                }
            }

            return Trans;
        }
    }
}