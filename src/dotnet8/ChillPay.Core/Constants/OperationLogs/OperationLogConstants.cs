namespace ChillPay.Core.Constants.OperationLogs;

public enum ChillpayOperationLogModuleType
{
    User = 1,
    Merchant = 2,
    Settings = 3,
    Transactions = 4,
    Settlements = 5,
    ManageTransaction = 6,
    PayLink = 7,
    Fraud = 8,
    Etax = 9,
    OddService = 10,
    ChillAppPartner = 11,
    Wallet = 12,
    Recurring = 13,
    Commission = 14,
}

public enum ChillpayOperationLogMenuType
{
    // Module 1 — User
    Account = 101,
    Role = 102,

    // Module 2 — Merchant
    Merchant = 200,
    MerchantUser = 201,
    MerchantFee = 202,
    MerchantRoute = 203,
    MerchantServiceFee = 204,
    GenerateMerchantKeys = 205,
    MerchantEmails = 206,

    // Module 3 — Settings
    PaymentChannel = 300,
    PaymentRoute = 301,
    PaymentRouteInquiry = 302,
    CreditCardConfig = 303,
    SwitchPaymentRouteChannel = 305,
    ChillPayMaintenance = 306,
    BankMaintenance = 307,
    BankPaymentApiSetting = 308,
    SMSRoute = 309,
    ExchangeRate = 310,
    ExchangeRateLog = 311,

    // Module 4 — Transactions
    Payment = 401,
    Void = 402,
    RefundOld = 403,
    RefundTransaction = 404,
    ApproveRefund = 405,
    SettlementOld = 406,
    PaymentSummaryOld = 407,
    ImportMcc = 408,
    InquiryTransaction = 409,
    WarningTransaction = 410,

    // Module 5 — Settlements
    SettlementDashboard = 501,
    Settlement = 502,
    SettlementUploadFile = 503,
    SettlementMerchantTransfer = 504,
    SettlementMerchantSetting = 505,
    SettlementStatus = 506,
    DownloadPaymentSummary = 507,

    // Module 6 — Manage Transaction
    UpdateTransactionStatus = 601,
    UpdateSettlementStatus = 602,
    UpdateTransferDate = 603,

    // Module 7 — PayLink
    PayLinkLink = 701,
    PayLinkTransaction = 702,

    // Module 8 — Fraud
    FraudCreditCardTransactions = 801,
    FraudObserveConfiguration = 802,
    FraudObserveTransactions = 803,
    FraudAlertConfiguration = 804,
    FraudAlertTransactions = 805,

    // Module 9 — Etax
    EtaxMerchantProfile = 901,
    EtaxEndUserProfile = 902,
    EtaxSaleManage = 903,
    EtaxUploadSettlement = 904,
    EtaxUpdateSettlement = 905,
    EtaxSettlementPayOut = 906,
    EtaxInvoiceMerchant = 907,
    EtaxInvoiceEndUser = 908,
    EtaxInvoiceTransactions = 909,
    EtaxPnd = 910,
    EtaxInvoiceAbbTransactions = 911,
    EtaxOutstandingInvoice = 912,
    EtaxCancelInvoice = 913,
    EtaxCreditNote = 914,
    EtaxDebitNote = 915,
    EtaxCreditNoteInvAbb = 916,

    // Module 10 — Odd Service
    OddRoute = 1001,
    OddMerchantRoute = 1002,
    OddRegister = 1003,
    OddBayConfigs = 1004,
    OddBayCustomer = 1005,
    OddScbConfigs = 1006,
    OddScbCustomer = 1007,
    OddKtbConfigs = 1008,

    // Module 11 — Chill App Partner
    ChillAppMembers = 1101,
    ChillAppTransactions = 1102,
    ChillAppPartners = 1103,
    ChillAppPayTransactions = 1104,
    ChillAppSettlementPayTransactions = 1105,

    // Module 12 — Wallet
    WalletMerchant = 1201,
    WalletShopSendMoneyFee = 1202,
    WalletTransaction = 1203,
    WalletSettlementTransaction = 1204,
    WalletSettlementReport = 1205,
    WalletSettleVersion = 1206,

    // Module 13 — Recurring
    RecurringMerchant = 1301,
    RecurringSchedule = 1302,
    RecurringTransactions = 1303,

    // Module 14 — Commission
    CommissionReseller = 1401,
    CommissionResellerPayout = 1402,
    CommissionReports = 1403,
    CommissionDownloadReports = 1404,
}

