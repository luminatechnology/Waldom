using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LUMCustomizations.DAC;
using LUMCustomizations.Helper;
using LUMCustomizations.Helper.Model;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.Licensing;
using PX.Objects.IN;
using PX.Objects.PO;

namespace LUMCustomizations.Graph
{
    public class BinLocationsExportProcess : PXGraph<BinLocationsExportProcess>
    {

        #region const
        const string GI_NAME = "Export Bin Locations";
        #endregion

        #region View
        public PXProcessing<ExportBinLocations, Where<True>, OrderBy<Asc<ExportBinLocations.partNumber>>> Templates;
        public IEnumerable templates()
        {
            PXGenericInqGrph gi = PXGenericInqGrph.CreateInstance(GI_NAME, GI_NAME);
            PXResultset<GenericResult> results = gi.Results.Select();
            int i = 0;
            foreach (GenericResult item in results)
            {
                INItemCost itemCost = item.Values["INItemCost"] as INItemCost;
                INLotSerialStatus lotSerialStatus = item.Values["INLotSerialStatus"] as INLotSerialStatus;
                INSite site = item.Values["INSite"] as INSite;
                InventoryItem inventoryItem = item.Values["InventoryItem"] as InventoryItem;
                POReceiptLineSplit receiptLineSplit = item.Values["POReceiptLineSplit"] as POReceiptLineSplit;

                //INSite
                PXCache<INSite> siteCache = gi.Caches<INSite>();
                INSiteBuilding branch = PXSelectorAttribute.Select<INSite.branchID>(siteCache, site) as INSiteBuilding;

                //InventoryItem
                PXCache<InventoryItem> iiCache = gi.Caches<InventoryItem>();
                INItemClass itemClass = PXSelectorAttribute.Select<InventoryItem.itemClassID>(iiCache, inventoryItem) as INItemClass;

                //INLotSerialStatus
                PXCache<INLotSerialStatus> lotCache = gi.Caches<INLotSerialStatus>();
                InventoryItem lotInventoryItem = PXSelectorAttribute.Select<INLotSerialStatus.inventoryID>(lotCache, lotSerialStatus) as InventoryItem;
                INLocation lotLocation = PXSelectorAttribute.Select<INLotSerialStatus.locationID>(lotCache, lotSerialStatus) as INLocation;
                INSite lotSite = PXSelectorAttribute.Select<INLotSerialStatus.siteID>(lotCache, lotSerialStatus) as INSite;

                yield return new ExportBinLocations()
                {
                    Seq = i++,
                    RegionName = branch?.Descr == "Singapore Office" ? "APAC" : "CHINA",
                    Location = lotSite?.SiteCD,//INLostSerialStatus.siteID
                    UIDNumber = inventoryItem?.InventoryID,//inventoryItem.inventiryID
                    PRC = itemClass?.ItemClassCD,//inventoryItem.itemClassID
                    PartNumber = lotInventoryItem?.InventoryCD,//INLostSerialStatus.inventoryID
                    BinLocation = lotLocation?.LocationCD,//INLostSerialStatus.loactionID
                    Quantity = lotSerialStatus?.QtyOnHand ?? 0,//INLostSerialStatus.QtyOnHand
                    Cost = itemCost?.AvgCost,//INItemCost.avgCost
                    DateCreated = lotSerialStatus?.ReceiptDate,//INLostSerialStatus.ReceiptDate
                    DateCreatedAscending = lotSerialStatus?.ReceiptDate,//INLostSerialStatus.ReceiptDate
                    COO = subStr(lotSerialStatus?.LotSerialNbr, 6, 2),//INLostSerialStatus  =Substring([INLotSerialStatus.LotSerialNbr] , 7, 2)
                    StockRecoveryFlag = "1",
                    AgedInventoryFlag = "",
                    DateCode = left(lotSerialStatus?.LotSerialNbr, 6),//=Left( [INLotSerialStatus.LotSerialNbr], 6)
                    DateCodeDecoded = left(lotSerialStatus?.LotSerialNbr, 6),//=Left( [INLotSerialStatus.LotSerialNbr], 6)
                    LotCode = "",
                    PONumber = receiptLineSplit?.PONbr,//POReceiptLineSplit.PONbr
                    ReceiptNumber = receiptLineSplit?.ReceiptNbr,//POReceiptLineSplit.ReceiptNbr
                    Currency = itemCost?.CuryID,//INItemCost.CuryID
                };
            }

        }
        #endregion

