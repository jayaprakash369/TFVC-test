<%@ Page Title="" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="EmpDbTest.aspx.cs" 
    Inherits="dev_EmpDbTest" %>
    <%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="SAKEY" HeaderText="SAKEY" SortExpression="SAKEY" />
            <asp:BoundField DataField="SAALP" HeaderText="SAALP" SortExpression="SAALP" />
            <asp:BoundField DataField="SANUM" HeaderText="SANUM" SortExpression="SANUM" />
        </Columns>
    </asp:GridView>
    <%-- 
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EmployeeConnectionString %>" SelectCommand="SELECT [tKey], [tName] FROM [Test1]"></asp:SqlDataSource>
     --%>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="Dsn=ADV320_SP;system=10.41.30.4;uid=SSGODBC;dbq=QGPL OMDTALIB;dftpkglib=QGPL;languageid=ENU;pkg=QGPL/DEFAULT(IBM),2,0,1,0,512;qrystglmt=-1;pwd=SSGODBC" 
        ProviderName="System.Data.Odbc" SelectCommand="select * from OMDTALIB.SFFA"></asp:SqlDataSource>
</asp:Content>

