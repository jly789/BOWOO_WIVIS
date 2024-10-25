using System;
using System.Collections.Generic;
using System.Globalization;

namespace lib.Common
{
    /// <summary>
    /// WeekOfYear에서 년도와 주를 표현하는 Data Object입니다.
    /// </summary>
    public class YearWeek : IEquatable<YearWeek> // ,ValueBase, IYearWeek
    {
        #region << Constructors >>

        public YearWeek() { }
        public YearWeek(int? year, int? week)
        {
            this.Year = year ?? 0;
            this.Week = week ?? 1;
        }
        //public YearWeek(IYearWeek src) : this(src.Year, src.Week) {}

        #endregion

        /// <summary>
        /// 년도
        /// </summary>
        public virtual int? Year { get; set; }
        /// <summary>
        /// 년도의 주 (Week)
        /// </summary>
        public virtual int? Week { get; set; }

        #region << Overrides >>

        public override int GetHashCode()
        {
            int hash = (Year ?? 0).GetHashCode();
            hash ^= 27 ^ (Week ?? 0).GetHashCode();

            return hash;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(YearWeek other)
        {
            if (other != null)
                return this.GetHashCode().Equals(other.GetHashCode());

            return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as YearWeek);
        }

        public override string ToString()
        {
            return string.Format("Year={0}, Week={1}", Year, Week);
        }

        #endregion
    }


    /// <summary>
    /// WeekOfYear 에 대한 Extension Methods 입니다.
    /// ref : http://www.simpleisbest.net/archive/2005/10/27/279.aspx
    /// ref : http://en.wikipedia.org/wiki/ISO_8601#Week_dates
    /// </summary>
    /// <remarks>
    /// "CalendarWeekRule" 값에 따라 WeekOfYear 가 결정된다.
    /// 
    /// FirstDay : 1월1일이 포함된 주를 무조건 첫째 주로 삼는다. (우리나라, 미국 등의 기준) : .NET의 설정대로 하면 이렇게 된다.
    /// FirstForDayWeek : 1월1일이 포함된 주가 4일 이상인 경우에만 그 해의 첫 번째 주로 삼는다.    (ISO 8601)
    ///                    예) 한 주의 시작 요일이 일요일이고 1월1일이 일/월/화/수 중 하나이면 1월1일이 포함된 주는 해당 해의 첫 번째 주이다.
    ///                    예) 한 주의 시작 요일이 일요일이고 1월1일이 목/금/토 중 하나이면 1월1일이 포함된 주는 해당 해의 첫 번째 주로 간주하지 않는다.
    ///                    예) 2005년 1월 1일은 토요일이므로 1월1일이 포함된 주는 2005년의 첫 번째 주로 간주하지 않는다.
    /// FirstFullWeek : 1월의 첫 번째 주가 7일이 아니면 해당 해의 첫 번째 주로 삼지 않는다.
    ///                 예) 한 주의 시작 요일이 일요일인 경우, 1월1일이 일요일이 아니라면 1월1일이 포함된 주는 해당 해의 첫 번째 주로 간주하지 않는다.
    /// </remarks> 
    static public class DateTimeEx
    {
		#region DateTime
		static public int ToIntMonth(this DateTime date)
		{
			return date.Year * 100 + date.Month;
		}

		static public int ToIntDay(this DateTime date)
		{
			return date.Year * 10000 + date.Month * 100 + date.Day;
		}
		#endregion
        
        #region << CalendarWeekRule >>

        public const string CalendarWeekRuleKey = "WeekOfYear.CalendarWeekRule";

        private static readonly IDictionary<CultureInfo, CalendarWeekRule> _calendarWeekRules = new Dictionary<CultureInfo, CalendarWeekRule>();

        /// <summary>
        /// Week Of Year 계산 시, 새해 첫째주에 대한 기본 산정 규칙을 환경설정 정보로부터 읽어서 반환한다. 
        /// 만약 환경설정에 WeekOfYear.CalendarWeekRule 정의되어 있지 않다면, <see cref="CultureInfo.DateTimeFormat"/>의 CalendarWeekRule 값을 사용한다.
        /// </summary>
        /// <example>
        ///   <code>
        ///     // App.config / Web.config
        ///     <appSettings>
        ///         <!-- 주차를 계산하는 로직을 선택합니다. System.Globalization.CalendarWeekRule [FirstDay|FirstFourDayWeek(ISO 8601)|FirstFullWeek]-->
        ///         <add key="WeekOfYear.CalendarWeekRule" value="FirstFourFullDay"/>
        ///     </appSettings>
        ///   </code>
        /// </example>
        /// <param name="culture">문화권, null이면 현재 문화권 사용</param>
        /// <returns>
        /// 환경설정에 WeekOfYear.CalendarWeekRule 키값에 
        /// 정의된 방식의 "CalendarWeekRule" 없으면 "CultureInfo.DateTimeFormat"의 CalendarWeekRule 값 반환
        /// </returns>
        /// <seealso cref="GetWeekOfYear(System.DateTime,System.Globalization.CultureInfo,System.Globalization.CalendarWeekRule)"/>
        /// <seealso cref="CalendarWeekRule"/>
        static public CalendarWeekRule GetDefaultCalendarWeekRule(this CultureInfo culture)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (_calendarWeekRules.ContainsKey(culture) == false)
            {
                lock (_calendarWeekRules)
                {
                    //object weekRule = AppConfig.GetAppSettings(CalendarWeekRuleKey);

                    object weekRule = "FirstFourDayWeek";
                    var s = Enum.Parse(typeof(CalendarWeekRule), weekRule.ToString(), true);
                    _calendarWeekRules.Add(culture, (CalendarWeekRule)s);
                }
            }

            return _calendarWeekRules[culture];
        }


        /// <summary>
        /// CalendarWeekRule 기본 설정이 "CalendarWeekRule.FirstDay"와 같은가?
        /// </summary>
        /// <returns></returns>
        static public bool IsDefaultCalenarWeekRuleIsFirstDay()
        {
            return GetDefaultCalendarWeekRule(CultureInfo.CurrentCulture) == CalendarWeekRule.FirstDay;
        }

        #endregion
        
        #region "GetOfDay"
        static public DateTime GetStartOfDay(this DateTime t)
        {
            return t;
        }

        static public DateTime GetEndOfDay(this DateTime t)
        {
            return t;
        }
        #endregion

        #region "GetOfWeek"
        static public DateTime GetStartOfWeek(this DateTime t)
        {
            return t;
        }

        static public DateTime GetEndOfWeek(this DateTime t)
        {
            return t;
        }
        #endregion

        #region "GetOfMonth"
        static public DateTime GetStartOfMonth(this DateTime t)
        {
            return t;
        }

        static public DateTime GetEndOfMonth(this DateTime t)
        {
            return t;
        }

        static public DateTime GetStartOfMonth(this int year, int month)
        {
            DateTime t = new DateTime(year, month, 1);

            return t;
        }

        static public DateTime GetEndOfMonth(this int year, int month)
        {
            DateTime t = new DateTime(year, month, 31);

            return t;
        }
        #endregion

        #region "GetOfYear"
        static public DateTime GetStartOfYear(this DateTime t)
        {
            return t;
        }

        static public DateTime GetEndOfYear(this DateTime t)
        {
            return t;
        }

        static public DateTime GetStartOfYear(this int year)
        {
            DateTime t = new DateTime(year, 1, 1);

            return t;
        }

        static public DateTime GetEndOfYear(this int year)
        {
            DateTime t = new DateTime(year, 12, 31);

            return t;
        }
        #endregion
    }
}