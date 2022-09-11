using System.Collections;
using System.Collections.Generic;
using PX.Data;

namespace PX.Objects.SO
{
    public class SOOrderEntry_Extension : PXGraphExtension<SOOrderEntry>
    {
        #region Actions
        // add into printing and email by customization project > screens
        public PXAction<SOOrder> CommercialInvoice;
        [PXButton(DisplayOnMainToolbar = false, CommitChanges = true)]
        [PXUIField(DisplayName = "Commercial Invoice", Enabled = true, MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable commercialInvoice(PXAdapter adapter)
        {
            if (Base.Document.Current != null)
            {
                var _reportID = "LM641000";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters["OrderNbr"] = Base.Document.Current.OrderNbr;
                parameters["OrderType"] = Base.Document.Current.OrderType;
                throw new PXReportRequiredException(parameters, _reportID, $"Report {_reportID}") { Mode = PXBaseRedirectException.WindowMode.New };
            }
            return adapter.Get();
        }
        #endregion

        #region Event Handlers
        protected void _(Events.FieldUpdated<SOLine.inventoryID> e, PXFieldUpdated baseHandler)
        {
            baseHandler?.Invoke(e.Cache, e.Args);

            SOLine row = e.Row as SOLine;

            if (row != null && Base.soordertype.Current?.GetExtension<SOOrderTypeExt>()?.UsrEnablePOCreateAuto == true)
            {
                row.POCreate = true;
                row.POSource = IN.INReplenishmentSource.DropShipToOrder;
            }
        }
        #endregion
    }
}