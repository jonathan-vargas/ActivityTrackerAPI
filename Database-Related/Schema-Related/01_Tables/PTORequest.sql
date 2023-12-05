USE [activityTracker]
GO

/****** Object:  Table [dbo].[PTORequest]    Script Date: 11/28/2023 6:59:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PTORequest](
	[PTORequestId] [int] IDENTITY(1,1) NOT NULL,
	[StartedDate] [date] NOT NULL,
	[FinishedDate] [date] NOT NULL,
	[PTOStatusId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[TeamLeadEmployeeId] [int] NOT NULL,
 CONSTRAINT [PK_PTORequest] PRIMARY KEY CLUSTERED 
(
	[PTORequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

