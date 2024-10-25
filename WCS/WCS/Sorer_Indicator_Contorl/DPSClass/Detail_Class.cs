using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorer_Indicator_Contorl.DPSClass
{
    /// <summary>
    /// 오더 디테일 정보를 가지고 있는 클래스.
    /// </summary>
    public class Detail_Class
    {
        public string loc_no, addr ,ord_no ,work_seq,kit_type;
        public int maxzone, line_no, zone_no, block_no ,multI_no;
        public int ord_qty ,work_qty ,can_qty;
        public int status,snd_st;
        public string item_cd, item_nm;

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