public enum ChillpayOperationLogActionType
{
    Create = 1,
    Update = 2,
    Activate = 3,
    Inactivate = 4,
    Delete = 5,
    Generate = 7,
    UpdateStatus = 8,
    Approve = 9,
    Reject = 10,
}

public enum ChillpayOperationLogRefType
{
    Undefined = 0,

    // User (Module 1 — 10001+)
    Account = 10001,
    Role = 10002,

    // Merchant (Module 2 — 20000+)
    Merchant = 20000,
    MerchantUser = 20001,
    MerchantFee = 20002,
    MerchantRoute = 20003,
    MerchantServiceFee = 20004,
    MerchantEmail = 20005,

    // Settings (Module 3 — 30000+)
    PaymentChannel = 30000,
    PaymentRoute = 30001,
    PaymentRouteInquiry = 30002,
    CreditCardConfig = 30003,
    ChillPayMaintenance = 30004,
    BankMaintenance = 30005,
    BankPaymentApiSetting = 30006,
    SMSRoute = 30007,
    ExchangeRate = 30008,
    ExchangeRateLog = 30009,

    // Transactions (Module 4 — 40000+)
    PaymentTransaction = 40000,

    // Settlements (Module 5 — 50000+)
    SettlementRecord = 50000,

    // Manage Transaction (Module 6 — 60000+)
    ManagedTransaction = 60000,

    // PayLink (Module 7 — 70000+)
    PayLinkRecord = 70000,

    // Fraud (Module 8 — 80000+)
    FraudRecord = 80000,

    // Etax (Module 9 — 90000+)
    EtaxRecord = 90000,

    // Odd Service (Module 10 — 100000+)
    OddRecord = 100000,

    // Chill App Partner (Module 11 — 110000+)
    ChillAppRecord = 110000,

    // Wallet (Module 12 — 120000+)
    WalletRecord = 120000,

    // Recurring (Module 13 — 130000+)
    RecurringRecord = 130000,

    // Commission (Module 14 — 140000+)
    CommissionRecord = 140000,
}

public static class ChillpayOperationLogRegistry
{
    public const string OperationSystemAdmin = "Admin";
    public const string OperationSystemBackend = "Backend";
    public const string OperationSystemApi = "API";
    public const string OperationSystemJob = "Job";
    public const string OperationSystemMerchantApi = "MerchantApi";

    public static bool CheckValidRequestSystem(string system)
    {
        if (string.IsNullOrWhiteSpace(system) || system.Length > 20)
        {
            return false;
        }

        return system == OperationSystemAdmin
            || system == OperationSystemBackend
            || system == OperationSystemApi
            || system == OperationSystemJob
            || system == OperationSystemMerchantApi;
    }

    public static int GetModuleType(ChillpayOperationLogMenuType menuType) => (int)menuType / 100;

    public static int GetModuleType(int menuType) => menuType / 100;

    public static bool IsValidModuleType(int moduleType)
        => Enum.IsDefined(typeof(ChillpayOperationLogModuleType), moduleType);

    public static bool IsValidMenuType(int menuType) => Enum.IsDefined(typeof(ChillpayOperationLogMenuType), menuType);

    public static bool IsMenuTypeInModule(int menuType, int moduleType)
        => IsValidMenuType(menuType) && GetModuleType(menuType) == moduleType;

    public static bool IsValidLogType(int logType) => Enum.IsDefined(typeof(ChillpayOperationLogActionType), logType);

    public static bool IsValidRefType(int? refType)
    {
        if (!refType.HasValue || refType.Value == (int)ChillpayOperationLogRefType.Undefined)
        {
            return true;
        }

        return Enum.IsDefined(typeof(ChillpayOperationLogRefType), refType.Value);
    }

    public static bool IsValidAddRequest(int moduleType, int menuType, int logType, int? refType, int? ref2Type)
    {
        return IsValidModuleType(moduleType)
            && IsMenuTypeInModule(menuType, moduleType)
            && IsValidLogType(logType)
            && IsValidRefType(refType)
            && IsValidRefType(ref2Type);
    }

    public static long? ResolveMerchantId(
        int moduleType,
        int menuType,
        int? refType,
        long? refId,
        int? ref2Type,
        long? ref2Id,
        long? dataId,
        long? merchantId = null)
    {
        if (merchantId.HasValue && merchantId.Value > 0)
        {
            return merchantId;
        }

        if (moduleType != (int)ChillpayOperationLogModuleType.Merchant)
        {
            return null;
        }

        const int merchantRef = (int)ChillpayOperationLogRefType.Merchant;
        const int merchantMenu = (int)ChillpayOperationLogMenuType.Merchant;

        if (refType == merchantRef && refId is > 0)
        {
            return refId;
        }

        if (ref2Type == merchantRef && ref2Id is > 0)
        {
            return ref2Id;
        }

        if (menuType == merchantMenu && refId is > 0)
        {
            return refId;
        }

        if (menuType == merchantMenu && dataId is > 0)
        {
            return dataId;
        }

        if (menuType == (int)ChillpayOperationLogMenuType.GenerateMerchantKeys && refId is > 0)
        {
            return refId;
        }

        return null;
    }

