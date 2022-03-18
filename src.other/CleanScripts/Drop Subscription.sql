begin tran

ALTER TABLE [configuration].[SubBusinessRole] DROP CONSTRAINT [FK_SubBusinessRole_subscriptionId_Subscription]
ALTER TABLE [configuration].[Subscription] DROP CONSTRAINT [FK_Subscription_domainTypeId_DomainType]
ALTER TABLE [configuration].[Subscription] DROP CONSTRAINT [FK_Subscription_conditionId_SubscriptionLambda]
ALTER TABLE [configuration].[Subscription] DROP CONSTRAINT [FK_Subscription_messageTemplateId_MessageTemplate]
ALTER TABLE [configuration].[Subscription] DROP CONSTRAINT [FK_Subscription_generationId_SubscriptionLambda]
ALTER TABLE [configuration].[Subscription] DROP CONSTRAINT [FK_Subscription_copyGenerationId_SubscriptionLambda]
ALTER TABLE [configuration].[Subscription] DROP CONSTRAINT [FK_Subscription_replyToGenerationId_SubscriptionLambda]
ALTER TABLE [configuration].[Subscription] DROP CONSTRAINT [FK_Subscription_dynamicSourceId_SubscriptionLambda]
ALTER TABLE [configuration].[Subscription] DROP CONSTRAINT [FK_Subscription_attachmentId_SubscriptionLambda]
ALTER TABLE [configuration].[SubscriptionLambda] DROP CONSTRAINT [FK_SubscriptionLambda_domainTypeId_DomainType]
ALTER TABLE [configuration].[SubscriptionSecurityItem] DROP CONSTRAINT [FK_SubscriptionSecurityItem_subscriptionId_Subscription]
ALTER TABLE [configuration].[SubscriptionSecurityItem] DROP CONSTRAINT [FK_SubscriptionSecurityItem_sourceId_SubscriptionLambda]

DROP TABLE [configuration].SubBusinessRole
DROP TABLE [configuration].Subscription
DROP TABLE [configuration].SubscriptionLambda
DROP TABLE [configuration].SubscriptionSecurityItem

commit tran