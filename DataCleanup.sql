-- DROP CONSTRAINTS

ALTER TABLE [dbo].[facts_Applications] DROP CONSTRAINT [FK_facts_Applications_dim_Ages]
GO

ALTER TABLE [dbo].[facts_Applications] DROP CONSTRAINT [FK_facts_Applications_dim_EducationTypes]
GO

ALTER TABLE [dbo].[facts_Applications] DROP CONSTRAINT [FK_facts_Applications_dim_IncomeTypes]
GO

ALTER TABLE [dbo].[facts_Applications] DROP CONSTRAINT [FK_facts_Applications_dim_Occupations]
GO

-- TRUNCATE TABLES

TRUNCATE TABLE dbo.dim_Ages
TRUNCATE TABLE dbo.dim_EducationTypes
TRUNCATE TABLE dbo.dim_IncomeTypes
TRUNCATE TABLE dbo.dim_Occupations
TRUNCATE TABLE dbo.facts_Applications

-- ENABLE CONSTRAINTS

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

