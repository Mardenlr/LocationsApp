USE [master]
GO

/****** Object:  Database [Locations]    Script Date: 14/04/2022 17:20:29 ******/
CREATE DATABASE [Locations]
GO

USE [Locations]
GO
/****** Object:  Table [dbo].[Addresses]    Script Date: 17/04/2022 15:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ZipCode] [nvarchar](15) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DistrictId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Cities]    Script Date: 17/04/2022 15:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[StateId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Countries]    Script Date: 17/04/2022 15:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Districts]    Script Date: 17/04/2022 15:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Districts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CityId] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_Districts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[States]    Script Date: 17/04/2022 15:58:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShortCode] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Capital] [nvarchar](100) NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Addresses] ON 

GO
INSERT [dbo].[Addresses] ([Id], [ZipCode], [Name], [DistrictId], [CityId], [StateId], [CountryId]) VALUES (1, N'38408292', N'Rua Manoel Ascenço Batista', 1, 1, 1, 5)
GO
INSERT [dbo].[Addresses] ([Id], [ZipCode], [Name], [DistrictId], [CityId], [StateId], [CountryId]) VALUES (2, N'P1A 2S7', N'Borge Ave', 2, 4, 17, 6)
GO
SET IDENTITY_INSERT [dbo].[Addresses] OFF
GO
SET IDENTITY_INSERT [dbo].[Cities] ON 

GO
INSERT [dbo].[Cities] ([Id], [Name], [StateId], [CountryId]) VALUES (1, N'Uberlândia', 1, 5)
GO
INSERT [dbo].[Cities] ([Id], [Name], [StateId], [CountryId]) VALUES (2, N'Vancouver', 7, 6)
GO
INSERT [dbo].[Cities] ([Id], [Name], [StateId], [CountryId]) VALUES (4, N'Toronto', 17, 6)
GO
INSERT [dbo].[Cities] ([Id], [Name], [StateId], [CountryId]) VALUES (5, N'Hamilton', 17, 6)
GO
INSERT [dbo].[Cities] ([Id], [Name], [StateId], [CountryId]) VALUES (6, N'Goiânia', 5, 5)
GO
SET IDENTITY_INSERT [dbo].[Cities] OFF
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (5, N'BRAZIL')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (6, N'CANADA')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (7, N'UNITED STATES OF AMERICA')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (8, N'PORTUGAL')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (9, N'JAPAN')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (10, N'PERU')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (11, N'SOUTH AFRICA')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (12, N'UKRAINE')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (13, N'SPAIN')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (14, N'UNITED KINGDOM')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (15, N'SWEDEN')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (16, N'RUSSIA')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (17, N'NORWAY')
GO
INSERT [dbo].[Countries] ([Id], [Name]) VALUES (18, N'MEXICO')
GO
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Districts] ON 

GO
INSERT [dbo].[Districts] ([Id], [Name], [CityId], [StateId], [CountryId]) VALUES (1, N'Santa Mônica', 1, 1, 5)
GO
INSERT [dbo].[Districts] ([Id], [Name], [CityId], [StateId], [CountryId]) VALUES (2, N'North York', 4, 17, 6)
GO
SET IDENTITY_INSERT [dbo].[Districts] OFF
GO
SET IDENTITY_INSERT [dbo].[States] ON 

GO
INSERT [dbo].[States] ([Id], [ShortCode], [Name], [Capital], [CountryId]) VALUES (1, N'MG', N'Minas Gerais', N'Belo Horizonte', 5)
GO
INSERT [dbo].[States] ([Id], [ShortCode], [Name], [Capital], [CountryId]) VALUES (2, N'SP', N'São Paulo', N'São Paulo', 5)
GO
INSERT [dbo].[States] ([Id], [ShortCode], [Name], [Capital], [CountryId]) VALUES (3, N'RJ', N'Rio de Janeiro', N'Rio de Janeiro', 5)
GO
INSERT [dbo].[States] ([Id], [ShortCode], [Name], [Capital], [CountryId]) VALUES (5, N'GO', N'Goiás', N'Goiânia', 5)
GO
INSERT [dbo].[States] ([Id], [ShortCode], [Name], [Capital], [CountryId]) VALUES (7, N'BC', N'Britsh Columbia', N'Victoria', 6)
GO
INSERT [dbo].[States] ([Id], [ShortCode], [Name], [Capital], [CountryId]) VALUES (17, N'ON', N'Ontario', N'Toronto', 6)
GO
SET IDENTITY_INSERT [dbo].[States] OFF
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_Cities] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_Cities]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_Countries]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_Districts] FOREIGN KEY([DistrictId])
REFERENCES [dbo].[Districts] ([Id])
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_Districts]
GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_States] FOREIGN KEY([StateId])
REFERENCES [dbo].[States] ([Id])
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_States]
GO
ALTER TABLE [dbo].[Cities]  WITH CHECK ADD  CONSTRAINT [FK_Cities_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Cities] CHECK CONSTRAINT [FK_Cities_Countries]
GO
ALTER TABLE [dbo].[Cities]  WITH CHECK ADD  CONSTRAINT [FK_Cities_States] FOREIGN KEY([StateId])
REFERENCES [dbo].[States] ([Id])
GO
ALTER TABLE [dbo].[Cities] CHECK CONSTRAINT [FK_Cities_States]
GO
ALTER TABLE [dbo].[Districts]  WITH CHECK ADD  CONSTRAINT [FK_Districts_Cities] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Districts] CHECK CONSTRAINT [FK_Districts_Cities]
GO
ALTER TABLE [dbo].[Districts]  WITH CHECK ADD  CONSTRAINT [FK_Districts_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[Districts] CHECK CONSTRAINT [FK_Districts_Countries]
GO
ALTER TABLE [dbo].[Districts]  WITH CHECK ADD  CONSTRAINT [FK_Districts_States] FOREIGN KEY([StateId])
REFERENCES [dbo].[States] ([Id])
GO
ALTER TABLE [dbo].[Districts] CHECK CONSTRAINT [FK_Districts_States]
GO
ALTER TABLE [dbo].[States]  WITH CHECK ADD  CONSTRAINT [FK_States_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[States] CHECK CONSTRAINT [FK_States_Countries]
GO
