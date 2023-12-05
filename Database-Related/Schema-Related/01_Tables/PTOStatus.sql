USE [activityTracker]
GO

/****** Object:  Table [dbo].[PTOStatus]    Script Date: 11/28/2023 6:59:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PTOStatus](
	[PTOStatusId] [int] NOT NULL,
	[Description] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO

