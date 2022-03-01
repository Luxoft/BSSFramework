

/*
-- Create and populate Full Text Catalog

CREATE FULLTEXT CATALOG [SampleSystem]WITH ACCENT_SENSITIVITY = OFF AUTHORIZATION [dbo]
CREATE FULLTEXT INDEX ON [app].[Employee] KEY INDEX [PK_Employee] ON ([SampleSystem]) WITH (CHANGE_TRACKING AUTO)
ALTER FULLTEXT INDEX ON [app].[Employee] ADD ([email])
ALTER FULLTEXT INDEX ON [app].[Employee] ENABLE
*/

ALTER TABLE [app].[SqlParserTestObj] ALTER COLUMN [notNullColumn] [nvarchar](255) NOT NULL