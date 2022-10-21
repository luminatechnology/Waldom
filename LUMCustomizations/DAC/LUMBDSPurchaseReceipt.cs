using System;
using PX.Data;

namespace LUMCustomizations.DAC
{
    [Serializable]
    [PXCacheName("LUMBDSPurchaseReceipt")]
    public class LUMBDSPurchaseReceipt : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region POOrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "POOrder Nbr")]
        public virtual string POOrderNbr { get; set; }
        public abstract class pOOrderNbr : PX.Data.BQL.BqlString.Field<pOOrderNbr> { }
        #endregion

        #region POLineNbr
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "POLine Nbr")]
        public virtual int? POLineNbr { get; set; }
        public abstract class pOLineNbr : PX.Data.BQL.BqlInt.Field<pOLineNbr> { }
        #endregion

        #region Region
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Region")]
        public virtual string Region { get; set; }
        public abstract class region : PX.Data.BQL.BqlString.Field<region> { }
        #endregion

        #region CSVType
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Type")]
        public virtual string CSVType { get; set; }
        public abstract class cSVType : PX.Data.BQL.BqlString.Field<cSVType> { }
        #endregion

        #region Vendor
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Vendor")]
        public virtual string Vendor { get; set; }
        public abstract class vendor : PX.Data.BQL.BqlString.Field<vendor> { }
        #endregion

        #region ReceiptDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Receipt Date")]
        public virtual DateTime? ReceiptDate { get; set; }
        public abstract class receiptDate : PX.Data.BQL.BqlDateTime.Field<receiptDate> { }
        #endregion

        #region Sitecd
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Warhouse")]
        public virtual string Sitecd { get; set; }
        public abstract class sitecd : PX.Data.BQL.BqlString.Field<sitecd> { }
        #endregion

        #region PartNumberCD
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Part Number")]
        public virtual string PartNumberCD { get; set; }
        public abstract class partNumberCD : PX.Data.BQL.BqlString.Field<partNumberCD> { }
        #endregion

        #region Uom
        [PXDBString(10, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "UOM")]
        public virtual string Uom { get; set; }
        public abstract class uom : PX.Data.BQL.BqlString.Field<uom> { }
        #endregion

        #region Quantity
        [PXDBInt()]
        [PXUIField(DisplayName = "Quantity")]
        public virtual int? Quantity { get; set; }
        public abstract class quantity : PX.Data.BQL.BqlInt.Field<quantity> { }
        #endregion

        #region UnitPrice
        [PXDBDecimal(6)]
        [PXUIField(DisplayName = "Unit Price")]
        public virtual Decimal? UnitPrice { get; set; }
        public abstract class unitPrice : PX.Data.BQL.BqlDecimal.Field<unitPrice> { }
        #endregion

        #region DateCode
        [PXDBString(1024)]
        [PXUIField(DisplayName = "Date Code")]
        public virtual string DateCode { get; set; }
        public abstract class dateCode : PX.Data.BQL.BqlString.Field<dateCode> { }
        #endregion

        #region Currency
        [PXDBString(3, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Currency")]
        public virtual string Currency { get; set; }
        public abstract class currency : PX.Data.BQL.BqlString.Field<currency> { }
        #endregion

        #region InvoiceNumber
        [PXDBString(15, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Invoice Number")]
        public virtual string InvoiceNumber { get; set; }
        public abstract class invoiceNumber : PX.Data.BQL.BqlString.Field<invoiceNumber> { }
        #endregion

        #region InvoiceDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Invoice Date")]
        public virtual DateTime? InvoiceDate { get; set; }
        public abstract class invoiceDate : PX.Data.BQL.BqlDateTime.Field<invoiceDate> { }
        #endregion

        #region ShipVia
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Ship Via")]
        public virtual string ShipVia { get; set; }
        public abstract class shipVia : PX.Data.BQL.BqlString.Field<shipVia> { }
        #endregion

        #region TrackingNumber
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Tracking Number")]
        public virtual string TrackingNumber { get; set; }
        public abstract class trackingNumber : PX.Data.BQL.BqlString.Field<trackingNumber> { }
        #endregion

        #region IsProcessed
        [PXDBBool]
        [PXUIField(DisplayName = "Is Processed")]
        public virtual bool? IsProcessed { get; set; }
        public abstract class isProcessed : PX.Data.BQL.BqlBool.Field<isProcessed> { }
        #endregion

        #region ErrorMessage
        [PXDBString]
        [PXUIField(DisplayName = "Error Message")]
        public virtual string ErrorMessage { get; set; }
        public abstract class errorMessage : PX.Data.BQL.BqlString.Field<errorMessage> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}