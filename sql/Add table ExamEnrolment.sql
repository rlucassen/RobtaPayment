CREATE TABLE [dbo].[ExamEnrolment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[StudentNumber] [nvarchar](255) NULL,
	[EnrolmentDate] [datetime] NULL,
	[Active] [bit] NULL,
	[Preposition] [nvarchar](255) NULL,
	[Lastname] [nvarchar](255) NULL,
	[Transaction] [int] NULL,
	[Exam] [int] NULL,
	[Present] [bit] NULL,
	[Address] [nvarchar](255) NULL,
	[Place] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ExamEnrolment]  WITH CHECK ADD  CONSTRAINT [FK776C8B3E1003FF] FOREIGN KEY([Transaction])
REFERENCES [dbo].[Transaction] ([Id])
GO

ALTER TABLE [dbo].[ExamEnrolment] CHECK CONSTRAINT [FK776C8B3E1003FF]
GO

ALTER TABLE [dbo].[ExamEnrolment]  WITH CHECK ADD  CONSTRAINT [FK776C8B3E7E8F9D50] FOREIGN KEY([Exam])
REFERENCES [dbo].[Exam] ([Id])
GO

ALTER TABLE [dbo].[ExamEnrolment] CHECK CONSTRAINT [FK776C8B3E7E8F9D50]
GO


