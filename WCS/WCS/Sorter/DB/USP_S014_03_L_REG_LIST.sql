USE [SORTER_WIVIS]
GO
/****** Object:  StoredProcedure [dbo].[USP_S014_03_L_REG_LIST]    Script Date: 2024-10-25 오후 2:48:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ================================================
-- Author      : <KIM SEONG MIN>
-- Create date : <2016-07-19>
-- Alter Date  :
-- Alter Devel : 
-- Alter Descr :
-- Description : <S008 배치 관리>
--             <등록 리스트>
-- ================================================

ALTER PROC [dbo].[USP_S014_03_L_REG_LIST]

   @HEADER_PARAMS VARCHAR(8000) = '' 
   ,@GRID_PARAMS VARCHAR(8000) = ''

AS

BEGIN

   SET NOCOUNT ON;

   DECLARE @V_DETERMITER VARCHAR(2) = ';#' -- 구분자
   -- Header Params Parsing -------------------------------------------------
   DECLARE @TBL_HEADER_PARAMS TABLE (KEY_STR VARCHAR(100), VALUE1 VARCHAR(200))
   INSERT INTO @TBL_HEADER_PARAMS
   SELECT KEY_STR, VALUE1 FROM FN_SPLIT_KEY_VALUE(@HEADER_PARAMS,@V_DETERMITER)
   --------------------------------------------------------------------------
   -- Grid Search Params Parsing --------------------------------------------
   DECLARE @TBL_GRID_PARAMS TABLE (KEY_STR VARCHAR(100), VALUE1 VARCHAR(200))
   INSERT INTO @TBL_GRID_PARAMS
   SELECT KEY_STR, VALUE1 FROM FN_SPLIT_KEY_VALUE(@GRID_PARAMS,@V_DETERMITER)
   --------------------------------------------------------------------------


   -- Set Header Param Value
   DECLARE @D_USER   VARCHAR(30) = (SELECT VALUE1 FROM @TBL_HEADER_PARAMS WHERE KEY_STR = 'USER')
   DECLARE @D_AUTH   VARCHAR(5) = (SELECT VALUE1 FROM @TBL_HEADER_PARAMS WHERE KEY_STR = 'AUTH')
   DECLARE @D_BIZ_DAY VARCHAR(8) = (SELECT VALUE1 FROM @TBL_HEADER_PARAMS WHERE KEY_STR = 'BIZDAY')
   DECLARE @D_BRAND VARCHAR(10) = (SELECT VALUE1 FROM @TBL_HEADER_PARAMS WHERE KEY_STR = 'BRAND')
   DECLARE @D_BATCH_NO VARCHAR(10) = (SELECT VALUE1 FROM @TBL_HEADER_PARAMS WHERE KEY_STR = 'WRKSEQ')


   -- Set Grid Param Value
   DECLARE @G_CHUTE_NO VARCHAR(100) = (SELECT VALUE1 FROM @TBL_GRID_PARAMS WHERE KEY_STR = 'G_CHUTE_NO')
   DECLARE @G_BRAND_CD VARCHAR(100) = (SELECT VALUE1 FROM @TBL_GRID_PARAMS WHERE KEY_STR = 'G_BRAND_CD')
   DECLARE @G_ITEM_CD VARCHAR(100) = (SELECT VALUE1 FROM @TBL_GRID_PARAMS WHERE KEY_STR = 'G_ITEM_CD')
   DECLARE @G_ITEM_COLOR VARCHAR(100) = (SELECT VALUE1 FROM @TBL_GRID_PARAMS WHERE KEY_STR = 'G_ITEM_COLOR')
   DECLARE @G_ITEM_SIZE VARCHAR(100) = (SELECT VALUE1 FROM @TBL_GRID_PARAMS WHERE KEY_STR = 'G_ITEM_SIZE')
   DECLARE @G_ITEM_BAR VARCHAR(100) = (SELECT VALUE1 FROM @TBL_GRID_PARAMS WHERE KEY_STR = 'G_ITEM_BAR')
   DECLARE @G_ORD_QTY VARCHAR(100) = (SELECT VALUE1 FROM @TBL_GRID_PARAMS WHERE KEY_STR = 'G_ORD_QTY')

   -- Set Header
   --SELECT
   --        '<HEADERCHECKBOX=FALSE;WIDTH=20>',
   --      '<ISVISIBLE=FALSE;DATA=TRUE>BIZ_DAY',
   --      '<ISVISIBLE=FALSE;DATA=TRUE>CENTER_CD',
   --      '<ISVISIBLE=FALSE;DATA=TRUE>BATCH_NO',
   --      '<ISVISIBLE=FALSE;DATA=TRUE>BRAND_CD',
   --      '<ISVISIBLE=FALSE;;DATA=TRUE>제품코드',
   --      '<ISVISIBLE=FALSE;DATA=TRUE>색상',
   --      '<ISVISIBLE=FALSE;DATA=TRUE>사이즈',
		 --  '<ISVISIBLE=FALSE;FILE=TRUE>슈트',
   --      '<ISVISIBLE=FALSE;FILE=TRUE;DATA=TRUE>바코드',
   --      '<ISVISIBLE=FALSE;FILE=TRUE>수량',
       
   --      '<MERGE=TRUE;WIDTH=50;TEXTALIGN=CENTER>슈트',
   --      '<MERGE=TRUE;WIDTH=100;TEXTALIGN=CENTER>브랜드',
   --      '<MERGE=TRUE;WIDTH=120;TEXTALIGN=CENTER>제품코드',
   --      '<MERGE=TRUE;WIDTH=70;TEXTALIGN=CENTER>색상',
   --      '<MERGE=TRUE;WIDTH=70;TEXTALIGN=CENTER>사이즈',
   --      '<MERGE=TRUE;WIDTH=188;TEXTALIGN=CENTER>바코드'
      

	  
   -- Set Header
   SELECT
       '<HEADERCHECKBOX=FALSE;WIDTH=20>',
    '<ISVISIBLE=FALSE;DATA=TRUE>BIZ_DAY',
    '<ISVISIBLE=FALSE;DATA=TRUE>CENTER_CD',
    '<ISVISIBLE=FALSE;DATA=TRUE>배치',
    '<ISVISIBLE=FALSE;DATA=TRUE>BRAND_CD',
    '<ISVISIBLE=FALSE;DATA=TRUE>제품코드',
    '<ISVISIBLE=FALSE;DATA=TRUE>색상',
    '<ISVISIBLE=FALSE;DATA=TRUE>사이즈',
    '<ISVISIBLE=TRUE;FILE=TRUE;DATA=TRUE>슈트',
    '<ISVISIBLE=TRUE;FILE=TRUE;DATA=TRUE>바코드',
    '<ISVISIBLE=TRUE;FILE=TRUE;DATA=TRUE>수량',
    '<ISVISIBLE=FALSE; WIDTH=50;TEXTALIGN=CENTER>슈트',
	'<ISVISIBLE=FALSE; WIDTH=50;TEXTALIGN=CENTER>바코드',
    '<ISVISIBLE=FALSE; WIDTH=50;TEXTALIGN=CENTER>수량',
    '<ISVISIBLE=FALSE; WIDTH=50;TEXTALIGN=CENTER>브랜드',
    '<ISVISIBLE=FALSE; WIDTH=50;TEXTALIGN=CENTER>제품코드',
    '<ISVISIBLE=FALSE; WIDTH=50;TEXTALIGN=CENTER>색상',
    '<ISVISIBLE=FALSE; WIDTH=50;TEXTALIGN=CENTER>사이즈'
    

   -- Select Data
   SELECT 
         CAST(0 AS BIT) 'CheckBox',
         RD.BIZ_DAY,
         RD.CENTER_CD,
         RD.BATCH_NO,
         RD.BRAND_CD,
         RD.ITEM_CD,
         RD.ITEM_COLOR,
         RD.ITEM_SIZE,
		    RD.CHUTE_NO,
         RD.ITEM_BAR,
         RD.ORD_QTY,
         RD.CHUTE_NO   AS G_CHUTE_NO,
		   RD.ITEM_BAR   AS G_ITEM_BAR,
		   RD.ORD_QTY AS G_ORD_QTY,
         RD.BRAND_CD   AS G_BRAND_CD,
         RD.ITEM_CD AS G_ITEM_CD,
         RD.ITEM_COLOR AS G_ITEM_COLOR,
         RD.ITEM_SIZE AS G_ITEM_SIZE
       
     FROM IF_RCV_DATA AS RD
   WHERE RD.BIZ_DAY = @D_BIZ_DAY
  
 
    
	    ORDER BY RD.CHUTE_NO 
      



END

SELECT * FROM IF_RCV_DATA
WHERE BIZ_DAY='20231124'

UPDATE IF_RCV_DATA SET ORD_QTY = 1
WHERE BIZ_DAY='20231124'