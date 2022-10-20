using LUMCustomizations.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMCustomizations.Graph
{
    public class LUMWaldomSetup : PXGraph<LUMWaldomSetup>
    {
        public PXSave<LUMWaldomPreference> Save;
        public PXCancel<LUMWaldomPreference> Cancel;
        public SelectFrom<LUMWaldomPreference>.View Setup;
    }
}
