SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonathan Vargas
-- Create date: 11/28/2023
-- Description:	Saves an Activity (Insert or Update)
-- =============================================
CREATE PROCEDURE spActivitySave
	-- Add the parameters for the stored procedure here
	@ActivityId as int output,
	@Description as nvarchar(max),
	@StartedDate as date,
	@FinishedDate as date,
	@EmployeeId as int,
	@ErrorCodeId as int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @ActivityId > 0
	BEGIN
		-- WE VERIFY IF THE ACTIVITY ID IS VALID
		IF NOT EXISTS (SELECT 1 FROM dbo.Activity WHERE ActivityId = @ActivityId)
		BEGIN
			SET @ErrorCodeId = 102 -- column Id doesn't exist.
			RETURN;
		END
		
		UPDATE a
		SET a.Description = @Description,
			a.StartedDate = @StartedDate,
			a.FinishedDate = @FinishedDate
		from dbo.Activity a
		where a.ActivityId = @ActivityId;
	END;
	ELSE
	BEGIN
		INSERT dbo.Activity(Description, StartedDate,FinishedDate,EmployeeId) 
					values (@Description, @StartedDate, @FinishedDate, @EmployeeId);

		set @ActivityId = SCOPE_IDENTITY();
		SET @ErrorCodeId = 0 -- no error
	END;
END
GO
