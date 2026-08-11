--IF NOT EXISTS(SELECT 1 FROM sys.columns
--          WHERE Name = N'number2'
--          AND Object_ID = Object_ID(N'configuration.DomainObjectEvent'))
--BEGIN

--move to FluentMigrator
--alter table configuration.DomainObjectEvent add number bigint NOT NULL IDENTITY (1, 1)

--END