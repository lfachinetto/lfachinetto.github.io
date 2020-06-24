<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrinhoView.aspx.cs" Inherits="Tratorfix.Pages.CarrinhoView" MasterPageFile="~/Pages/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodyContent"  runat="server">
         <div style="width: auto; margin: 0 10%;">

             <br />
        <div class="alert alert-info" role="alert">
        <p>Se preferir, faça seu orçamento entrando entrando em contato conosco:</p>
             <a href="<%= ContatoUrl %>"><button type="button" class="btn btn-secondary">Entrar em contato</button></a>
             </div>

             <br />
             <center><h1>Orçamento online</h1></center>
        <!--<h2>Produtos listados no site</h2></p>-->
        <br />
             <div id="area" visible="false" runat="server" />
        <table class="table">
            <thead><tr>
                <th>Nº do item</th>
                <th>Produto</th>
                <th>Medida/Tipo</th>
                <th>Quantidade</th>
                <th></th>
                <th></th>
                    </tr></thead>
             <tbody>
                 <asp:Repeater ItemType="Tratorfix.Models.CartLine" SelectMethod="GetCartLines" runat="server" EnableViewState="false">
                     <ItemTemplate>
                         <tr>
                             <td><%# Item.CartLineId %></td>
                             <td><%# Item.Product.Nome %></td>
                             <td><%# Item.Measure %></td>
                              <td><%# Item.Quantity %></td>         
                             <td>
                                 <a href="alterar"><button type="submit" class="btn btn-info" name="alterar"
                                     value="<%#Item.CartLineId %>">Alterar</button></a>

                                 <button type="submit" class="btn btn-danger" name="remove"
                                     value="<%#Item.CartLineId %>">
                                     Remover</button>
                             </td>
                         </tr>
                     </ItemTemplate>
                 </asp:Repeater>
             </tbody>
        </table>

             <a href="/Pages/Lista.aspx">
                 <button type="button" class="btn btn-primary float-right">Ver mais produtos do site...</button></a></right>
             <br />
             <br />
             <center><a href="<%= CheckoutUrl %>"><button type="button" class="btn btn-success">Solicitar orçamento</button></a></center>



             


             <!-- Flexbox container for aligning the toasts -->
             <!--<div aria-live="polite" aria-atomic="true" class="d-flex justify-content-center align-items-center" style="min-height: 200px;" name="oi">-->

                 <!-- Then put toasts within -->
                 <!--<div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
                     <div class="toast-header">
                         <img src="..." class="rounded mr-2" alt="...">
                         <strong class="mr-auto">Bootstrap</strong>
                         <small>11 mins ago</small>
                         <button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">
                             <span aria-hidden="true">&times;</span>
                         </button>
                     </div>
                     <div class="toast-body">
                         Hello, world! This is a toast message.
                     </div>
             </div>-->
                 </div>
</asp:Content>
