/*
   woensdag 19 september 20129:20:54
   User: 
   Server: statler
   Database: glr
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Building ADD
	Slug nvarchar(255) NULL
GO
ALTER TABLE dbo.Building SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
