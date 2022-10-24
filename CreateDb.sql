/****** Object:  Table [dbo].[dim_Ages]    Script Date: 07/10/2022 17:41:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dim_Ages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Age] [int] NOT NULL,
 CONSTRAINT [PK_dim_AgesTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[dim_EducationTypes]    Script Date: 07/10/2022 17:41:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dim_EducationTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EducationType] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dim_EducationTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[dim_IncomeTypes]    Script Date: 07/10/2022 17:41:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dim_IncomeTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IncomeType] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dim_IncomeTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[dim_Occupations]    Script Date: 07/10/2022 17:41:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dim_Occupations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Occupation] [varchar](255) NOT NULL,
 CONSTRAINT [PK_dim_Occupations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[facts_Applications]    Script Date: 07/10/2022 17:41:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[facts_Applications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExternalId] [varchar](50) NOT NULL,
	[Gender] [char](1) NOT NULL,
	[OwnCar] [bit] NOT NULL,
	[OwnProperty] [bit] NOT NULL,
	[Children] [int] NOT NULL,
	[Income] [money] NOT NULL,
	[FamilyStatus] [varchar](50) NOT NULL,
	[HousingType] [varchar](50) NOT NULL,
	[DaysEmployed] [int] NOT NULL,
	[MobilePhone] [bit] NOT NULL,
	[WorkPhone] [bit] NOT NULL,
	[FlaggedPhone] [bit] NOT NULL,
	[FlaggedEmail] [bit] NOT NULL,
	[FamilyMembers] [int] NOT NULL,
	[AgeId] [int] NOT NULL,
	[EducationId] [int] NOT NULL,
	[IncomeTypeId] [int] NOT NULL,
	[OccupationId] [int] NULL,
	[IsApproved] [bit] NOT NULL,
 CONSTRAINT [PK_facts_Applications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ExternalID_facts_Applications]    Script Date: 07/10/2022 17:41:00 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ExternalID_facts_Applications] ON [dbo].[facts_Applications]
(
	[ExternalId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO
/****** Object:  Index [ColIX_facts_Applications]    Script Date: 07/10/2022 17:41:00 ******/
CREATE NONCLUSTERED COLUMNSTORE INDEX [ColIX_facts_Applications] ON [dbo].[facts_Applications]
(
	[ExternalId],
	[Gender],
	[OwnCar],
	[OwnProperty],
	[Children],
	[Income],
	[FamilyStatus],
	[HousingType],
	[DaysEmployed],
	[MobilePhone],
	[WorkPhone],
	[FlaggedPhone],
	[FlaggedEmail],
	[FamilyMembers],
	[AgeId],
	[EducationId],
	[IncomeTypeId],
	[OccupationId],
	[IsApproved]
)WITH (DROP_EXISTING = OFF, COMPRESSION_DELAY = 0) ON [PRIMARY]
GO
ALTER TABLE [dbo].[facts_Applications] ADD  CONSTRAINT [DF_facts_Applications_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[facts_Applications]  WITH CHECK ADD  CONSTRAINT [FK_facts_Applications_dim_Ages] FOREIGN KEY([AgeId])
REFERENCES [dbo].[dim_Ages] ([Id])
GO
ALTER TABLE [dbo].[facts_Applications] CHECK CONSTRAINT [FK_facts_Applications_dim_Ages]
GO
ALTER TABLE [dbo].[facts_Applications]  WITH CHECK ADD  CONSTRAINT [FK_facts_Applications_dim_EducationTypes] FOREIGN KEY([EducationId])
REFERENCES [dbo].[dim_EducationTypes] ([Id])
GO
ALTER TABLE [dbo].[facts_Applications] CHECK CONSTRAINT [FK_facts_Applications_dim_EducationTypes]
GO
ALTER TABLE [dbo].[facts_Applications]  WITH CHECK ADD  CONSTRAINT [FK_facts_Applications_dim_IncomeTypes] FOREIGN KEY([IncomeTypeId])
REFERENCES [dbo].[dim_IncomeTypes] ([Id])
GO
ALTER TABLE [dbo].[facts_Applications] CHECK CONSTRAINT [FK_facts_Applications_dim_IncomeTypes]
GO
ALTER TABLE [dbo].[facts_Applications]  WITH CHECK ADD  CONSTRAINT [FK_facts_Applications_dim_Occupations] FOREIGN KEY([OccupationId])
REFERENCES [dbo].[dim_Occupations] ([Id])
GO
ALTER TABLE [dbo].[facts_Applications] CHECK CONSTRAINT [FK_facts_Applications_dim_Occupations]
GO
