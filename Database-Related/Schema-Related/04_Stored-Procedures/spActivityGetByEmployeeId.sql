USE [activityTracker]
GO
/****** Object:  StoredProcedure [dbo].[spActivityGetByEmployeeId]    Script Date: 11/28/2023 9:12:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spActivityGetByEmployeeId] 
	-- Add the parameters for the stored procedure here
	@EmployeeId as int,
	@ErrorCodeId as int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @EmployeeId <= 0
	BEGIN
		SET @ErrorCodeId = 101; -- Invalid Parameter Value
		return;
	END


    -- Insert statements for procedure here
	SELECT	a.ActivityId, 
			a.StartedDate, 
			a.FinishedDate, 
			a.Description, 
			a.EmployeeId
	FROM dbo.Activity a
	where a.EmployeeId = @EmployeeId;
	SET @ErrorCodeId = 0;
END
