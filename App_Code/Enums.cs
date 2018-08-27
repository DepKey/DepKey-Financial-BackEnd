using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Enums
/// </summary>
public enum SalesStatusEnum
{
    New = 2,
    Online = 4,
    Transfered = 5,
    Void = 6,
    Refunded = 7,
    PendingComplementary = 54,
    ApprovedComplementary = 55,
    RefusedComplementary = 56,
    PendingVoid = 87,
    PendingEdit = 95,
    PendingDelete = 94,
    PendingRevert = 117,
}

public enum JournalTypeEnum
{
    Sales = 8,
    Cost = 9,
    JvEntry = 10,
    CashIn = 11,
    CashOut = 12,
    BankVisaIn = 14,
    BankVisaOut = 15,
    JournalRefund = 16,
    SysJV = 57
}

public enum PaymentMethodEnum
{
    Cash = 17,
    Card = 18,
    Credit = 19,
    Visa = 20,
    From_Advanced_Recieved = 21,
    Complementary = 23,
    Miscellaneous = 24
}

public enum SalesTypeEnum
{
    BSP = 25,
    Voucher = 26,
    XO = 27,
    Insurance = 28,
    Visa = 29,
    Cruise = 30,
    Others = 31,
    LC = 49,
    RefundBSP = 36,
    RefundVoucher = 37,
    RefundXO = 38,
    RefundInsurance = 39,
    RefundVisa = 40,
    RefundCruise = 41,
    RefundOthers = 42,
    RefundLC = 50,
    RefundBSPToCustomer = 98,
    RefundVoucherToCustomer = 99,
    RefundXOToCustomer = 100,
    RefundInsuranceToCustomer = 101,
    RefundVisaToCustomer = 102,
    RefundCruiseToCustomer = 103,
    RefundOthersToCustomer = 105,
    RefundLCToCustomer = 104
}
