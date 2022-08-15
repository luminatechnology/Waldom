using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CM;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMCustomizations.Graph_Extensions
{
    public class SOInvoiceEntry_Extension : PXGraphExtension<SOInvoiceEntry>
    {
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            // Set Attribute CURYRATE value
            var effectRates = SelectFrom<CurrencyRate>
                              .Where<CurrencyRate.toCuryID.IsEqual<P.AsString>
                                .And<CurrencyRate.fromCuryID.IsEqual<P.AsString>>>
                              .View.Select(Base, "USD", "SGD").RowCast<CurrencyRate>();
            try
            {
                if (Base.Document.Current != null)
                {
                    effectRates = effectRates.Where(x => x.CuryEffDate?.Date <= Base.Document.Current?.DocDate?.Date).OrderByDescending(x => x.CuryEffDate);
                    var attr = Base.Document.Cache.GetValueExt(Base.Document.Current, PX.Objects.CS.Messages.Attribute + "CURYRATE") as PXFieldState;
                    if (effectRates.Count() > 0 && attr != null && string.IsNullOrEmpty((string)attr.Value))
                        Base.Document.Cache.SetValueExt(Base.Document.Current, PX.Objects.CS.Messages.Attribute + "CURYRATE", effectRates.FirstOrDefault()?.CuryMultDiv == "D" ? effectRates.FirstOrDefault()?.CuryRate : effectRates.FirstOrDefault()?.RateReciprocal);
                }
            }
            catch (Exception ex)
            {
                PXTrace.WriteError(ex.Message);
            }
            // Invoke Base Method
            baseMethod();
        }
    }
}
