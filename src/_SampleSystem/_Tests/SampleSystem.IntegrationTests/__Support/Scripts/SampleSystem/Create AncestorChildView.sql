CREATE VIEW [app].[BusinessUnitToAncestorChildView]
AS
SELECT sourceId = [ancestorId], childOrAncestorId = childid
FROM [app].[BusinessUnitAncestorLink]
UNION
SELECT sourceId = [childId], childOrAncestorId = ancestorId
FROM [app].[BusinessUnitAncestorLink]

GO

CREATE VIEW [app].[ManagementUnitToAncestorChildView]
AS
SELECT sourceId = [ancestorId], childOrAncestorId = childid
FROM [app].[ManagementUnitAncestorLink]
UNION
SELECT sourceId = [childId], childOrAncestorId = ancestorId
FROM [app].[ManagementUnitAncestorLink]

GO

CREATE VIEW [app].[LocationToAncestorChildView]
AS
SELECT sourceId = [ancestorId], childOrAncestorId = childid
FROM [app].[LocationAncestorLink]
UNION
SELECT sourceId = [childId], childOrAncestorId = ancestorId
FROM [app].[LocationAncestorLink]

GO