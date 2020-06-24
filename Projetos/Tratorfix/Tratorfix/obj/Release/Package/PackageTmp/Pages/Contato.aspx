<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contato.aspx.cs" MasterPageFile="/Pages/Site.Master" Inherits="Tratorfix.Pages.Contato" %>

<asp:Content ContentPlaceHolderID="bodyContent" runat="server">

    <!-- Load Facebook SDK for JavaScript -->
    <div id="fb-root"></div>
    <script>(function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = 'https://connect.facebook.net/pt_BR/sdk/xfbml.customerchat.js';
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));</script>

    <!-- Your customer chat code -->
    <div class="fb-customerchat"
        attribution="setup_tool"
        page_id="760177791026930"
        theme_color="#0084ff"
        logged_in_greeting="Olá! Como posso ajudá-lo?"
        logged_out_greeting="Olá! Como posso ajudá-lo?">
    </div>

    <div style="width: auto; margin: 0 30%;">
        <br />
        <center><h1>Tratorfix Comércio de Parafusos</h1></center>
        <b>Endereço:</b><br />
        Av. Brasil Leste, 915 - Bairro Petrópolis<br />
        CEP 99050-073
        Passo Fundo - RS
        <br />
        <b>E-mail:</b><br />
        <a href="malito:tratorfixparafusos@gmail.com">tratorfixparafusos@gmail.com</a>
        <br />
        <b>Telefone:</b><br />
        <a href="tel:5433116175">(54) 3311-6175</a><br />

        <!--<h2>Localização</h2>
        <iframe width="600" height="450" frameborder="0" style="border: 0" src="https://www.google.com/maps/embed/v1/place?q=tratorfix%20com%C3%A9rcio%20de%20parafusos&key=AIzaSyBImxmSyNpySVjIae-9bnf6wPUIQQrn7iY" allowfullscreen></iframe>
        -->

        <br />
        <br />
        <div id="contato" runat="server">
            <center><h1>Formulário de contato</h1></center>
            <form method="post" action="Contato.aspx">
                <div class="form-group">
                    <label for="exampleFormControlInput1">Nome:</label>
                    <input type="text" class="form-control" style="color: black" id="nome" name="nome" placeholder="João da Silva">
                </div>
                <div class="form-group">
                    <label for="exampleFormControlInput2">E-mail:</label>
                    <input type="email" class="form-control" style="color: black" id="email" name="email" placeholder="nome@exemplo.com">
                </div>
                <div class="form-group">
                    <label for="exampleFormControlInput3">Telefone:</label>
                    <input type="tel" class="form-control" style="color: black" id="telefone" name="telefone" placeholder="(xx) xxxx-xxxx">
                </div>
                <div class="form-group">
                    <label for="exampleFormControlInput4">Assunto:</label>
                    <input type="text" class="form-control" style="color: black" id="assunto" name="assunto" placeholder="">
                </div>
                <div class="form-group">
                    <label for="exampleFormControlTextarea1">Mensagem:</label><br />
                    <textarea class="form-control" id="mensagem" name="mensagem" rows="3"></textarea>
                </div>
                <asp:Button ID="enviar" class="btn btn-primary" Text="Enviar" OnClick="enviar_Click" runat="server" />
            </form>
        </div>
        <div id="checkoutMessage" runat="server">
            <h2>Mensagem enviada!</h2>
            Agradecemos seu contato. Você receberá uma resposta o mais breve possível.
        </div>
    </div>
</asp:Content>
