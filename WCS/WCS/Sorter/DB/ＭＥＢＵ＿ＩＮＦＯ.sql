SELECT TOP (1000) [SEQ]
      ,[NAME]
      ,[URL]
      ,[ACSLV]
      ,[USEYN]
  FROM [SORTER_WIVIS].[dbo].[MENU_INFO]



  INSERT INTO dbo.MENU_INFO (SEQ,NAME, URL, ACSLV,USEYN) 
VALUES (14,'반품 관리','S014_ManageReturn',2,'Y')