using System;
using System.Collections;
using System.Collections.Generic;
using PX.Common;
using PX.Data;

namespace PX.Objects.SO
{
    public class SOOrderEntry_Extension : PXGraphExtension<SOOrderEntry>
    {
        // add into printing and email by customization project > screens
        #region Actions
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
    }
}