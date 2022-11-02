<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM501001.aspx.cs" Inherits="Pages_LM501001"  Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMCustomizations.Graph.BinLocationsExportProcess"
        PrimaryView="Templates"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="Templates">
			    <Columns>
				<px:PXGridColumn DataField="RegionName" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Location" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="UIDNumber" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PRC" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PartNumber" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="BinLocation" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Quantity" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Cost" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DateCreated" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DateCreatedAscending" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="COO" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="StockRecoveryFlag" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="AgedInventoryFlag" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DateCode" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DateCodeDecoded" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LotCode" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PONumber" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ReceiptNumber" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Currency" Width="70" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>