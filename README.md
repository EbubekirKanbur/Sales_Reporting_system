# Satış Raporlama ve Müşteri Yönetim Sistemi 💼📊

Bu uygulama, C# Windows Forms ile geliştirilmiş gelişmiş bir satış ve müşteri raporlama sistemidir. Satış kayıtlarını tablo halinde görüntüler, müşteri bilgilerini içerir ve Excel çıktısı alabilir.

## 🚀 Özellikler
- 📋 DataGridView ile satış listesi (Tarih, Ürün, Tutar, Müşteri)
- 👤 Müşteri bilgisi: Ad Soyad, E-posta, Telefon
- 📊 Grafiklerle haftalık ve aylık satış karşılaştırması
- 📤 Excel çıktısı (ClosedXML ile)
- 🔍 Arama ve filtreleme (ürün, müşteri adı)

## 🔧 Kullanım
1. Projeyi Visual Studio ile aç
2. `sales.json` ve `customers.json` dosyasını düzenle
3. Satışlar tablo halinde gösterilir
4. "Excel'e Aktar" butonuyla satış listesi dışa aktarılır
5. Grafik sekmesinden satışlar görsel olarak izlenebilir

## 📦 Gereksinimler
- .NET 6 SDK
- NuGet Paketleri:
  - ClosedXML
  - System.Windows.Forms.DataVisualization

