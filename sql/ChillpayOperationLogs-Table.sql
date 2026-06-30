/*
  Chillpay Operation Logs — Table + MerchantId migrate
  เอกสาร: docs/Chillpay-Operation-Logs.md Table script
*/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ChillpayOperationLogs')
BEGIN
    CREATE TABLE [dbo].[ChillpayOperationLogs](
        [Id]              [bigint] IDENTITY(1,1) NOT NULL,
        [ModuleType]      [int] NOT NULL,
        [MenuType]        [int] NOT NULL,
        [LogType]         [int] NOT NULL,
        [Message]         [nvarchar](500) NULL,
        [OldValue]        [nvarchar](max) NULL,
        [NewValue]        [nvarchar](max) NULL,
        [DataId]          [bigint] NULL,
        [RefType]         [int] NULL,
        [RefId]           [bigint] NULL,
        [Ref2Type]        [int] NULL,
        [Ref2Id]          [bigint] NULL,
        [MerchantId]      [bigint] NULL,
        [RequestSystem]   [nvarchar](20) NOT NULL,
        [RequestBy]       [bigint] NOT NULL,
        [RequestByName]   [nvarchar](200) NULL,
        [AddedDate]       [datetime] NOT NULL CONSTRAINT [DF_ChillpayOperationLogs_AddedDate] DEFAULT (GETDATE()),
        CONSTRAINT [PK_ChillpayOperationLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ChillpayOperationLogs') AND name = 'MerchantId')
BEGIN
    ALTER TABLE [dbo].[ChillpayOperationLogs] ADD [MerchantId] [bigint] NULL;
END
GO

-- backfill MerchantId สำหรับ row เก่า (Module Merchant)
UPDATE [dbo].[ChillpayOperationLogs]
SET [MerchantId] = COALESCE(
        CASE WHEN [RefType] = 20000 THEN [RefId] END,
        CASE WHEN [Ref2Type] = 20000 THEN [Ref2Id] END,
        CASE WHEN [MenuType] = 200 THEN [DataId] END
    )
WHERE [ModuleType] = 2
  AND [MerchantId] IS NULL;
GO
