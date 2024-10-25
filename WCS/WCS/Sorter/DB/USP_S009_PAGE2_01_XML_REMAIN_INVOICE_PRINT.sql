USE [SORTER_WIVIS]
GO
/****** Object:  StoredProcedure [dbo].[USP_S009_PAGE2_01_XML_REMAIN_INVOICE_PRINT]    Script Date: 2024-10-25 오후 2:49:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






ALTER PROC [dbo].[USP_S009_PAGE2_01_XML_REMAIN_INVOICE_PRINT]
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
	
	--exec IF_SP_INVOICE
	select L1.BIZ_DAY	,L1.CHUTE_NO		,L1.BOX_NO	,L1.ITEM_CD		,L1.style	,L1.ITEM_COLOR	,L1.ITEM_SIZE  ,L1.index_group
					,L1.wrk_qty,L1.index_no			,L3.FORWARDING_AMOUNT		,L3.SALES_AMOUNT ,ISNULL(L3.VRKME,'') AS VRKME	   ,ISNULL(L3.SHOP_NM,'') AS SHOP_NM
					,ISNULL(L3.REPRESENTATIVE,'') AS REPRESENTATIVE				,ISNULL(L3.SHOP_ADDR_1,'') AS ADDR								,ISNULL(L1.BOX_INV,'') AS BOX_INV
					,DENSE_RANK() OVER(PARTITION BY  L1.BOX_NO ORDER BY L1.BOX_NO,L1.STYLE,L1.ITEM_COLOR) row_no
			from (
					select a.BIZ_DAY	,a.CHUTE_NO		,a.BOX_NO	,A.ITEM_CD	,b.style	,a.ITEM_COLOR	,a.ITEM_SIZE  ,b.index_group
							,SUM(a.wrk_qty) WRK_QTY		,A.BOX_INV	,A.BATCH	,A.SHOP_CD
							,case when(INDEX_GROUP = 'UP01') THEN 0 
							 WHEN(INDEX_GROUP = 'SH01') THEN 1 
							 WHEN(INDEX_GROUP = 'BT01') THEN 2 
							 WHEN(INDEX_GROUP = 'BT02') THEN 3 
							 WHEN(INDEX_GROUP = 'AC01') THEN 4 
							 WHEN(INDEX_GROUP = 'UP04') THEN 5 
							 WHEN(INDEX_GROUP = 'UP10') THEN 6 
							 WHEN(INDEX_GROUP = 'BT09') THEN 7 
							ELSE 10  END as index_no
					from IF_BOX_LIST (nolock) as a
					join item_info as b
					on substring(a.ITEM_CD,1,8) = b.style and a.ITEM_COLOR = b.color
					where BIZ_DAY = @D_BIZ_DAY
					and WORK_TYPE =  2
					GROUP BY A.BIZ_DAY		,A.CHUTE_NO		,A.BOX_NO	,A.ITEM_CD	,b.style	,a.ITEM_COLOR	,a.ITEM_SIZE  
							,b.index_group ,A.BOX_INV		,A.BATCH	,A.SHOP_CD
				) L1
			join (
				SELECT BIZ_DAY ,FORWARDING_AMOUNT		,SALES_AMOUNT,ISNULL(VRKME,'') AS VRKME,ISNULL(SHOP_NM,'') AS SHOP_NM
					,ISNULL(REPRESENTATIVE,'') AS REPRESENTATIVE				,ISNULL(SHOP_ADDR_1,'') AS SHOP_ADDR_1
					,ITEM_CD		,CHUTE_NO
				FROM IF_ORDER (nolock)
				where BIZ_DAY = @D_BIZ_DAY
				GROUP BY BIZ_DAY ,FORWARDING_AMOUNT		,SALES_AMOUNT	,VRKME	,SHOP_NM	,REPRESENTATIVE	,SHOP_ADDR_1,ITEM_CD		,CHUTE_NO
				) as L3
			on L1.BIZ_DAY = L3.BIZ_DAY AND L1.ITEM_CD = L3.ITEM_CD  AND L1.CHUTE_NO = REPLICATE('0', 3 - LEN(L3.CHUTE_NO)) + L3.CHUTE_NO
			JOIN
			(
				SELECT DISTINCT root.value('(./CHUTE_NO)[1]','varchar(5)') as CHUTE_NO		,root.value('(./BOX_NO)[1]','nvarchar(5)') as BOX_NO			
				,root.value('(./BATCH)[1]','nvarchar(5)') as BATCH							,root.value('(./SHOP_CD)[1]','varchar(10)') as SHOP_CD
				FROM @IN_XML.nodes('/root/data') AS R(root)
			) AS L4
			ON L1.BATCH = L4.BATCH	AND	L1.CHUTE_NO	= L4.CHUTE_NO	AND	L1.SHOP_CD = L4.SHOP_CD AND L1.BOX_NO = L4.BOX_NO
			ORDER BY  L1.BOX_NO,L1.style,L1.ITEM_COLOR,L1.ITEM_SIZE

	IF @@ROWCOUNT < 1
	BEGIN
		SET @R_OK = 'NO'
	END
	ELSE
	BEGIN
		SET @R_OK = 'OK'
	END


END TRY

BEGIN CATCH


END CATCH

END







