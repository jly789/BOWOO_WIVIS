using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

namespace Sorer_Indicator_Contorl
{
    class Matching
    {
        /// <summary>
        /// 클래스에 오더,로케이션 등록하는 메소드
        /// </summary>
        /// <param name="work_dataSet"></param> 디비에서 셀렉트한 오더 정보,디테일정보,로케이션정보
        /// <param name="secondLineZoneNumber"></param> 2라인 1존의 존번호
        /// <param name="serch_indicator"></param> 로케이션 정보를 딕셔너리로 가지고 있음. 표시기에서 데이터 들어올때 쉽게 존,블럭 정보를 가져오기위함. 클래스 형태로 가지고 있음
        /// <param name="work_List"></param> 존별로 셀표시기에 지시한 정보를 가지고 있음. 해당 존 작업 완료 후 데이터를 뿌린 표시기만 LD를 보내기 위함.
        /// <param name="instructList"></param> 경량랙 멀티 작업을 하기위함. 작업 지시할때 멀티 정보를 딕셔너리에 저장 후 셀표시기에서 컨펌이 들어오면 해당 딕셔너리에서 데이터 select
        /// <returns></returns>
        public void initmatch(DataSet work_dataSet,           ref DPSClass.Sector_Class sectorClass,
                              ref int secondLineZoneNumber,   ref Dictionary<string, DPSClass.indicator_Class> serch_indicator,
                              ref List<Dictionary<int, List<DPSClass.Detail_Class>>> work_List,
                              ref Dictionary<string,List<DPSClass.Detail_Class>> instructList)
        {
            //DPSClass.Sector_Class sectorClass = new DPSClass.Sector_Class();
            
            //2라인 첫번째 존 정보 가져오기
            DataRow[] zone_select = work_dataSet.Tables[2].Select("line_no = 2", "zone_no asc");
            secondLineZoneNumber = Convert.ToInt32(zone_select[0]["zone_no"]);


            //존 클래스 매칭 ----------------------------------------------------------------------
            for (int i = 0; i < work_dataSet.Tables[3].Rows.Count; i++)
            {
                int tempLineNumber = (i + 1) < secondLineZoneNumber ? 1 : 2;
                DPSClass.Zone_Class tempZone = new DPSClass.Zone_Class();

                tempZone.line_no = tempLineNumber;
                sectorClass.zone_List.Add(tempZone);
                work_List.Add(new Dictionary<int, List<DPSClass.Detail_Class>>());//존 갯수 만큼 생성
            }


            //오더 매칭 ----------------------------------------------------------------------------
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();

            List<DPSClass.Ord_Class> tempOrdList = new List<DPSClass.Ord_Class>(); //오더 클레스와 디테일 클레스 매칭시키기 위함.
            match(work_dataSet, ref sectorClass);

            


            Stopwatch sw3 = new Stopwatch();
            sw3.Start();
            //블럭 클래스 초기화
            var result = work_dataSet.Tables[2].Select().AsEnumerable()
                        .GroupBy(r => new
                        {
                            gr = r["block_no"],
                            gr1 = r["zone_no"]
                        })
                        .Select(g => new
                        {
                            block_no = g.Key.gr,
                            zone_no = g.Key.gr1
                        });

            foreach (var data in result)
            {
                DPSClass.Block_Class tempBlock = new DPSClass.Block_Class();
                tempBlock.zone_no = Convert.ToInt32(data.zone_no);
                tempBlock.block_no = Convert.ToInt32(data.block_no);
                tempBlock.line_no = sectorClass.zone_List[tempBlock.zone_no - 1].line_no;
                sectorClass.zone_List[Convert.ToInt32(data.zone_no) - 1].block_list.Add(tempBlock.block_no, tempBlock);
                //work_dictionary[tempBlock.zone_no.To
                work_List[tempBlock.zone_no - 1].Add(tempBlock.block_no, new List<DPSClass.Detail_Class>());

            }

            //로케이션 테이블 데이터
            foreach (DataRow dr in work_dataSet.Tables[2].Rows)
            {
                if (dr["description"].ToString() == "CELL")
                {
                    DPSClass.CellBtn_Class tempCell = new DPSClass.CellBtn_Class();
                    tempCell.addr = dr["addr"].ToString();
                    tempCell.loc_no = dr["loc_no"].ToString();
                    tempCell.kit_type = dr["kit_type"].ToString();
                    tempCell.description = dr["description"].ToString();
                    tempCell.line_no = Convert.ToInt32(dr["line_no"]);
                    tempCell.zone_no = Convert.ToInt32(dr["zone_no"]);
                    tempCell.block_no = Convert.ToInt32(dr["block_no"]);
                    tempCell.rack_no = Convert.ToInt32(dr["rack_no"]);
                    tempCell.col_no = Convert.ToInt32(dr["col_no"]);
                    tempCell.row_no = Convert.ToInt32(dr["row_no"]);
                    tempCell.multi_no = Convert.ToInt32(dr["multi_no"]);
                    sectorClass.zone_List[tempCell.zone_no - 1].block_list[tempCell.block_no].cellBtn_List.Add(tempCell);
                    if(tempCell.multi_no == 1)
                    {
                        serch_indicator.Add(tempCell.addr, tempCell);
                        instructList.Add(tempCell.addr, new List<DPSClass.Detail_Class>());
                    }
                }
                else if (dr["description"].ToString() == "ZONE")
                {
                    DPSClass.ZoneBtn_Class tempZone = new DPSClass.ZoneBtn_Class();
                    tempZone.addr = dr["addr"].ToString();
                    tempZone.loc_no = dr["loc_no"].ToString();
                    tempZone.kit_type = dr["kit_type"].ToString();
                    tempZone.description = dr["description"].ToString();
                    tempZone.line_no = Convert.ToInt32(dr["line_no"]);
                    tempZone.zone_no = Convert.ToInt32(dr["zone_no"]);
                    tempZone.block_no = Convert.ToInt32(dr["block_no"]);
                    tempZone.rack_no = Convert.ToInt32(dr["rack_no"]);
                    tempZone.col_no = Convert.ToInt32(dr["col_no"]);
                    tempZone.row_no = Convert.ToInt32(dr["row_no"]);
                    sectorClass.zone_List[tempZone.zone_no - 1].block_list[tempZone.block_no].zoneBtn_List.Add(tempZone);
                    serch_indicator.Add(tempZone.addr, tempZone);
                    instructList.Add(tempZone.addr, new List<DPSClass.Detail_Class>());

                }
                else if (dr["description"].ToString() == "BCR")
                {
                    DPSClass.BcrBtn_Class tempBcr = new DPSClass.BcrBtn_Class();
                    tempBcr.addr = dr["addr"].ToString();
                    tempBcr.loc_no = dr["loc_no"].ToString();
                    tempBcr.kit_type = dr["kit_type"].ToString();
                    tempBcr.description = dr["description"].ToString();
                    tempBcr.line_no = Convert.ToInt32(dr["line_no"]);
                    tempBcr.zone_no = Convert.ToInt32(dr["zone_no"]);
                    tempBcr.block_no = Convert.ToInt32(dr["block_no"]);
                    tempBcr.rack_no = Convert.ToInt32(dr["rack_no"]);
                    tempBcr.col_no = Convert.ToInt32(dr["col_no"]);
                    tempBcr.row_no = Convert.ToInt32(dr["row_no"]);
                    sectorClass.zone_List[tempBcr.zone_no - 1].block_list[tempBcr.block_no].bcrBtn_List.Add(tempBcr);
                    serch_indicator.Add(tempBcr.addr, tempBcr);
                    instructList.Add(tempBcr.addr, new List<DPSClass.Detail_Class>());

                }
            }

            sw3.Stop();
            Console.WriteLine("로케이션 매칭 : " + sw3.ElapsedMilliseconds + "[ms]");
            //------------------------------------------------------------------------------------

            //return sectorClass;
        }

