USE [character]
GO
/****** Object:  Table [dbo].[character]    Script Date: 2021/3/1 上午 09:58:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[character](
	[SID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Sex] [nvarchar](50) NOT NULL,
	[Family] [nvarchar](50) NOT NULL,
	[Birthdate] [datetime2](7) NOT NULL,
	[Origin] [nvarchar](50) NOT NULL,
	[Age] [float] NOT NULL
) ON [PRIMARY]
GO
