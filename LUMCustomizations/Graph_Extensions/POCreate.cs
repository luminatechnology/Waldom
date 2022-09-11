using PX.Data;
using PX.Objects.IN;

namespace PX.Objects.PO
{
    public class POCreate_Extension : PXGraphExtension<PX.Objects.PO.POCreate>
    {
        public static bool IsActive() => PX.Data.Update.PXInstanceHelper.CurrentCompany == 3;

        #region Override Methods
        public delegate void EnumerateAndPrepareFixedDemandRowDelgate(PXResult<POFixedDemand> rec);
        [PXOverride]
        public void EnumerateAndPrepareFixedDemandRow(PXResult<POFixedDemand> rec, EnumerateAndPrepareFixedDemandRowDelgate baseMethod)
        {
            baseMethod(rec);

            var demand = (POFixedDemand)rec;

            // The Vendor should be auto-filled by the default warehouse custom field, if there was no default vendor predefined in the specific Stock Item. 
            if (demand.VendorID == null)
            {
                Base.FixedDemand.Cache.SetValueExt<POFixedDemand.vendorID>(demand, INSite.PK.Find(Base, demand.SiteID)?.GetExtension<INSiteExt>().UsrDefVendorID);
            }
        }
        #endregion
    }
}
