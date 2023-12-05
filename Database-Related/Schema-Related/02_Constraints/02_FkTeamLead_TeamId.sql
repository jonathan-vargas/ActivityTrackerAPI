USE [activityTracker]
GO

ALTER TABLE [dbo].[TeamLead]  WITH CHECK ADD  CONSTRAINT [FkTeamLead_TeamId] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Team] ([TeamId])
GO

ALTER TABLE [dbo].[TeamLead] CHECK CONSTRAINT [FkTeamLead_TeamId]
GO

