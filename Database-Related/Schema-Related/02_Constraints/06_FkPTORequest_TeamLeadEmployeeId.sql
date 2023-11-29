USE [activityTracker]
GO

ALTER TABLE [dbo].[PTORequest]  WITH CHECK ADD  CONSTRAINT [FkPTORequest_TeamLeadEmployeeId] FOREIGN KEY([PTORequestId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO

ALTER TABLE [dbo].[PTORequest] CHECK CONSTRAINT [FkPTORequest_TeamLeadEmployeeId]
GO

