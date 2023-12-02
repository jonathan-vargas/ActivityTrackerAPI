USE [activityTracker]
GO

ALTER TABLE [dbo].[Team]  WITH CHECK ADD  CONSTRAINT [FkTeam_TeamLeadEmployeeId] FOREIGN KEY([TeamLeadEmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO

ALTER TABLE [dbo].[Team] CHECK CONSTRAINT [FkTeam_TeamLeadEmployeeId]
GO