        public void match(DataSet work_dataSet, ref DPSClass.Sector_Class sectorClass)
        {
            List<DPSClass.Ord_Class> tempOrdList = new List<DPSClass.Ord_Class>(); //오더 클레스와 디테일 클레스 매칭시키기 위함.
            
            //오더 테이블 데이터
            foreach (DataRow dr in work_dataSet.Tables[0].Rows)
            {
                DPSClass.Ord_Class tempOrd = new DPSClass.Ord_Class();
                tempOrd.ord_no = dr["ord_no"].ToString();
                tempOrd.work_seq = dr["work_seq"].ToString();
                tempOrd.print_st = Convert.ToInt32(dr["prnt_status"]);
                tempOrd.scan_st = Convert.ToInt32(dr["scan_status"]);
                tempOrd.status = Convert.ToInt32(dr["status"]);
                tempOrd.nowzone = Convert.ToInt32(dr["nowzone"]);
                tempOrd.maxzone = Convert.ToInt32(dr["maxzone"]);
                tempOrd.dork_no = Convert.ToInt32(dr["dork_no"]);
                tempOrd.tns_code = dr["tns_code"].ToString();
                tempOrd.scan_time = dr["scan_time"].ToString();
                sectorClass.zone_List[tempOrd.nowzone - 1].ord_List.Add(tempOrd);
                tempOrdList.Add(tempOrd);
            }

            //디테일 매칭 ----------------------------------------------------------------------------


            ////디테일 테이블 데이터
            foreach (DataRow dr in work_dataSet.Tables[1].Rows)
            {
                DPSClass.Detail_Class tempDetail = new DPSClass.Detail_Class();
                tempDetail.loc_no = dr["loc_no"].ToString();
                tempDetail.item_cd = dr["item_cd"].ToString();
                tempDetail.item_nm = dr["item_nm"].ToString();
                tempDetail.ord_no = dr["ord_no"].ToString();
                tempDetail.addr = dr["addr"].ToString();
                tempDetail.kit_type = dr["kit_type"].ToString();
                tempDetail.work_seq = dr["work_seq"].ToString();
                tempDetail.maxzone = Convert.ToInt32(dr["maxzone"]);
                tempDetail.line_no = Convert.ToInt32(dr["line_no"]);
                tempDetail.zone_no = Convert.ToInt32(dr["zone_no"]);
                tempDetail.block_no = Convert.ToInt32(dr["block_no"]);
                tempDetail.multI_no = Convert.ToInt32(dr["MULTI_NO"]);
                tempDetail.ord_qty = Convert.ToInt32(dr["ord_qty"]);
                tempDetail.work_qty = Convert.ToInt32(dr["wrk_qty"]);
                tempDetail.can_qty = Convert.ToInt32(dr["can_qty"]);
                tempDetail.status = Convert.ToInt32(dr["status"]);
                tempDetail.snd_st = Convert.ToInt32(dr["snd_st"]);
                DPSClass.Ord_Class tempOrd = tempOrdList.Find(item => item.ord_no == tempDetail.ord_no);
                tempOrd.detail_List.Add(tempDetail);
            }
           

        }

