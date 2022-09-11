using PX.Common;
using PX.Data;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.GL;
using PX.Objects.PO;
using PX.Objects.PO.GraphExtensions.POOrderEntryExt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PX.Objects.SO
{
    public class SOCreateIntercompanySalesOrders_Extension : PXGraphExtension<PX.Objects.SO.SOCreateIntercompanySalesOrders>
    {
		public static bool IsActive() => PX.Data.Update.PXInstanceHelper.CurrentCompany == 3;

		#region Delegate Data View
		public IEnumerable documents()
        {
			List<POForSalesOrderDocument> list = new List<POForSalesOrderDocument>();

			POForSalesOrderFilter filter = Base.Filter.Current;

			if (filter != null)
			{
				using (new PXReadBranchRestrictedScope())
				{
					if (filter.PODocType == POCrossCompanyDocType.PurchaseOrders)
					{
						var orders = PXSelectJoin<POOrder,
								InnerJoin<Branch, On<Branch.branchID, Equal<POOrder.branchID>>,
								InnerJoin<BAccount, On<BAccount.bAccountID, Equal<Branch.bAccountID>,
									And<BAccount.isBranch, Equal<True>>>,
								LeftJoin<SOOrder, On<SOOrder.FK.IntercompanyPOOrder>>>>,
								Where2<Where<POOrder.orderDate, LessEqual<Current<POForSalesOrderFilter.docDate>>,
										Or<Current<POForSalesOrderFilter.docDate>, IsNull>>,
									And2<Where<POOrder.vendorID, Equal<Current<POForSalesOrderFilter.sellingCompany>>,
										Or<Current<POForSalesOrderFilter.sellingCompany>, IsNull>>,
									And2<Where<BAccount.bAccountID, Equal<Current<POForSalesOrderFilter.purchasingCompany>>,
										Or<Current<POForSalesOrderFilter.purchasingCompany>, IsNull>>,
									And<POOrder.isIntercompany, Equal<True>,
									And<POOrder.orderType, Equal<POOrderType.regularOrder>,
									And<POOrder.status, Equal<POOrderStatus.open>,
									And<POOrder.excludeFromIntercompanyProc, Equal<False>,
									And<SOOrder.orderNbr, IsNull>>>>>>>>>.Select(Base);

						foreach (PXResult<POOrder, Branch, BAccount, SOOrder> document in orders)
						{
							POOrder order = (POOrder)document;

							POForSalesOrderDocument doc = new POForSalesOrderDocument
							{
								DocType = order.OrderType,
								DocNbr = order.OrderNbr,
								VendorID = order.VendorID,
								BranchID = order.BranchID,
								CuryID = order.CuryID,
								CuryDocTotal = order.CuryOrderTotal,
								CuryDiscTot = order.CuryDiscTot,
								CuryTaxTotal = order.CuryTaxTotal,
								ExpectedDate = order.ExpectedDate,
								DocDate = order.OrderDate,
								EmployeeID = order.EmployeeID,
								DocDesc = order.OrderDesc
							};

							POForSalesOrderDocument cachedDoc = Base.Documents.Locate(doc);
							if (cachedDoc != null)
							{
								doc.Selected = cachedDoc.Selected;
								doc.Excluded = cachedDoc.Excluded;
							}

							list.Add(Base.Documents.Update(doc));
						}
					}
					else if (filter.PODocType == POCrossCompanyDocType.PurchaseReturns)
					{
						var receipts =
							PXSelectJoin<POReceipt,
									InnerJoin<Branch, On<Branch.branchID, Equal<POReceipt.branchID>>,
									InnerJoin<BAccount, On<BAccount.bAccountID, Equal<Branch.bAccountID>,
											And<BAccount.isBranch, Equal<True>>>,
									LeftJoin<SOOrder,
										On<SOOrder.intercompanyPOReturnNbr, Equal<POReceipt.receiptNbr>>>>>,
									Where2<Where<POReceipt.receiptDate, LessEqual<Current<POForSalesOrderFilter.docDate>>,
											Or<Current<POForSalesOrderFilter.docDate>, IsNull>>,
										And2<Where<POReceipt.vendorID, Equal<Current<POForSalesOrderFilter.sellingCompany>>,
											Or<Current<POForSalesOrderFilter.sellingCompany>, IsNull>>,
										And2<Where<BAccount.bAccountID, Equal<Current<POForSalesOrderFilter.purchasingCompany>>,
											Or<Current<POForSalesOrderFilter.purchasingCompany>, IsNull>>,
										And<POReceipt.isIntercompany, Equal<True>,
										And<POReceipt.receiptType, Equal<POReceiptType.poreturn>,
										And<POReceipt.released, Equal<True>,
										And<POReceipt.excludeFromIntercompanyProc, Equal<False>,
										And<SOOrder.orderNbr, IsNull>>>>>>>>>.Select(Base);

						foreach (PXResult<POReceipt, Branch, BAccount, SOOrder> document in receipts)
						{
							POReceipt poReturn = (POReceipt)document;

							POForSalesOrderDocument doc = new POForSalesOrderDocument
							{
								DocType = poReturn.ReceiptType,
								DocNbr = poReturn.ReceiptNbr,
								VendorID = poReturn.VendorID,
								BranchID = poReturn.BranchID,
								CuryID = poReturn.CuryID,
								CuryDocTotal = poReturn.CuryOrderTotal,
								DocQty = poReturn.OrderQty,
								DocDate = poReturn.ReceiptDate,
								OwnerID = poReturn.OwnerID,
								WorkgroupID = poReturn.WorkgroupID,
								FinPeriodID = poReturn.FinPeriodID
							};

							POForSalesOrderDocument cachedDoc = Base.Documents.Locate(doc);
							if (cachedDoc != null)
							{
								doc.Selected = cachedDoc.Selected;
								doc.Excluded = cachedDoc.Excluded;
							}

							list.Add(Base.Documents.Update(doc));
						}
					}
					else if (filter.PODocType == POCrossCompanyDocType2.DropShip)
					{
						var orders = PXSelectJoin<POOrder, InnerJoin<Branch, On<Branch.branchID, Equal<POOrder.branchID>>,
																	 InnerJoin<BAccount, On<BAccount.bAccountID, Equal<Branch.bAccountID>,
																							 And<BAccount.isBranch, Equal<True>>>,
																			   LeftJoin<SOOrder, On<SOOrder.FK.IntercompanyPOOrder>>>>,
														   Where2<Where<POOrder.orderDate, LessEqual<Current<POForSalesOrderFilter.docDate>>,
																		  Or<Current<POForSalesOrderFilter.docDate>, IsNull>>,
																  And2<Where<POOrder.vendorID, Equal<Current<POForSalesOrderFilter.sellingCompany>>,
																			  Or<Current<POForSalesOrderFilter.sellingCompany>, IsNull>>,
																	   And2<Where<BAccount.bAccountID, Equal<Current<POForSalesOrderFilter.purchasingCompany>>,
																				  Or<Current<POForSalesOrderFilter.purchasingCompany>, IsNull>>,
																 And<POOrder.isIntercompany, Equal<True>,
																 And<POOrder.orderType, Equal<POOrderType.dropShip>,
																 And<POOrder.status, Equal<POOrderStatus.open>,
																 And<POOrder.excludeFromIntercompanyProc, Equal<False>,
																 And<SOOrder.orderNbr, IsNull>>>>>>>>>.Select(Base);

						foreach (PXResult<POOrder, Branch, BAccount, SOOrder> document in orders)
						{
							POOrder order = (POOrder)document;

							POForSalesOrderDocument doc = new POForSalesOrderDocument
							{
								DocType = order.OrderType,
								DocNbr = order.OrderNbr,
								VendorID = order.VendorID,
								BranchID = order.BranchID,
								CuryID = order.CuryID,
								CuryDocTotal = order.CuryOrderTotal,
								CuryDiscTot = order.CuryDiscTot,
								CuryTaxTotal = order.CuryTaxTotal,
								ExpectedDate = order.ExpectedDate,
								DocDate = order.OrderDate,
								EmployeeID = order.EmployeeID,
								DocDesc = order.OrderDesc
							};

							POForSalesOrderDocument cachedDoc = Base.Documents.Locate(doc);

							if (cachedDoc != null)
							{
								doc.Selected = cachedDoc.Selected;
								doc.Excluded = cachedDoc.Excluded;
							}

							list.Add(Base.Documents.Update(doc));
						}
					}
				}
			}

			return list;
		}
		#endregion

		#region Cache Attached
		[PXRemoveBaseAttribute(typeof(POCrossCompanyDocType.ListAttribute))]
		[POCrossCompanyDocType2.List]
		protected void _(Events.CacheAttached<POForSalesOrderFilter.pODocType> e) { }

		[PXMergeAttributes(Method = MergeMethod.Merge)]
		[PXSelector(typeof(Search2<SOOrderType.orderType, InnerJoin<SOOrderTypeOperation, On<SOOrderTypeOperation.orderType, Equal<SOOrderType.orderType>,
																							 And<SOOrderTypeOperation.operation, Equal<SOOrderType.defaultOperation>,
																								 And<SOOrderTypeOperation.iNDocType, NotEqual<IN.INTranType.transfer>>>>>,
								   Where2<Where<Current<POForSalesOrderFilter.pODocType>, Equal<POCrossCompanyDocType.purchaseOrder>, And<SOOrderType.behavior, In3<SOBehavior.sO, SOBehavior.iN>>>,
										  Or2<Where<Current<POForSalesOrderFilter.pODocType>, Equal<POCrossCompanyDocType.purchaseReturn>, And<SOOrderType.behavior, In3<SOBehavior.rM, SOBehavior.cM>>>,
											  Or<Where<Current<POForSalesOrderFilter.pODocType>, Equal<POCrossCompanyDocType2.dropShip>, And<SOOrderType.behavior, In3<SOBehavior.sO, SOBehavior.iN>>>>>>>),
					DescriptionField = typeof(SOOrderType.descr))]
		protected void _(Events.CacheAttached<POForSalesOrderFilter.intercompanyOrderType> e) { }
		#endregion

		#region Event Handlers
		public void _(Events.RowSelected<POForSalesOrderFilter> e, PXRowSelected baseHandler)
        {
			baseHandler?.Invoke(e.Cache, e.Args);

			///<remarks> Override process delegate function. </remarks>
			Base.Documents.SetProcessDelegate(itemsList => GenerateSalesOrder2(itemsList, e.Row));
		}
		#endregion

		#region Static Method
		/// <summary>
		/// Create a new copy standard method to add a new type (Drop ship).
		/// </summary>
		public static void GenerateSalesOrder2(List<POForSalesOrderDocument> itemsList, POForSalesOrderFilter filter)
		{
			if (filter == null) { return; }

			POCreateSalesOrderProcess processingGraph = PXGraph.CreateInstance<POCreateSalesOrderProcess>();

			if (filter.PODocType == POCrossCompanyDocType.PurchaseOrders)
			{
				processingGraph.GenerateSalesOrdersFromPurchaseOrders(itemsList, filter);
			}
			else if (filter.PODocType == POCrossCompanyDocType.PurchaseReturns)
			{
				processingGraph.GenerateSalesOrdersFromPurchaseReturns(itemsList, filter);
			}
			else if (filter.PODocType == POCrossCompanyDocType2.DropShip)
            {
				processingGraph.GetExtension<POCreateSalesOrderProces_Extension>().GenerateSalesOrdersFromDropShip(itemsList, filter);
            }
		}
		#endregion
	}

	public class POCreateSalesOrderProces_Extension : PXGraphExtension<POCreateSalesOrderProcess>
    {
        #region Methods
        public virtual void GenerateSalesOrdersFromDropShip(List<POForSalesOrderDocument> itemsList, POForSalesOrderFilter filter)
		{
			var orderEntry = PXGraph.CreateInstance<POOrderEntry>();

			foreach (POForSalesOrderDocument item in itemsList)
			{
				SetProcessingResult(GenerateSalesOrderFromDropShip(orderEntry, item, filter));
			}
		}

		public virtual ProcessingResult GenerateSalesOrderFromDropShip(POOrderEntry orderEntry, POForSalesOrderDocument item, POForSalesOrderFilter filter)
		{
			ProcessingResult result = new ProcessingResult();
			PXFilteredProcessing<POForSalesOrderDocument, POForSalesOrderFilter>.SetCurrentItem(item);

			orderEntry.Clear();
			orderEntry.Document.Current = orderEntry.Document.Search<POOrder.orderNbr>(item.DocNbr, item.DocType);
			POOrder po = orderEntry.Document.Current;

			if (item.Excluded == true)
			{
				try
				{
					po.ExcludeFromIntercompanyProc = true;
					po = orderEntry.Document.Update(po);
					orderEntry.Save.Press();
				}
				catch (Exception ex)
				{
					result.AddErrorMessage(ex.Message);
				}
				return result;
			}

			List<POLine> pOLines = orderEntry.Transactions.Select().RowCast<POLine>().ToList();

			if (filter.CopyProjectDetails == true)
			{
				int? projectID = null;
				foreach (POLine poLine in pOLines)
				{
					if (projectID == null)
					{
						projectID = poLine.ProjectID;
					}
					else if (projectID != poLine.ProjectID)
					{
						result.AddErrorMessage(Messages.IntercompanyDifferentProjectIDsOnPOLines);
						return result;
					}
				}
			}

			POShipAddress shipAddress = orderEntry.Shipping_Address.Select();
			POShipContact shipContact = orderEntry.Shipping_Contact.Select();

			List<POOrderDiscountDetail> discountLines = orderEntry.DiscountDetails.Select().RowCast<POOrderDiscountDetail>().ToList();

			try
			{
				SOOrder generatedSO = orderEntry.GetExtension<IntercompanyExt>().GenerateIntercompanySalesOrder4DP(po, shipAddress, shipContact, pOLines, discountLines, filter.IntercompanyOrderType, filter.CopyProjectDetails ?? false);

				item.OrderType = generatedSO.OrderType;
				item.OrderNbr = generatedSO.OrderNbr;
				result = ProcessingResult.CreateSuccess(generatedSO);
				result.AddMessage(PXErrorLevel.RowInfo, Messages.SOCreatedSuccessfully, generatedSO.OrderType, generatedSO.OrderNbr);

				if (generatedSO.CuryTaxTotal != item.CuryTaxTotal)
				{
					result.AddMessage(PXErrorLevel.RowWarning, Messages.IntercompanyTaxTotalDiffers);
				}
				if (generatedSO.CuryOrderTotal != item.CuryDocTotal)
				{
					result.AddMessage(PXErrorLevel.RowWarning, Messages.IntercompanyOrderTotalDiffers);
				}
			}
			catch (Exception ex)
			{
				result.AddErrorMessage(ex.Message);
			}

			return result;
		}

		private void SetProcessingResult(ProcessingResult result)
		{
			if (!result.IsSuccess)
			{
				PXFilteredProcessing<POForSalesOrderDocument, POForSalesOrderDocument>.SetError(result.GeneralMessage);
			}
			else if (result.HasWarning)
			{
				PXFilteredProcessing<POForSalesOrderDocument, POForSalesOrderDocument>.SetWarning(result.GeneralMessage);
			}
			else
			{
				PXFilteredProcessing<POForSalesOrderDocument, POForSalesOrderDocument>.SetProcessed();
				PXFilteredProcessing<POForSalesOrderDocument, POForSalesOrderDocument>.SetInfo(result.GeneralMessage);
			}
		}
		#endregion
	}

	#region Inherited Class
	public class POCrossCompanyDocType2 : POCrossCompanyDocType
	{
		public static bool IsActive() => PX.Data.Update.PXInstanceHelper.CurrentCompany == 3;

		public new class ListAttribute : PXStringListAttribute
		{
			public ListAttribute() : base(new[]
										  {
										  	  Pair(PurchaseOrders, Messages.PurchaseOrders),
											  Pair(PurchaseReturns, Messages.PurchaseReturns),
											  Pair(DropShip, Messages.DropShip),
										  })
			{ }
		}

		public const string DropShip = POOrderType.DropShip;

		public class dropShip : PX.Data.BQL.BqlString.Constant<dropShip>
		{
			public dropShip() : base(DropShip) { }
		}
	}
	#endregion
}
