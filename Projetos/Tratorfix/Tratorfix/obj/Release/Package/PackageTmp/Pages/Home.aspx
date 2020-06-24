<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" MasterPageFile="/Pages/Site.Master" Inherits="Tratorfix.Pages.Home" %>

<%@ Import Namespace="System.Web.Routing" %>

<asp:Content ContentPlaceHolderID="bodyContent" runat="server">
    <!--<div id="fb-root"></div>
    <script>(function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = 'https://connect.facebook.net/pt_BR/sdk.js#xfbml=1&version=v3.2';
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));</script>-->

    <!-- Load Facebook SDK for JavaScript -->
      <div id="fb-root"></div>
      <script>
          window.fbAsyncInit = function () {
              FB.init({
                  xfbml: true,
                  version: 'v4.0'
              });
          };

          (function (d, s, id) {
              var js, fjs = d.getElementsByTagName(s)[0];
              if (d.getElementById(id)) return;
              js = d.createElement(s); js.id = id;
              js.src = 'https://connect.facebook.net/pt_BR/sdk/xfbml.customerchat.js';
              fjs.parentNode.insertBefore(js, fjs);
          }(document, 'script', 'facebook-jssdk'));</script>

      <!-- Your customer chat code -->
      <div class="fb-customerchat"
        attribution=setup_tool
        page_id="760177791026930"
  logged_in_greeting="Olá! Posso te ajudar?"
  logged_out_greeting="Olá! Posso te ajudar?">
      </div>

    <script src="/Scripts/jquery-3.3.1.slim.min.js"></script>
    <script src="/Scripts/bootstrap.min.js"></script>

    <div id="myCarousel" class="carousel slide" style="max-width: 100%; height: auto" data-ride="carousel">

        <ol class="carousel-indicators">
            <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
            <!--<li data-target="#myCarousel" data-slide-to="1"></li>
            <li data-target="#myCarousel" data-slide-to="2"></li>-->
        </ol>

        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="/Content/Images/Banner.jpg" alt="..." width="100%" height="100%" preserveaspectratio="xMidYMid slice" focusable="false" /> <!-- Foto da loja com endereço -->
            </div>
            <!--<div class="carousel-item">
                <img src="/Content/Images/Banner.jpg" alt="..." width="100%" height="100%" preserveaspectratio="xMidYMid slice" focusable="false">
            </div>-->
            <!--<div class="carousel-item">
                <img src="/Content/Images/final-ano-escolas.jpg" alt="..." width="100%" height="100%" preserveaspectratio="xMidYMid slice" focusable="false">
            </div>-->
        </div>

        <a class="carousel-control-prev" href="#myCarousel" role="button" data-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>

            <span class="sr-only">Anterior</span>
        </a>
        <a class="carousel-control-next" href="#myCarousel" role="button" data-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="sr-only">Próximo</span>
        </a>
    </div>
    <br />

    <div class="container">

        <div class="row">

            <div class="col">
                <div class="fb-page" data-href="https://www.facebook.com/tratorfixparafusos" data-tabs="timeline" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true">
                    <blockquote cite="https://www.facebook.com/tratorfixparafusos" class="fb-xfbml-parse-ignore"><a href="https://www.facebook.com/tratorfixparafusos"></a></blockquote>
                </div>
                <br />
            </div>

            <div class="col">

                <h1>Produtos</h1>

                <div class="card-deck">
                    <asp:Repeater ItemType="Tratorfix.Models.Produto" SelectMethod="GetProdutos" runat="server">
                        <ItemTemplate>

                            <div class="card mb-3">
                                <center>
                                        <br />
                            <img src="<%# Item.Imagem%>" style="max-width: 100px;" class="card-img-top" alt="...">
                            <div class="card-body">
                              <span class="text" style="max-width: 150px;"><h5 class="card-title"><%# Item.Nome.Replace("@", "\r\n") %></h5></span>
                            </div>
                            </div>

                            <%  if (this.i == 2)
                                {%>
                            <br />
                            </div>
                        <div class="card-deck">
                            <%i = 0;
                                }
                                i++; %>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <a href="<%= ProdutosUrl %>">
                    <button type="button" class="btn btn-primary float-right">Ver mais produtos...</button></a>

            </div>

        </div>

        <br />
        <br />
        <div name="newsletter" runat="server"><center>
        <!--<div class="alert alert-info" role="alert">-->

            <!-- Begin Mailchimp Signup Form -->
            <link href="//cdn-images.mailchimp.com/embedcode/horizontal-slim-10_7.css" rel="stylesheet" type="text/css">
            <style type="text/css">
                #mc_embed_signup {
                    background: #fff;
                    clear: left;
                    font: 14px Helvetica,Arial,sans-serif;
                    width: 100%;
                }
                /* Add your own Mailchimp form style overrides in your site stylesheet or in this style block.
	   We recommend moving this block and the preceding CSS link to the HEAD of your HTML file. */
            </style>
            <div id="mc_embed_signup">
                <form action="https://tratorfix.us20.list-manage.com/subscribe/post?u=c9e1647d4147e15f0d798c0f8&amp;id=9d33cc1232" method="post" id="mc-embedded-subscribe-form" name="mc-embedded-subscribe-form" class="validate" target="_blank" novalidate>
                    <div id="mc_embed_signup_scroll">
                        <label for="mce-EMAIL">Inscreva-se em nosso newsletter!</label>
                        <input type="email" value="" name="EMAIL" class="email" id="mce-EMAIL" placeholder="E-mail" required>
                        <!-- real people should not fill this in and expect good things - do not remove this or risk form bot signups-->
                        <div style="position: absolute; left: -5000px;" aria-hidden="true">
                            <input type="text" name="b_c9e1647d4147e15f0d798c0f8_9d33cc1232" tabindex="-1" value=""></div>
                        <div class="clear">
                            <input type="submit" value="Subscribe" name="subscribe" id="mc-embedded-subscribe" class="button"></div>
                    </div>
                </form>
            </div>

            <!--End mc_embed_signup-->

            <!--<div id="newstaller" style="width: auto; margin: 0 20%;" runat="server">
          <form method="post" action="Home.aspx">
          <center>Cadastre-se em nosso newsletter e receba nossos produtos e promoções:</center>

          <div class="form-row">
                <div class="form-group col-md-5">
                    <label for="exampleFormControlInput1">Nome:</label>
                    <input type="text" class="form-control" style="color: black" id="nome" name="nome" placeholder="João da Silva">
                </div>
                <div class="form-group col-md-5">
                    <label for="exampleFormControlInput2">E-mail:</label>
                    <input type="email" class="form-control" style="color: black" id="email" name="email" placeholder="nome@exemplo.com">
                </div>
              <div class="form-group col-md-2">
                  <br />
                  <asp:Button id="cadastrar" class="btn btn-primary" OnClick="cadastrar_Click" runat="server" Text="Cadastrar" />
                  </div>
            </div>
              </form>
        </div>

        <div id="successMessage" runat="server" visible="false">
            <h3 style="text-align:center;color:green;">E-mail cadastrado com sucesso!</h3>
        </div>-->

        <!--</div>-->
        </center><br /></div>
        
</asp:Content>
