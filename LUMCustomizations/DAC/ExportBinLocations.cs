using PX.Data;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMCustomizations.DAC
{
    [PXCacheName("Export Bin Locations")]
    public class ExportBinLocations : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region Seq
        [PXInt(IsKey = true)]
        public virtual int? Seq { get; set; }
        public abstract class seq : PX.Data.BQL.BqlString.Field<seq> { }
        #endregion

        #region Region
        [PXString]
        [PXUIField(DisplayName = "Region")]
        public virtual String RegionName { get; set; }
        public abstract class regionName : PX.Data.BQL.BqlString.Field<regionName> { }
        #endregion

        #region Location
        [PXString]
        [PXUIField(DisplayName = "Location")]
        public virtual String Location { get; set; }
        public abstract class location : PX.Data.BQL.BqlString.Field<location> { }
        #endregion

        #region UIDNumber
        [PXInt]
        [PXUIField(DisplayName = "UIDNumber")]
        public virtual int? UIDNumber { get; set; }
        public abstract class uIDNumber : PX.Data.BQL.BqlInt.Field<uIDNumber> { }
        #endregion

        #region PRC
        [PXString]
        [PXUIField(DisplayName = "PRC")]
        public virtual String PRC { get; set; }
        public abstract class pRC : PX.Data.BQL.BqlString.Field<pRC> { }
        #endregion

        #region PartNumber
        [PXString]
        [PXUIField(DisplayName = "PartNumber")]
        public virtual String PartNumber { get; set; }
        public abstract class partNumber : PX.Data.BQL.BqlString.Field<partNumber> { }
        #endregion

        #region BinLocation
        [PXString]
        [PXUIField(DisplayName = "BinLocation")]
        public virtual String BinLocation { get; set; }
        public abstract class binLocation : PX.Data.BQL.BqlString.Field<binLocation> { }
        #endregion

        #region Quantity
        //[PXDecimal]
        [PXQuantity]
        [PXUIField(DisplayName = "Quantity")]
        public virtual decimal? Quantity { get; set; }
        public abstract class quantity : PX.Data.BQL.BqlDecimal.Field<quantity> { }
        #endregion

        #region Cost
        //[PXDecimal]
        [PXPriceCost]
        [PXUIField(DisplayName = "Cost")]
        public virtual decimal? Cost { get; set; }
        public abstract class cost : PX.Data.BQL.BqlDecimal.Field<cost> { }
        #endregion

        #region DateCreated
        [PXDate]
        [PXUIField(DisplayName = "DateCreated")]
        public virtual DateTime? DateCreated { get; set; }
        public abstract class dateCreated : PX.Data.BQL.BqlDateTime.Field<dateCreated> { }
        #endregion

        #region DateCreatedAscending
        [PXDate]
        [PXUIField(DisplayName = "DateCreatedAscending")]
        public virtual DateTime? DateCreatedAscending { get; set; }
        public abstract class dateCreatedAscending : PX.Data.BQL.BqlDateTime.Field<dateCreatedAscending> { }
        #endregion

        #region COO
        [PXString]
        [PXUIField(DisplayName = "COO")]
        public virtual String COO { get; set; }
        public abstract class cOO : PX.Data.BQL.BqlString.Field<cOO> { }
        #endregion

        #region StockRecoveryFlag
        [PXString]
        [PXUIField(DisplayName = "StockRecoveryFlag")]
        public virtual String StockRecoveryFlag { get; set; }
        public abstract class stockRecoveryFlag : PX.Data.BQL.BqlString.Field<stockRecoveryFlag> { }
        #endregion

        #region AgedInventoryFlag
        [PXString]
        [PXUIField(DisplayName = "AgedInventoryFlag")]
        public virtual String AgedInventoryFlag { get; set; }
        public abstract class agedInventoryFlag : PX.Data.BQL.BqlString.Field<agedInventoryFlag> { }
        #endregion

        #region DateCode
        [PXString]
        [PXUIField(DisplayName = "DateCode")]
        public virtual String DateCode { get; set; }
        public abstract class dateCode : PX.Data.BQL.BqlString.Field<dateCode> { }
        #endregion

        #region DateCodeDecoded
        [PXString]
        [PXUIField(DisplayName = "DateCodeDecoded")]
        public virtual String DateCodeDecoded { get; set; }
        public abstract class dateCodeDecoded : PX.Data.BQL.BqlString.Field<dateCodeDecoded> { }
        #endregion

        #region LotCode
        [PXString]
        [PXUIField(DisplayName = "LotCode")]
        public virtual String LotCode { get; set; }
        public abstract class lotCode : PX.Data.BQL.BqlString.Field<lotCode> { }
        #endregion

        #region PONumber
        [PXString]
        [PXUIField(DisplayName = "PONumber")]
        public virtual String PONumber { get; set; }
        public abstract class pONumber : PX.Data.BQL.BqlString.Field<pONumber> { }
        #endregion

        #region ReceiptNumber
        [PXString]
        [PXUIField(DisplayName = "ReceiptNumber")]
        public virtual String ReceiptNumber { get; set; }
        public abstract class receiptNumber : PX.Data.BQL.BqlString.Field<receiptNumber> { }
        #endregion

        #region Currency
        [PXString]
        [PXUIField(DisplayName = "Currency")]
        public virtual String Currency { get; set; }
        public abstract class currency : PX.Data.BQL.BqlString.Field<currency> { }
        #endregion

    }
}
