USE [activityTracker]
GO

ALTER TABLE [dbo].[TeamLead]  WITH CHECK ADD  CONSTRAINT [FkTeamLead_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO

ALTER TABLE [dbo].[TeamLead] CHECK CONSTRAINT [FkTeamLead_EmployeeId]
GO

