USE [activityTracker]
GO

INSERT INTO [dbo].[Error]
           ([ErrorId]
           ,[Description])
     VALUES
           (101
           ,'Invalid Parameter Value')
GO

INSERT INTO [dbo].[Error]
           ([ErrorId]
           ,[Description])
     VALUES
           (102
           ,'Column Id doesn''t exist.')
GO

INSERT INTO [dbo].[Error]
           ([ErrorId]
           ,[Description])
     VALUES
           (103
           ,'Invalid value')
GO