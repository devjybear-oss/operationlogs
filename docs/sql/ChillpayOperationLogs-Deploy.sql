/* Chillpay Operation Logs — Full deploy (Table + Index + View) */
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

/*
  Chillpay Operation Logs — Indexes
  เอกสาร: docs/Chillpay-Operation-Logs.md Index script
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

/*
  Chillpay Operation Logs — View VW_ChillpayOperationLogs
  เอกสาร: docs/Chillpay-Operation-Logs.md View script
  ต้องมีตาราง ChillpayOperationLogs + Merchants ก่อนรัน
*/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER VIEW [dbo].[VW_ChillpayOperationLogs]
AS
SELECT b.*
    , (CONVERT(varchar(10), b.[AddedDate], 105) + N' ' + CONVERT(varchar(8), b.[AddedDate], 8)) AS [AddedDateText]
    , (CASE b.[ModuleType]
        WHEN 1 THEN N'User'
        WHEN 2 THEN N'Merchant'
        WHEN 3 THEN N'Settings'
        WHEN 4 THEN N'Transactions'
        WHEN 5 THEN N'Settlements'
        WHEN 6 THEN N'Manage Transaction'
        WHEN 7 THEN N'PayLink'
        WHEN 8 THEN N'Fraud'
        WHEN 9 THEN N'Etax'
        WHEN 10 THEN N'Odd Service'
        WHEN 11 THEN N'Chill App Partner'
        WHEN 12 THEN N'Wallet'
        WHEN 13 THEN N'Recurring'
        WHEN 14 THEN N'Commission'
        ELSE CAST(b.[ModuleType] AS nvarchar(20))
      END) AS [ModuleTypeText]
    , (CASE b.[MenuType]
        WHEN 101 THEN N'Account'
        WHEN 102 THEN N'Role'
        WHEN 200 THEN N'Merchant'
        WHEN 201 THEN N'Merchant User'
        WHEN 202 THEN N'Merchant Fee'
        WHEN 203 THEN N'Merchant Route'
        WHEN 204 THEN N'Merchant Service Fee'
        WHEN 205 THEN N'Generate Merchant Keys'
        WHEN 206 THEN N'Merchant Emails'
        WHEN 300 THEN N'Payment Channel'
        WHEN 301 THEN N'Payment Route'
        WHEN 302 THEN N'Payment Route Inquiry'
        WHEN 303 THEN N'Credit Card Config'
        WHEN 305 THEN N'Switch Payment Route Channel'
        WHEN 306 THEN N'ChillPay Maintenance'
        WHEN 307 THEN N'Bank Maintenance'
        WHEN 308 THEN N'Bank Payment Api Setting'
        WHEN 309 THEN N'SMS Route'
        WHEN 310 THEN N'Exchange Rate'
        WHEN 311 THEN N'Exchange Rate Log'
        WHEN 401 THEN N'Payment'
        WHEN 402 THEN N'Void'
        WHEN 403 THEN N'Refund (Old)'
        WHEN 404 THEN N'Refund Transaction'
        WHEN 405 THEN N'Approve Refund'
        WHEN 406 THEN N'Settlement (Old)'
        WHEN 407 THEN N'Payment Summary (Old)'
        WHEN 408 THEN N'Import MCC'
        WHEN 409 THEN N'Inquiry Transaction'
        WHEN 410 THEN N'Warning Transaction'
        WHEN 501 THEN N'Settlement Dashboard'
        WHEN 502 THEN N'Settlement'
        WHEN 503 THEN N'Upload File'
        WHEN 504 THEN N'Merchant Transfer'
        WHEN 505 THEN N'Merchant Setting'
        WHEN 506 THEN N'Settlement Status'
        WHEN 507 THEN N'Download Payment Summary'
        WHEN 601 THEN N'Update Transaction Status'
        WHEN 602 THEN N'Update Settlement Status'
        WHEN 603 THEN N'Update Transfer Date'
        WHEN 701 THEN N'PayLink Link'
        WHEN 702 THEN N'PayLink Transaction'
        WHEN 801 THEN N'CreditCard Transactions'
        WHEN 802 THEN N'Observe Configuration'
        WHEN 803 THEN N'Observe Transactions'
        WHEN 804 THEN N'FraudAlert Configuration'
        WHEN 805 THEN N'FraudAlert Transactions'
        WHEN 901 THEN N'Etax Merchant Profile'
        WHEN 902 THEN N'Etax EndUser Profile'
        WHEN 903 THEN N'Sale Name Manage'
        WHEN 904 THEN N'Upload Summary Settlement'
        WHEN 905 THEN N'Update Summary Settlement'
        WHEN 906 THEN N'Settlement PayOut'
        WHEN 907 THEN N'Invoice Merchant'
        WHEN 908 THEN N'Invoice EndUser'
        WHEN 909 THEN N'Invoice Transaction'
        WHEN 910 THEN N'PND'
        WHEN 911 THEN N'Invoice ABB Transaction'
        WHEN 912 THEN N'Outstanding Invoice'
        WHEN 913 THEN N'Cancel Invoice'
        WHEN 914 THEN N'Credit Note'
        WHEN 915 THEN N'Debit Note'
        WHEN 916 THEN N'CreditNote Invoice ABB'
        WHEN 1001 THEN N'ODD Route'
        WHEN 1002 THEN N'ODD Merchant Route'
        WHEN 1003 THEN N'ODD Register'
        WHEN 1004 THEN N'ODD BAY Configs'
        WHEN 1005 THEN N'ODD BAY Customer'
        WHEN 1006 THEN N'ODD SCB Configs'
        WHEN 1007 THEN N'ODD SCB Customer'
        WHEN 1008 THEN N'ODD KTB Configs'
        WHEN 1101 THEN N'Chill App Members'
        WHEN 1102 THEN N'Chill App Transactions'
        WHEN 1103 THEN N'Chill App Partners'
        WHEN 1104 THEN N'Chill App PayTransactions'
        WHEN 1105 THEN N'Chill App Settlement PayTransactions'
        WHEN 1201 THEN N'Wallet Merchant'
        WHEN 1202 THEN N'Shop Send Money Fee'
        WHEN 1203 THEN N'Wallet Transaction'
        WHEN 1204 THEN N'Wallet Settlement Transaction'
        WHEN 1205 THEN N'Wallet Settlement Report'
        WHEN 1206 THEN N'Settle Version'
        WHEN 1301 THEN N'Recurring Merchant'
        WHEN 1302 THEN N'Recurring Schedule'
        WHEN 1303 THEN N'Recurring Transactions'
        WHEN 1401 THEN N'Commission Reseller'
        WHEN 1402 THEN N'Commission Reseller Payout'
        WHEN 1403 THEN N'Commission Reports'
        WHEN 1404 THEN N'Commission Download Reports'
        ELSE CAST(b.[MenuType] AS nvarchar(20))
      END) AS [MenuTypeText]
    , (CASE b.[LogType]
        WHEN 1 THEN N'Create'
        WHEN 2 THEN N'Update'
        WHEN 3 THEN N'Activate'
        WHEN 4 THEN N'Inactivate'
        WHEN 5 THEN N'Delete'
        WHEN 7 THEN N'Generate'
        WHEN 8 THEN N'UpdateStatus'
        WHEN 9 THEN N'Approve'
        WHEN 10 THEN N'Reject'
        ELSE CAST(b.[LogType] AS nvarchar(20))
      END) AS [LogTypeText]
    , (CASE WHEN b.[RefType] IS NULL THEN N''
        ELSE CASE b.[RefType]
            WHEN 0 THEN N'Undefined'
            WHEN 20000 THEN N'Merchant'
            WHEN 20001 THEN N'Merchant User'
            WHEN 20002 THEN N'Merchant Fee'
            WHEN 20003 THEN N'Merchant Route'
            WHEN 20004 THEN N'Merchant Service Fee'
            WHEN 20005 THEN N'Merchant Email'
            WHEN 30000 THEN N'Payment Channel'
            WHEN 30001 THEN N'Payment Route'
            WHEN 30002 THEN N'Payment Route Inquiry'
            WHEN 30003 THEN N'Credit Card Config'
            WHEN 30004 THEN N'ChillPay Maintenance'
            WHEN 30005 THEN N'Bank Maintenance'
            WHEN 30006 THEN N'Bank Payment Api Setting'
            WHEN 30007 THEN N'SMS Route'
            WHEN 30008 THEN N'Exchange Rate'
            WHEN 30009 THEN N'Exchange Rate Log'
            WHEN 10001 THEN N'Account'
            WHEN 10002 THEN N'Role'
            WHEN 40000 THEN N'Payment Transaction'
            WHEN 50000 THEN N'Settlement Record'
            WHEN 60000 THEN N'Managed Transaction'
            WHEN 70000 THEN N'PayLink Record'
            WHEN 80000 THEN N'Fraud Record'
            WHEN 90000 THEN N'Etax Record'
            WHEN 100000 THEN N'Odd Record'
            WHEN 110000 THEN N'Chill App Record'
            WHEN 120000 THEN N'Wallet Record'
            WHEN 130000 THEN N'Recurring Record'
            WHEN 140000 THEN N'Commission Record'
            ELSE CAST(b.[RefType] AS nvarchar(20))
        END
      END) AS [RefTypeText]
    , (CASE WHEN b.[Ref2Type] IS NULL THEN N''
        ELSE CASE b.[Ref2Type]
            WHEN 0 THEN N'Undefined'
            WHEN 20000 THEN N'Merchant'
            WHEN 20001 THEN N'Merchant User'
            WHEN 20002 THEN N'Merchant Fee'
            WHEN 20003 THEN N'Merchant Route'
            WHEN 20004 THEN N'Merchant Service Fee'
            WHEN 20005 THEN N'Merchant Email'
            WHEN 30000 THEN N'Payment Channel'
            WHEN 30001 THEN N'Payment Route'
            WHEN 30002 THEN N'Payment Route Inquiry'
            WHEN 30003 THEN N'Credit Card Config'
            WHEN 30004 THEN N'ChillPay Maintenance'
            WHEN 30005 THEN N'Bank Maintenance'
            WHEN 30006 THEN N'Bank Payment Api Setting'
            WHEN 30007 THEN N'SMS Route'
            WHEN 30008 THEN N'Exchange Rate'
            WHEN 30009 THEN N'Exchange Rate Log'
            WHEN 10001 THEN N'Account'
            WHEN 10002 THEN N'Role'
            WHEN 40000 THEN N'Payment Transaction'
            WHEN 50000 THEN N'Settlement Record'
            WHEN 60000 THEN N'Managed Transaction'
            WHEN 70000 THEN N'PayLink Record'
            WHEN 80000 THEN N'Fraud Record'
            WHEN 90000 THEN N'Etax Record'
            WHEN 100000 THEN N'Odd Record'
            WHEN 110000 THEN N'Chill App Record'
            WHEN 120000 THEN N'Wallet Record'
            WHEN 130000 THEN N'Recurring Record'
            WHEN 140000 THEN N'Commission Record'
            ELSE CAST(b.[Ref2Type] AS nvarchar(20))
        END
      END) AS [Ref2TypeText]
    , m.[MerchantCode], m.[ShortName], m.[CompanyName], m.[ShortNameEN]
FROM [dbo].[ChillpayOperationLogs] AS b WITH (NOLOCK)
LEFT OUTER JOIN [dbo].[Merchants] AS m WITH (NOLOCK) ON b.[MerchantId] = m.[Id];
GO
