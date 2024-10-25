using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorer_Indicator_Contorl.DPSClass
{
    public class Block_Class
    {
        
        public int line_no, block_no ,zone_no;
        public List<CellBtn_Class> cellBtn_List = new List<CellBtn_Class>();
        public List<ZoneBtn_Class> zoneBtn_List = new List<ZoneBtn_Class>();
        public List<BcrBtn_Class> bcrBtn_List = new List<BcrBtn_Class>();


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
