USE [FTD]
GO

SELECT r.[Id]
      ,[IdTo]
      ,[IdFrom]
      ,r.[Identifier]
      ,[IdRequestor]
      ,[Date]
      ,[LastModified]
      ,[Subject]
      ,[Message]
      ,[IdStatus]
      ,r.[IdFlowType]
      ,[IdFlowSteps]
      ,[IsCompleted],
	  df.Name departmentFrom,
	  dt.Name departmentTo,
	  e.Name requestor,
	  s.Name [status],
	  ft.Name flowtype,
	  fs.StepName step,
	  df.Identifier dFromIdentifier,
	  ef.Name directorFrom,
	  et.Name directorTo
  FROM [dbo].[Requests] r
  INNER JOIN Departments df on df.Id = r.IdFrom 
  INNER JOIN Departments dt on dt.Id = r.IdTo
  INNER JOIN Employees e on e.Id = r.IdRequestor
  INNER JOIN [Status] s on s.Id = r.IdStatus
  INNER JOIN FlowTypes ft on ft.Id = r.IdFlowType
  INNER JOIN FlowSteps fs on fs.Id = r.IdFlowSteps
  INNER JOIN Employees ef on df.IdDirector = ef.Id
  INNER JOIN Employees et on dt.IdDirector = et.Id


  USE [FTD]
GO

SELECT [Id]
      ,[Name]
      ,[IdRequest]
      ,[ByteContent]
      ,[ContentType]
      ,[IdAttachmentType]
  FROM [dbo].[Attachments]
GO

USE [FTD]
GO

SELECT [Id]
      ,[IdRequest]
      ,[DateApproval]
      ,[IdEmployee]
      ,[Comments]
      ,[IdStatus]
      ,[IdFlowSteps]
  FROM [dbo].[RequestHistories]
 INNER JOIN 




