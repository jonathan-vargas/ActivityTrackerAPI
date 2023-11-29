USE [activityTracker]
GO

/****** Object:  StoredProcedure [dbo].[spActivitySave]    Script Date: 11/28/2023 9:17:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Jonathan Vargas
-- Create date: 11/28/2023
-- Description:	Saves a PTO Request (Insert or Update)
-- =============================================
CREATE PROCEDURE [dbo].spPTORequestSave
	-- Add the parameters for the stored procedure here
	@PTORequestId as int output,
	@StartedDate as date,
	@FinishedDate as date,
	@PTOStatusId as int,
	@EmployeeId as int,
	@TeamLeadEmployeeId as int,
	@ErrorCodeId as int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @PTORequestId > 0
	BEGIN
		-- WE VERIFY IF THE PTORequest ID IS VALID
		IF NOT EXISTS (SELECT 1 FROM dbo.PTORequest WHERE PTORequestId = @PTORequestId)
		BEGIN
			SET @ErrorCodeId = 102 -- column Id doesn't exist.
			RETURN;
		END
		
		-- WE verify if the PTOStatusId is valid. 
		IF @PTOStatusId = 2 -- 1 is Pending(1), Approved(2), Canceled(3)
		BEGIN
			SET @ErrorCodeId = 103 -- invalid value
			RETURN;
		END


		UPDATE p
		SET p.StartedDate = @StartedDate,
			p.FinishedDate = @FinishedDate,
			p.PTOStatusId = @PTOStatusId
		from dbo.PTORequest p
		where p.PTORequestId = @PTORequestId;
	END;
	ELSE
	BEGIN
		INSERT dbo.PTORequest(StartedDate,FinishedDate,EmployeeId, TeamLeadEmployeeId) 
					values (@StartedDate, @FinishedDate, @EmployeeId, @TeamLeadEmployeeId);

		set @PTORequestId = SCOPE_IDENTITY();
		SET @ErrorCodeId = 0 -- no error
	END;
END
GO


