<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" CodeFile="LM501000.aspx.cs" Inherits="Pages_LM501000" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMCustomizations.Graph.ProductReleaseExportProcess"
        PrimaryView="Transactions">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false" PageSize="100" AllowPaging="True">
        <Levels>
            <px:PXGridLevel DataMember="Transactions">
                <Columns>
                    <px:PXGridColumn DataField="Selected" Type="CheckBox" AllowCheckAll="true"></px:PXGridColumn>
                    <px:PXGridColumn DataField="RegionName" />
                    <px:PXGridColumn DataField="InventoryID" />
                    <px:PXGridColumn DataField="InventoryCD" />
                    <px:PXGridColumn DataField="ItemClassCD" />
                    <px:PXGridColumn DataField="MoqAttribute" />
                    <px:PXGridColumn DataField="SpqAttribute" />
                    <px:PXGridColumn DataField="BreakQty" />
                    <px:PXGridColumn DataField="SalesPrice" />
                    <px:PXGridColumn DataField="CuryID" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
        <ActionBar>
        </ActionBar>
        <%--        <Mode AllowUpdate="True"  />--%>
    </px:PXGrid>

</asp:Content>
