USE [activityTracker]
GO

/****** Object:  StoredProcedure [dbo].[spPTORequestSave]    Script Date: 11/28/2023 9:39:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Jonathan Vargas
-- Create date: 11/28/2023
-- Description:	Processes PTO Request (Accepted or cancelled)
-- =============================================
CREATE PROCEDURE [dbo].spPTORequestProcess
	-- Add the parameters for the stored procedure here
	@PTORequestId as int output,
	@PTOStatusId as int,
	@ErrorCodeId as int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM dbo.PTORequest WHERE PTORequestId = @PTORequestId)
	BEGIN
		SET @ErrorCodeId = 102 -- column Id doesn't exist.
		RETURN;
	END
		
	-- We verify if the PTOStatusId is valid. 
	IF @PTOStatusId = 1 -- 1 is Pending(1), Approved(2), Canceled(3)
	BEGIN
		SET @ErrorCodeId = 103 -- invalid value
		RETURN;
	END

	UPDATE p
	SET p.PTOStatusId = @PTOStatusId
	FROM dbo.PTORequest p
	where p.PTORequestId = @PTORequestId;
	set @ErrorCodeId = 0;
END
GO


