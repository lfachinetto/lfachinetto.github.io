<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListaProdutos.ascx.cs" Inherits="Tratorfix.Controls.ListaProdutos" %>

<%=CreateHomeLinkHtml() %>

<% foreach (string cat in GetProdutos())
    {
        Response.Write(CreateLinkHtml(cat));
    }%>
