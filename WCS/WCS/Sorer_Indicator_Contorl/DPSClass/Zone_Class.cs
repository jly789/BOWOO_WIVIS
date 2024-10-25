using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorer_Indicator_Contorl.DPSClass
{
    public class Zone_Class
    {
        //로케이션 정보
        public int line_no;


        //오더정보
        //public Dictionary<string, Ord_Class> ord_List = new Dictionary<string, Ord_Class>();
        public List<Ord_Class> ord_List = new List<Ord_Class>();
        //public List<Block_Class> block_list = new List<Block_Class>();
        public Dictionary<int,Block_Class> block_list = new Dictionary<int, Block_Class>();





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
