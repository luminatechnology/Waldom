<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM101000.aspx.cs" Inherits="Pages_LM101000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMCustomizations.Graph.LUMWaldomSetup"
        PrimaryView="Setup">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab DataMember="Setup" ID="tab" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="FTP">
                <Template>
                    <px:PXLayoutRule ControlSize="" runat="server" ID="CstPXLayoutRule4" StartGroup="True" GroupCaption="WALDOM FTP SETTING"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit1" DataField="FtpHost" Width="300px"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit2" DataField="FTPPort"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit3" DataField="FTPUserName"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit5" DataField="FTPPassword"></px:PXTextEdit>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200"></AutoSize>
    </px:PXTab>
</asp:Content>
