USE [activityTracker]
GO

ALTER TABLE [dbo].[PTORequest]  WITH CHECK ADD  CONSTRAINT [FkPTORequest_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO

ALTER TABLE [dbo].[PTORequest] CHECK CONSTRAINT [FkPTORequest_EmployeeId]
GO