        #region Constructor
        public BinLocationsExportProcess()
        {
            Templates.SetProcessAllCaption("PROCESS");
            Templates.SetProcessVisible(false);
            var _this = this;
            Templates.SetProcessDelegate(delegate (List<ExportBinLocations> list) { DoProcess(list, _this); });
        }
        #endregion

        #region ProcessDelegate
        protected static void DoProcess(List<ExportBinLocations> datas, BinLocationsExportProcess graph)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
                    {
                        #region title
                        string[] title = {
                               "Region","Location","UIDNumber","PRC","PartNumber"
                                ,"BinLocation","Quantity","Cost","DateCreated","DateCreatedAscending"
                                ,"COO","StockRecoveryFlag","AgedInventoryFlag","DateCode","DateCodeDecoded"
                                ,"LotCode","PONumber","ReceiptNumber","Currency"
                            };
                        sw.WriteLine(string.Join("|", title));
                        #endregion
                        #region detail
                        foreach (var item in datas)
                        {
                            List<string> detail = new List<string>();
                            detail.Add(ToCsvStr(item.RegionName));//Region
                            detail.Add(ToCsvStr(item.Location));//Location
                            detail.Add(ToCsvStr(item.UIDNumber?.ToString()));//UIDNumber
                            detail.Add(ToCsvStr(item.PRC));//PRC
                            detail.Add(ToCsvStr(item.PartNumber));//PartNumber
                            detail.Add(ToCsvStr(item.BinLocation));//BinLocation
                            detail.Add(ToCsvStr(item.Quantity?.ToString("0")));//Quantity
                            detail.Add(ToCsvStr(item.Cost?.ToString("#.00000")));//Cost
                            detail.Add(ToCsvStr(item.DateCreated?.ToString("MM/dd/yyyy")));//DateCreated
                            detail.Add(ToCsvStr(item.DateCreatedAscending?.ToString("MM/dd/yyyy")));//DateCreatedAscending
                            detail.Add(ToCsvStr(item.COO));//COO
                            detail.Add(ToCsvStr(item.StockRecoveryFlag));//StockRecoveryFlag
                            detail.Add(ToCsvStr(item.AgedInventoryFlag));//AgedInventoryFlag
                            detail.Add(ToCsvStr(item.DateCode));//DateCode
                            detail.Add(ToCsvStr(item.DateCodeDecoded));//DateCodeDecoded
                            detail.Add(ToCsvStr(item.LotCode));//LotCode
                            detail.Add(ToCsvStr(item.PONumber));//PONumber
                            detail.Add(ToCsvStr(item.ReceiptNumber));//ReceiptNumber
                            detail.Add(ToCsvStr(item.Currency));//Currency
                            sw.WriteLine(string.Join("|", detail));
                        }
                        #endregion
                    }

                    #region FTP
                    var setup = SelectFrom<LUMWaldomPreference>.View.Select(graph).TopFirst;
                    WaldomFTPConfig config = new WaldomFTPConfig()
                    {
                        FtpHost = setup?.FtpHost,
                        FtpUserName = setup?.FTPUserName,
                        FtpPassword = setup?.FTPPassword,
                        FtpPort = setup?.FTPPort?.ToString()
                    };
                    FTPHelper helper = new FTPHelper(config);
                    var uploadResult = helper.UploadFileToFTP(stream.ToArray(), @"/Upload/", $"Export_Bin_Location_INC_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
                    if (!uploadResult)
                        throw new Exception("Upload FTP Fail");
                    #endregion
                }
            }
            catch (PXRedirectToFileException)
            {
                PXProcessing<LUMProductRelease>.SetProcessed();
                throw;
            }
            catch (Exception ex)
            {
                PXProcessing<LUMProductRelease>.SetError(ex);
                throw;
            }
        }

        #endregion

        #region Method
        private static string ToCsvStr(string str)
        {
            if (str == null) return "";
            return "\"" + str.Trim() + "\"";
        }

        private string subStr(string str, int index, int length)
        {
            if (str == null) return null;
            if (index > str.Length - 1) return "";
            if (length > index + length) return "";
            return str.Substring(index, length);
        }
        private string left(string str, int length)
        {
            if (str == null) return null;
            length = length > str.Length ? str.Length : length;
            return subStr(str, 0, length);
        }
        #endregion
    }
}