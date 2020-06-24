<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Orçamentos.aspx.cs" Inherits="Tratorfix.Pages.Admin.Orçamentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="outerContainer">
        <table id="ordersTable">
            <tr>
                <th>Name</th>
                <th>City</th>
                <th>Items</th>
                <th>Total</th>
                <th></th>
            </tr>
            <asp:Repeater runat="server" SelectMethod="GetOrçamentos"
                ItemType="Tratorfix.Models.Orçamento">
                <ItemTemplate>
                    <tr>
                        <td><%#: Item.Nome %></td>
                        <td><%#: Item.Cidade %></td>
 <td><%# Item.LinhasOrçamento.Sum(ol => ol.Quantidade) %></td>                        
                        <td>
                            <asp:PlaceHolder Visible="<%# !Item.Enviado %>"
                                runat="server">
                                <button type="submit" name="dispatch"
                                    value="<%# Item.OrçamentoId %>">
                                    Dispatch</button>
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div id="ordersCheck">
        <asp:CheckBox ID="showDispatched" runat="server" Checked="false"
            AutoPostBack="true" />Show Dispatched Orders?
    </div>
</asp:Content>
