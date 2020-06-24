<%@ Page Title="Envio" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="~/Pages/Envio.aspx.cs" Inherits="Tratorfix.Pages.Envio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodyContent" runat="server">
    <div style="width: auto; margin: 0 30%;">

        <div id="envioForm" class="envio" runat="server">
            <h2>Solicite seu orçamento</h2>
            Por favor,
            <!--faça login ou -->
            preencha seus dados para solicitar seu orçamento.
        
        <div id="errors" style="color: red;" data-valmsg-summary="true">
            <ul>
                <li style="display: none;"></li>
            </ul>
            <asp:ValidationSummary runat="server" />
        </div>
            <!--
        <div id="login">
            <h2>Opção 1: Fazer login (em breve)</h2>
            Se já possui cadastro no site, digite seu usuário e senha para não precisar preencher seus dados novamente.
            
            <br/>

            <div>
                <label for="usuario">E-mail/CPF/CNPJ:</label>
                <input id="usuario" name="usuario" />
            </div>
            <div>
                <label for="senha">Senha:</label>
                <input id="senha" name="senha" />
            </div>
            <button class="actionButtons" type="submit">Fazer login e solicitar orçamento</button>
        </div>
        
        <h2>Opção 2: Preencher seus dados</h2>
        <p>Se ainda não possui cadastro no site, preencha seus dados abaixo para solicitar seu orçamento.</p>
        
        <h3>Dados</h3>-->
            <div>
                <label for="nome">Nome:</label>
                <input id="nome" style="color: black" type="text" class="form-control" name="Nome" data-val="true" data-val-required="Digite o nome" />
            </div>
            <div>
                <label for="cnp">CPF/CNPJ:</label>
                <input id="cnp" type="text" style="color: black" name="CNP" class="form-control" data-val="true" data-val-required="Digite o CPF/CNPJ" />
            </div>
            <div>
                <label for="email">E-mail:</label>
                <input id="email" style="color: black" name="Email" type="email" class="form-control" data-val="true" data-val-required="Digite o e-mail" />
            </div>
            <div>
                <label for="fonefixo">Telefone fixo:</label>
                <input id="fonefixo" type="tel" style="color: black" type="text" class="form-control" name="TelFixo" />
            </div>
            <div>
                <label for="fonecelular">Telefone celular:</label>
                <input id="fonecelular" type="tel" style="color: black" type="text" class="form-control" name="TelCelular" />
            </div>

            <!--<h3>Endereço</h3>
            <div>
                <label for="CEP">CEP:</label>
                <input id="CEP" type="text" style="color: black" class="form-control" name="CEP" data-val="true" data-val-required="Digite o CEP" />
            </div>
            <br />
            <div>
                <label for="Rua">Rua:</label>
                <input id="rua" type="text" style="color: black" class="form-control" name="Rua" data-val="true" data-val-required="Digite a rua" />
            </div>
            <div>
                <label for="numero">Número:</label>
                <input id="numero" type="text" style="color: black" class="form-control" name="Numero" data-val="true" data-val-required="Digite o número do endereço" />
            </div>
            <div>
                <label for="bairro">Bairro:</label>
                <input id="bairro" type="text" style="color: black" class="form-control" name="Bairro" data-val="true" data-val-required="Digite o bairro" />
            </div>
            <div>
                <label for="pontoref">Ponto de referência (opcional):</label>
                <input id="pontoref" type="text" style="color: black" class="form-control" name="PontoRef" />
            </div>-->
            <div>
                <label for="estado">Estado:</label>
                <!--<input id="estado" type="text" class="form-control" name="Estado" data-val="true" data-val-required="Digite o estado" />-->

                <select class="custom-select" style="color: black" name="estado">
                    <option selected="" value="">Selecione o Estado</option>
                    <option value="Acre">Acre</option>
                    <option value="Alagoas">Alagoas</option>
                    <option value="Amapá">Amapá</option>
                    <option value="Amazonas">Amazonas</option>
                    <option value="Bahia">Bahia</option>
                    <option value="Ceará">Ceará</option>
                    <option value="Distrito Federal">Distrito Federal</option>
                    <option value="Espírito Santo">Espírito Santo</option>
                    <option value="Goiás">Goiás</option>
                    <option value="Maranhão">Maranhão</option>
                    <option value="Mato Grosso">Mato Grosso</option>
                    <option value="Mato Grosso do Sul">Mato Grosso do Sul</option>
                    <option value="Minas Gerais">Minas Gerais</option>
                    <option value="Pará">Pará</option>
                    <option value="Paraíba">Paraíba</option>
                    <option value="Paraná">Paraná</option>
                    <option value="Pernambuco">Pernambuco</option>
                    <option value="Piauí">Piauí</option>
                    <option value="Rio de Janeiro">Rio de Janeiro</option>
                    <option value="Rio Grande do Sul">Rio Grande do Sul</option>
                    <option value="Rio Grande do Norte">Rio Grande do Norte</option>
                    <option value="Rondônia">Rondônia</option>
                    <option value="Roraima">Roraima</option>
                    <option value="Santa Catarina">Santa Catarina</option>
                    <option value="São Paulo">São Paulo</option>
                    <option value="Sergipe">Sergipe</option>
                    <option value="Tocantins">Tocantins</option>
                </select>

            </div>
            <div>
                <label for="cidade">Cidade:</label>
                <input id="cidade" type="text" style="color: black" class="form-control" name="Cidade" data-val="true" data-val-required="Digite a cidade" />
            </div>
            <!--
        <h3>Dados para login</h3>
        <div>
            <label for="senhaCad">Crie uma senha:</label>
            <input id="senhaCad" name="Senha" data-val="true" data-val-required="Digite uma senha" />
        </div>-->
            <br />
            <div class="form-check">
                <input class="custom-control-input" name="Novidades" type="checkbox" id="defaultCheck1" name="Enviado" checked="true" />
                <label class="custom-control-label" for="defaultCheck1">Desejo receber novidades da Tratorfix</label>
            </div>
             <br />
             <h3>Observações</h3>
             <textarea class="form-control" id="observações" name="Observações" rows="3" placeholder="Digite aqui produtos não listados no site..."></textarea>
             <br />
            <button type="submit" class="btn btn-primary">Solicitar orçamento</button>
        </div>
        <div id="checkoutMessage" runat="server">
            <h2>Obrigado!</h2>
            Obrigado por solicitar seu orçamento. Você receberá uma resposta via e-mail em até 2 dias úteis.
        </div>
    </div>
</asp:Content>
