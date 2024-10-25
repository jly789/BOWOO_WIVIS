using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorer_Indicator_Contorl.DPSClass
{
    public class CellBtn_Class : indicator_Class
    {
        public int multi_no;
        public int remain_qty,item_qty,workIndex,box_no; // ord_Qty = 작업 지시수량 , status = 지시 유무 1: 작업지시함, 0: 작업 지시 안함.
        //remain_qty = 남은 작업 수량.
        public string barocde; //제품 바코드
        //workIndex 작업 순서 인덱스
    }
}
