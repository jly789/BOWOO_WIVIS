using bowoo.Framework.common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Xml;
using System.Reflection;
using static Telerik.WinControls.NativeMethods;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace lib.Common.Management
{

    public partial class UC01_GridView : BaseControl
    {
        public DataSet ds_Gird;
        public Dictionary<string, object> Usp_Load_Parameters = new Dictionary<string, object>();
        //Dictionary는 C#에서 키-값 쌍을 저장할 수 있는 자료구조입니다.
        //Usp_Load_Parameters는 저장 프로시저를 호출할 때 사용할 매개변수를 담는 Dictionary입니다.

        public Dictionary<string, object> Add_Data_Parameters = new Dictionary<string, object>();
        public Dictionary<string, Dictionary<string, string>> Add_Set_Data_Parameters = new Dictionary<string, Dictionary<string, string>>();
        public List<object> Add_Set_Data_List = null;
        public SqlParameter[] Usp_Save_Parameters = new SqlParameter[4];
        //SqlParameter 객체를 저장할 수 있는 배열을 선언하고 초기화하는 구문입니다. 이 배열은 SQL 저장 프로시저를 호출할 때, 매개변수들을 관리하고 전달하는 데 사용됩니다.
        //이 배열은 SQL 저장 프로시저에 여러 매개변수를 전달해야 할 때 사용됩니다. c#=>db로 매개변수 전달 용도


        //public SqlParameter[] Usp_Save_Parameters = new SqlParameter[5];

        GridViewSummaryItem summaryItem;
        private GridViewSummaryRowItem topSummaryRow;

        public RadCheckBox radChkRefresh;
        public Timer refreshTimer;

        string rawHeaderText = "";
        string tagText = "";
        string columnHeaderText = "";
        int tagStartIndex = -1;
        int tagEndIndex = -1;
        string[] splitedTagText;
        string[] tagKeyandValue;
        public string gridUspName = "";

        //조회용 텍스트 박스 생성 여부 (첫 조회 여부)
        bool isCreatedSearchTxt = false;

        Dictionary<string, object> _paramss;

        private static lib.Common.SuspendDrawingUpdate suspendLayout;

        #region * 프로퍼티 *
        //그리드 데이터 셋

        //-그리드뷰-
        //그리드 타이틀
        private string gridTitleText = "";
        public string GridTitleText { get { return gridTitleText; } set { gridTitleText = LanguagePack.Translate(value); this.radLblGridTitle.Text = LanguagePack.Translate(value); } }

        //그리드 수정 여부
        public bool GridAllowEditRow { get { return this.GridViewData.AllowEditRow; } set { this.GridViewData.AllowEditRow = value; } }
        //새 행 표현 여부
        public bool GridAllowAddNewRow { get { return this.GridViewData.AllowAddNewRow; } set { this.GridViewData.AllowAddNewRow = value; } }
        //그룹 행 표현 여부
        public bool GridShowGroupPanel { get { return this.GridViewData.ShowGroupPanel; } set { this.GridViewData.ShowGroupPanel = value; } }
        //다중 선택 여부
        public bool GridMultiSelect { get { return this.GridViewData.MultiSelect; } set { this.GridViewData.MultiSelect = value; } }

        //버튼 숨김 설정
        public bool Button1_Visible { set { this.radButton1.Visible = value; } }
        public bool Button2_Visible { set { this.radButton2.Visible = value; } }
        public bool Button3_Visible { set { this.radButton3.Visible = value; } }
        public bool Button4_Visible { set { this.radButton4.Visible = value; } }
        public bool Button5_Visible { set { this.radButton5.Visible = value; } }
        //버튼 enabled 설정
        public bool Button1_enabled { set { this.radButton1.Enabled = value; } }
        public bool Button2_enabled { set { this.radButton2.Enabled = value; } }
        public bool Button3_enabled { set { this.radButton3.Enabled = value; } }
        public bool Button4_enabled { set { this.radButton4.Enabled = value; } }
        public bool Button5_enabled { set { this.radButton5.Enabled = value; } }
        //버튼 텍스트
        public string Button1_Text { set { this.radButton1.Text = LanguagePack.Translate(value); } }
        public string Button2_Text { set { this.radButton2.Text = LanguagePack.Translate(value); } }
        public string Button3_Text { set { this.radButton3.Text = LanguagePack.Translate(value); } }
        public string Button4_Text { set { this.radButton4.Text = LanguagePack.Translate(value); } }
        public string Button5_Text { set { this.radButton5.Text = LanguagePack.Translate(value); } }
        //버튼 이벤트
        public EventHandler Button1_Click { set { this.radButton1.Click += new EventHandler(value); } }
        public EventHandler Button2_Click { set { this.radButton2.Click += new EventHandler(value); } }
        public EventHandler Button3_Click { set { this.radButton3.Click += new EventHandler(value); } }
        public EventHandler Button4_Click { set { this.radButton4.Click += new EventHandler(value); } }
        public EventHandler Button5_Click { set { this.radButton5.Click += new EventHandler(value); } }
        #endregion

        //생성자
        public UC01_GridView()
        {
            InitializeComponent();
            if (!Usp_Load_Parameters.ContainsKey("@HEADER_PARAMS")) Usp_Load_Parameters.Add("@HEADER_PARAMS", "");
            //Usp_Load_Parameters 와 Usp_Save_Parameters 차이
            //Usp_Load_Parameters는 저장 프로시저 호출 시 사용할 입력 매개변수들을 담고 있으며, 필요할 경우 키-값 쌍으로 매개변수를 추가합니다. 
            //데이터를 불러오기 위해 사용하는 매개변수

            //Usp_Save_Parameters는 저장 프로시저에 전달될 SqlParameter 객체들을 담고 있으며, 입력 매개변수(@HEADER_PARAMS, @DATA_PARAMS)와 출력 매개변수(@R_OK, @R_MESSAGE)를 설정합니다.
            //데이터를 저장하기 위해 사용하는 매개변수.

            //Usp_Load_Parameters는 우리가 데이터베이스에서 데이터를 읽어올 때 필요한 정보들을 담고 있어요.예를 들어, 화면에 표시할 데이터를 불러오거나, 특정 조건에 맞는 데이터를 가져오는 데 사용해요.
            //Usp_Save_Parameters는 우리가 데이터베이스에 데이터를 저장할 때 필요한 정보들을 담고 있어요.예를 들어, 사용자가 입력한 정보를 데이터베이스에 저장하거나, 수정한 내용을 반영할 때 사용해요.

            //비유하자면
            //Usp_Load_Parameters는 책장에서 책을 꺼낼 때 필요한 정보(어떤 책을 꺼낼지, 책의 위치 등)를 담는 가방이라고 생각하면 돼요.
            //Usp_Save_Parameters는 책장에 책을 넣을 때 필요한 정보(어떤 책을 어디에 넣을지, 책의 크기 등)를 담는 가방이에요.
            //둘 다 데이터베이스와 소통하기 위해 매개변수를 설정하는 것이지만, 하나는 불러오기용, 다른 하나는 저장하기용으로 역할이 다른 거예요.

            //Usp_Load_Parameters는 저장 프로시저를 호출할 때 사용할 매개변수를 담는 Dictionary입니다.

            if (!Usp_Load_Parameters.ContainsKey("@GRID_PARAMS")) Usp_Load_Parameters.Add("@GRID_PARAMS", "");
            //if (!Usp_Parameters.ContainsKey("@KEY_PARAMS")) Usp_Parameters.Add("@KEY_PARAMS", "");
            Usp_Save_Parameters[0] = new SqlParameter();
            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
            //첫 번째 매개변수 @HEADER_PARAMS는 헤더 데이터를 저장하는 데 사용됩니다.SqlParameter 객체를 생성하고 매개변수 이름과 값을 설정합니다.현재 값은 빈 문자열입니다.
            Usp_Save_Parameters[0].Value = "";
            Usp_Save_Parameters[1] = new SqlParameter();
            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
            //두 번째 매개변수 @DATA_PARAMS는 데이터와 관련된 정보를 저장하는 데 사용됩니다. SqlParameter 객체를 생성하고 이름과 값을 설정합니다. 현재는 빈 문자열로 초기화됩니

            Usp_Save_Parameters[1].Value = "";
            Usp_Save_Parameters[2] = new SqlParameter();
            Usp_Save_Parameters[2].ParameterName = "@R_OK";
            //@R_OK: 저장 프로시저가 성공적으로 수행되었는지를 확인하는 매개변수입니다.
            Usp_Save_Parameters[2].Size = 10;
            Usp_Save_Parameters[2].Value = "";
            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
            Usp_Save_Parameters[3] = new SqlParameter();
            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
            //@R_MESSAGE: 저장 프로시저 실행 시 메시지를 반환하는 매개변수입니다.
            Usp_Save_Parameters[3].Size = 8000;
            Usp_Save_Parameters[3].Value = "";
            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;

            // @R_OK와 @R_MESSAGE 매개변수는 출력 매개변수로 설정되었습니다. 이는 저장 프로시저가 실행된 후 반환되는 값을 받을 때 사용됩니다.

            //Usp_Save_Parameters[4] = new SqlParameter();
            //Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
            //Usp_Save_Parameters[4].Size = 8000;
            //Usp_Save_Parameters[4].Value = "";
            //Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

            //Collapsible Panel 테두리 선 없앰
            this.radCollapsiblePanel1.ControlsContainer.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

            this.radLayoutControl1.DrawBorder = false;

            //* 그리드뷰 기본 설정 *

            //새 행 추가 불가
            this.GridViewData.AllowAddNewRow = false;

            //그룹 행 숨김
            this.GridViewData.ShowGroupPanel = false;

            //행 다중 선택 가능
            this.GridViewData.MultiSelect = true;

            //Full Row Select
            this.GridViewData.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.FullRowSelect;

            //세로 스크롤 항상 표시
            this.GridViewData.MasterTemplate.VerticalScrollState = ScrollState.AlwaysShow;

            //행 크기 조절 불가
            this.GridViewData.AllowRowResize = true;

            //컬럼 자동 사이즈 조절
            this.GridViewData.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            //행 헤더 숨김
            this.GridViewData.ShowRowHeaderColumn = false;

            //새 행 텍스트
            this.GridViewData.MasterTemplate.NewRowText = LanguagePack.Translate("새 행을 추가하려면 이곳을 클릭하세요.");

            this.GridViewData.AllowDeleteRow = false;

            //기본으로 버튼 숨김
            this.radButton1.Visible = false;
            this.radButton2.Visible = false;
            this.radButton3.Visible = false;
            this.radButton4.Visible = false;
            this.radButton5.Visible = false;

            //그리드 뷰 이벤트
            this.GridViewData.ViewCellFormatting += GridViewData_ViewCellFormatting;
            this.radLblGridTitle.TextChanged += radLblGridTitle_TextChanged;

            this.GridViewData.ContextMenuOpening += GridViewData_ContextMenuOpening;
            this.GridViewData.RowFormatting += GridViewData_RowFormatting;

            //this.radButton1.TextChanged += radButton1_TextChanged;
        }

        #region 오른쪽 마우스 관련

        RadMenuItem menuItem1;
        RadMenuItem menuItem2;
        RadMenuItem menuItem3;
        RadMenuItem menuItem4;

        //오른쪽 마우스 오픈
        private void GridViewData_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            GridDataCellElement cell = e.ContextMenuProvider as GridDataCellElement;
            GridHeaderCellElement hcell = e.ContextMenuProvider as GridHeaderCellElement;

            //데이터영역 셀일 경우 
            if (cell != null)
            {
                foreach (object obj in e.ContextMenu.Items)
                {
                    RadMenuItem item = obj as RadMenuItem;

                    if (item != null)
                    {
                        switch (item.Text)
                        {
                            case "Copy":
                                item.Text = LanguagePack.Translate("복사");
                                break;

                            case "Delete":
                                item.Text = LanguagePack.Translate("삭제");
                                break;

                            case "Paste":
                                item.Text = LanguagePack.Translate("붙여넣기");
                                break;

                            case "Edit":
                                item.Text = LanguagePack.Translate("수정");
                                break;

                            case "Clear Value":
                                item.Text = LanguagePack.Translate("값 지우기");
                                break;

                            case "Delete Row":
                                item.Text = LanguagePack.Translate("행 삭제");
                                break;

                            default:
                                break;
                        }
                    }
                }

                //체크박스 컬럼 존재 하지 않을 경우 return
                if (this.GridViewData.Columns["CheckBox"] != null)
                {
                    //오른쪽 마우스 선택 반전, 전체 선택, 선택 행 체크 추가
                    menuItem1 = new RadMenuItem(LanguagePack.Translate("선택 영역 체크"), "MENU1");
                    menuItem2 = new RadMenuItem(LanguagePack.Translate("전체 체크"), "MENU2");
                    menuItem3 = new RadMenuItem(LanguagePack.Translate("체크 영역 반전"), "MENU3");
                    menuItem4 = new RadMenuItem(LanguagePack.Translate("체크 해제"), "MENU4");

                    menuItem1.Click += new EventHandler(menuItem_Click);
                    menuItem2.Click += new EventHandler(menuItem_Click);
                    menuItem3.Click += new EventHandler(menuItem_Click);
                    menuItem4.Click += new EventHandler(menuItem_Click);


                    e.ContextMenu.Items.Insert(0, menuItem2);//전체체크
                    e.ContextMenu.Items.Insert(1, menuItem1);//선택영역체크
                    e.ContextMenu.Items.Insert(2, menuItem3);//체크영역반전
                    e.ContextMenu.Items.Insert(3, menuItem4);//체크해제
                    e.ContextMenu.Items.Insert(4, new RadMenuSeparatorItem());
                }
            }

            if (hcell != null)
            {
                e.ContextMenu = null;
            }
        }

        //오른쪽 마우스 상자 아이템 클릭
        /// <summary>
        /// 체크박스 헤더 오른쪽 마우스 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_Click(object sender, EventArgs e)
        {
            RadMenuItem item = sender as RadMenuItem;

            if (item == null)
            {
                return;
            }

            CheckByCondition(item.Tag.ToString());
        }

        //전체체크, 선택반전, 체크 해제 ...
        private void CheckByCondition(string item_text)
        {
            switch (item_text)
            {
                case "MENU1":
                    //선택행 체크
                    for (int i = 0; i < GridViewData.Rows.Count; i++)
                    {
                        GridViewData.Rows[i].Cells["CheckBox"].Value = false;
                    }

                    for (int i = 0; i < GridViewData.SelectedRows.Count; i++)
                    {
                        GridViewData.SelectedRows[i].Cells["CheckBox"].Value = true;
                    }

                    break;

                case "MENU2":
                    //전체체크
                    for (int i = 0; i < GridViewData.Rows.Count; i++)
                    {
                        GridViewData.Rows[i].Cells["CheckBox"].Value = true;
                    }

                    break;

                case "MENU3":
                    //선택된 체크 반전
                    for (int i = 0; i < GridViewData.Rows.Count; i++)
                    {
                        GridViewData.Rows[i].Cells["CheckBox"].Value = (bool)GridViewData.Rows[i].Cells["CheckBox"].Value ? false : true;
                    }

                    break;

                case "MENU4":
                    //전체 체크 해제
                    for (int i = 0; i < GridViewData.Rows.Count; i++)
                    {
                        GridViewData.Rows[i].Cells["CheckBox"].Value = false;
                    }

                    break;

                default:
                    break;
            }
        }

        #endregion

        //타이틀 변경 시 
        void radLblGridTitle_TextChanged(object sender, EventArgs e)
        {
            this.radLblGridTitle.Text = gridTitleText + " <" + this.GridViewData.Rows.Count.ToString() + LanguagePack.Translate("건>");
        }

        public void SetGridTitleWithRowCount()
        {
            this.radLblGridTitle.Text = gridTitleText + " <" + this.GridViewData.Rows.Count.ToString() + LanguagePack.Translate("건>");
        }

        //조회조건 확장
        void radCollapsiblePanel1_Initialized(object sender, EventArgs e)
        {
            ((Telerik.WinControls.UI.CollapsiblePanelButtonElement)(this.radCollapsiblePanel1.GetChildAt(0).GetChildAt(1).GetChildAt(0))).Click += Collapse_Click;
        }

        //조회조건 확장
        /// <summary>
        /// Move the Location Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Collapse_Click(object sender, EventArgs e)
        {
            this.layoutControlItem3.MaxSize = this.radCollapsiblePanel1.IsExpanded ? new Size(layoutControlItem3.Size.Width, 30) : new Size(layoutControlItem3.Size.Width, 55);
            this.layoutControlItem3.MinSize = this.radCollapsiblePanel1.IsExpanded ? new Size(layoutControlItem3.Size.Width, 30) : new Size(layoutControlItem3.Size.Width, 55);

            if (this.radCollapsiblePanel1.IsExpanded)
            {
                bool isTxtExists = false;

                foreach (object item in this.radCollapsiblePanel1.Controls[0].Controls[0].Controls)
                {
                    RadTextBox txtitem = item as RadTextBox;

                    if (txtitem is RadTextBox)
                    {
                        if (!string.IsNullOrEmpty(txtitem.Text))
                        {
                            isTxtExists = true;
                        }

                        txtitem.Text = "";
                    }
                }

                if (isTxtExists)
                {
                    BindData(gridUspName, null);

                    if (this.GridViewData.Rows.Count > 0)
                    {
                        this.GridViewData.Rows[0].IsSelected = false;
                        this.GridViewData.Rows[0].IsSelected = true;
                    }
                }
            }

            else
            {
                //조회 조건 텍스트 박스 생성
                CreateSearchTextBox();
            }
        }

        // DECLARE @V_BIZ_DAY VARCHAR(10) = (SELECT VALUE1 FROM @TBL_DATA_PARAMS WHERE KEY_STR = 'BIZ_DAY') 구문은 
        //SQL 프로시저 내에서 데이터베이스에서 특정 값을 조회하는 것입니다.여기서 @V_BIZ_DAY 변수는 @TBL_DATA_PARAMS 테이블에서
        //KEY_STR이 'BIZ_DAY'인 행의 VALUE1 값을 가져와 할당받습니다.
        //이와 관련하여 C# 코드에서 ExcuteSaveSp 메소드는 저장 프로시저를 실행하고, SQL 서버에 매개변수를 전달하는 역할을 합니다. 구체적으로는:
        //헤더 파라미터와 데이터 파라미터를 설정합니다:
        //@HEADER_PARAMS: SQL 프로시저에 전달될 헤더 파라미터로, 데이터베이스에서 필요한 정보를 담고 있습니다.
        //@DATA_PARAMS: SQL 프로시저에 전달될 데이터 파라미터로, 특정 레코드에 대한 정보를 담고 있습니다.
        //GetDataParam 메소드를 통해 _dataRowIdx 인덱스에 해당하는 데이터 파라미터 값을 가져오고, 이를 SQL 프로시저에 전달합니다.
        //연관성SQL 프로시저와 C# 코드 모두 서로 연결되어 있습니다. C#에서 설정된 @DATA_PARAMS는 SQL 프로시저의 @DATA_PARAMS 매개변수와 동일하며, 
        //이는 SQL 프로시저 내에서 파싱되어 각 변수(@V_BIZ_DAY, 등)에 할당됩니다.
        //C# 코드에서 데이터 파라미터를 통해 SQL 프로시저로 필요한 정보를 전달하고, SQL 프로시저는 이를 기반으로 비즈니스 로직을 수행하며,
        //데이터베이스에서 필요한 값을 조회하거나 삭제합니다.
        //따라서 C# 코드와 SQL 프로시저는 서로 밀접하게 연결되어 있으며, 데이터 처리 흐름을 공유하고 있습니다. 
        //C#에서 전달된 데이터는 SQL 프로시저 내에서 사용되며, 각각의 목적에 맞게 조작됩니다.







        //데이터 바인딩
        /// <summary>
        /// 헤더테이블 및 데이터 테이블 그리드뷰에 조건에 맞게 바인딩
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paramss"></param>
        public void BindData(string spName, Dictionary<string, object> paramss)
        {
            if (this.GridViewData.IsInEditMode)
            {
                this.GridViewData.EndEdit();
            }

            //this.GridViewData.Visible = false;
            //this.GridViewData.MasterTemplate.ShowColumnHeaders = false;
            //this.GridViewData. = false;

            //SP 이름 저장
            if (string.IsNullOrWhiteSpace(gridUspName))
            {
                gridUspName = spName;
            }

            _paramss = paramss;

            //헤더 파라미터 설정
            if (_paramss == null)
            //_paramss가 null일 경우, Usp_Load_Parameters의 @HEADER_PARAMS 값에 BaseEntity.sessInq를 설정합니다.
            {
                Usp_Load_Parameters["@HEADER_PARAMS"] = BaseEntity.sessInq;

            }

            else
            //_paramss가 존재할 경우, BaseEntity.sessInq에 이어서 _paramss의 @HEADER_PARAMS 값을 추가하여 설정합니다.
            {
                Usp_Load_Parameters["@HEADER_PARAMS"] = BaseEntity.sessInq + ";#" + paramss["@HEADER_PARAMS"].ToString();
                //BaseEntity.sessInq는 세션 정보나 조회에 필요한 기본값을 나타내는 것으로 보입니다.
            }





            //조회 파라미터 설정
            Usp_Load_Parameters["@GRID_PARAMS"] = GetGridParams();


            //첫 번째 셀렉트 문은 헤더 두 번째는 데이터
            //데이터셋 초기화
            ds_Gird = new DataSet();

            //DateTime beforetime = DateTime.Now;
            //DB통신
            ds_Gird = DBUtil.ExecuteDataSet(gridUspName, Usp_Load_Parameters, CommandType.StoredProcedure);


            //DBUtil.ExecuteDataSet 메서드를 사용하여 저장 프로시저를 호출하고, 결과를 ds_Gird에 저장합니다.
            //gridUspName은 호출할 저장 프로시저의 이름이고, Usp_Load_Parameters는 저장 프로시저에 전달할 파라미터 컬렉션입니다.
            //CommandType.StoredProcedure는 이 쿼리가 저장 프로시저 호출임을 명시합니다.

            //DateTime aftertime = DateTime.Now;
            //Console.WriteLine(string.Format("[bind]시작시간 : {0} 초 {1} \r\n[bind]종료시간 : {2} 초 {3}", beforetime.Second.ToString(), beforetime.Millisecond.ToString(), aftertime.Second.ToString(), aftertime.Millisecond.ToString()));

            BindData(ds_Gird);

        }

        public void BindData(DataSet _ds_Gird)
        {

            //데이터 소스 초기화
            this.GridViewData.DataSource = null;
            this.GridViewData.Columns.Clear();

            //상위 요약 행 초기화
            topSummaryRow = new GridViewSummaryRowItem();
            this.GridViewData.SummaryRowsTop.Clear();

            //데이터 테이블 2개 존재 확인
            if (_ds_Gird == null || _ds_Gird.Tables.Count < 2)
            {
                return;
            }

            //헤더 테이블 데이터 테이블 컬럼 수 비교
            if (_ds_Gird.Tables[0].Columns.Count != _ds_Gird.Tables[1].Columns.Count)
            {
                //new lib.Common.Management.MessageForm().Show("헤더와 데이터 정보가 유효하지 않습니다.\r\n관리자에게 문의하세요.");
                BaseControl.bowooMessageBox.Show(LanguagePack.Translate("헤더와 데이터 정보가 유효하지 않습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //헤더 데이터 존재 확인
            if (_ds_Gird.Tables[0].Rows.Count < 1) return;

            //그리드 업데이트 시작 (레이아웃 멈춤)
            if (!this.GridViewData.MasterTemplate.IsUpdating)
            {
                this.GridViewData.MasterTemplate.BeginUpdate();
            }

            //데이터 테이블 바인딩
            this.GridViewData.DataSource = _ds_Gird.Tables[1];

            //타이틀 + 데이터 건수
            this.radLblGridTitle.Text = gridTitleText;

            //컬럼 별 설정
            for (int i = 0; i < _ds_Gird.Tables[0].Columns.Count; i++)
            {
                //기본 컬럼 읽기 전용
                this.GridViewData.Columns[i].ReadOnly = true;

                //기본 컬럼 리사이즈 불가
                this.GridViewData.Columns[i].AllowResize = true;

                // 컬럼을 이동 불가 로 변경
                this.GridViewData.Columns[i].AllowReorder = false;

                //헤더에서 태그추출
                rawHeaderText = _ds_Gird.Tables[0].Rows[0][i].ToString();

                //태그 <> 확인
                if (rawHeaderText.Contains('<') && rawHeaderText.Contains('>'))
                {
                    //태그 별 설정 메서드
                    Proc_Column_By_Tag(rawHeaderText, this.GridViewData.Columns[i]);
                }

                else
                {
                    //태그 없을 시 헤더 텍스트 설정
                    columnHeaderText = rawHeaderText;
                }

                //태그 제거된 헤더 택스트 설정
                this.GridViewData.Columns[i].HeaderText = LanguagePack.Translate(columnHeaderText);
            }

            //TYPE 별 컨트롤 BIND
            BindControlByType();

            //상위 요약 행 추가
            if (topSummaryRow.Count > 0)
            {
                this.GridViewData.SummaryRowsTop.Add(topSummaryRow);
            }

            //this.GridViewData.MasterTemplate.ShowColumnHeaders = true;
            //this.GridViewData.Visible = true;

            if (this.GridViewData.Columns.Contains("CheckBox"))
            {
                ConditionalFormattingObject co1 = new ConditionalFormattingObject("CheckedBox Color Change", ConditionTypes.Equal, "True", "", true);
                co1.RowBackColor = Color.FromArgb(213, 213, 213);
                co1.CellBackColor = Color.FromArgb(213, 213, 213);

                this.GridViewData.Columns["CheckBox"].ConditionalFormattingObjectList.Add(co1);
            }


            //그리드 end 업데이트 (변경 레이아웃 적용)
            if (this.GridViewData.MasterTemplate.IsUpdating)
            {
                this.GridViewData.MasterTemplate.EndUpdate();
            }

            //기본 컬럼 리사이즈 불가
            //this.GridViewData.AllowColumnResize = false;
        }

        public void BindData()
        {
            BindData(this.gridUspName, null);
        }

        //태그별 컬럼 설정
        /// <summary>
        /// 입력된 텍스트 값을 파라미터로 조회
        /// </summary>

        /// <summary>
        /// 태그 값에 따라 컬럼 설정 변경
        /// </summary>
        /// <param name="rawHeaderText"></param>
        /// <param name="column"></param>
        private void Proc_Column_By_Tag(string rawHeaderText, GridViewDataColumn column)
        {
            var checkboxColumn = column as GridViewCheckBoxColumn;

            if (checkboxColumn != null)
            {
                //체크 박스일 경우 정렬 금지
                column.AllowSort = false;
                column.ReadOnly = false;
            }

            //KEY, DATA 컬럼 여부 지정 위해 컬럼 태그 설정 
            column.Tag = new ParamType();
            var colTag = column.Tag as ParamType;

            //헤더 태그 시작 인덱스
            tagStartIndex = rawHeaderText.IndexOf('<');

            //헤더 태그 끝 인덱스
            tagEndIndex = rawHeaderText.IndexOf('>');

            //태그 추출
            tagText = rawHeaderText.Substring(tagStartIndex + 1, tagEndIndex - tagStartIndex - 1);

            //태그 삭제 후 헤더 텍스트 설정
            columnHeaderText = rawHeaderText.Remove(tagStartIndex, tagEndIndex - tagStartIndex + 1);

            //태그 텍스트 분할
            splitedTagText = tagText.Split(';');

            if (splitedTagText.Length > 0)
            {
                foreach (string _tagText in splitedTagText)
                {
                    //태그 값 존재 확인
                    if (!string.IsNullOrWhiteSpace(_tagText))
                    {
                        //'=' 구분된 태그 값 확인, 대문자로 변경
                        tagKeyandValue = _tagText.ToUpper().Split('=');

                        if (tagKeyandValue.Length > 1)
                        {
                            //sql tag용
                            if (tagKeyandValue.Length > 2)
                            {
                                for (int i = 2; i < tagKeyandValue.Length; i++)
                                {
                                    tagKeyandValue[1] += "=" + tagKeyandValue[i];
                                }
                            }

                            //조건 별 태그 실행
                            switch (tagKeyandValue[0])
                            {
                                //숨김 처리
                                case "ISVISIBLE":
                                    if (tagKeyandValue[1] == "FALSE")
                                    {
                                        column.IsVisible = false;
                                    }
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        column.IsVisible = true;
                                    }
                                    break;

                                //행 너비
                                case "WIDTH":
                                    int colWidth = 0;
                                    //열 설정이 보임이고, 태그 값 숫자 일시
                                    if (column.IsVisible && Int32.TryParse(tagKeyandValue[1], out colWidth))
                                    {
                                        column.Width = colWidth;
                                    }
                                    else if (column.IsVisible && tagKeyandValue[1] == "BESTFIT")
                                    {
                                        column.BestFit();
                                    }
                                    break;

                                //읽기 전용
                                case "READONLY":
                                    if (column.IsVisible && tagKeyandValue[1] == "FALSE")
                                    {
                                        column.ReadOnly = false;
                                    }
                                    break;

                                //데이터 영역 정렬
                                case "TEXTALIGN":
                                    if (column.IsVisible && tagKeyandValue[1] == "LEFT")
                                    {
                                        column.TextAlignment = ContentAlignment.MiddleLeft;
                                    }
                                    if (column.IsVisible && tagKeyandValue[1] == "CENTER")
                                    {
                                        column.TextAlignment = ContentAlignment.MiddleCenter;
                                    }
                                    if (column.IsVisible && tagKeyandValue[1] == "RIGHT")
                                    {
                                        column.TextAlignment = ContentAlignment.MiddleRight;
                                    }
                                    break;

                                //헤더 체크 박스 여부
                                case "HEADERCHECKBOX":
                                    if (column.IsVisible && checkboxColumn != null && tagKeyandValue[1] == "TRUE")
                                    {
                                        checkboxColumn.EnableHeaderCheckBox = true;
                                    }
                                    break;

                                //키 컬럼, 데이터 컬럼 여부 지정
                                case "KEY":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.Key = true;
                                    }
                                    break;

                                case "DATA":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.Data = true;
                                    }
                                    break;

                                case "FILE":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.File = true;
                                    }
                                    break;

                                //세로 병합 설정
                                case "MERGE":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        MergeVertically(column.Name);
                                        //MERGE 시 정렬 기능 불가
                                        this.GridViewData.EnableSorting = false;
                                    }
                                    break;

                                //Sum 추가
                                case "SUM":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.Sum = true;

                                        //Summary Row Add
                                        summaryItem = new GridViewSummaryItem();
                                        summaryItem.Name = column.Name;

                                        //summaryItem.FormatString = "SUM : {0}";
                                        summaryItem.Aggregate = GridAggregateFunction.Sum; //Sum
                                        topSummaryRow.Add(summaryItem);
                                    }
                                    break;

                                //COUNT 추가
                                case "COUNT":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.Count = true;

                                        //Summary Row Add
                                        summaryItem = new GridViewSummaryItem();
                                        summaryItem.Name = column.Name;

                                        //summaryItem.FormatString = "COUNT : {0}";
                                        summaryItem.Aggregate = GridAggregateFunction.Count; //Sum
                                        topSummaryRow.Add(summaryItem);
                                    }
                                    break;

                                case "STITLE":
                                    //Summary Row Add
                                    colTag.Title = true;
                                    summaryItem = new GridViewSummaryItem();
                                    summaryItem.Name = column.Name;
                                    summaryItem.FormatString = tagKeyandValue[1];
                                    summaryItem.Aggregate = GridAggregateFunction.Count;
                                    topSummaryRow.Add(summaryItem);
                                    break;

                                case "TYPE":
                                    colTag.BindType = tagKeyandValue[1];
                                    break;

                                case "SOURCETYPE":
                                    colTag.BindSourceType = tagKeyandValue[1];
                                    break;

                                case "SOURCE":
                                    colTag.BindSource = tagKeyandValue[1];
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void CreateSearchTextBox()
        {
            if (!isCreatedSearchTxt)
            {
                //텍스트 박스 높이
                int radTxtSearchHeight = 20;

                //왼쪽 첫 텍스트 박스 간격
                int leftRadTxtSearchWidth = 0;

                //상단 텍스트 박스 간격
                int topRadTxtSearchHeight = 0;

                //텍스트 박스 사이 간격
                int spaceRadtxtSearch = 1;

                //마지막 로케이션
                Point lastRadTxtSearchLocation = new Point(0, 0);

                if (ds_Gird == null)
                {
                    return;
                }

                for (int i = 0; i < ds_Gird.Tables[0].Columns.Count; i++)
                {
                    //VISIBLE 컬럼에 한해 조회 텍스트 박스 동적 생성 (태그 별 설정 메서드 이후)
                    if (this.GridViewData.Columns[i].IsVisible)
                    {
                        RadTextBox radTxtSearch = new RadTextBox();

                        radTxtSearch.Tag = this.GridViewData.Columns[i].Name;
                        radTxtSearch.Font = new Font("굴림체", 10);
                        radTxtSearch.Width = this.GridViewData.Columns[i].Width - spaceRadtxtSearch;
                        radTxtSearch.Height = radTxtSearchHeight;
                        radTxtSearch.Location = new Point(lastRadTxtSearchLocation.X + leftRadTxtSearchWidth, lastRadTxtSearchLocation.Y + topRadTxtSearchHeight);
                        radTxtSearch.KeyDown += new KeyEventHandler(radTxtSearch_KeyDown);

                        if (this.GridViewData.Columns[i].Name.ToUpper() != "CHECKBOX")
                        {
                            this.radCollapsiblePanel1.Controls.Add(radTxtSearch);
                        }

                        leftRadTxtSearchWidth = 0;
                        topRadTxtSearchHeight = 0;
                        lastRadTxtSearchLocation = new Point(radTxtSearch.Location.X + radTxtSearch.Width, radTxtSearch.Location.Y);
                    }
                }

                isCreatedSearchTxt = true;
            }
        }

        //데이터 셋 새로고침
        public void RefreshData()
        {
            //데이터셋 초기화
            //ds_Gird = new DataSet();

            //DateTime beforetime = DateTime.Now;

            //DB통신
            ds_Gird = DBUtil.ExecuteDataSet(gridUspName, Usp_Load_Parameters, CommandType.StoredProcedure);
            //DBUtil은 데이터베이스 작업을 수행하는 유틸리티 클래스입니다. 이 클래스는 데이터베이스와의 연결, 쿼리 실행, 결과 처리 등을 포함한 여러 메서드를 가질 수 있습니다.
            //ExecuteDataSet 메서드는 저장 프로시저를 실행하고, 그 결과를 DataSet 형식으로 반환하는 메서드입니다. 이 메서드는 주로 SQL 쿼리를 실행하거나 저장 프로시저를 호출할 때 사용됩니다.
            //gridUspName : 저장 프로시저의 이름을 나타내는 문자열 변수입니다. 이 이름을 사용하여 해당 저장 프로시저를 호출합니다.

            //Usp_Load_Parameters: 이 Dictionary는 저장 프로시저 호출 시 전달할 매개변수들을 포함합니다.
            //키는 매개변수 이름(예: @HEADER_PARAMS), 값은 해당 매개변수에 할당할 값입니다.
            //이를 통해 저장 프로시저가 데이터를 조회할 때 필요한 정보나 조건을 전달합니다.
            //CommandType.StoredProcedure는 이 쿼리가 단순한 SQL 문이 아니라 저장 프로시저임을 명시합니다.

            //데이터 테이블 바인딩
            //this.GridViewData.DataSource = ds_Gird.Tables[1];      
        }

        //저장 sp 실행
        public DataSet ExcuteSaveSp(string _saveSp, int _dataRowIdx)
        {
            Usp_Save_Parameters[0] = new SqlParameter();
            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
            Usp_Save_Parameters[0].DbType = DbType.String;
            Usp_Save_Parameters[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터
            Usp_Save_Parameters[1] = new SqlParameter();
            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
            Usp_Save_Parameters[1].DbType = DbType.String;
            Usp_Save_Parameters[1].Value = this.GetDataParam(_dataRowIdx); //데이터파라미터
            Usp_Save_Parameters[2] = new SqlParameter();
            Usp_Save_Parameters[2].ParameterName = "@R_OK";
            Usp_Save_Parameters[2].Size = 10;
            Usp_Save_Parameters[2].DbType = DbType.String;
            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
            Usp_Save_Parameters[3] = new SqlParameter();
            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
            Usp_Save_Parameters[3].Size = 8000;
            Usp_Save_Parameters[3].Value = "";
            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;
            //Usp_Save_Parameters[4] = new SqlParameter();
            //Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
            //Usp_Save_Parameters[4].Size = 8000;
            //Usp_Save_Parameters[4].Value = "";
            //Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

            DataSet r_ds = DBUtil.ExecuteDataSetSqlParam(_saveSp, Usp_Save_Parameters);

            //DBUtil.ExecuteDataSetSqlParam 메서드를 호출하여 저장 프로시저 _saveSp를 실행합니다.
            //Usp_Save_Parameters에 설정한 매개변수들을 저장 프로시저에 전달합니다.
            //이 메서드는 저장 프로시저 실행 후 결과를 DataSet 형식으로 반환하고, r_ds에 저장합니다.

            return r_ds;
        }

        public DataSet ExcuteSaveSpXml(string _saveSp, List<int> _dataRowIdx)
        {
            Usp_Save_Parameters[0] = new SqlParameter();
            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
            Usp_Save_Parameters[0].DbType = DbType.String;
            Usp_Save_Parameters[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터
            Usp_Save_Parameters[1] = new SqlParameter();
            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
            Usp_Save_Parameters[1].DbType = DbType.Xml;
            Usp_Save_Parameters[1].Value = this.GetDataParamXml(_dataRowIdx); //데이터파라미터
            Usp_Save_Parameters[2] = new SqlParameter();
            Usp_Save_Parameters[2].ParameterName = "@R_OK";
            Usp_Save_Parameters[2].Size = 10;
            Usp_Save_Parameters[2].DbType = DbType.String;
            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
            Usp_Save_Parameters[3] = new SqlParameter();
            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
            Usp_Save_Parameters[3].Size = 8000;
            Usp_Save_Parameters[3].Value = "";
            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;
            //Usp_Save_Parameters[4] = new SqlParameter();
            //Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
            //Usp_Save_Parameters[4].Size = 8000;
            //Usp_Save_Parameters[4].Value = "";
            //Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

            DataSet r_ds = DBUtil.ExecuteDataSetSqlParam(_saveSp, Usp_Save_Parameters);
            return r_ds;
        }

        public DataSet ExcuteSaveSpXml(string _saveSp, List<int> _dataRowIdx, List<int> _setDataRowIdx)
        {
            Usp_Save_Parameters[0] = new SqlParameter();
            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
            Usp_Save_Parameters[0].DbType = DbType.String;
            Usp_Save_Parameters[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터
            Usp_Save_Parameters[1] = new SqlParameter();
            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
            Usp_Save_Parameters[1].DbType = DbType.String;
            Usp_Save_Parameters[1].Value = this.GetDataParamXml(_dataRowIdx, _setDataRowIdx); //데이터파라미터
            Usp_Save_Parameters[2] = new SqlParameter();
            Usp_Save_Parameters[2].ParameterName = "@R_OK";
            Usp_Save_Parameters[2].Size = 10;
            Usp_Save_Parameters[2].DbType = DbType.String;


            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
            Usp_Save_Parameters[3] = new SqlParameter();
            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
            Usp_Save_Parameters[3].Size = 8000;
            Usp_Save_Parameters[3].Value = "";
            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;
            //Usp_Save_Parameters[4] = new SqlParameter();
            //Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
            //Usp_Save_Parameters[4].Size = 8000;
            //Usp_Save_Parameters[4].Value = "";
            //Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

            DataSet r_ds = DBUtil.ExecuteDataSetSqlParamXml(_saveSp, Usp_Save_Parameters);

            return r_ds;
        }

        //저장 sp 실행
        public DataSet ExcuteSaveSp(string _saveSp, List<int> checkedIdx)
        {
            Usp_Save_Parameters[0] = new SqlParameter();
            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
            Usp_Save_Parameters[0].DbType = DbType.String;
            Usp_Save_Parameters[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터
            Usp_Save_Parameters[1] = new SqlParameter();
            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
            Usp_Save_Parameters[1].DbType = DbType.String;

            foreach (int _dataRowIdx in checkedIdx)
            {
                Usp_Save_Parameters[1].Value += this.GetDataParam(_dataRowIdx);//데이터파라미터
            }

            Usp_Save_Parameters[2] = new SqlParameter();
            Usp_Save_Parameters[2].ParameterName = "@R_OK";
            Usp_Save_Parameters[2].Size = 10;
            Usp_Save_Parameters[2].DbType = DbType.String;
            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
            Usp_Save_Parameters[3] = new SqlParameter();
            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
            Usp_Save_Parameters[3].Size = 8000;
            Usp_Save_Parameters[3].Value = "";
            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;
            //Usp_Save_Parameters[4] = new SqlParameter();
            //Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
            //Usp_Save_Parameters[4].Size = 8000;
            //Usp_Save_Parameters[4].Value = "";
            //Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

            return DBUtil.ExecuteDataSetSqlParam(_saveSp, Usp_Save_Parameters);
        }

        //저장 sp 실행
        public DataSet ExcuteSaveSp(string _saveSp)
        {
            Usp_Save_Parameters[0] = new SqlParameter();
            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
            Usp_Save_Parameters[0].DbType = DbType.String;
            Usp_Save_Parameters[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터
            Usp_Save_Parameters[1] = new SqlParameter();
            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
            Usp_Save_Parameters[1].DbType = DbType.String;
            Usp_Save_Parameters[1].Value = GetDataParam(-1); //데이터파라미터
            Usp_Save_Parameters[2] = new SqlParameter();
            Usp_Save_Parameters[2].ParameterName = "@R_OK";
            Usp_Save_Parameters[2].Size = 10;
            Usp_Save_Parameters[2].DbType = DbType.String;
            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
            Usp_Save_Parameters[3] = new SqlParameter();
            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
            Usp_Save_Parameters[3].Size = 8000;
            Usp_Save_Parameters[3].Value = "";
            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;
            //Usp_Save_Parameters[4] = new SqlParameter();
            //Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
            //Usp_Save_Parameters[4].Size = 8000;
            //Usp_Save_Parameters[4].Value = "";
            //Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

            return DBUtil.ExecuteDataSetSqlParam(_saveSp, Usp_Save_Parameters);
        }

        //저장 sp 실행
        public DataSet ExcuteSaveSp(SqlTransaction tran, string _saveSp, int _dataRowIdx)
        {
            Usp_Save_Parameters[0] = new SqlParameter();
            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
            Usp_Save_Parameters[0].DbType = DbType.String;
            Usp_Save_Parameters[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터
            Usp_Save_Parameters[1] = new SqlParameter();
            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
            Usp_Save_Parameters[1].DbType = DbType.String;
            Usp_Save_Parameters[1].Value = this.GetDataParam(_dataRowIdx); //데이터파라미터
            Usp_Save_Parameters[2] = new SqlParameter();
            Usp_Save_Parameters[2].ParameterName = "@R_OK";
            Usp_Save_Parameters[2].Size = 10;
            Usp_Save_Parameters[2].DbType = DbType.String;
            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
            Usp_Save_Parameters[3] = new SqlParameter();
            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
            Usp_Save_Parameters[3].Size = 8000;
            Usp_Save_Parameters[3].Value = "";
            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;
            //Usp_Save_Parameters[4] = new SqlParameter();
            //Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
            //Usp_Save_Parameters[4].Size = 8000;
            //Usp_Save_Parameters[4].Value = "";
            //Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

            return bowoo.Lib.DataBase.SqlHelper.ExecuteDataset(tran, CommandType.StoredProcedure, _saveSp, Usp_Save_Parameters);
        }


        #region 컨트롤 바인딩 관련
        //타입별 컬럼 바인딩
        /// <summary>
        /// 바인드 타입별 컨트롤 바인딩
        /// </summary>
        private void BindControlByType()
        {
            for (int i = 0; i < this.GridViewData.Columns.Count; i++)
            {
                if ((this.GridViewData.Columns[i].Tag as ParamType).BindType == "")
                {
                    continue;
                }

                else
                {
                    string datecolname = this.GridViewData.Columns[i].FieldName;
                    int datecolidx = this.GridViewData.Columns[i].Index;

                    switch ((this.GridViewData.Columns[i].Tag as ParamType).BindType)
                    {
                        case "DATE":
                            //table for convert date column
                            DataTable dateConvertDt = new DataTable();

                            dateConvertDt = (this.GridViewData.DataSource as DataTable).Copy();
                            dateConvertDt.Columns.Remove(datecolname);
                            dateConvertDt.Columns.Add(datecolname, typeof(DateTime));

                            CultureInfo provider = CultureInfo.InvariantCulture;

                            string format = "yyyyMMdd";

                            for (int j = 0; j < dateConvertDt.Rows.Count; j++)
                            {
                                dateConvertDt.Rows[j][datecolname] = DateTime.ParseExact(ds_Gird.Tables[1].Rows[j][datecolname].ToString(), format, provider);
                            }

                            this.GridViewData.DataSource = dateConvertDt;

                            GridViewDateTimeColumn datecol = new GridViewDateTimeColumn(datecolname);
                            datecol.FormatString = "{0:yyyy-MM-dd}";
                            datecol.Format = DateTimePickerFormat.Custom;
                            datecol.CustomFormat = "yyyy-MM-dd";
                            datecol.HeaderText = this.GridViewData.Columns[i].HeaderText;
                            datecol.TextAlignment = this.GridViewData.Columns[i].TextAlignment;
                            datecol.Tag = this.GridViewData.Columns[i].Tag;
                            datecol.Width = this.GridViewData.Columns[i].Width;

                            //column swap
                            this.GridViewData.Columns.Remove(datecolname);
                            this.GridViewData.Columns.Insert(datecolidx, datecol);

                            break;

                        case "COMBO":
                            GridViewComboBoxColumn comboCol = new GridViewComboBoxColumn();

                            string bindSourceType = (this.GridViewData.Columns[i].Tag as ParamType).BindSourceType;
                            string bindSource = (this.GridViewData.Columns[i].Tag as ParamType).BindSource;

                            object r_comboBindSource = GetBindControlSource(bindSourceType, bindSource);

                            DataTable dtComboSource = r_comboBindSource as DataTable;

                            if (dtComboSource != null)
                            {
                                comboCol.ValueMember = dtComboSource.Columns[0].ColumnName;

                                if (dtComboSource.Columns.Count > 1)
                                {
                                    comboCol.DisplayMember = dtComboSource.Columns[1].ColumnName;
                                }
                            }

                            comboCol.DataSource = r_comboBindSource;
                            comboCol.HeaderText = this.GridViewData.Columns[i].HeaderText;
                            comboCol.TextAlignment = this.GridViewData.Columns[i].TextAlignment;
                            comboCol.Tag = this.GridViewData.Columns[i].Tag;
                            comboCol.Width = this.GridViewData.Columns[i].Width;
                            comboCol.DropDownStyle = RadDropDownStyle.DropDown;
                            comboCol.FieldName = datecolname;

                            //column swap
                            this.GridViewData.Columns.Remove(datecolname);
                            this.GridViewData.Columns.Insert(datecolidx, comboCol);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        //combobox열에 바인딩 될 소스를 가져온다 
        private object GetBindControlSource(string bindSourceType, string bindSource)
        {
            if (bindSourceType == "SP")
            {
                return DBUtil.ExecuteDataSet(bindSource, null, CommandType.StoredProcedure).Tables[0];
            }

            else if (bindSourceType == "SQL")
            {
                return DBUtil.ExecuteDataSet(bindSource).Tables[0];
            }

            else if (bindSourceType == "TEXT")
            {
                string[] textSource = bindSource.Split(',');

                return textSource;
            }

            else
            {
                return null;
            }
        }
        #endregion

        #region 파라미터 생성 관련
        //자식 그리드 키 파라미터 생성
        /// <summary>
        /// 상세 그리드로 키 값을 전달하기 위해 파라미터 생성
        /// </summary>
        public string GetKeyParam()
        {
            string KeyParams = "";

            if (this.GridViewData.SelectedRows.Count > 0)
            {
                //int selectedIndex = this.GridViewData.SelectedRows[0].Index;
                foreach (GridViewDataColumn col in this.GridViewData.Columns)
                {
                    if (col.Tag is ParamType && (col.Tag as ParamType).Key)
                    {
                        //KeyParams += col.Name + "=" + this.GridViewData.Rows[selectedIndex].Cells[col.Name].Value.ToString() + ";#";
                        KeyParams += col.Name + "=" + this.GridViewData.SelectedRows[0].Cells[col.Name].Value.ToString() + ";#";
                    }
                }
            }

            return KeyParams;
        }

        //프로시저용 데이터 파라미터 생성
        /// <summary>
        /// 해당 행의 데이터 파라미터 생성
        /// </summary>
        /// <returns></returns>
        public string GetDataParam(int rowIndex)
        {
            string DataParams = "";

            if (this.GridViewData.Rows.Count > 0 && rowIndex > -1)
            {
                foreach (GridViewDataColumn col in this.GridViewData.Columns)
                {
                    if (col.Tag is ParamType && (col.Tag as ParamType).Data)
                    {

                        DataParams += col.Name + "=" + this.GridViewData.Rows[rowIndex].Cells[col.Name].Value.ToString() + ";#";
                    }
                }
            }

            //DATA Param 추가
            if (Add_Data_Parameters.Count > 0)
            {
                foreach (string key in Add_Data_Parameters.Keys)
                {
                    DataParams += key + "=" + Add_Data_Parameters[key].ToString() + ";#";
                }
            }


            return DataParams;
        }

        //xml 데이터
        public string GetDataParamXml(List<int> rowIndex)
        {
            XmlDocument xdoc = new XmlDocument();

            // 루트노드
            XmlNode root = xdoc.CreateElement("root");
            xdoc.AppendChild(root);

            //자식 노드
            XmlNode[] childNode = new XmlNode[rowIndex.Count];

            //셋팅값 노드
            XmlNode[] setValueNode = new XmlNode[Add_Data_Parameters.Count];
            int set_count = 0;

            string DataParams = "";
            string columnName = string.Empty;
            string columnValue = string.Empty;

            if (this.GridViewData.Rows.Count > 0)
            {
                for (int i = 0; i < rowIndex.Count; i++)
                {
                    //데이터 집합 로드
                    XmlNode data = xdoc.CreateElement("data");
                    root.AppendChild(data);
                    for (int j = 0; j < this.GridViewData.ColumnCount; j++)
                    {
                        //is는 형변환이 가능한지 여부 판단, as는 형변환이 안될경우 null값 반환
                        if (this.GridViewData.Columns[j].Tag is ParamType && (this.GridViewData.Columns[j].Tag as ParamType).Data)
                        {
                            columnName = this.GridViewData.Columns[j].Name;
                            columnValue = this.GridViewData.Rows[rowIndex[i]].Cells[j].Value.ToString();
                            //자식 노드 태그명 설정
                            childNode[i] = xdoc.CreateElement(columnName);
                            //자식 노드 이너 값 설정
                            childNode[i].InnerText = columnValue;
                            data.AppendChild(childNode[i]);

                        }
                    }

                    if (Add_Set_Data_List != null)
                    {
                        //셋팅을 해야하는 데이터 값
                        if (Add_Set_Data_List.Count > 0)
                        {
                            PropertyInfo[] piAry = Add_Set_Data_List[i].GetType().GetProperties();
                            foreach (PropertyInfo pi in piAry)
                            {
                                //sb.Append(string.Format("<{0}>{1}</{0}>", pi.Name, pi.GetValue(obj, null)));
                                columnName = pi.Name;
                                columnValue = pi.GetValue(Add_Set_Data_List[i], null).ToString();
                                //자식 노드 태그명 설정
                                childNode[i] = xdoc.CreateElement(columnName);
                                //자식 노드 이너 값 설정
                                childNode[i].InnerText = columnValue;
                                data.AppendChild(childNode[i]);
                            }

                        }
                    }
                }

            }

            //DATA Param 추가
            if (Add_Data_Parameters.Count > 0)
            {
                foreach (string key in Add_Data_Parameters.Keys)
                {
                    //셋팅값 집합 로드
                    XmlNode setValueData = xdoc.CreateElement("setValueData");
                    root.AppendChild(setValueData);
                    //자식노드 태그명 설정
                    setValueNode[set_count] = xdoc.CreateElement(key);
                    //자식노드 이너값 값정
                    setValueNode[set_count].InnerText = Add_Data_Parameters[key].ToString();
                    setValueData.AppendChild(setValueNode[set_count]);
                    //DataParams += key + "=" + Add_Data_Parameters[key].ToString() + ";#";
                    set_count++;
                }
            }

            xdoc.Save(@"d:\Emp.xml");
            string retrunString = xdoc.OuterXml.ToString();
            return retrunString;
        }

        //xml 데이터
        public string GetDataParamXml(List<int> rowIndex, List<int> setRowIndex)
        {
            XmlDocument xdoc = new XmlDocument();

            // 루트노드
            XmlNode root = xdoc.CreateElement("root");
            xdoc.AppendChild(root);

            //자식 노드
            XmlNode[] childNode = new XmlNode[rowIndex.Count];

            //셋팅값 노드
            XmlNode[] setValueNode = new XmlNode[Add_Data_Parameters.Count];
            int set_count = 0;

            string DataParams = "";
            string columnName = string.Empty;
            string columnValue = string.Empty;

            if (this.GridViewData.Rows.Count > 0)
            {
                for (int i = 0; i < rowIndex.Count; i++)
                {
                    //데이터 집합 로드
                    XmlNode data = xdoc.CreateElement("data");
                    root.AppendChild(data);
                    for (int j = 0; j < this.GridViewData.ColumnCount; j++)
                    {
                        //is는 형변환이 가능한지 여부 판단, as는 형변환이 안될경우 null값 반환
                        if (this.GridViewData.Columns[j].Tag is ParamType && (this.GridViewData.Columns[j].Tag as ParamType).Data)
                        {
                            columnName = this.GridViewData.Columns[j].Name;
                            columnValue = this.GridViewData.Rows[i].Cells[j].Value.ToString();
                            //자식 노드 태그명 설정
                            childNode[i] = xdoc.CreateElement(columnName);
                            //자식 노드 이너 값 설정
                            childNode[i].InnerText = columnValue;
                            data.AppendChild(childNode[i]);

                        }
                    }

                    //셋팅을 해야하는 데이터 값
                    if (Add_Set_Data_List.Count > 0)
                    {
                        PropertyInfo[] piAry = Add_Set_Data_List[i].GetType().GetProperties();
                        foreach (PropertyInfo pi in piAry)
                        {
                            //sb.Append(string.Format("<{0}>{1}</{0}>", pi.Name, pi.GetValue(obj, null)));
                            columnName = pi.Name;
                            columnValue = pi.GetValue(Add_Set_Data_List[i], null).ToString();
                            //자식 노드 태그명 설정
                            childNode[i] = xdoc.CreateElement(columnName);
                            //자식 노드 이너 값 설정
                            childNode[i].InnerText = columnValue;
                            data.AppendChild(childNode[i]);
                        }

                    }
                }
            }
            //if (this.GridViewData.Rows.Count > 0 && rowIndex > -1)
            //{
            //    foreach (GridViewDataColumn col in this.GridViewData.Columns)
            //    {
            //        if (col.Tag is ParamType && (col.Tag as ParamType).Data)
            //        {

            //            DataParams += col.Name + "=" + this.GridViewData.Rows[rowIndex].Cells[col.Name].Value.ToString() + ";#";
            //        }
            //    }
            //}

            //DATA Param 추가
            if (Add_Data_Parameters.Count > 0)
            {
                foreach (string key in Add_Data_Parameters.Keys)
                {
                    //셋팅값 집합 로드
                    XmlNode setValueData = xdoc.CreateElement("setValueData");
                    root.AppendChild(setValueData);
                    //자식노드 태그명 설정
                    setValueNode[set_count] = xdoc.CreateElement(key);
                    //자식노드 이너값 값정
                    setValueNode[set_count].InnerText = Add_Data_Parameters[key].ToString();
                    setValueData.AppendChild(setValueNode[set_count]);
                    //DataParams += key + "=" + Add_Data_Parameters[key].ToString() + ";#";
                    set_count++;
                }
            }

            xdoc.Save(@"d:\Emp.xml");
            string retrunString = xdoc.OuterXml.ToString();
            return retrunString;
        }

        //조회조건 파라미터 생성
        /// <summary>
        /// 그리드 위 조회조건 텍스트 박스의 파라미터를 생성
        /// </summary>
        /// <returns></returns>
        public string GetGridParams()
        {
            string gridParams = "";

            foreach (Control control in this.radCollapsiblePanel1.Controls[0].Controls[0].Controls)
            {
                if (control is RadTextBox)
                {
                    String test = ((RadTextBox)control).Tag.ToString();
                    if (((RadTextBox)control).Text != "")
                    {
                        gridParams += ((RadTextBox)control).Tag.ToString() + "=" + ((RadTextBox)control).Text + ";#";
                    }

                }
            }

            return gridParams;
        }
        #endregion

        #region MERGE 기능 관련
        //세로 MERGE 기능
        /// <summary>
        /// MERGE CELL Vertically
        /// </summary>
        /// <param name="radGridView"></param>
        /// <param name="startColumnIndex"></param>
        /// <param name="endColumnIndex"></param>
        public void MergeVertically(string columnName)
        {
            GridViewRowInfo Prev = null;

            foreach (GridViewRowInfo item in this.GridViewData.Rows)
            {
                if (Prev != null)
                {
                    string firstCellText = string.Empty;
                    string secondCellText = string.Empty;

                    GridViewCellInfo firstCell = Prev.Cells[columnName];
                    GridViewCellInfo secondCell = item.Cells[columnName];

                    firstCellText = (firstCell != null && firstCell.Value != null ? firstCell.Value.ToString() : string.Empty);
                    secondCellText = (secondCell != null && secondCell.Value != null ? secondCell.Value.ToString() : string.Empty);

                    setCellBorders(firstCell, Color.FromArgb(209, 225, 245));
                    setCellBorders(secondCell, Color.FromArgb(209, 225, 245));

                    if (firstCellText == secondCellText)
                    {
                        firstCell.Style.BorderBottomColor = Color.Transparent;
                        secondCell.Style.BorderTopColor = Color.Transparent;
                        secondCell.Style.ForeColor = Color.Transparent;
                        //secondCell.Style.BorderBottomColor = Color.FromArgb(209, 225, 245);
                    }

                    else
                    {
                        secondCell.Style.BorderTopColor = Color.FromArgb(209, 225, 245);
                        secondCell.Style.BorderBottomColor = Color.FromArgb(209, 225, 245);
                        //secondCell.Style.BorderTopColor = Color.FromArgb(49, 55, 255);
                        //secondCell.Style.BorderBottomColor = Color.FromArgb(49, 55, 255);
                        secondCell.Style.ForeColor = Color.Black;

                        Prev = item;

                        continue;
                    }
                }

                else
                {
                    Prev = item;
                }
            }
        }

        //세로 MERGE 기능
        /// <summary>
        /// 셀 테두리 설정
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="color"></param>
        private void setCellBorders(GridViewCellInfo cell, Color color)
        {
            cell.Style.CustomizeBorder = true;
            cell.Style.BorderBoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
            cell.Style.BorderLeftColor = color;
            cell.Style.BorderRightColor = color;

            if (cell.RowInfo.Index == 0)
            {
                cell.Style.BorderTopColor = color;
            }

            if (cell.RowInfo.Index == this.GridViewData.Rows.Count - 1)
            {
                cell.Style.BorderBottomColor = color;
            }
        }
        #endregion

        //조회조건 엔터
        private void radTxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //텍스트 박스 엔터키 누를 시 이벤트
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    base.ShowLoading();

                    if (suspendLayout == null)
                    {
                        suspendLayout = new lib.Common.SuspendDrawingUpdate(this);
                    }

                    //조회
                    BindData(gridUspName, _paramss);

                    if (this.GridViewData.Rows.Count == 0)
                    {
                        if (base.childGrid != null)
                        {
                            base.childGrid.Usp_Load_Parameters["@KEY_PARAMS"] = "";
                            base.childGrid.BindData();
                        }
                    }

                    else
                    {
                        this.GridViewData.Rows[0].IsSelected = false;
                        this.GridViewData.Rows[0].IsSelected = true;
                    }

                    if (suspendLayout != null)
                    {
                        suspendLayout.Dispose();
                        suspendLayout = null;
                    }
                }

                catch (Exception exc)
                {
                    bowooMessageBox.Show(exc.Message.ToString());

                    return;
                }

                finally
                {
                    base.HideLoading();
                }
            }
        }

        Color selectedCellBackColor = Color.FromArgb(178, 204, 255);
        Color selectedCellBackColor2 = Color.FromArgb(181, 178, 255);

        //선택 행 스타일
        void GridViewData_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            //행 선택 시 스타일
            if (e.RowElement.IsSelected || e.RowElement.IsCurrent)
            {
                e.RowElement.GradientStyle = GradientStyles.Linear;
                e.RowElement.BackColor = selectedCellBackColor;
                e.RowElement.BackColor2 = selectedCellBackColor2;
            }
        }

        //셀스타일 설정(요약행 스타일)
        /// <summary>
        /// TOP Summary Row Style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GridViewData_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            GridDataCellElement dCell = e.CellElement as GridDataCellElement;

            if (dCell != null)
            {
                if (e.CellElement.IsCurrent)
                {
                    e.CellElement.IsCurrent = false;
                    e.CellElement.IsCurrentColumn = false;
                }

                else
                {
                    e.CellElement.ResetValue(LightVisualElement.DrawBorderProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                }

                if (e.CellElement.RowElement.IsCurrent || e.CellElement.RowElement.IsSelected)
                {
                    e.CellElement.BorderBottomColor = selectedCellBackColor;
                    e.CellElement.BorderTopColor = selectedCellBackColor;

                }

                else
                {
                    //마우스 오버 시 행 색상 변경
                    e.CellElement.BackColor = selectedCellBackColor;
                    e.CellElement.BorderColor = selectedCellBackColor;

                    if (e.CellElement.Value != null && !string.IsNullOrEmpty(e.CellElement.Value.ToString()))
                    {
                        e.CellElement.ToolTipText = e.CellElement.Value.ToString();
                    }
                }
            }

            GridSummaryCellElement cell = e.CellElement as GridSummaryCellElement;

            if (cell != null)
            {
                e.CellElement.ForeColor = Color.Red;
                e.CellElement.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                if ((cell.ColumnInfo.Tag as ParamType).Title)
                {
                    e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                }

                else if ((cell.ColumnInfo.Tag as ParamType).Sum)
                {
                    e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
                }

                else if ((cell.ColumnInfo.Tag as ParamType).Count)
                {
                    e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
                }
            }

            //헤더 폰트 사이즈 
            GridHeaderCellElement hcell = e.CellElement as GridHeaderCellElement;

            if (hcell != null)
            {
                e.CellElement.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
        }

        //조회조건 숨김
        /// <summary>
        /// 조회 조건 패널 사용 X
        /// </summary>
        public void HideSearchCondition()
        {
            //this.layoutControlItem3.Visibility = ElementVisibility.Collapsed;
            this.layoutControlItem3.MaxSize = new Size(0, 1);
            this.layoutControlItem3.MinSize = new Size(0, 1);
        }

        public void HideTitle()
        {
            this.layoutControlItem1.MaxSize = new Size(1, this.layoutControlItem1.Size.Height);
            this.layoutControlItem1.MinSize = new Size(1, this.layoutControlItem1.Size.Height);
            //this.layoutControlItem1.Enabled = false;
        }

        //행 선택
        public void SelectRow(int _rowIndex)
        {
            if (this.GridViewData.Rows.Count == 0)
            {
                return;
            }

            if (_rowIndex == -1 && this.GridViewData.Rows.Count > 0)
            {
                this.GridViewData.Rows[0].IsCurrent = true;
                this.GridViewData.Rows[0].IsSelected = true;

                return;
            }

            if (this.GridViewData.Rows.Count > _rowIndex)
            {
                this.GridViewData.Rows[_rowIndex].IsCurrent = true;
                this.GridViewData.Rows[_rowIndex].IsSelected = true;
            }
        }

        //interval 초 간격 새로 고침 체크 박스 생성
        public void CreateRefreshCheckBox(int interval)
        {
            radChkRefresh = new RadCheckBox();
            radChkRefresh.Text = string.Format(LanguagePack.Translate("{0}초 간격 새로 고침"), (interval / 1000).ToString());
            radChkRefresh.Tag = "refresh";
            radChkRefresh.Location = new Point(this.radButton2.Location.X - 110, 5);
            radChkRefresh.CheckStateChanged += radChkRefresh_CheckStateChanged;

            this.radPnlTop.Controls.Add(radChkRefresh);

            refreshTimer = new Timer();
            refreshTimer.Interval = interval;
            refreshTimer.Tick += refreshTimer_Tick;
        }

        //새로고침 틱당 이벤트
        void refreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                BindData(null, null);
            }

            catch
            {
                refreshTimer.Stop();
            }
        }

        // 새로고침 체크 변경 시 
        void radChkRefresh_CheckStateChanged(object sender, EventArgs e)
        {
            RadCheckBox chk = sender as RadCheckBox;

            if (chk != null)
            {
                if (chk.Checked)
                {
                    BindData(null, null);
                    refreshTimer.Start();
                }

                else
                {
                    refreshTimer.Stop();
                }
            }
        }

        public void RefreshTimerStop()
        {
            //refreshTimer.Stop();
            if (radChkRefresh.Checked)
            {
                radChkRefresh.Checked = false;
            }
        }
    }

    public class ParamType
    {
        public bool Key = false;
        public bool Data = false;
        public bool Sum = false;
        public bool Count = false;
        public bool Title = false;
        public bool File = false;
        public string BindType = "";
        public string BindSourceType = "";
        public string BindSource = "";
    }

    public class CustomDropDownListEditor : RadDropDownListEditor
    {
        public override bool EndEdit()
        {
            GridComboBoxCellElement cellElement = this.OwnerElement as GridComboBoxCellElement;
            RadGridView grid = cellElement.GridControl;

            cellElement.Tag = ((RadDropDownListEditorElement)this.EditorElement).Text;

            return base.EndEdit();
        }
    }

    public class CustomBehavior : BaseGridBehavior
    {
        protected override bool OnMouseDownLeft(MouseEventArgs e)
        {
            if (this.GridControl.IsInEditMode)
            {
                this.GridControl.EndEdit();

                return false;
            }

            return base.OnMouseDownLeft(e);
        }
    }
}