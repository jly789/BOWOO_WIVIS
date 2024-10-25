using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Microsoft.Practices.EnterpriseLibrary.Data;

namespace bowoo.Framework.common
{
    [Serializable()]
    public class BaseEntity : Object
    {
        public BaseEntity()
        {
        }

        public static Dictionary<string, string> Lang_Set = new Dictionary<string, string>();

        public enum enmLoginResult
        {
            LoginSucess = 1,
            WrongPW = 2,
            WrongCn = 3,
            WrongID = 4,
        }

        /// <summary>
        /// Session Value
        /// </summary>
        public string test = string.Empty;
        public static string selectPrintvalue = string.Empty;
        public static string sessSID = string.Empty;
        public static string sessSName = string.Empty;
        public static int sessLv = 0;
        public static bool sessWrkStatus = false;
        public static string sessWrkMonitor = string.Empty;
        public static string sessInq = string.Empty;
        public static int sessWrkDelay = 0;
        public static string sessMenuName = string.Empty;

        //Set UI Common Value
        public static string SelectGridName = string.Empty;
        public static int visibleLoadingCount = 0;
        public static string beforeSMenu = string.Empty;
        public static bool hideLoading = false;
        public static bool IsFiredPrint = false;

        /// <summary>
        /// Session Value for Das
        /// </summary>
        public static int sessLineCount = 0;
        public static int sessController = 0;
        public static string sessPort = string.Empty;
        public static int sessPc = 0;
        public static string sessLocalIP = string.Empty;
        public static int sessmulti = 0;
        public static int sessBlocks = 0;
        public static int sessSCANNER = 0;
        public static string sessHANEDEX = string.Empty;
        public static int sessDISP = 0;
        public static int sessDISPL = 0;
        public static int sessDISPT = 0;
        public static int sessJBCOUNT = 0;
        public static int sessLCDFALSH = 0;
        public static int sessLCDLIGHT = 0;
        public static List<string> sessBrand = new List<string>();
        public static List<string> sessWHouse = new List<string>();
        public static string sessToday = string.Empty;
        public static string sessActiveForm = string.Empty;
    }
}
