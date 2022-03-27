using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineTechnicalSupport.Models.Data
{
    public class ProductComputerDetail
    {
        [Key]
        public int ProductComputerDetailId { get; set; }

        [DisplayName("Ethernet 10/100")]
        public string Ethernet { get; set; }
        [DisplayName("Bluetooth Özelliği")]
        public string Bluetooth { get; set; }

        public string Ram { get; set; }
        [DisplayName("Ram Tipi")]
        public string RamType { get; set; }
        [DisplayName("Bellek Hızı")]
        public string MemorySpeed { get; set; }
        [DisplayName("Bellek Yuvası")]
        public string MemorySlot { get; set; }

        [DisplayName("Boyutlar")]
        public string Dimensions { get; set; }
        [DisplayName("Cihaz Ağırlığı")]
        public string DeviceWeight { get; set; }

        [DisplayName("Dokunmatik Ekran")]
        public string TouchScreen { get; set; }
        [DisplayName("Ekran Boyutu")]
        public string ScreenSize { get; set; }
        [DisplayName("Maksimum Ekran Çözünürlüğü")]
        public string MaximumScreenResolution { get; set; }
        [DisplayName("Ekran Özelliği")]
        public string ScreenFeature { get; set; }
        [DisplayName("Ekran Panel Tipi")]
        public string DisplayPanelType { get; set; }

        [DisplayName("Ekran Kartı")]
        public string GraphicsCard { get; set; }
        [DisplayName("Ekran Kartı Tipi")]
        public string GraphicsCardType { get; set; }
        [DisplayName("Ekran Kartı İşlemcisi")]
        public string GraphicsCardProcessor { get; set; }
        [DisplayName("Ekran Kartı")]
        public string GraphicsCardMemory { get; set; }
        [DisplayName("Ekran Kartı Bellek Tipi")]
        public string GraphicsCardMemoryType { get; set; }

        [DisplayName("SSD Kapasitesi")]
        public string SSDCapacity { get; set; }
        [DisplayName("Harddisk Kapasitesi")]
        public string HarddriveCapacity { get; set; }
        [DisplayName("Harddisk Hızı")]
        public string HarddriveSpeed { get; set; }
        [DisplayName("eMMC Kapasitesi")]
        public string eMMCCapacity { get; set; }

        [DisplayName("İşlemci")]
        public string Processor { get; set; }
        [DisplayName("İşlemci Tipi")]
        public string ProcessorType { get; set; }
        [DisplayName("İşlemci Nesli")]
        public string ProcessorGeneration { get; set; }
        [DisplayName("İşlemci Cache")]
        public string ProcessorCache { get; set; }
        [DisplayName("İşlemci Temel Hızı")]
        public string BasicProcessorSpeed { get; set; }
        [DisplayName("İşlemci Maksimum Hızı")]
        public string MaximumProcessorSpeed { get; set; }

        [DisplayName("İşletim Sistemi")]
        public string OS { get; set; }
        [DisplayName("Kart Okuyucu")]
        public string CardReader { get; set; }
        [DisplayName("Parmak İzi Okuyucu")]
        public string FingerprintReader { get; set; }
        [DisplayName("Webcam Kamera")]
        public string Webcam { get; set; }
        [DisplayName("Optik Sürücü")]
        public string OpticalDriver { get; set; }
        public string HDMI { get; set; }
        [DisplayName("Klavye")]
        public string Keyboard { get; set; }

        [DisplayName("Pil")]
        public string Battery { get; set; }
        [DisplayName("Şarjlı Kullanım Süresi")]
        public string ChargedUsageTime { get; set; }

        [DisplayName("Usb 2.0")]
        public string Usb20 { get; set; }
        [DisplayName("Usb 3.0")]
        public string Usb30 { get; set; }
        [DisplayName("Usb 3.1")]
        public string Usb31 { get; set; }

        [DisplayName("Kullanım Amacı")]
        public string UsageType { get; set; }
        [DisplayName("Renk")]
        public string Color { get; set; }

        [DisplayName("Garanti Tipi")]
        public string WarrantyType { get; set; }
        [DisplayName("Garanti Süresi")]
        public string WarrantyPeriod { get; set; }

        [DisplayName("Ürün")]
        [Required(ErrorMessage = "Ürün seçilmedi.")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}