<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Lista.aspx.cs" MasterPageFile="/Pages/Site.Master" Inherits="Tratorfix.Pages.Lista" %>

<%@ Import Namespace="System.Web.Routing" %>

<asp:Content ContentPlaceHolderID="bodyContent" runat="server">
    <div style="width: auto; margin: 0 10%;">
        <br />

        <div class="sticky-top shadow-none p-3 mb-5 bg-light rounded text-right">
        <SS:CartSummary runat="server" />
            <br />
        <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" Text="Item adicionado ao orçamento" runat="server" Visible="false" />

            <script type="text/javascript">
    function HideLabel() {
        var seconds = 5;
        setTimeout(function () {
            document.getElementById("<%=lblMessage.ClientID %>").style.display = "none";
        }, seconds * 1000);
    };
</script>

            </div>

        <br />
        <asp:Repeater ItemType="Tratorfix.Models.Produto" SelectMethod="GetProdutos" runat="server">
            <ItemTemplate>
                <ul class="list-group">
                    <li class="list-group-item">
                        <div class="card-deck">
                        <div class="card" style="border: none;">
                            <img style="max-width:200px;" src="<%# Item.Imagem%>" />
                            </div>
                            <div class="card" style="border: none;">
                                        <h3><%# Item.Nome %></h3>
                                        <%# Item.Descrição %>
                                </div>
                            <div class="col-sm-3">
                            <div class="card" style="border:none;">
                                        Medida:<br />
                                        <input type="text" class="form-control" style="color: black" name="medida<%# Item.ProdutoId %>"/>
                                        Quantidade:<br />
                                        <input type="text" class="form-control" style="color: black" name="quantidade<%# Item.ProdutoId %>"/><br />                              
                                        <button name="add" type="submit" class="btn btn-primary" value="<%# Item.ProdutoId %>">Adicionar ao orçamento</button> 
                                </div>
                                </div>
                            </div>
                    </li>
                </ul>

            </ItemTemplate>

        </asp:Repeater>

    </div>
    <!--Parte com erro
    <div class="pager">
        <%/*for (int i = 1; i <= MaxPage; i++)
            {
                string path = RouteTable.Routes.GetVirtualPath(null, null, new RouteValueDictionary() { { "page", i } }).VirtualPath;
                Response.Write(
                    string.Format("<a href='{0}' {1}>{2}</a>",
                    path, i == CurrentPage ? "class='selected'" : "", i));
            }*/%>
    </div>-->
</asp:Content>
