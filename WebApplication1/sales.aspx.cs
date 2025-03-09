using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Sales : System.Web.UI.Page
    {
        private static readonly string tokenUrl = "http://istest.birfatura.net/";
        private static readonly string salesUrl = "http://istest.birfatura.net/api/test/SatislarGetir";
        private static readonly string invoiceUrl = "http://localhost:44371/api/sales/generateInvoice"; // Backend API
        private static readonly string username = "test@test.com";
        private static readonly string password = "Test123";

        private static readonly HttpClient client = new HttpClient();

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await LoadSales();
            }
        }

        private async Task<string> GetToken()
        {
            using (var client = new HttpClient())
            {
                var credentials = new { username, password };
                var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(tokenUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic tokenObj = JsonConvert.DeserializeObject(result);
                    return tokenObj.access_token;
                }
                else
                {
                    // Loglayarak hatayı anlamaya çalışalım
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);  // Yanıtı konsola yazdıralım
                }
            }
            return null;
        }

        private async Task LoadSales()
        {
            try
            {
                var token = await GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    SalesGridView.EmptyDataText = "Kimlik doğrulama başarısız. Lütfen tekrar deneyin.";
                    SalesGridView.DataBind();
                    return;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(salesUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    // API yanıtını kontrol edelim
                    Console.WriteLine(result);  // Yanıtı loglayalım

                    List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(result);

                    // Burada verinin dolu olup olmadığını kontrol ediyoruz
                    if (sales != null && sales.Count > 0)
                    {
                        SalesGridView.DataSource = sales;
                        SalesGridView.DataBind();
                    }
                    else
                    {
                        // Eğer veri yoksa bu mesajı gösterelim
                        SalesGridView.EmptyDataText = "Satış kaydı bulunamadı.";
                        SalesGridView.DataBind();
                    }
                }
                else
                {
                    throw new Exception($"API isteği başarısız. HTTP Durum Kodu: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                SalesGridView.EmptyDataText = $"Hata: {ex.Message}";
                SalesGridView.DataBind();
            }
        }

        protected void SalesGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SalesGridView.PageIndex = e.NewPageIndex;
            _ = LoadSales(); // Sayfalandırma sonrası verileri tekrar yükle
        }

        protected async void GenerateInvoice_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int orderId = Convert.ToInt32(btn.CommandArgument);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{invoiceUrl}?orderId={orderId}");
                if (response.IsSuccessStatusCode)
                {
                    byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();
                    string fileName = $"Invoice_{orderId}.pdf";

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                    Response.BinaryWrite(pdfBytes);
                    Response.End();
                }
                else
                {
                    SalesGridView.EmptyDataText = "Fatura oluşturma başarısız.";
                    SalesGridView.DataBind();
                }
            }
        }
    }

    // Sale modeli
    public class Sale
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}