    public static int? ResolveRefTypeFromMenuType(int menuType)
    {
        if (!IsValidMenuType(menuType))
        {
            return null;
        }

        switch ((ChillpayOperationLogMenuType)menuType)
        {
            case ChillpayOperationLogMenuType.Account: return (int)ChillpayOperationLogRefType.Account;
            case ChillpayOperationLogMenuType.Role: return (int)ChillpayOperationLogRefType.Role;
            case ChillpayOperationLogMenuType.Merchant:
            case ChillpayOperationLogMenuType.GenerateMerchantKeys: return (int)ChillpayOperationLogRefType.Merchant;
            case ChillpayOperationLogMenuType.MerchantUser: return (int)ChillpayOperationLogRefType.MerchantUser;
            case ChillpayOperationLogMenuType.MerchantFee: return (int)ChillpayOperationLogRefType.MerchantFee;
            case ChillpayOperationLogMenuType.MerchantRoute: return (int)ChillpayOperationLogRefType.MerchantRoute;
            case ChillpayOperationLogMenuType.MerchantServiceFee: return (int)ChillpayOperationLogRefType.MerchantServiceFee;
            case ChillpayOperationLogMenuType.MerchantEmails: return (int)ChillpayOperationLogRefType.MerchantEmail;
            case ChillpayOperationLogMenuType.PaymentChannel: return (int)ChillpayOperationLogRefType.PaymentChannel;
            case ChillpayOperationLogMenuType.PaymentRoute:
            case ChillpayOperationLogMenuType.SwitchPaymentRouteChannel: return (int)ChillpayOperationLogRefType.PaymentRoute;
            case ChillpayOperationLogMenuType.PaymentRouteInquiry: return (int)ChillpayOperationLogRefType.PaymentRouteInquiry;
            case ChillpayOperationLogMenuType.CreditCardConfig: return (int)ChillpayOperationLogRefType.CreditCardConfig;
            case ChillpayOperationLogMenuType.ChillPayMaintenance: return (int)ChillpayOperationLogRefType.ChillPayMaintenance;
            case ChillpayOperationLogMenuType.BankMaintenance: return (int)ChillpayOperationLogRefType.BankMaintenance;
            case ChillpayOperationLogMenuType.BankPaymentApiSetting: return (int)ChillpayOperationLogRefType.BankPaymentApiSetting;
            case ChillpayOperationLogMenuType.SMSRoute: return (int)ChillpayOperationLogRefType.SMSRoute;
            case ChillpayOperationLogMenuType.ExchangeRate: return (int)ChillpayOperationLogRefType.ExchangeRate;
            case ChillpayOperationLogMenuType.ExchangeRateLog: return (int)ChillpayOperationLogRefType.ExchangeRateLog;
            default:
                var moduleType = GetModuleType(menuType);
                if (moduleType >= (int)ChillpayOperationLogModuleType.Transactions
                    && moduleType <= (int)ChillpayOperationLogModuleType.Commission)
                {
                    return moduleType * 10000;
                }

                return (int)ChillpayOperationLogRefType.Undefined;
        }
    }

    public static readonly int[] AllModuleTypes =
    [
        (int)ChillpayOperationLogModuleType.User,
        (int)ChillpayOperationLogModuleType.Merchant,
        (int)ChillpayOperationLogModuleType.Settings,
        (int)ChillpayOperationLogModuleType.Transactions,
        (int)ChillpayOperationLogModuleType.Settlements,
        (int)ChillpayOperationLogModuleType.ManageTransaction,
        (int)ChillpayOperationLogModuleType.PayLink,
        (int)ChillpayOperationLogModuleType.Fraud,
        (int)ChillpayOperationLogModuleType.Etax,
        (int)ChillpayOperationLogModuleType.OddService,
        (int)ChillpayOperationLogModuleType.ChillAppPartner,
        (int)ChillpayOperationLogModuleType.Wallet,
        (int)ChillpayOperationLogModuleType.Recurring,
        (int)ChillpayOperationLogModuleType.Commission,
    ];
}
