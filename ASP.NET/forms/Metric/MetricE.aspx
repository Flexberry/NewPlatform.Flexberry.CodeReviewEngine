﻿<%--flexberryautogenerated="true"--%>
<%@ Page Language="C#" MasterPageFile="~/Site1.Master"  AutoEventWireup="true" CodeBehind="MetricE.aspx.cs" Inherits="IIS.CodeReviewEngine.MetricE" %>

<%-- Autogenerated section start [Register] --%>
<%-- Autogenerated section end [Register] --%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ics-form ics-form-edit">
        <h2 class="ics-form-header ics-form-header-edit">Metric</h2>
        <div class="ics-form-toolbar ics-form-toolbar-edit ics-sticky">
            <asp:ImageButton ID="SaveBtn" runat="server" SkinID="SaveBtn" OnClick="SaveBtn_Click" AlternateText="<%$ Resources: Resource, Save %>" ValidationGroup="savedoc" />
            <asp:ImageButton ID="SaveAndCloseBtn" runat="server" SkinID="SaveAndCloseBtn" OnClick="SaveAndCloseBtn_Click" AlternateText="<%$ Resources: Resource, Save_Close %>" ValidationGroup="savedoc" />
            <asp:ImageButton ID="CancelBtn" runat="server" SkinID="CancelBtn" OnClick="CancelBtn_Click" AlternateText="<%$ Resources: Resource, Cancel %>" />
        </div>
        <div class="ics-form-controls ics-form-controls-edit">
            <%-- Autogenerated section start [Controls] --%>
<!-- autogenerated start -->
<div>
	<div class="clearfix">
  <asp:Label CssClass="descLbl" ID="ctrlNameLabel" runat="server" Text="Name" EnableViewState="False">
</asp:Label>
<asp:TextBox CssClass="descTxt" ID="ctrlName" runat="server">
</asp:TextBox>

<ac:Hinter ID="ctrlNameHint" runat="server" Title="Название метрики. Введите значение как оно приводится в VisualStudio."/>
<asp:RequiredFieldValidator ID="ctrlNameValidator" runat="server" ControlToValidate="ctrlName"
ErrorMessage="Не указано: Name" EnableClientScript="true" ValidationGroup="savedoc">*</asp:RequiredFieldValidator>

</div>

</div>
<!-- autogenerated end -->
            <%-- Autogenerated section end [Controls] --%>
        </div>
    </div>
</asp:Content>
