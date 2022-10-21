<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM501001.aspx.cs" Inherits="Pages_LM501001" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LUMCustomizations.Graph.LUMBDSPurchaseReceiptPorcess" PrimaryView="Transactions">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<%--<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:pxformview id="form" runat="server" datasourceid="ds" datamember="Filter" width="100%" height="150px" allowautohide="false">
        <Template>
            <px:PXDateTimeEdit runat="server" ID="edFromDate" DataField="FromDate" Width="180px"></px:PXDateTimeEdit>
            <px:PXDateTimeEdit runat="server" ID="edToDate" DataField="ToDate" Width="180px"></px:PXDateTimeEdit>
            <px:PXDropDown runat="server" ID="edProcessType" DataField="ProcessType" Width="200px"></px:PXDropDown>
        </Template>
    </px:pxformview>
</asp:Content>--%>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="Transactions">
                <Columns>
                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="POOrderNbr" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="POLineNbr" Width="80"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Region" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CSVType" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Vendor" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ReceiptDate" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Sitecd" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="PartNumberCD" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Uom" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Quantity" Width="100"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UnitPrice" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Currency" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="InvoiceNumber" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="InvoiceDate" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ShipVia" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TrackingNumber" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsProcessed" Width="120" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ErrorMessage" Width="200"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreatedDateTime" Width="130" DisplayFormat="g"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXSegmentMask AllowEdit="True" runat="server" ID="CstPXSegmentMask4" DataField="BranchID"></px:PXSegmentMask>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowDelete="True" />
    </px:PXGrid>
</asp:Content>
