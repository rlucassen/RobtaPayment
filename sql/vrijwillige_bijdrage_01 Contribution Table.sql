/*
   vrijdag 21 september 201211:47:02
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
CREATE TABLE dbo.Contribution
	(
	Id int NOT NULL IDENTITY (1, 1),
	Guid uniqueidentifier NOT NULL,
	Price decimal(19, 5) NOT NULL,
	ContributionType int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Contribution ADD CONSTRAINT
	PK_Contribution PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Contribution SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Contribution', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Contribution', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Contribution', 'Object', 'CONTROL') as Contr_Per 