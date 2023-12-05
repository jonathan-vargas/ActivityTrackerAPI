SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jonathan Vargas
-- Create date: 11/28/2023
-- Description:	Deletes an activity based on ActivityId
-- =============================================
CREATE PROCEDURE spActivityDeleteByActivityId 
	-- Add the parameters for the stored procedure here
	@ActivityId as int,
	@ErrorCodeId as int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @ActivityId <= 0
	BEGIN
		SET @ErrorCodeId = 101; -- invalid parameter value
		return;
	END;

	DELETE
	FROM dbo.Activity
	WHERE ActivityId = @ActivityId;
	SET @ErrorCodeId = 0;
END
GO
