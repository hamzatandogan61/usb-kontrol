using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace kontroluyg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Form yükleme olayı (Form açıldığında ilk işlem burada yapılabilir)
        private void Form1_Load(object sender, EventArgs e)
        {
            // Eğer gerekliyse başlangıç işlemleri yapılabilir.
        }

        
        // USB portlarının durumunu değiştiren metod
        private void USBDurumDegistir(int deger)
        {
            try
            {
                string registryKeyPath = @"SYSTEM\CurrentControlSet\Services\USBSTOR";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("Start", deger, RegistryValueKind.DWord);
                        MessageBox.Show("Kayıt defteri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("USBSTOR kayıt anahtarı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Yetki hatası: Yönetici olarak çalıştırmayı deneyin.", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Sadece dosya transferini engelleyen metod
        private void SadeceDosyaTransferiniEngelle()
        {
            try
            {
                string registryKeyPath = @"SYSTEM\CurrentControlSet\Services\USBSTOR";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyPath, true);
                if (key != null)
                {
                    key.SetValue("Start", 3, RegistryValueKind.DWord); // Normal mod
                    key.Close();
                }

                // Çevre birimleri (klavye, fare gibi) çalışmaya devam eder.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // USB portlarının gücünü tamamen kes.
            USBDurumDegistir(4); // Registry değerini "4" yap
            MessageBox.Show("USB portları Aktif modda: Güç kesildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // USB portlarını pasif moda al: Hiçbir işlem yapılmaz.
            USBDurumDegistir(3); // Registry değerini "3" yap
            MessageBox.Show("USB portları Pasif modda (normal çalışıyor).", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // USB portlarında sadece dosya transferini engelle.
            SadeceDosyaTransferiniEngelle();
            MessageBox.Show("USB portları Yarı-Aktif modda: Dosya transferi engellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Uygulamanın ana metodunu oluşturmak için aşağıdaki kodu Program.cs dosyasına eklemelisiniz:
        // 
        // static void Main()
        // {
        //     Application.EnableVisualStyles();
        //     Application.SetCompatibleTextRenderingDefault(false);
        //     Application.Run(new Form1());
        // }
    }
}
