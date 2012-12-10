/*
   vrijdag 21 september 201211:51:00
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
ALTER TABLE dbo.[Transaction] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.[Transaction]', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.[Transaction]', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.[Transaction]', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Contribution SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Contribution', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Contribution', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Contribution', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
CREATE TABLE dbo.ContributionEnrolment
	(
	Id int NOT NULL IDENTITY (1, 1),
	Guid uniqueidentifier NOT NULL,
	Name nvarchar(255) NOT NULL,
	Email nvarchar(255) NOT NULL,
	StudentNumber nvarchar(255) NOT NULL,
	EnrolmentDate nvarchar(255) NOT NULL,
	Contribution int NULL,
	[Transaction] int NULL,
	Active bit NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ContributionEnrolment ADD CONSTRAINT
	PK_ContributionEnrolment PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ContributionEnrolment ADD CONSTRAINT
	FK_Contribution FOREIGN KEY
	(
	Contribution
	) REFERENCES dbo.Contribution
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ContributionEnrolment ADD CONSTRAINT
	FK_Transaction FOREIGN KEY
	(
	[Transaction]
	) REFERENCES dbo.[Transaction]
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ContributionEnrolment SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ContributionEnrolment', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ContributionEnrolment', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ContributionEnrolment', 'Object', 'CONTROL') as Contr_Per 