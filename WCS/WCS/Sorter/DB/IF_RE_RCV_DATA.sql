CREATE TABLE IF_RE_RCV_DATA(
   BIZ_DAY varchar(10) NOT NULL,
   CENTER_CD varchar(4) NOT NULL,
   BATCH_NO varchar(11) NOT NULL,
   BRAND_CD varchar(4) NOT NULL,
   POS_NUM int NOT NULL,
   ITEM_CD varchar(8) NULL,
   ITEM_COLOR varchar(2) NULL,
   ITEM_SIZE varchar(3) NULL,
   ITEM_STYLE varchar(20) NULL,
   ITEM_NM varchar(200) NULL,
   ITEM_BAR varchar(15) NULL,
   ORD_QTY int NULL,
   CHUTE_NO int NOT NULL,
   STATUS int NOT NULL
)

INSERT INTO dbo.IF_RE_RCV_DATA(BIZ_DAY, CENTER_CD, BATCH_NO, BRAND_CD, POS_NUM, ITEM_CD, ITEM_COLOR, ITEM_SIZE, ITEM_STYLE, ITEM_NM, ITEM_BAR, ORD_QTY,
CHUTE_NO, STATUS)
VALUES (20241024, 'B00', 001, 'K', 1, '', '', '', '', '', 'SDKJSDF45789', 0, 50, 0)

INSERT INTO dbo.IF_RE_RCV_DATA(BIZ_DAY, CENTER_CD, BATCH_NO, BRAND_CD, POS_NUM, ITEM_CD, ITEM_COLOR, ITEM_SIZE, ITEM_STYLE, ITEM_NM, ITEM_BAR, ORD_QTY,
CHUTE_NO, STATUS)
VALUES (20241024, 'B00', 001, 'K', 1, '', '', '', '', '', 'POWEJFIWE789', 0, 30, 0), 
(20241024, 'B00', 001, 'K', 1, '', '', '', '', '', 'WEJKFJEF1223', 0, 35, 0),
(20241024, 'B00', 001, 'K', 1, '', '', '', '', '', 'QMVDLDL8526', 0, 32, 0),
(20241024, 'B00', 001, 'K', 1, '', '', '', '', '', 'BJMLFDLF7412', 0, 36, 0),
(20241024, 'B00', 001, 'K', 1, '', '', '', '', '', 'WIJLDKDF456', 0, 60, 0),
(20241023, 'B00', 001, 'K', 1, '', '', '', '', '', 'EIJFEOFD47568', 0, 105, 0),
(20241023, 'B00', 001, 'K', 1, '', '', '', '', '', 'ASDKO456FGF', 0, 80, 0),
(20241023, 'B00', 001, 'K', 1, '', '', '', '', '', 'GLG89SDFD45', 0, 95, 0),
(20241022, 'B00', 001, 'K', 1, '', '', '', '', '', 'WEROKDFK916', 0, 100, 0),
(20241022, 'B00', 001, 'K', 1, '', '', '', '', '', 'PIFJ894SDDF45', 0, 212, 0)

update IF_RE_RCV_DATA
SET CENTER_CD = 'B000'
where CENTER_CD = 'B00'







CREATE table IF_RE_RCV_DETAIL(
구분 varchar(1),
브랜드 varchar(10),
서브브랜드 varchar(10),
브랜드명 varchar(50),
의뢰일자 varchar(8),
의뢰구분 varchar(2),
장비구분 varchar(2),
작업일자 varchar(8),
작업차수 varchar(4),
품번 varchar(8),
색상 varchar(2),
규격 varchar(3),
규격존 varchar(2),
순번 varchar(2),
바코드 varchar(20),
매장대표자 varchar(20),
매장코드 varchar(6),
매장명 varchar(50),
매장전화번호 varchar(50),
매장주소 varchar(200),
매장등록번호 varchar(50),
송장번호 varchar(50),
출고지시수량 int,
작업수량 int,
출고단가 varchar(20),
우선순위 varchar(10),
SORTER수신유무 varchar(1),
DAS수신유무 varchar(1),
재고수량 int,
ASSORT유형 varchar(1),
ASSORT수 int,
신품번 varchar(10),
지시번호 varchar(4),
지시연번 varchar(4),
창고코드 varchar(6),
사입처리 varchar(1),
등록사번 varchar(10),
등록일자 date,
판매단가 varchar(20),
고객사코드 varchar(20),
브랜드코드 varchar(20),
센터코드 varchar(20)
)


