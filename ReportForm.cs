
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace SalesReportingSystem
{
    // Satış nesnesi
    public class Sale
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Product { get; set; }
        public string Customer { get; set; }
    }

    // Müşteri nesnesi
    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }
    }

    // Ana Form
    public class MainForm : Form
    {
        private List<Sale> sales = new List<Sale>();
        private List<Customer> customers = new List<Customer>();

        private DataGridView dgv = new DataGridView();
        private ComboBox cmbFilter = new ComboBox();
        private TextBox txtSearch = new TextBox();
        private Button btnExport = new Button();

        public MainForm()
        {
            Text = "Satış ve Müşteri Raporlama";
            Size = new Size(1000, 600);

            // DataGridView ayarları
            dgv.Dock = DockStyle.Bottom;
            dgv.Height = 450;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Arama kutusu
            txtSearch.Location = new Point(10, 10);
            txtSearch.Width = 200;
            txtSearch.PlaceholderText = "Ürün veya müşteri adı";

            // Filtreleme combo
            cmbFilter.Location = new Point(220, 10);
            cmbFilter.Width = 150;
            cmbFilter.Items.AddRange(new string[] { "Tümü", "Ürün", "Müşteri" });
            cmbFilter.SelectedIndex = 0;

            // Excel aktar butonu
            btnExport.Text = "Excel'e Aktar";
            btnExport.Location = new Point(400, 10);
            btnExport.Click += (s, e) => ExportToExcel();

            // Olaylar
            txtSearch.TextChanged += (s, e) => ApplyFilters();
            cmbFilter.SelectedIndexChanged += (s, e) => ApplyFilters();

            Controls.Add(txtSearch);
            Controls.Add(cmbFilter);
            Controls.Add(btnExport);
            Controls.Add(dgv);

            LoadData();
            ApplyFilters();
        }

        // JSON dosyalarından verileri yükle
        void LoadData()
        {
            if (File.Exists("sales.json"))
            {
                var json = File.ReadAllText("sales.json");
                sales = JsonSerializer.Deserialize<List<Sale>>(json);
            }

            if (File.Exists("customers.json"))
            {
                var json = File.ReadAllText("customers.json");
                customers = JsonSerializer.Deserialize<List<Customer>>(json);
            }
        }

        // Filtreleme ve arama uygulama
        void ApplyFilters()
        {
            string query = txtSearch.Text.ToLower();
            var filtered = sales;

            if (!string.IsNullOrWhiteSpace(query))
            {
                if (cmbFilter.SelectedIndex == 1) // Ürün
                    filtered = filtered.Where(s => s.Product.ToLower().Contains(query)).ToList();
                else if (cmbFilter.SelectedIndex == 2) // Müşteri
                    filtered = filtered.Where(s => s.Customer.ToLower().Contains(query)).ToList();
                else // Tümü
                    filtered = filtered.Where(s => s.Product.ToLower().Contains(query) || s.Customer.ToLower().Contains(query)).ToList();
            }

            dgv.DataSource = filtered.Select(s => new
            {
                Tarih = s.Date.ToShortDateString(),
                Ürün = s.Product,
                Tutar = s.Amount,
                Müşteri = s.Customer,
                Eposta = customers.FirstOrDefault(c => c.Name == s.Customer)?.Email ?? "-",
                Telefon = customers.FirstOrDefault(c => c.Name == s.Customer)?.Phone ?? "-"
            }).ToList();
        }

        // Excel'e aktarım
        void ExportToExcel()
        {
            using (SaveFileDialog save = new SaveFileDialog())
            {
                save.Filter = "Excel Dosyası|*.xlsx";
                save.FileName = "SatisListesi.xlsx";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    var wb = new XLWorkbook();
                    var ws = wb.Worksheets.Add("Satışlar");

                    // Başlıklar
                    ws.Cell(1, 1).Value = "Tarih";
                    ws.Cell(1, 2).Value = "Ürün";
                    ws.Cell(1, 3).Value = "Tutar";
                    ws.Cell(1, 4).Value = "Müşteri";
                    ws.Cell(1, 5).Value = "Eposta";
                    ws.Cell(1, 6).Value = "Telefon";

                    // Satırlar
                    int row = 2;
                    foreach (DataGridViewRow dgvRow in dgv.Rows)
                    {
                        for (int col = 0; col < dgv.Columns.Count; col++)
                            ws.Cell(row, col + 1).Value = dgvRow.Cells[col].Value?.ToString();
                        row++;
                    }

                    wb.SaveAs(save.FileName);
                    MessageBox.Show("Excel dosyası başarıyla oluşturuldu.");
                }
            }
        }
    }
}
