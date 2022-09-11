using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.Objects.CM;
using PX.Objects.GL;
using PX.Objects.SO;
using PX.Objects.Common;
using PX.Objects.Extensions.MultiCurrency;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PX.Objects.PO.GraphExtensions.POOrderEntryExt
{
    #region Protected Access
    [PXProtectedAccess]
	public abstract class Intercompany_ProtectedExt : PXGraphExtension<Intercompany, POOrderEntry>
    {
		[PXProtectedAccess(typeof(Intercompany))]
		public abstract void UpdatePOOrderOnSOOrderRowPersisted(PXCache sender, PXRowPersistedEventArgs e);
	}
	#endregion

	public class IntercompanyExt : PXGraphExtension<Intercompany_ProtectedExt, Intercompany, POOrderEntry>
    {
		public static bool IsActive() => PX.Data.Update.PXInstanceHelper.CurrentCompany == 3;

		public virtual SOOrder GenerateIntercompanySalesOrder4DP(POOrder po, POShipAddress shipAddress, POShipContact shipContact,
														  	     IEnumerable<POLine> lines, IEnumerable<POOrderDiscountDetail> discountLines,
															     string orderType, bool copyProjectDetails)
		{
			if (!string.IsNullOrEmpty(po.IntercompanySONbr) || po.OrderType != POOrderType.DropShip)
			{
				throw new PXInvalidOperationException();
			}

			Branch customerBranch = Branch.PK.Find(Base, po.BranchID);
			Customer customer = Customer.PK.Find(Base, customerBranch?.BAccountID);

			if (customer == null)
			{
				throw new PXException(Messages.BranchIsNotExtendedToCustomer, customerBranch?.BranchCD.TrimEnd());
			}

			var vendorBranch = PXAccess.GetBranchByBAccountID(po.VendorID);

			var graph = PXGraph.CreateInstance<SOOrderEntry>();
			orderType = orderType ?? graph.sosetup.Current.DfltIntercompanyOrderType;
			bool hold = false;
			if (PXAccess.FeatureInstalled<FeaturesSet.approvalWorkflow>())
			{
				SOSetupApproval setupApproval = graph.SetupApproval.Select(orderType);
				hold = (setupApproval?.IsActive == true);
			}
			var doc = new SOOrder
			{
				OrderType = orderType,
				BranchID = vendorBranch.BranchID,
				Hold = hold,
			};
			doc = PXCache<SOOrder>.CreateCopy(graph.Document.Insert(doc));
			doc.CustomerID = customer.BAccountID;
			doc.ProjectID = (copyProjectDetails && lines.Any()) ? lines.First().ProjectID : PM.ProjectDefaultAttribute.NonProject();
			doc.IntercompanyPOType = po.OrderType;
			doc.IntercompanyPONbr = po.OrderNbr;
			doc.IntercompanyPOWithEmptyInventory = lines.Any(pol => pol.InventoryID == null && pol.LineType != POLineType.Description);
			doc.ShipSeparately = true;
			doc = PXCache<SOOrder>.CreateCopy(graph.Document.Update(doc));
			doc.BranchID = vendorBranch.BranchID;
			doc.OrderDate = po.OrderDate;
			doc.RequestDate = po.ExpectedDate;
			doc.CustomerOrderNbr = po.OrderNbr;
			doc = PXCache<SOOrder>.CreateCopy(graph.Document.Update(doc));
			doc.DisableAutomaticDiscountCalculation = true;
			doc = PXCache<SOOrder>.CreateCopy(graph.Document.Update(doc));

			AddressAttribute.CopyRecord<SOOrder.shipAddressID>(graph.Document.Cache, doc, shipAddress, true);
			ContactAttribute.CopyRecord<SOOrder.shipContactID>(graph.Document.Cache, doc, shipContact, true);
			doc = PXCache<SOOrder>.CreateCopy(graph.Document.Update(doc));

			doc.CuryID = po.CuryID;
			doc = PXCache<SOOrder>.CreateCopy(graph.Document.Update(doc));
			CurrencyInfo origCuryInfo = Base.FindImplementation<IPXCurrencyHelper>().GetCurrencyInfo(po.CuryInfoID).GetCM();
			CurrencyInfo curyInfo = graph.currencyinfo.Select();
			if (string.Equals(origCuryInfo.BaseCuryID, curyInfo.BaseCuryID, StringComparison.OrdinalIgnoreCase))
			{
				PXCache<CurrencyInfo>.RestoreCopy(curyInfo, origCuryInfo);
				curyInfo.CuryInfoID = doc.CuryInfoID;
			}

			foreach (POLine line in lines.Where(pol => pol.InventoryID != null))
			{
				var soLine = new SOLine
				{
					BranchID = vendorBranch.BranchID,
				};
				soLine = PXCache<SOLine>.CreateCopy(graph.Transactions.Insert(soLine));
				soLine.InventoryID = line.InventoryID;
				soLine.SubItemID = line.SubItemID;
				soLine.RequestDate = line.PromisedDate;
				soLine.TaxCategoryID = line.TaxCategoryID;
				soLine.TaskID = copyProjectDetails ? line.TaskID : null;
				soLine.CostCodeID = copyProjectDetails ? line.CostCodeID : null;
				soLine.IntercompanyPOLineNbr = line.LineNbr;
				soLine = PXCache<SOLine>.CreateCopy(graph.Transactions.Update(soLine));
				soLine.TranDesc = line.TranDesc;
				soLine.UOM = line.UOM;
				soLine.ManualPrice = true;
				soLine = PXCache<SOLine>.CreateCopy(graph.Transactions.Update(soLine));
				soLine.OrderQty = line.OrderQty;
				soLine.CuryUnitPrice = line.CuryUnitCost;
				soLine = PXCache<SOLine>.CreateCopy(graph.Transactions.Update(soLine));
				soLine.CuryExtPrice = line.CuryLineAmt;
				soLine = PXCache<SOLine>.CreateCopy(graph.Transactions.Update(soLine));
				soLine.DiscPct = line.DiscPct;
				soLine = PXCache<SOLine>.CreateCopy(graph.Transactions.Update(soLine));
				soLine.CuryDiscAmt = line.CuryDiscAmt;
				soLine = graph.Transactions.Update(soLine);
			}

			if (PXAccess.FeatureInstalled<FeaturesSet.customerDiscounts>())
			{
				foreach (POOrderDiscountDetail discountLine in discountLines)
				{
					SOOrderDiscountDetail soDiscLine = new SOOrderDiscountDetail
					{
						IsManual = true,
					};
					soDiscLine = PXCache<SOOrderDiscountDetail>.CreateCopy(graph.DiscountDetails.Insert(soDiscLine));
					soDiscLine.CuryDiscountAmt = discountLine.CuryDiscountAmt;
					soDiscLine.Description = discountLine.Description;
					soDiscLine.CuryDiscountableAmt = discountLine.CuryDiscountableAmt;
					soDiscLine.DiscountableQty = discountLine.DiscountableQty;
					soDiscLine = graph.DiscountDetails.Update(soDiscLine);
				}
			}

			if (graph.Document.Current != null)
			{
				doc = PXCache<SOOrder>.CreateCopy(graph.Document.Current);
				doc.CuryControlTotal = doc.CuryOrderTotal;
				graph.Document.Update(doc);
			}

			graph.RowPersisted.AddHandler<SOOrder>(Base2.UpdatePOOrderOnSOOrderRowPersisted);
			var uniquenessChecker = new UniquenessChecker<SelectFrom<SOOrder>.Where<SOOrder.FK.IntercompanyPOOrder.SameAsCurrent>>(po);
			graph.OnBeforeCommit += uniquenessChecker.OnBeforeCommitImpl;
			try
			{
				graph.Save.Press();
			}
			finally
			{
				graph.RowPersisted.RemoveHandler<SOOrder>(Base2.UpdatePOOrderOnSOOrderRowPersisted);
				graph.OnBeforeCommit -= uniquenessChecker.OnBeforeCommitImpl;
			}

			return graph.Document.Current;
		}
	}
}
