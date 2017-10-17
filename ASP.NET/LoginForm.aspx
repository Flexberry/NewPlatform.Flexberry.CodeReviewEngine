<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginForm.aspx.cs" Inherits="ICSSoft.STORMNET.Web.LoginForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Вход в систему</title>
</head>
<body class="login-form">
    <form id="form1" runat="server">
        <div class="container">
            <div class="login-form-header">
                <div>
                    <asp:Localize ID="Localize2" Text="<%$ Resources: Resource, Master_Page_Header_Title %>" runat="server"></asp:Localize>
                </div>
            </div>
            <div class="login-form-inner">
                <h2>
                    <asp:Localize ID="Localize1" Text="<%$ Resources: Resource, Log_In %>" runat="server"></asp:Localize>
                </h2>
                <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
                    <LayoutTemplate>
                        <asp:Label runat="server" ID="FailureText" CssClass="login-from-failure-text"></asp:Label>
                        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                            ValidationGroup="LoginUserValidationGroup" Visible="False" />
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="login-form-label">
                            <asp:Localize ID="Localize2" Text="<%$ Resources: Resource, Login %>" runat="server"></asp:Localize>
                        </asp:Label>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                            CssClass="failureNotification" ErrorMessage="<%$ Resources: Resource, Required %>"
                            ToolTip="<%$ Resources: Resource, Required %>" ValidationGroup="LoginUserValidationGroup"></asp:RequiredFieldValidator>
                        <asp:TextBox Placeholder="Логин" ID="UserName" runat="server" CssClass="login-form-input input-block-level"></asp:TextBox>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" CssClass="login-form-label">
                            <asp:Localize ID="Localize3" Text="<%$ Resources: Resource, Password %>" runat="server"></asp:Localize>
                        </asp:Label>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                            CssClass="failureNotification" ErrorMessage="<%$ Resources: Resource, Required %>"
                            ToolTip="<%$ Resources: Resource, Required %>" ValidationGroup="LoginUserValidationGroup"></asp:RequiredFieldValidator>
                        <asp:TextBox Placeholder="Пароль" ID="Password" runat="server" CssClass="login-form-input input-block-level" TextMode="Password"></asp:TextBox>
                        <span class="login-form-remember">
                            <asp:CheckBox ID="RememberMe" runat="server" Text="Запомнить меня"></asp:CheckBox>
                        </span>
                        <span class="login-form-forgot">
                            <a href="#">
                                <asp:Literal ID="RemindPassword" runat="server" Text="Забыли пароль?"></asp:Literal>
                            </a>
                        </span>
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="<%$ Resources: Resource, Enter %>"
                            ValidationGroup="LoginUserValidationGroup" CssClass="login-form-btn" />
                    </LayoutTemplate>
                </asp:Login>
            </div>
        </div>
    </form>
</body>
</html>
