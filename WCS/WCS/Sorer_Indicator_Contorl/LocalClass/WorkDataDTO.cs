using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorer_Indicator_Contorl
{
    class WorkDataDTO
    {
        public string biz_day { get; set; }
        public string batch_no { get; set; }
        public string work_seq { get; set; }
        public string ord_no { get; set; }
        public string addr { get; set; }
        public string loc_no { get; set; }
        public string line_no { get; set; }
        public string zone_no { get; set; }
        public string rack_no { get; set; }
        public string block_no { get; set; }
        public string row_no { get; set; }
        public string col_no { get; set; }
        public string description { get; set; }
        public string kit_type { get; set; } // 슬라이딩인지 경량랙인지
        public string dork_no { get; set; }
        public string nowzone { get; set; }
        public string maxzone { get; set; }
        public string inbox_qty { get; set; }
        public string ord_qty { get; set; }
        public string wrk_qty { get; set; }
        public string can_qty { get; set; }
        public string do_status { get; set; }
        public string status { get; set; }
        public string snd_st { get; set; }
        public string prnt_status { get; set; }
        public string scan_status { get; set; }
        public string shop_nm { get; set; }
        public string item_cd { get; set; }
        public string item_nm { get; set; }
    }
}
