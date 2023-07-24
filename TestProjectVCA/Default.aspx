<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestProjectVCA._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <asp:Label ID="lblFirstName" runat="server" Text="First name"></asp:Label>
        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lblLastName" runat="server" Text="Last name"></asp:Label>
        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btnadd" runat="server" Text="Add" OnClick="btnAdd_Click" />
        <asp:Button ID="btnupdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
        <br />
        <br />
        <asp:GridView ID="gridContacs" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="firstname" HeaderText="First name" />
                <asp:BoundField DataField="lastname" HeaderText="Last name" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSelect" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="lnkSelect_Click">Select</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>