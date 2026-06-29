/*
  Chillpay Operation Logs — Indexes
  เอกสาร: docs/Chillpay-Operation-Logs.md §7.4.2
  ต้องรัน Table script ก่อน
*/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChillpayOperationLogs_Module_Menu' AND object_id = OBJECT_ID('dbo.ChillpayOperationLogs'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ChillpayOperationLogs_Module_Menu]
        ON [dbo].[ChillpayOperationLogs]([ModuleType] ASC, [MenuType] ASC, [AddedDate] DESC);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChillpayOperationLogs_LogType' AND object_id = OBJECT_ID('dbo.ChillpayOperationLogs'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ChillpayOperationLogs_LogType]
        ON [dbo].[ChillpayOperationLogs]([LogType] ASC, [AddedDate] DESC);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChillpayOperationLogs_DataId' AND object_id = OBJECT_ID('dbo.ChillpayOperationLogs'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ChillpayOperationLogs_DataId]
        ON [dbo].[ChillpayOperationLogs]([DataId] ASC)
        WHERE [DataId] IS NOT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChillpayOperationLogs_RefType_RefId' AND object_id = OBJECT_ID('dbo.ChillpayOperationLogs'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ChillpayOperationLogs_RefType_RefId]
        ON [dbo].[ChillpayOperationLogs]([RefType] ASC, [RefId] ASC)
        WHERE [RefType] IS NOT NULL AND [RefId] IS NOT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ChillpayOperationLogs_MerchantId' AND object_id = OBJECT_ID('dbo.ChillpayOperationLogs'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ChillpayOperationLogs_MerchantId]
        ON [dbo].[ChillpayOperationLogs]([MerchantId] ASC)
        WHERE [MerchantId] IS NOT NULL;
END
GO
