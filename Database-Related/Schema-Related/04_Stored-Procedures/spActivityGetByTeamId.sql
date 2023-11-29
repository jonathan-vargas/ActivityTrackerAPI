SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonathan Vargas
-- Create date: 11/28/2023
-- Description:	Gets a list of activities based on the TeamId the employees belong to
-- =============================================
CREATE PROCEDURE spActivityGetByTeamId 
	-- Add the parameters for the stored procedure here
	@TeamId as int,
	@ErrorCodeId as int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @TeamId <= 0
	BEGIN
		SET @ErrorCodeId = 101; -- invalid parameter value
		return;
	END


    -- Insert statements for procedure here
	SELECT	a.ActivityId, 
			a.StartedDate, 
			a.FinishedDate, 
			a.Description, 
			a.EmployeeId
	FROM dbo.Activity a
	inner join dbo.Employee e
	on e.EmployeeId = a.EmployeeId
	where e.TeamId = @TeamId;
	SET @ErrorCodeId = 0;
END
GO
