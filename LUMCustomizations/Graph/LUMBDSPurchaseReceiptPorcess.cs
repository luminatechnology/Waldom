using LUMCustomizations.DAC;
using LUMCustomizations.Helper;
using LUMCustomizations.Helper.Model;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CR;
using PX.Objects.PO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Data.PXAccess;

namespace LUMCustomizations.Graph
{
    public class LUMBDSPurchaseReceiptPorcess : PXGraph<LUMBDSPurchaseReceiptPorcess>
    {
        public PXSave<LUMBDSPurchaseReceipt> Save;
        public PXCancel<LUMBDSPurchaseReceipt> Cancel;
        //public PXProcessing<LUMBDSPurchaseReceipt, Where<LUMBDSPurchaseReceipt.isProcessed, Equal<False>, Or<LUMBDSPurchaseReceipt.isProcessed, Equal<Null>>>> Transactions;
        public PXProcessing<LUMBDSPurchaseReceipt> Transactions;

        public LUMBDSPurchaseReceiptPorcess()
        {
            Transactions.SetProcessDelegate(delegate (List<LUMBDSPurchaseReceipt> list)
            {
                GoProcessing(list);
            });
        }

        #region Action
        public PXAction<LUMBDSPurchaseReceipt> importCSVData;
        [PXUIField(DisplayName = "Import CSV Data From FTP", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Visible = true)]
        [PXProcessButton(IsLockedOnToolbar = true)]
        public virtual IEnumerable ImportCSVData(PXAdapter adapter)
        {
            PXLongOperation.StartOperation(this, delegate ()
            {
                InsertCSVData(this);
            });
            return adapter.Get();
        }
        #endregion


        #region Method

        public static void GoProcessing(List<LUMBDSPurchaseReceipt> list)
        {
            var baseGraph = CreateInstance<LUMBDSPurchaseReceiptPorcess>();
            baseGraph.CreatePOReceipt(baseGraph, list);
        }

        public static void InsertCSVData(LUMBDSPurchaseReceiptPorcess baseGraph)
        {
            var setup = SelectFrom<LUMWaldomPreference>.View.Select(baseGraph).TopFirst;
            WaldomFTPConfig config = new WaldomFTPConfig()
            {
                FtpHost = setup?.FtpHost,
                FtpUserName = setup?.FTPUserName,
                FtpPassword = setup?.FTPPassword,
                FtpPort = setup?.FTPPort?.ToString()
            };
            FTPHelper helper = new FTPHelper(config);
            var fileList = helper.GetFTPFileList("/DownLoad/");
            var csv = helper.GetCSVFile("/DownLoad/", fileList);
            foreach (var csvLine in csv.Skip(1))
            {
                var data = csvLine.Split('|');
                var line = baseGraph.Transactions.Cache.CreateInstance() as LUMBDSPurchaseReceipt;
                #region Convert CSV to Entity
                line.Region = data[0];
                line.CSVType = data[1];
                line.Vendor = data[2];
                if (!string.IsNullOrEmpty(data[3]))
                    line.ReceiptDate = DateTime.Parse(data[3]);
                line.POOrderNbr = data[4];
                line.Sitecd = data[5];
                line.POLineNbr = int.Parse(data[6]);
                line.PartNumberCD = data[7];
                line.Uom = data[8];
                if (!string.IsNullOrEmpty(data[9]))
                    line.Quantity = int.Parse(data[9]);
                if (!string.IsNullOrEmpty(data[10]))
                    line.UnitPrice = decimal.Parse(data[10]);
                line.DateCode = data[11];
                line.Currency = data[12];
                line.InvoiceNumber = data[13];
                if (!string.IsNullOrEmpty(data[14]))
                    line.InvoiceDate = DateTime.Parse(data[14]);
                line.ShipVia = data[15];
                line.TrackingNumber = data[16];
                #endregion
                baseGraph.Transactions.Cache.Insert(line);
            }
            baseGraph.Save.Press();
        }

