using PX.Data;

namespace PX.Objects.IN
{
    public class INSiteExt : PXCacheExtension<PX.Objects.IN.INSite>
    {
        public static bool IsActive() => PX.Data.Update.PXInstanceHelper.CurrentCompany == 3;

        #region UsrDefVendorID
        [AP.Vendor(DisplayName = "Def. Vendor 4 Drop-Ship")]
        public virtual int? UsrDefVendorID { get; set; }
        public abstract class usrDefVendorID : PX.Data.BQL.BqlInt.Field<usrDefVendorID> { }
        #endregion
    }
}