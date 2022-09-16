using LUMCustomizations.DAC;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.IN;
using PX.Objects.AR;
using System.IO;
using LUMCustomizations.Helper.Model;
using LUMCustomizations.Helper;

namespace LUMCustomizations.Graph
{
    public class ProductReleaseExportProcess : PXGraph<ProductReleaseExportProcess>
    {
        public PXSave<LUMProductRelease> Save;

        public PXCancel<LUMProductRelease> Cancel;

        public PXProcessing<LUMProductRelease> Transactions;

        [PXHidden]
        public SelectFrom<InventoryItem>.View InventoryList;

        public ProductReleaseExportProcess()
        {
            this.Transactions.SetProcessVisible(false);
            this.Transactions.SetProcessDelegate(delegate (List<LUMProductRelease> list)
                {
                    GoProcessing(list);
                });
        }

        public IEnumerable transactions()
        {
            PXGenericInqGrph gi = PXGenericInqGrph.CreateInstance("EXPORT PRODUCT RESALES", "EXPORT PRODUCT RESALES");
            PXResultset<GenericResult> results = gi.Results.Select();
            foreach (var item in results)
            {
                ARSalesPrice salePriceInfo = ((GenericResult)item).Values["ARSalesPrice"] as ARSalesPrice;
                InventoryItem itemInfo = ((GenericResult)item).Values["InventoryItem"] as InventoryItem;
                var moqAttribute = InventoryList.View.Cache.GetValueExt(itemInfo, PX.Objects.CS.Messages.Attribute + "MOQ") as PXFieldState;
                var spqAttribute = InventoryList.View.Cache.GetValueExt(itemInfo, PX.Objects.CS.Messages.Attribute + "SPQ") as PXFieldState;
                INItemClass itemClassInfo = INItemClass.PK.Find(this, itemInfo?.ItemClassID);
                yield return new LUMProductRelease()
                {
                    RegionName = salePriceInfo?.CuryID == "USD" ? "APAC" : "CHINA",
                    InventoryCD = itemInfo?.InventoryCD,
                    InventoryID = itemInfo?.InventoryID,
                    ItemClassCD = itemClassInfo?.ItemClassCD,
                    MoqAttribute = (string)moqAttribute?.Value,
                    SpqAttribute = (string)spqAttribute?.Value,
                    BreakQty = salePriceInfo?.BreakQty,
                    SalesPrice = salePriceInfo?.SalesPrice,
                    CuryID = salePriceInfo?.CuryID
                };
            }
        }

        #region Method

        public static void GoProcessing(List<LUMProductRelease> list)
        {
            var graph = PXGraph.CreateInstance<ProductReleaseExportProcess>();
            graph.ExportCsv(list, graph);
        }

        public void ExportCsv(List<LUMProductRelease> list, ProductReleaseExportProcess baseGraph)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter sw = new StreamWriter(stream, Encoding.ASCII))
                    {
                        // Get Data
                        List<LUMProductRelease> data = new List<LUMProductRelease>();
                        PXGenericInqGrph gi = PXGenericInqGrph.CreateInstance("EXPORT PRODUCT RESALES", "EXPORT PRODUCT RESALES");
                        PXResultset<GenericResult> results = gi.Results.Select();
                        foreach (var item in results)
                        {
                            ARSalesPrice salePriceInfo = ((GenericResult)item).Values["ARSalesPrice"] as ARSalesPrice;
                            InventoryItem itemInfo = ((GenericResult)item).Values["InventoryItem"] as InventoryItem;
                            var moqAttribute = InventoryList.View.Cache.GetValueExt(itemInfo, PX.Objects.CS.Messages.Attribute + "MOQ") as PXFieldState;
                            var spqAttribute = InventoryList.View.Cache.GetValueExt(itemInfo, PX.Objects.CS.Messages.Attribute + "SPQ") as PXFieldState;
                            INItemClass itemClassInfo = INItemClass.PK.Find(this, itemInfo?.ItemClassID);
                            data.Add(new LUMProductRelease()
                            {
                                RegionName = salePriceInfo?.CuryID == "USD" ? "APAC" : "CHINA",
                                InventoryCD = itemInfo?.InventoryCD,
                                InventoryID = itemInfo?.InventoryID,
                                ItemClassCD = itemClassInfo?.ItemClassCD,
                                MoqAttribute = (string)moqAttribute?.Value,
                                SpqAttribute = (string)spqAttribute?.Value,
                                BreakQty = salePriceInfo?.BreakQty,
                                SalesPrice = salePriceInfo?.SalesPrice,
                                CuryID = salePriceInfo?.CuryID
                            });
                        }

                        var groupResult = data.GroupBy(x => new { x.RegionName, x.ItemClassCD, x.InventoryID, x.InventoryCD, x.MoqAttribute, x.SpqAttribute, x.CuryID });
                        var maxColumnCount = groupResult.Max(x => x.Count());

                        #region Header
                        List<string> Header = new List<string>();
                        Header.Add("Region");
                        Header.Add("PRC");
                        Header.Add("PartNumber");
                        Header.Add("UIDNumber");
                        Header.Add("ReleaseMOQ");
                        Header.Add("ReleaseSPQ");
                        for (int i = 1; i <= maxColumnCount; i++)
                        {
                            Header.Add($"ReleaseQuantity{i}");
                            Header.Add($"ReleasePrice{i}");
                        }
                        Header.Add("Currency");
                        // Write Header
                        sw.WriteLine(string.Join("|", Header));
                        #endregion

                        #region Details
                        foreach (var item in groupResult)
                        {
                            List<string> line = new List<string>();
                            line.Add(item.Key?.RegionName);
                            line.Add(item.Key?.ItemClassCD);
                            line.Add(item.Key?.InventoryCD);
                            line.Add(item.Key?.InventoryID?.ToString("0"));
                            line.Add(item.Key?.MoqAttribute);
                            line.Add(item.Key?.SpqAttribute);
                            for (int i = 0; i < maxColumnCount; i++)
                            {
                                line.Add(item.ElementAtOrDefault(i)?.BreakQty?.ToString("0"));
                                line.Add(item.ElementAtOrDefault(i)?.SalesPrice?.ToString("0.00000"));
                            }
                            line.Add(item.Key?.CuryID);
                            sw.WriteLine(string.Join("|", line));
                            // Export File
                        }
                        #endregion

                        var setup = SelectFrom<LUMWaldomPreference>.View.Select(this).TopFirst;
                        WaldomFTPConfig config = new WaldomFTPConfig()
                        {
                            FtpHost = setup?.FtpHost,
                            FtpUserName = setup?.FTPUserName,
                            FtpPassword = setup?.FTPPassword,
                            FtpPort = setup?.FTPPort?.ToString()
                        };
                        FTPHelper helper = new FTPHelper(config);
                        var uploadResult = helper.UploadFileToFTP(stream.ToArray(), @"/Upload/", $"EXPORT_PRODUCT_RELEASE_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
                        if (!uploadResult)
                            throw new Exception("Upload FTP Fail");
                    }
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
    }
}
