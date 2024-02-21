<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PwnedPassword._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

     <mian>
         Enter Password: <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
         <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Submit" />
         <p>Status: <asp:Label runat="server" ID="lblMsg"></asp:Label></p>
         <p>Exception: <asp:Label runat="server" ID="lblifException"></asp:Label></p>
     </mian>

</asp:Content>
