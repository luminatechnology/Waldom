using PX.Data;

namespace PX.Objects.PO
{
    public class POOrderEntry_Extension : PXGraphExtension<PX.Objects.PO.POOrderEntry>
    {
        public static bool IsActive() => PX.Data.Update.PXInstanceHelper.CurrentCompany == 3;

        #region Cache Attached
        [PXMergeAttributes(Method = MergeMethod.Replace)]
        [PXDBBool]
        [PXDefault]
        [PXFormula(typeof(Where<POOrder.orderType, In3<POOrderType.regularOrder, POOrderType.dropShip>,
                                And<Selector<POOrder.vendorID, AP.Vendor.isBranch>, Equal<True>>>))]
        protected void _(Events.CacheAttached<POOrder.isIntercompany> e) { }
        #endregion
    }
}
