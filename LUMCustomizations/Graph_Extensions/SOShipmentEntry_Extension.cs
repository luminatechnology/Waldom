using System;
using System.Collections;
using System.Collections.Generic;
using PX.Common;
using PX.Data;

namespace PX.Objects.SO
{
    public class SOShipmentEntry_Extension : PXGraphExtension<SOShipmentEntry>
    {
        [PXOverride]
        public PXAction<SOShipment> printShipmentConfirmation;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Print Shipment Confirmation", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        public virtual IEnumerable PrintShipmentConfirmation(PXAdapter adapter)
        {
            if (Base.Document.Current != null)
            {
                PXReportRequiredException ex = null;

                var _reportAID = "SO642000";
                Dictionary<string, string> parametersA = new Dictionary<string, string>();
                parametersA["ShipmentNbr"] = Base.Document.Current.ShipmentNbr;

                ex = new PXReportRequiredException(parametersA, _reportAID, $"Report {_reportAID}") { Mode = PXBaseRedirectException.WindowMode.New };

                var _reportBID = "LM641099";
                Dictionary<string, string> parametersB = new Dictionary<string, string>();
                parametersB["ShipmentNbr"] = Base.Transactions.Current.ShipmentNbr;
                ex.AddSibling(_reportBID, parametersB, false);

                throw ex;
            }
            return adapter.Get();
        }
    }
}