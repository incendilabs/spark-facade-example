USE [SparkFacadeExample]
GO

/****** Object:  Table [dbo].[Patient]    Script Date: 05.10.2021 12:37:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Patient](
	[Id] [uniqueidentifier] NOT NULL,
	[Ssn] [nvarchar](11) NULL,
	[Given] [nvarchar](50) NULL,
	[Surname] [nvarchar](50) NULL,
	[Birthdate] [nvarchar](50) NULL,
	[Gender] [nvarchar](50) NULL,
	[Citizenship] [nvarchar](2) NULL,
	[Phone] [nvarchar](11) NULL,
	[MunicipalityCode] [nvarchar](4) NULL,
	[AddressLine] [nvarchar](50) NULL,
	[ZipCode] [nvarchar](4) NULL,
	[City] [nvarchar](50) NULL,
	[District] [nvarchar](50) NULL,
	[Country] [nvarchar](2) NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO