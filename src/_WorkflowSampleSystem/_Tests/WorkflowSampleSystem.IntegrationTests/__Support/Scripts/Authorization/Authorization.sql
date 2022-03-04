﻿SET IDENTITY_INSERT [appAudit].[AuditRevisionEntity] ON

INSERT [auth].[BusinessRole] ([active], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (1, CAST(N'2014-09-17T01:06:42.607' AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'SystemIntegration', N'df74d544-5945-4380-944e-a3a9001252be', N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2014-09-17T01:08:23.963' AS DateTime), N'SystemIntegration')
INSERT [auth].[BusinessRole] ([active], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (1, NULL, NULL, N'', N'd9c1d2f0-0c2f-49ab-bb0b-de13a456169e', N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2015-12-18T16:10:56.867' AS DateTime), N'Administrator')
INSERT [auth].[BusinessRole] ([active], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (1, CAST(0x0000A07500FA6D0F AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', NULL, N'8fd79f66-218a-47bc-9649-a07500fa6d11', N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A07500FA7726 AS DateTime), N'SecretariatNotification')
INSERT [auth].[BusinessRole] ([active], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (1, CAST(0x0000A32D00BD852B AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'', N'dbf3556d-7106-4175-b5e4-a32d00bd857a', N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A4C9010F6771 AS DateTime), N'SE Manager')

INSERT [auth].[EntityType] ([active], [createDate], [createdBy], [expandable], [id], [isFilter], [modifiedBy], [modifyDate], [name]) VALUES (1, NULL, NULL, 1, N'263d2c60-7bce-45d6-a0af-a0830152353e', 1, NULL, NULL, N'BusinessUnit')
INSERT [appAudit].[AuditRevisionEntity] ([Id], [Author], [RevisionDate]) VALUES (1660, N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A2F600139229 AS DateTime))
INSERT [authAudit].[EntityTypeAudit] ([active], [active_MOD], [createDate], [createDate_MOD], [createdBy], [createdBy_MOD], [expandable], [expandable_MOD], [id], [isFilter], [isFilter_MOD], [modifiedBy], [modifiedBy_MOD], [modifyDate], [modifyDate_MOD], [name], [name_MOD], [REV], [REVTYPE])  VALUES (1, 0, NULL, 0, NULL, 0, 1, 0, N'263d2c60-7bce-45d6-a0af-a0830152353e', 1, 0, NULL, 0, NULL, 0, N'BusinessUnit', 0, 1660, 0)

INSERT [auth].[EntityType] ([active], [createDate], [createdBy], [expandable], [id], [isFilter], [modifiedBy], [modifyDate], [name]) VALUES (1, NULL, NULL, 1, N'4641395b-9079-448e-9cb8-a083015235a3', 1, N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A0EB015A2F58 AS DateTime), N'Location')
INSERT [appAudit].[AuditRevisionEntity] ([Id], [Author], [RevisionDate]) VALUES (1661, N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A2F600139229 AS DateTime))
INSERT [authAudit].[EntityTypeAudit] ([active], [active_MOD], [createDate], [createDate_MOD], [createdBy], [createdBy_MOD], [expandable], [expandable_MOD], [id], [isFilter], [isFilter_MOD], [modifiedBy], [modifiedBy_MOD], [modifyDate], [modifyDate_MOD], [name], [name_MOD], [REV], [REVTYPE])  VALUES (1, 0, NULL, 0, NULL, 0, 1, 0, N'4641395b-9079-448e-9cb8-a083015235a3', 1, 0, N'NT AUTHORITY\NETWORK SERVICE', 0, CAST(0x0000A0EB015A2F58 AS DateTime), 0, N'Location', 0, 1661, 0)

INSERT [auth].[Operation] ([active], [approveOperationId], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (1, NULL, CAST(N'2014-09-16T23:46:12.850' AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'Display Internal Error', N'ab8afd01-40d2-48d0-b5f3-a12177b00d0d', N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2014-09-16T23:46:14.463' AS DateTime), N'DisplayInternalError')
INSERT [auth].[Operation] ([active], [approveOperationId], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (1, NULL, CAST(N'2014-09-16T23:46:12.850' AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'Can integrate', N'0ba8a6b0-43b9-4f59-90ce-2fcbe37b97c9', N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2014-09-16T23:46:14.463' AS DateTime), N'SystemIntegration')
INSERT [auth].[Operation] ([active], [approveOperationId], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (1, NULL, CAST(N'2014-09-16T23:46:12.850' AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'Process Modifications', N'c1ef265c-d118-4fce-a251-27a542787449', N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2014-09-16T23:46:14.463' AS DateTime), N'ProcessModifications')
INSERT [auth].[Operation] ([active], [approveOperationId], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (1, NULL, CAST(N'2012-06-18T20:43:23.047' AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'ChangeBUSecretariatNotification', N'31254abe-1ae4-480e-ae79-5a25bb7c0e86', N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2012-06-18T20:43:23.473' AS DateTime), N'ChangeBUSecretariatNotification')
INSERT [auth].[Operation] ([active], [approveOperationId], [createDate], [createdBy], [description], [id], [modifiedBy], [modifyDate], [name]) VALUES (0, NULL, CAST(0x00009E7F013BEE20 AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'Can see employees working in BU', N'c73dd1f6-74d5-4445-a265-2e96832a7f89', N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A2F5017E2472 AS DateTime), N'EmployeeView')

INSERT [auth].[BusinessRoleOperationLink] ([active], [businessRoleId], [createDate], [createdBy], [id], [isDenormalized], [modifiedBy], [modifyDate], [operationId]) VALUES (1, N'df74d544-5945-4380-944e-a3a9001252be', CAST(N'2014-09-17T01:07:53.197' AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'ec3e2249-6213-4761-8377-5c54c575c6a2', 0, N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2014-09-17T01:07:53.257' AS DateTime), N'c1ef265c-d118-4fce-a251-27a542787449')
INSERT [auth].[BusinessRoleOperationLink] ([active], [businessRoleId], [createDate], [createdBy], [id], [isDenormalized], [modifiedBy], [modifyDate], [operationId]) VALUES (1, N'df74d544-5945-4380-944e-a3a9001252be', CAST(N'2014-09-17T01:07:53.197' AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'78726998-23d0-46d5-9f40-a3a90012a558', 0, N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2014-09-17T01:07:53.257' AS DateTime), N'0ba8a6b0-43b9-4f59-90ce-2fcbe37b97c9')
INSERT [auth].[BusinessRoleOperationLink] ([active], [businessRoleId], [createDate], [createdBy], [id], [isDenormalized], [modifiedBy], [modifyDate], [operationId]) VALUES (1, N'df74d544-5945-4380-944e-a3a9001252be', CAST(N'2014-09-17T01:08:23.947' AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'70b5c937-fad4-4102-ae9b-a3a90012c954', 0, N'NT AUTHORITY\NETWORK SERVICE', CAST(N'2014-09-17T01:08:23.960' AS DateTime), N'ab8afd01-40d2-48d0-b5f3-a12177b00d0d')
INSERT [auth].[BusinessRoleOperationLink] ([active], [businessRoleId], [createDate], [createdBy], [id], [isDenormalized], [modifiedBy], [modifyDate], [operationId]) VALUES (1, N'8fd79f66-218a-47bc-9649-a07500fa6d11', CAST(0x0000A07500FA7722 AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'e9bbb890-a43e-49ed-885d-a07500fa7725', 0, N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A30600EEF3F6 AS DateTime), N'31254abe-1ae4-480e-ae79-5a25bb7c0e86')
INSERT [auth].[BusinessRoleOperationLink] ([active], [businessRoleId], [createDate], [createdBy], [id], [isDenormalized], [modifiedBy], [modifyDate], [operationId]) VALUES (1, N'dbf3556d-7106-4175-b5e4-a32d00bd857a', CAST(0x0000A32D00BD9DA7 AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'76e8faa8-78f8-47e1-824d-a32d00bd9dd1', 0, N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A32D00BD9DD2 AS DateTime), N'c73dd1f6-74d5-4445-a265-2e96832a7f89')
INSERT [auth].[BusinessRoleOperationLink] ([active], [businessRoleId], [createDate], [createdBy], [id], [isDenormalized], [modifiedBy], [modifyDate], [operationId]) VALUES (1, N'd9c1d2f0-0c2f-49ab-bb0b-de13a456169e', CAST(0x0000A32D00BD9DA7 AS DateTime), N'NT AUTHORITY\NETWORK SERVICE', N'76e8faa8-78f8-47e1-824d-a32d00bd9dd2', 0, N'NT AUTHORITY\NETWORK SERVICE', CAST(0x0000A32D00BD9DD2 AS DateTime), N'c73dd1f6-74d5-4445-a265-2e96832a7f89')

SET IDENTITY_INSERT [appAudit].[AuditRevisionEntity] OFF