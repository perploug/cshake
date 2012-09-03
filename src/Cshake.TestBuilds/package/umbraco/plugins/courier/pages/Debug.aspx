<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Debug.aspx.cs" Inherits="Umbraco.Courier.UI.Pages.Debug" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <h1>Item providers</h1>
        <asp:repeater id="rp_items" runat="server">
            <ItemTemplate>
                <h4> <%# Eval("Value") %> </h4>
                <small><%# Eval("Key") %> </small><br />
            </ItemTemplate>
         </asp:repeater>

        <h1>persistance providers</h1>
        <asp:repeater id="rp_persistence" runat="server">
            <ItemTemplate>
                <h4> <%# Eval("Value") %> </h4>
                <small><%# Eval("Key") %> </small><br />
            </ItemTemplate>
         </asp:repeater>

        <h1>dataresolver providers</h1>
        <asp:repeater id="rp_dataresolvers" runat="server">
            <ItemTemplate>
                <h4> <%# Eval("Value") %> </h4>
                <small><%# Eval("Key") %> </small><br />
            </ItemTemplate>
         </asp:repeater>

        <h1>repository providers</h1>
        <asp:repeater id="rp_repos" runat="server">
            <ItemTemplate>
                <h4> <%# Eval("Value") %> </h4>
                <small><%# Eval("Key") %> </small><br />
            </ItemTemplate>
         </asp:repeater>
        <h1>event providers</h1>
        <asp:repeater id="rp_events" runat="server">
            <ItemTemplate>
                <h4> <%# Eval("Value") %> </h4>
                <small><%# Eval("Key") %> </small><br />
            </ItemTemplate>
         </asp:repeater>
        <h1>resource resolver providers</h1>
        <asp:repeater id="rp_resourceresolvers" runat="server">
            <ItemTemplate>
                <h4> <%# Eval("Value") %> </h4>
                <small><%# Eval("Key") %> </small><br />
            </ItemTemplate>
         </asp:repeater>
    </div>
    </form>
</body>
</html>
