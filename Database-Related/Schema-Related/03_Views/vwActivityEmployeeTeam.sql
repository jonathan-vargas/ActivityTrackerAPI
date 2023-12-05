CREATE VIEW vwActivityEmployeeTeam
AS
SELECT	a.ActivityId, 
		a.Description,
		a.StartedDate,
		a.FinishedDate,
		DATEDIFF(day, a.StartedDate, a.FinishedDate) + 1 AS DurationDays,
		a.EmployeeId,
		e.TeamId
FROM dbo.Activity a
inner join Employee e
on e.EmployeeId = a.EmployeeId