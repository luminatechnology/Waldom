using PX.Data;
using PX.Objects.IN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMCustomizations.DAC
{
    [PXCacheName("LUMApptQuestionnaire")]
    public class LUMProductRelease : IBqlTable
    {
        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region Region
        [PXString]
        [PXUIField(DisplayName = "Region")]
        public virtual String RegionName { get; set; }
        public abstract class regionName : PX.Data.BQL.BqlString.Field<regionName> { }

        #endregion

        #region InventoryID
        [PXInt()]
        [PXUIField(DisplayName = "UID Number", Visibility = PXUIVisibility.Visible, Visible = false)]
        public virtual Int32? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }

        #endregion

        #region InventoryCD
        [PXString]
        [InventoryRaw(DisplayName = "Part Number")]
        public virtual String InventoryCD { get; set; }
        public abstract class inventoryCD : PX.Data.BQL.BqlString.Field<inventoryCD> { }

        #endregion

        #region ItemClassCD
        [PXString]
        [PXUIField(DisplayName = "PRC", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string ItemClassCD { get; set; }
        public abstract class itemClassCD : PX.Data.BQL.BqlInt.Field<itemClassCD> { }
        #endregion

        #region MoqAttribute
        [PXString]
        [PXUIField(DisplayName = "Release MOQ")]
        public virtual string MoqAttribute { get; set; }
        public abstract class moqAttribute : PX.Data.BQL.BqlString.Field<moqAttribute> { }
        #endregion

        #region SpqAttribute
        [PXString]
        [PXUIField(DisplayName = "Release SPQ")]
        public virtual string SpqAttribute { get; set; }
        public abstract class spqAttribute : PX.Data.BQL.BqlString.Field<spqAttribute> { }
        #endregion

        #region BreakQty
        [PXDecimal]
        [PXUIField(DisplayName = "Resale Quantity")]
        public virtual decimal? BreakQty { get; set; }
        public abstract class breakQty : PX.Data.BQL.BqlDecimal.Field<breakQty> { }
        #endregion

        #region Sales Price
        [PXDecimal]
        [PXUIField(DisplayName = "Resale Price")]
        public virtual decimal? SalesPrice { get; set; }
        public abstract class salesPrice : PX.Data.BQL.BqlDecimal.Field<salesPrice> { }
        #endregion

        #region CuryID
        [PXString]
        [PXUIField(DisplayName = "Currency")]
        public virtual string CuryID { get; set; }
        public abstract class curyID : PX.Data.BQL.BqlString.Field<curyID> { }
        #endregion

    }
}
