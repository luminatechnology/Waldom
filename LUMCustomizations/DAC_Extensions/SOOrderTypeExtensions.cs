using PX.Data;

namespace PX.Objects.SO
{
    public class SOOrderTypeExt : PXCacheExtension<PX.Objects.SO.SOOrderType>
    {
        public static bool IsActive() => PX.Data.Update.PXInstanceHelper.CurrentCompany == 3;

        #region UsrEnablePOCreateAuto
        [PXDBBool]
        [PXUIField(DisplayName = "Enable Mark for PO Automation")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? UsrEnablePOCreateAuto { get; set; }
        public abstract class usrEnablePOCreateAuto : PX.Data.BQL.BqlBool.Field<usrEnablePOCreateAuto> { }
        #endregion
    }
}