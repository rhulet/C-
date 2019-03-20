<%@ Page Language="C#" AutoEventWireup="true" CodeFile="createMeme.aspx.cs" Inherits="createMeme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create a Meme</title>
</head>
<body>
    <h1>Create a Meme</h1>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="userNameLabel" runat="server" Text="Label"></asp:Label>
        </div>
        <div>
            <asp:Label ID="userEmailLabel" runat="server" Text="Label"></asp:Label>
        </div>
        <br />
        <br />
        <div>
            <asp:Label ID="pathLabel" runat="server" Text="Label">Image Path:</asp:Label>
            <asp:TextBox ID="pathBox" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="altLabel" runat="server" Text="Label">Alt Text:</asp:Label>
            <asp:TextBox ID="altBox" runat="server"></asp:TextBox>
        </div>
        <br />
        <div>
            <asp:Button runat="server" ID="Button1" text="Submit Meme" onclick="Button1_Click"  />
        </div>
    </form>
</body>
</html>
