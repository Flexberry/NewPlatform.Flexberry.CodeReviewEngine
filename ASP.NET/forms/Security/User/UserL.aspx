<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserL.aspx.cs" Inherits="IIS.Product_19312.UserL" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div >
        Пользователи
        <br />
        <br />
        <ac:WebObjectListView ID="WebObjectListView1" runat="server" Visible="true" />
        </div>
</asp:Content>