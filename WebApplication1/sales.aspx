<<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sales.aspx.cs" Inherits="WebApplication1.Sales" Async="true" %>
<!DOCTYPE html>

<html lang="tr">
<head runat="server">
    <meta charset="UTF-8">
    <title>Satışlar Listesi</title>
    <!-- Bootstrap CSS -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        th, td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }
        th {
            background-color: #f2f2f2;
            font-weight: bold;
        }
        .btn {
            padding: 5px 15px;
            cursor: pointer;
            border: none;
            border-radius: 5px;
            background-color: #007bff;
            color: white;
        }
        .btn:hover {
            background-color: #0056b3;
        }
        .alert {
            padding: 15px;
            background-color: #f8d7da;
            color: #721c24;
            border-radius: 5px;
        }
        .text-center {
            text-align: center;
        }
        .text-danger {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1 class="text-center mt-5">Satışlar Listesi</h1>
            
            <asp:GridView ID="SalesGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered" 
                GridLines="None" BorderStyle="Solid" BorderWidth="1px" CellPadding="5" ForeColor="#333333" 
                AllowPaging="true" PageSize="10" OnPageIndexChanging="SalesGridView_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="OrderId" HeaderText="Sipariş ID" SortExpression="OrderId" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Müşteri Adı" SortExpression="CustomerName" />
                    <asp:BoundField DataField="TotalPrice" HeaderText="Toplam Fiyat" DataFormatString="{0:C}" SortExpression="TotalPrice" />
                    <asp:TemplateField HeaderText="Faturalandır">
                        <ItemTemplate>
                            <asp:Button ID="btnInvoice" runat="server" CssClass="btn btn-primary" Text="Faturalandır"
                                CommandArgument='<%# Eval("OrderId") %>' OnClick="GenerateInvoice_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-warning text-center">
                        Kayıt bulunamadı.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
            
            <asp:Label ID="LabelNoData" runat="server" Text="Veri bulunamadı." ForeColor="Red" Visible="false" CssClass="text-danger" />
        </div>
    </form>
</body>
</html>