using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Reporting;
using Telerik.ReportViewer;
using Telerik.WinControls.UI;

namespace lib.Common.Management
{
    public class DialogsLocalizationProvider : Telerik.WinControls.UI.PrintDialogsLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case PrintDialogsStringId.PreviewDialogTitle: return "미리보기";
                case PrintDialogsStringId.PreviewDialogPrint: return "프린트";
                case PrintDialogsStringId.PreviewDialogPrintSettings: return "환경 설정";
                case PrintDialogsStringId.PreviewDialogWatermark: return "표시마크";
                case PrintDialogsStringId.PreviewDialogPreviousPage: return "이전페이지";
                case PrintDialogsStringId.PreviewDialogNextPage: return "다음페이지";
                case PrintDialogsStringId.PreviewDialogZoomIn: return "확대";
                case PrintDialogsStringId.PreviewDialogZoomOut: return "축소";
                case PrintDialogsStringId.PreviewDialogZoom: return "줌";
                case PrintDialogsStringId.PreviewDialogAuto: return "자동";
                case PrintDialogsStringId.PreviewDialogLayout: return "화면영역";
                case PrintDialogsStringId.PreviewDialogFile: return "파일";
                case PrintDialogsStringId.PreviewDialogView: return "뷰";
                case PrintDialogsStringId.PreviewDialogTools: return "도구상자";
                case PrintDialogsStringId.PreviewDialogExit: return "종료";
                case PrintDialogsStringId.PreviewDialogStripTools: return "도구상자";
                case PrintDialogsStringId.PreviewDialogStripNavigation: return "추적페이지";
                case PrintDialogsStringId.WatermarkDialogTitle: return "표시마크 설정";
                case PrintDialogsStringId.WatermarkDialogButtonOK: return "확인";
                case PrintDialogsStringId.WatermarkDialogButtonCancel: return "취소";
                case PrintDialogsStringId.WatermarkDialogLabelPreview: return "미리보기";
                case PrintDialogsStringId.WatermarkDialogButtonRemove: return "표시마크 제거";
                case PrintDialogsStringId.WatermarkDialogLabelPosition: return "위치";
                case PrintDialogsStringId.WatermarkDialogRadioInFront: return "앞으로";
                case PrintDialogsStringId.WatermarkDialogRadioBehind: return "뒤로";
                case PrintDialogsStringId.WatermarkDialogLabelPageRange: return "페이지 설정";
                case PrintDialogsStringId.WatermarkDialogRadioAll: return "전체";
                case PrintDialogsStringId.WatermarkDialogRadioPages: return "페이지";
                case PrintDialogsStringId.WatermarkDialogLabelPagesDescription: return "(e.g. 1,3,5-12)";
                case PrintDialogsStringId.WatermarkDialogTabText: return "텍스트";
                case PrintDialogsStringId.WatermarkDialogTabPicture: return "사진";
                case PrintDialogsStringId.WatermarkDialogLabelText: return "텍스트";
                case PrintDialogsStringId.WatermarkDialogWatermarkText: return "Watermark text";
                case PrintDialogsStringId.WatermarkDialogLabelHOffset: return "수평 간격";
                case PrintDialogsStringId.WatermarkDialogLabelVOffset: return "수직 간격";
                case PrintDialogsStringId.WatermarkDialogLabelRotation: return "회전";
                case PrintDialogsStringId.WatermarkDialogLabelFont: return "폰트:";
                case PrintDialogsStringId.WatermarkDialogLabelSize: return "사이즈:";
                case PrintDialogsStringId.WatermarkDialogLabelColor: return "색상:";
                case PrintDialogsStringId.WatermarkDialogLabelOpacity: return "투명도:";
                case PrintDialogsStringId.WatermarkDialogLabelLoadImage: return "이미지 불러오기:";
                case PrintDialogsStringId.WatermarkDialogCheckboxTiling: return "Tiling";
                case PrintDialogsStringId.SettingDialogTitle: return "환경 설정";
                case PrintDialogsStringId.SettingDialogButtonPrint: return "프린트";
                case PrintDialogsStringId.SettingDialogButtonPreview: return "미리보기";
                case PrintDialogsStringId.SettingDialogButtonCancel: return "취소";
                case PrintDialogsStringId.SettingDialogButtonOK: return "확인";
                case PrintDialogsStringId.SettingDialogPageFormat: return "형식";
                case PrintDialogsStringId.SettingDialogPagePaper: return "페이퍼";
                case PrintDialogsStringId.SettingDialogPageHeaderFooter: return "머리글/바닥글";
                case PrintDialogsStringId.SettingDialogButtonPageNumber: return "페이지 번호";
                case PrintDialogsStringId.SettingDialogButtonTotalPages: return "총 페이지";
                case PrintDialogsStringId.SettingDialogButtonCurrentDate: return "현재 날짜";
                case PrintDialogsStringId.SettingDialogButtonCurrentTime: return "현재 시간";
                case PrintDialogsStringId.SettingDialogButtonUserName: return "유저명";
                //case PrintDialogsStringId.SettingDialogButtonFont: return "폰트";
                case PrintDialogsStringId.SettingDialogLabelHeader: return "머리글";
                case PrintDialogsStringId.SettingDialogLabelFooter: return "바닥글";
                case PrintDialogsStringId.SettingDialogCheckboxReverse: return "페이지 회전";
                case PrintDialogsStringId.SettingDialogLabelPage: return "페이지";
                case PrintDialogsStringId.SettingDialogLabelType: return "타입";
                case PrintDialogsStringId.SettingDialogLabelPageSource: return "페이지 경로";
                case PrintDialogsStringId.SettingDialogLabelMargins: return "간격 (inches)";
                case PrintDialogsStringId.SettingDialogLabelOrientation: return "위치조정";
                case PrintDialogsStringId.SettingDialogLabelTop: return "위:";
                case PrintDialogsStringId.SettingDialogLabelBottom: return "아래:";
                case PrintDialogsStringId.SettingDialogLabelLeft: return "좌:";
                case PrintDialogsStringId.SettingDialogLabelRight: return "우:";
                case PrintDialogsStringId.SettingDialogRadioPortrait: return "세로";
                case PrintDialogsStringId.SettingDialogRadioLandscape: return "가로";
                case PrintDialogsStringId.SchedulerSettingsLabelPrintStyle: return "프린트 스타일";
                case PrintDialogsStringId.SchedulerSettingsDailyStyle: return "일별 스타일";
                case PrintDialogsStringId.SchedulerSettingsWeeklyStyle: return "주별 스타일";
                case PrintDialogsStringId.SchedulerSettingsMonthlyStyle: return "월별 스타일";
                case PrintDialogsStringId.SchedulerSettingsDetailStyle: return "자세한 스타일";
                case PrintDialogsStringId.SchedulerSettingsButtonWatermark: return "Watermark...";
                //case PrintDialogsStringId.SchedulerSettingsButtonFont: return "폰트...";
                case PrintDialogsStringId.SchedulerSettingsLabelPrintRange: return "프린트 범위";
                case PrintDialogsStringId.SchedulerSettingsLabelStyleSettings: return "스타일 설정";
                case PrintDialogsStringId.SchedulerSettingsLabelPrintSettings: return "프린트 설정";
                case PrintDialogsStringId.SchedulerSettingsLabelStartDate: return "시작일";
                case PrintDialogsStringId.SchedulerSettingsLabelEndDate: return "종료일";
                case PrintDialogsStringId.SchedulerSettingsLabelStartTime: return "시작시간";
                case PrintDialogsStringId.SchedulerSettingsLabelEndTime: return "종료시간";
                case PrintDialogsStringId.SchedulerSettingsLabelDateFont: return "날짜 제목 글꼴";
                case PrintDialogsStringId.SchedulerSettingsLabelAppointmentFont: return "약속 글꼴";
                case PrintDialogsStringId.SchedulerSettingsLabelLayout: return "레이아웃";
                case PrintDialogsStringId.SchedulerSettingsPrintPageTitle: return "타이틀";
                case PrintDialogsStringId.SchedulerSettingsPrintCalendar: return "제목 달력표시";
                case PrintDialogsStringId.SchedulerSettingsPrintTimezone: return "프린트 타임존";
                case PrintDialogsStringId.SchedulerSettingsPrintNotesBlank: return "작성 영역 (blank)";
                case PrintDialogsStringId.SchedulerSettingsPrintNotesLined: return "작성 영역 (lined)";
                case PrintDialogsStringId.SchedulerSettingsNonworkingDays: return "비업무일 제외";
                case PrintDialogsStringId.SchedulerSettingsExactlyOneMonth: return "월별 페이지 인쇄";
                case PrintDialogsStringId.SchedulerSettingsLabelWeeksPerPage: return "주별 페이지 인쇄";
                case PrintDialogsStringId.SchedulerSettingsNewPageEach: return "페이지당 새내용";
                case PrintDialogsStringId.SchedulerSettingsStringDay: return "일";
                case PrintDialogsStringId.SchedulerSettingsStringMonth: return "월";
                case PrintDialogsStringId.SchedulerSettingsStringWeek: return "주";
                case PrintDialogsStringId.SchedulerSettingsStringPage: return "페이지";
                case PrintDialogsStringId.SchedulerSettingsStringPages: return "여러 페이지";
                case PrintDialogsStringId.SchedulerSettingsLabelGroupBy: return "묶기:";
                case PrintDialogsStringId.SchedulerSettingsGroupByNone: return "없음";
                case PrintDialogsStringId.SchedulerSettingsGroupByResource: return "위치";
                case PrintDialogsStringId.SchedulerSettingsGroupByDate: return "날짜";
                case PrintDialogsStringId.GridSettingsLabelPreview: return "미리보기";
                case PrintDialogsStringId.GridSettingsLabelStyleSettings: return "환경 설정";
                case PrintDialogsStringId.GridSettingsLabelFitMode: return "페이지 인쇄 모드:";
                case PrintDialogsStringId.GridSettingsLabelHeaderCells: return "머리글 영역";
                case PrintDialogsStringId.GridSettingsLabelGroupCells: return "그룹 영역";
                case PrintDialogsStringId.GridSettingsLabelDataCells: return "데이타 영역";
                case PrintDialogsStringId.GridSettingsLabelSummaryCells: return "합계 영역";
                case PrintDialogsStringId.GridSettingsLabelBackground: return "백그라운드";
                case PrintDialogsStringId.GridSettingsLabelBorderColor: return "경계 색상";
                case PrintDialogsStringId.GridSettingsLabelAlternatingRowColor: return "그리드 색상";
                case PrintDialogsStringId.GridSettingsLabelPadding: return "패딩";
                case PrintDialogsStringId.GridSettingsPrintGrouping: return "프린트 그룹";
                case PrintDialogsStringId.GridSettingsPrintSummaries: return "프린트 합계";
                case PrintDialogsStringId.GridSettingsPrintHiddenRows: return "프린트 행 숨기기";
                case PrintDialogsStringId.GridSettingsPrintHiddenColumns: return "프린트 열 숨기기";
                case PrintDialogsStringId.GridSettingsPrintHeader: return "페이지별 번호";
                case PrintDialogsStringId.GridSettingsButtonWatermark: return "표시마크";
                //case PrintDialogsStringId.GridSettingsButtonFont: return "폰트";
                case PrintDialogsStringId.GridSettingsFitPageWidth: return "페이지맞추기";
                case PrintDialogsStringId.GridSettingsNoFit: return "임의로 맞추기";
                case PrintDialogsStringId.GridSettingsNoFitCentered: return "중간 맞추기";
                case PrintDialogsStringId.GridSettingsLabelPrint: return "프린트";
            }
            return String.Empty;
        }


    }
}
