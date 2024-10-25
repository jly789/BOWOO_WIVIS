using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorer_Indicator_Contorl.DPSClass
{
    public class Ord_Class : ICloneable
    {
        public string ord_no, work_seq;
        public int print_st, scan_st, status;
        public int nowzone ,maxzone ,dork_no;
        public string tns_code;//배송사 정보
        public string scan_time;
        public List<Detail_Class> detail_List = new List<Detail_Class>();



        public object Clone()
        {
            Ord_Class newclass = new Ord_Class();
            newclass.ord_no = this.ord_no;
            newclass.work_seq = this.work_seq;
            newclass.print_st = this.print_st;
            newclass.scan_st = this.scan_st;
            newclass.nowzone = this.nowzone;
            newclass.maxzone = this.maxzone;
            newclass.dork_no = this.dork_no;
            newclass.tns_code = this.tns_code;
            newclass.scan_time = this.scan_time;
            newclass.detail_List = this.detail_List;
            return newclass;
        }

        public string ReturnKeyValue(Type ClassType, string Key)
        {
            string ReturnKeyValue = string.Empty;

            if (!this.GetType().FullName.Equals(ClassType.FullName)) return string.Empty;

            foreach (var item in this.GetType().GetFields())
            {
                if (item.Name.Equals(Key))
                {
                    ReturnKeyValue = this.GetType().GetField(Key).GetValue(this).ToString();
                }
            }

            return ReturnKeyValue;
        }
    }
}
