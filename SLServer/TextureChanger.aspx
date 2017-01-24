<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TextureChanger.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>The New SL Store - Texture changer</title>
    
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "images/minus.png");
    });
    $("[src*=minus]").live("click", function () {
        $(this).attr("src", "images/plus.png");
        $(this).closest("tr").next().remove();
    });
</script>

</head>
<body>
    <form id="form1" runat="server">
        
        <div align="center">
            <p>
                <asp:Table ID="Table1" runat="server" Width="100%">
                    <asp:TableRow runat="server">
                        <asp:TableCell>

                            <asp:Label ID="Label1" runat="server" Text="Units List" Font-Size="20" Font-Bold="true"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server" HorizontalAlign="Right">
                        <asp:TableCell>
                        <asp:LinkButton runat="server" OnClick="Lnk_LogOut_Click">Log out</asp:LinkButton>
                            </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </p>
        </div>
        <div align="center">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" Width="80%" HorizontalAlign="Center" 
            HeaderStyle-Font-Bold="true"  PageSize ="100" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated">
            <Columns>
                <asp:TemplateField>
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" src="images/plus.png" width="10" height="10" />
                <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass = "ChildGrid">
                        <Columns>
                            <asp:BoundField DataField="ObjectName" HeaderText="Object Name" />
                            <asp:BoundField DataField="ObjectType" HeaderText="Object Type" />
                            <asp:BoundField DataField="TextureName" HeaderText="Default texture/Scene number" />


                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ItemTemplate>
                    </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width ="25%">
                    <HeaderTemplate>
                        <asp:LinkButton ID="LnkBtnName" runat="server" Text="Name" CommandName="Sort" CommandArgument="Name"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                        <asp:HiddenField ID="IdHiddenURL" runat="server" Value='<%#Bind("URL") %>'></asp:HiddenField>
                         <asp:HiddenField ID="IdHiddenFieldGuid" runat="server" Value='<%#Bind("ObjectGUID") %>'></asp:HiddenField>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField ItemStyle-Width ="10%">
                    <HeaderTemplate>
                        <asp:LinkButton ID="LnkBtnType" runat="server" Text="Type" CommandName="Sort" CommandArgument="Type"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%#Bind("Type") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width ="10%">
                    <HeaderTemplate>
                        <asp:LinkButton ID="LnkBtnTexture" runat="server" Text="Current Theme" CommandName="Sort" CommandArgument="CurrentTheme"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label100" runat="server" Text='<%#Bind("CurrentTheme") %>'></asp:Label>
                    </ItemTemplate>
                     
                </asp:TemplateField>
                

                <asp:TemplateField ItemStyle-Width ="10%">
                    <HeaderTemplate>
                        <asp:LinkButton ID="LnkBtnRented" runat="server" Text="Rented Status" CommandName="Sort" CommandArgument="RentedStatus"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label101" runat="server" Text='<%#Bind("RentedStatus") %>'></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>

                 <asp:TemplateField ItemStyle-Width ="5%">
                    <HeaderTemplate>
                        <asp:LinkButton ID="LnkBtnRented" runat="server" Text="Version" CommandName="Sort" CommandArgument="Version"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LblVersion" runat="server" Text='<%#Bind("Version") %>'></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width ="40%">
                    <HeaderTemplate>
                        <asp:Label ID="Label13" runat="server" Text='Set Texture themes'></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                        </asp:PlaceHolder>
                        <asp:Label ID="Label102" runat="server" Text='<%#Bind("ThemesAvailable") %>'></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>

            </Columns>

<HeaderStyle Font-Bold="True"></HeaderStyle>
        </asp:GridView>
            
            
            <br />
            
            
            <asp:Label ID="Label103" runat="server" Text="Page size"></asp:Label>
                &nbsp;
                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="50" Value="50"></asp:ListItem>    
                    <asp:ListItem Text="100" Value="100"></asp:ListItem>    
                    <asp:ListItem Text="200" Value="200"></asp:ListItem>    
                    <asp:ListItem Text="500" Value="500"></asp:ListItem>    
                    <asp:ListItem Text="1000" Value="1000"></asp:ListItem>    
                    <asp:ListItem Text="2000" Value="2000"></asp:ListItem>    
                    <asp:ListItem Text="4000" Value="4000"></asp:ListItem>    
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>    

                </asp:DropDownList>
                <asp:Button ID="btnApplyPageSize" runat="server" Text="Apply" OnClick="btnApplyPageSize_Click" />
                </div>
            <br />
            
           </div>
        <div align ="center">
            <center>  
          <asp:Button ID="BtnHome" runat="server" Text="Home" OnClick="BtnHome_Click" />
          <asp:Button ID="BtnReloadData" runat="server" Text="Reload data" OnClick="BtnReloadData_Click" />
            <br />
            </center>
        </div>
    </form>
</body>
</html>
