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
        <div>
        <asp:FileUpload ID="FileUpload1" runat="server" onchange="Button1_Click"/>
           </div>
        <br />
        <div>
            <asp:Label ID="pathLabel" runat="server" Text="Label" Visible="false">Image Text:</asp:Label>
            <asp:TextBox ID="pathBox" runat="server" Visible="false"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="altLabel" runat="server" Text="Label" Visible="false">Alt Text:</asp:Label>
            <asp:TextBox ID="altBox" runat="server" Visible="false"></asp:TextBox>
        </div>
        <br />
        <div>
            <asp:Label ID="filterSelLabel" runat="server" Text="Label" Visible="false">Filters:</asp:Label>
        </div>
        <div>
            <asp:RadioButtonList runat="server" Visible="false" ID="filterSel">
                <asp:ListItem Text="No Filter" Value="0" Selected="True" />
                <asp:ListItem Text="Sepia" Value="1"/>
                <asp:ListItem Text="Black and White" Value="2"/>
            </asp:RadioButtonList>
        </div>
        <br />
        <div>
            <asp:Label ID="textAlignLabel" runat="server" Text="Label" Visible="false">Text Position:</asp:Label>
        </div>
        <div>
            <asp:RadioButtonList runat="server" Visible="false" ID="textAlign">
                <asp:ListItem Text="Top" Value="0" Selected="True" />
                <asp:ListItem Text="Bottom" Value="1"/>
            </asp:RadioButtonList>
        </div>
        <div>
            <asp:Button runat="server" ID="Preview" Text="Preview" Visible="false" onClick="previewClick"/>
        </div>
        <br />
        <asp:Image ID="previewBox" runat="server" Visible="false" />
        <div>
            <asp:Button runat="server" ID="Button1" text="Upload" onclick="Button1_Click"  />
        </div>
    </form>
</body>
</html>