SELECT * FROM IF_RE_RCV_DATA
select * from IF_RE_RCV_DETAIL

INSERT INTO dbo.IF_RE_RCV_DETAIL(
    구분, 브랜드, 서브브랜드, 브랜드명, 의뢰일자, 의뢰구분, 장비구분, 작업일자, 작업차수, 품번, 색상, 규격, 규격존,
    순번, 바코드, 매장대표자, 매장코드, 매장명, 매장전화번호, 매장주소, 매장등록번호, 송장번호, 출고지시수량, 작업수량, 출고단가, 우선순위,
    SORTER수신유무, DAS수신유무, 재고수량, ASSORT유형, ASSORT수, 신품번, 지시번호, 지시연번, 창고코드, 사입처리, 등록사번, 등록일자, 판매단가, 고객사코드,
    브랜드코드, 센터코드)
VALUES (
    'S', 'ZI', 'L', '지센', '20241024', '09', 'A', '20241024', '0006', 'LSJPN173', 'BK', '105', NULL, 
    '05', NULL, '김태완', 'MB0017', '범일', '0516345545', '부산 동구 범일동 832-3', '6052238792', NULL, 1, 0, '40710', '0',
    '2', '1', 22, NULL, 0, NULL, '0014', '0001', 'AS002', 'N', '210103', '2024-10-24', '59000', 'C001804',
    'ZISHEN', '000')



	INSERT INTO dbo.IF_RE_RCV_DETAIL(
    구분, 브랜드, 서브브랜드, 브랜드명, 의뢰일자, 의뢰구분, 장비구분, 작업일자, 작업차수, 품번, 색상, 규격, 규격존,
    순번, 바코드, 매장대표자, 매장코드, 매장명, 매장전화번호, 매장주소, 매장등록번호, 송장번호, 출고지시수량, 작업수량, 출고단가, 우선순위,
    SORTER수신유무, DAS수신유무, 재고수량, ASSORT유형, ASSORT수, 신품번, 지시번호, 지시연번, 창고코드, 사입처리, 등록사번, 등록일자, 판매단가, 고객사코드,
    브랜드코드, 센터코드)
VALUES (
    'S', 'ZI', 'L', '지센', '20241024', '09', 'A', '20241024', '0006', 'LSTSN271', 'BK', '105', NULL, 
    '06', NULL, '오현주', 'MP0043', '포항흥해', '0542624945', '경북 포항시 북구 흥해읍 옥성리 163번지', '5061572781', NULL, 1, 0, '9983', '0',
    '2', '1', 35, NULL, 0, NULL, '0012', '0003', 'AS002', 'N', '210103', '2024-10-24', '79000', 'C001804',
    'ZISHEN','000') ,
    (
    'S', 'ZI', 'L', '지센', '20241024', '09', 'A', '20241024', '0006', 'LSVTN171', 'LB', '100', NULL, 
    '06', NULL, '오현주', 'MP0043', '포항흥해', '0542624945', '경북 포항시 북구 흥해읍 옥성리 163번지', '5061572781', NULL, 1, 0, '9983', '0',
    '2', '1', 19, NULL, 0, NULL, '0012', '0003', 'AS002', 'N', '210103', '2024-10-24', '59000', 'C001804',
    'ZISHEN','000') ,
   (
    'S', 'ZI', 'L', '지센', '20241024', '09', 'A', '20241024', '0006', 'MCLWN101', 'PK', '100', NULL, 
    '05', NULL, '도상현', 'MS0128', '둔촌(직)', '024747717', '서울 강동구 양재대로 1363 (성내동) 1층 지센', '2118760782', NULL, 1, 0, '10940', '0',
    '2', '1', 17, NULL, 0, NULL, '0015', '0003', 'AS002', 'N', '210103', '2024-10-24', '59000', 'C001804',
    'ZISHEN','000') ,
   (
    'S', 'ZI', 'L', '지센', '20241024', '09', 'A', '20241024', '0006', 'MCLWN101', 'NA', '00F', NULL, 
    '14', NULL, '지윤혜', 'SK0032', '경기광주', '0314568452', '경기 광주시 광주대로 59 (경안동) 1층 103호', '2033222787', NULL, 1, 0, '159000', '0',
    '2', '1', 17, NULL, 0, NULL, '0012', '0004', 'AS002', 'N', '210103', '2024-10-24', '59000', 'C001804',
    'ZISHEN','000') ,
   (
    'S', 'ZI', 'L', '지센', '20241024', '09', 'A', '20241024', '0006', 'MCLWN101', 'RE', '00F', NULL, 
    '14', NULL, '박종태', 'MJ0068', '순천연향', '0617266111', '전남 순천시 연향상가1길 18 (연향동) 1층 지센', '4050452579', NULL, 1, 0, '49000', '0',
    '2', '1', 25, NULL, 0, NULL, '0012', '0005', 'AS002', 'N', '210103', '2024-10-24', '59000', 'C001804',
    'ZISHEN','000') ,
   (
    'S', 'ZI', 'M', '지센', '20241023', '09', 'A', '20241023', '0006', 'MCLWN101', 'BL', '103', NULL, 
    '05', NULL, '손윤희', 'MB0059', '부산서동', '0514562154', '부산광역시 금정구  반송로 408-1 (금사동 145-1)', '6210322345', NULL, 1, 0, '38350', '0',
    '2', '1', 6, NULL, 0, NULL, '0015', '0002', 'AS002', 'N', '210103', '2024-10-23', '49000', 'C001804',
    'ZISHEN','000') ,
   (
    'S', 'ZI', 'M', '지센', '20241023', '09', 'A', '20241023', '0006', 'MCTSN202', 'NA', '00F', NULL, 
    '06', NULL, '도상현', 'MS0128', '둔촌(직)', '024747717', '서울 강동구 양재대로 1363 (성내동) 1층 지센', '2118760782', NULL, 1, 0, '10940', '0',
    '2', '1', 6, NULL, 0, NULL, '0015', '0001', 'AS002', 'N', '210103', '2024-10-23', '49000', 'C001804',
    'ZISHEN','000') ,
   (
    'S', 'ZI', 'M', '지센', '20241023', '09', 'A', '20241023', '0006', 'MCTSN204', 'RE', '095', NULL, 
    '06', NULL, '김태완', 'MB0017', '범일', '0516345545', '부산 동구 범일동 832-3', '6052238792', NULL, 1, 0, '40710', '0',
    '2', '1', 8, NULL, 0, NULL, '0014', '0002', 'AS002', 'N', '210103', '2024-10-23', '59000', 'C001804',
    'ZISHEN','000') ,
   (
    'S', 'ZI', 'M', '지센', '20241023', '09', 'A', '20241023', '0006', 'LAABN305', 'NA', '095', NULL, 
    '06', NULL, '김상균,김태경', 'SK0016', '시흥신천상설', '0313113057', '경기 시흥시 신천동 411-15', '1400165929', NULL, 1, 0, '58405', '0',
    '2', '1', 9, NULL, 0, NULL, '0014', '0001', 'AS002', 'N', '210103', '2024-10-23', '69000', 'C001804',
    'ZISHEN','000')
	
	SELECT * FROM IF_RE_RCV_DATA
		SELECT * FROM IF_RE_RCV_DETAIL
	
	SELECT * FROM IF_RCV_DATA
		SELECT * FROM IF_RCV_DETAIL

	