        public void initmatch(DataSet work_dataSet, ref Dictionary<string, DPSClass.indicator_Class> serch_indicator)
        {
            

            //로케이션 테이블 데이터
            foreach (DataRow dr in work_dataSet.Tables[0].Rows)
            {
                if (dr["kit_type"].ToString() == "CELL")
                {
                    DPSClass.CellBtn_Class tempCell = new DPSClass.CellBtn_Class();
                    tempCell.addr = dr["addr"].ToString();
                    tempCell.loc_no = dr["loc_no"].ToString();
                    tempCell.kit_type = dr["kit_type"].ToString();
                    tempCell.description = dr["description"].ToString();
                    tempCell.line_no = Convert.ToInt32(dr["line_no"]);
                    tempCell.zone_no = Convert.ToInt32(dr["zone_no"]);
                    tempCell.block_no = Convert.ToInt32(dr["block_no"]);
                    tempCell.rack_no = Convert.ToInt32(dr["rack_no"]);
                    tempCell.col_no = Convert.ToInt32(dr["col_no"]);
                    tempCell.row_no = Convert.ToInt32(dr["row_no"]);
                    tempCell.multi_no = Convert.ToInt32(dr["multi_no"]);

                    if (tempCell.multi_no == 1)
                    {
                        serch_indicator.Add(tempCell.addr, tempCell);
                    }
                }
                else if (dr["kit_type"].ToString() == "ZONE")
                {
                    DPSClass.ZoneBtn_Class tempZone = new DPSClass.ZoneBtn_Class();
                    tempZone.addr = dr["addr"].ToString();
                    tempZone.loc_no = dr["loc_no"].ToString();
                    tempZone.kit_type = dr["kit_type"].ToString();
                    tempZone.description = dr["description"].ToString();
                    tempZone.line_no = Convert.ToInt32(dr["line_no"]);
                    tempZone.zone_no = Convert.ToInt32(dr["zone_no"]);
                    tempZone.block_no = Convert.ToInt32(dr["block_no"]);
                    tempZone.rack_no = Convert.ToInt32(dr["rack_no"]);
                    tempZone.col_no = Convert.ToInt32(dr["col_no"]);
                    tempZone.row_no = Convert.ToInt32(dr["row_no"]);
                    serch_indicator.Add(tempZone.addr, tempZone);

                }
                else if (dr["kit_type"].ToString() == "DIS16")
                {
                    DPSClass.Display16_Class tempZone = new DPSClass.Display16_Class();
                    tempZone.addr = dr["addr"].ToString();
                    tempZone.loc_no = dr["loc_no"].ToString();
                    tempZone.kit_type = dr["kit_type"].ToString();
                    tempZone.description = dr["description"].ToString();
                    tempZone.line_no = Convert.ToInt32(dr["line_no"]);
                    tempZone.zone_no = Convert.ToInt32(dr["zone_no"]);
                    tempZone.block_no = Convert.ToInt32(dr["block_no"]);
                    tempZone.rack_no = Convert.ToInt32(dr["rack_no"]);
                    tempZone.col_no = Convert.ToInt32(dr["col_no"]);
                    tempZone.row_no = Convert.ToInt32(dr["row_no"]);
                    serch_indicator.Add(tempZone.addr, tempZone);

                }
                else if (dr["kit_type"].ToString() == "BCR")
                {
                    DPSClass.BcrBtn_Class tempBcr = new DPSClass.BcrBtn_Class();
                    tempBcr.addr = dr["addr"].ToString();
                    tempBcr.loc_no = dr["loc_no"].ToString();
                    tempBcr.kit_type = dr["kit_type"].ToString();
                    tempBcr.description = dr["description"].ToString();
                    tempBcr.line_no = Convert.ToInt32(dr["line_no"]);
                    tempBcr.zone_no = Convert.ToInt32(dr["zone_no"]);
                    tempBcr.block_no = Convert.ToInt32(dr["block_no"]);
                    tempBcr.rack_no = Convert.ToInt32(dr["rack_no"]);
                    tempBcr.col_no = Convert.ToInt32(dr["col_no"]);
                    tempBcr.row_no = Convert.ToInt32(dr["row_no"]);
                    serch_indicator.Add(tempBcr.addr, tempBcr);

                }
            }

        }

       
    }
}
