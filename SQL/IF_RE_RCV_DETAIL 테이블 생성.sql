CREATE table IF_RE_RCV_DETAIL(
���� varchar(1),
�귣�� varchar(10),
����귣�� varchar(10),
�귣��� varchar(50),
�Ƿ����� varchar(8),
�Ƿڱ��� varchar(2),
��񱸺� varchar(2),
�۾����� varchar(8),
�۾����� varchar(4),
ǰ�� varchar(8),
���� varchar(2),
�԰� varchar(3),
�԰��� varchar(2),
���� varchar(2),
���ڵ� varchar(20),
�����ǥ�� varchar(20),
�����ڵ� varchar(6),
����� varchar(50),
������ȭ��ȣ varchar(50),
�����ּ� varchar(200),
�����Ϲ�ȣ varchar(50),
�����ȣ varchar(50),
������ü��� int,
�۾����� int,
���ܰ� varchar(20),
�켱���� varchar(10),
SORTER�������� varchar(1),
DAS�������� varchar(1),
������ int,
ASSORT���� varchar(1),
ASSORT�� int,
��ǰ�� varchar(10),
���ù�ȣ varchar(4),
���ÿ��� varchar(4),
â���ڵ� varchar(6),
����ó�� varchar(1),
��ϻ�� varchar(10),
������� date,
�ǸŴܰ� varchar(20),
�����ڵ� varchar(20),
�귣���ڵ� varchar(20),
�����ڵ� varchar(20)
)

select * from IF_RE_RCV_DETAIL

INSERT INTO dbo.IF_RE_RCV_DETAIL(
    ����, �귣��, ����귣��, �귣���, �Ƿ�����, �Ƿڱ���, ��񱸺�, �۾�����, �۾�����, ǰ��, ����, �԰�, �԰���,
    ����, ���ڵ�, �����ǥ��, �����ڵ�, �����, ������ȭ��ȣ, �����ּ�, �����Ϲ�ȣ, �����ȣ, ������ü���, �۾�����, ���ܰ�, �켱����,
    SORTER��������, DAS��������, ������, ASSORT����, ASSORT��, ��ǰ��, ���ù�ȣ, ���ÿ���, â���ڵ�, ����ó��, ��ϻ��, �������, �ǸŴܰ�, �����ڵ�,
    �귣���ڵ�, �����ڵ�)
VALUES (
    'S', 'ZI', 'L', '����', '20241024', '09', 'A', '20241024', '0006', 'LSJPN173', 'BK', '105', NULL, 
    '05', NULL, '���¿�', 'MB0017', '����', '0516345545', '�λ� ���� ���ϵ� 832-3', '6052238792', NULL, 1, 0, '40710', '0',
    '2', '1', 22, NULL, 0, NULL, '0014', '0001', 'AS002', 'N', '210103', '2024-10-24', '59000', 'C001804',
    'ZISHEN', '000')

