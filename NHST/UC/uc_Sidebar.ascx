<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_Sidebar.ascx.cs" Inherits="NHST.UC.uc_Sidebar" %>
<div class="sidebar">   
    <div class="sidebar-box">
        <h3 class="sidebar-title">Danh mục</h3>
        <asp:Literal ID="ltrCategory" runat="server" EnableViewState="false"></asp:Literal>
    </div>
        <div class="sidebar-box">
        <h3 class="sidebar-title">bài viết mới</h3>
        <asp:Literal ID="ltrList" runat="server" EnableViewState="false"></asp:Literal>       
    </div>
</div>