        public virtual void CreatePOReceipt(LUMBDSPurchaseReceiptPorcess baseGraph, List<LUMBDSPurchaseReceipt> selectedList)
        {

            foreach (var poReceiptGroupData in selectedList.GroupBy(x => new { x.Region, x.Vendor, x.CSVType, x.ReceiptDate, x.Currency, x.InvoiceNumber, x.InvoiceDate, x.TrackingNumber }))
            {
                var errorMsg = string.Empty;
                var DisplayGroupKey = $"{poReceiptGroupData.Key.Region}, {poReceiptGroupData.Key.Vendor}";
                PXLongOperation.SetCurrentItem(poReceiptGroupData.FirstOrDefault());
                try
                {
                    using (PXTransactionScope sc = new PXTransactionScope())
                    {
                        var getCurrentBranchID = SelectFrom<Branch>
                                                .Where<Branch.branchCD.IsEqual<P.AsString>>
                                                .View.Select(baseGraph, poReceiptGroupData.Key.Region == "CHINA" ? "SH" : "SG").TopFirst;
                        if (getCurrentBranchID == null)
                            throw new PXException($"BranchID: {poReceiptGroupData.Key.Region} can not find!!");
                        PXContext.SetBranchID(getCurrentBranchID.BranchID);
                        var poReceiptGraph = PXGraph.CreateInstance<POReceiptEntry>();

                        #region Header
                        var poDoc = poReceiptGraph.Document.Insert(poReceiptGraph.Document.Cache.CreateInstance() as POReceipt);
                        poDoc.ReceiptType = poReceiptGroupData.Key.CSVType == "Receipt" ? POReceiptType.POReceipt :
                                            poReceiptGroupData.Key.CSVType == "Return" ? POReceiptType.POReturn : POReceiptType.TransferReceipt;
                        poDoc.ReceiptDate = poReceiptGroupData.Key.ReceiptDate;
                        poDoc.VendorID = GetVendorInfoByAcctCD(baseGraph,$"{(poReceiptGroupData.Key.Region == "CHINA" ? "NV" : "SV")}{poReceiptGroupData.Key.Vendor}")?.BAccountID;
                        //poDoc.VendorID = GetVendorInfoByAcctCD(baseGraph, $"{poReceiptGroupData.Key.Vendor}")?.BAccountID;
                        poDoc.CuryID = poReceiptGroupData.Key.Currency;
                        poDoc.AutoCreateInvoice = true;
                        poDoc.InvoiceNbr = poReceiptGroupData.Key.InvoiceNumber;
                        poReceiptGraph.Document.Cache.Update(poDoc);
                        #endregion

                        #region User-defined
                        poReceiptGraph.Document.Cache.SetValueExt(poDoc, PX.Objects.CS.Messages.Attribute + "TRACKNBR", poReceiptGroupData.Key.TrackingNumber);
                        #endregion

                        #region POReceipt Line
                        foreach (var line in poReceiptGroupData)
                        {
                            // Setting ADD PO Line Filter
                            var filter = poReceiptGraph.filter.Current;
                            poReceiptGraph.filter.SetValueExt<POReceiptEntry.POOrderFilter.orderType>(filter, POOrderType.DropShip);
                            poReceiptGraph.filter.SetValueExt<POReceiptEntry.POOrderFilter.orderNbr>(filter, line?.POOrderNbr);
                            poReceiptGraph.filter.UpdateCurrent();
                            var poLineResult = poReceiptGraph.poLinesSelection.Select().Where(x => x.GetItem<POReceiptEntry.POLineS>().LineNbr == line.POLineNbr);
                            if (poLineResult.Count() == 0)
                                throw new PXException($"Can not find mapping POLine {line.POOrderNbr},{line.POLineNbr}");
                            foreach (var selectedLine in poLineResult)
                            {
                                var it = selectedLine.GetItem<POReceiptEntry.POLineS>() as POLine;
                                var poline = poReceiptGraph.AddPOLine(it);
                                if (poline != null)
                                {
                                    // update receipt Qty
                                    poline.ReceiptQty = line.Quantity;
                                    PXNoteAttribute.SetNote(poReceiptGraph.transactions.Cache, poline, line.DateCode);
                                    poReceiptGraph.AddPOOrderReceipt(it.OrderType, it.OrderNbr);
                                }
                            }
                        }
                        #endregion
                        poReceiptGraph.Save.Press();
                        poReceiptGraph.releaseFromHold.Press();
                        poReceiptGraph.release.Press();
                        sc.Complete();
                    }
                }
                catch (PXOuterException ex)
                {
                    errorMsg = $"Key:{DisplayGroupKey} Error: {ex.InnerMessages[0]}";
                    var errorItem = PXLongOperation.GetCurrentItem() as LUMBDSPurchaseReceipt;
                    errorItem.ErrorMessage = errorMsg;
                }
                catch (Exception ex)
                {
                    errorMsg = $"Key:{DisplayGroupKey} Error: {ex.Message}";
                    var errorItem = PXLongOperation.GetCurrentItem() as LUMBDSPurchaseReceipt;
                    errorItem.ErrorMessage = errorMsg;
                }
                finally
                {
                    // 有錯誤訊息
                    if (!string.IsNullOrEmpty(errorMsg))
                        PXProcessing.SetError<LUMBDSPurchaseReceipt>(errorMsg);
                    poReceiptGroupData.ToList().ForEach(x =>
                    {
                        x.IsProcessed = string.IsNullOrEmpty(errorMsg);
                        x.ErrorMessage = (x.IsProcessed ?? false) ? string.Empty : x.ErrorMessage;
                        baseGraph.Transactions.Update(x);
                    });
                    // Save
                    baseGraph.Actions.PressSave();
                }
            }// end foreach
        }

        public BAccount2 GetVendorInfoByAcctCD(LUMBDSPurchaseReceiptPorcess baseGraph, string acctCD)
            => SelectFrom<BAccount2>
              .Where<BAccount2.acctCD.IsEqual<P.AsString>>
              .View.Select(baseGraph, acctCD).TopFirst;

        #endregion

    }
}
