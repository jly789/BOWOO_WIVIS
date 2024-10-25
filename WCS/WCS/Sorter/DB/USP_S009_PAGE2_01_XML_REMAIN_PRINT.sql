USE [SORTER_WIVIS]
GO
/****** Object:  StoredProcedure [dbo].[USP_S009_PAGE2_01_XML_REMAIN_PRINT]    Script Date: 2024-10-25 오후 2:49:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





ALTER PROC [dbo].[USP_S009_PAGE2_01_XML_REMAIN_PRINT]
	--@DATA_PARAMS   NVARCHAR(max) = ''
	@HEADER_PARAMS VARCHAR(300)
	,@DATA_PARAMS   XML
	,@R_OK		   VARCHAR(2)    = 'OK' OUTPUT
	,@R_MESSAGE	   VARCHAR(MAX)  = '' OUTPUT
AS

BEGIN
BEGIN TRY
	DECLARE @V_DETERMITER VARCHAR(2) = ';#'; -- 구분자
	DECLARE @IN_XML XML = @DATA_PARAMS

	--SET @IN_XML = CONVERT(XML,@DATA_PARAMS,1)

	
	DECLARE @D_DETERMITER VARCHAR(2) = ';#' -- 구분자
	-- Header Params Parsing -------------------------------------------------
	DECLARE @TBL_HEADER_PARAMS TABLE (KEY_STR VARCHAR(100), VALUE1 VARCHAR(200))
	INSERT INTO @TBL_HEADER_PARAMS
	SELECT KEY_STR, VALUE1 FROM FN_SPLIT_KEY_VALUE(@HEADER_PARAMS,@D_DETERMITER)
	--------------------------------------------------------------------------
	DECLARE @D_BIZ_DAY VARCHAR(8) = (SELECT VALUE1 FROM @TBL_HEADER_PARAMS WHERE KEY_STR = 'BIZDAY')
	

	SELECT GUBUN_CD + '/' + '(주) 위비스' + '/' + BRAND_NM + '/' + CONVERT(VARCHAR,A.CHUTE_NO) + '/' + SHOP_NM + '/' + SHOP_ADDR1 + '/' + SHOP_ADDR2
		 + '/' + SHOP_TEL + '/' + CONVERT(VARCHAR,A.BOX_NO) + '/' + END_YN + '/' + CONVERT(VARCHAR,SND_QTY)	   + '/' + isnull(TML_CD,'')  + '/' + isnull(TML_NM,'')	  + '/' + BARCODE 
		 + '/' + ISNULL(a.ITEM_CD,'')	+ '/' + ISNULL(a.YEAR,'')			+ '/' + ISNULL(a.SEASON,'')	+ '/'  + ISNULL(a.BIZ_DAY,'')	+ '/' + CONVERT(VARCHAR,LABEL_POS) AS RE_DATA 
	FROM RE_LABEL_LIST AS A
	JOIN
		(
			SELECT DISTINCT root.value('(./CHUTE_NO)[1]','varchar(5)') as CHUTE_NO		,root.value('(./BOX_NO)[1]','nvarchar(5)') as BOX_NO			
			,root.value('(./BATCH)[1]','nvarchar(5)') as BATCH							,root.value('(./SHOP_CD)[1]','varchar(10)') as SHOP_CD
			FROM @IN_XML.nodes('/root/data') AS R(root)
		) AS B
	ON A.SHOP_CD = B.SHOP_CD
	AND A.BOX_NO = B.BOX_NO
	AND A.BATCH = B.BATCH
	WHERE A.BIZ_DAY = @D_BIZ_DAY

	IF @@ROWCOUNT < 1
	BEGIN
		SET @R_OK = 'NO'
	END
	ELSE
	BEGIN
		SET @R_OK = 'OK'
	END

	select * from RE_LABEL_LIST

END TRY

BEGIN CATCH


END CATCH

END






