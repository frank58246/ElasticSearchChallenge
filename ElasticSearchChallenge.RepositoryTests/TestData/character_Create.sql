USE [character]

/****** Object:  Table [dbo].[character]    Script Date: 2021/3/1 上午 09:58:52 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[character](
	[SID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Sex] [nvarchar](50) NOT NULL,
	[Family] [nvarchar](50) NOT NULL,
	[Birthdate] [datetime2](7) NOT NULL,
	[Origin] [nvarchar](50) NOT NULL,
	[Age] [float] NOT NULL
) ON [PRIMARY]

