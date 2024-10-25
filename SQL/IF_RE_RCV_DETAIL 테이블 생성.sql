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

