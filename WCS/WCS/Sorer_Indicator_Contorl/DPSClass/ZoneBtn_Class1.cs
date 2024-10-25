using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorer_Indicator_Contorl.DPSClass
{
    public class ZoneBtn_Class1
    {
        public string addr, loc_no, kit_type, description, display;
        public int line_no, zone_no, block_no, rack_no;

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
