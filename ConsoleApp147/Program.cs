using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bilgisayar_Kapatma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            string[] dizi = maskedTextBox1.Text.Split(‘:’);
            if (!string.IsNullOrWhiteSpace(dizi[0]))
            {
                if (dizi[0].IsNumeric() == true)
                {
                    if (dizi[0].Length == 2)
                    {
                        if (int.Parse(dizi[0]) > 24)
                        {
                            dizi[0] = “24”;
                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(dizi[1]))
            {
                if (dizi[1].IsNumeric() == true)
                {
                    if (dizi[1].Length == 2)
                    {
                        if (int.Parse(dizi[1]) > 59)
                        {
                            dizi[1] = “59”;
                        }
                    }
                }
            }
            maskedTextBox1.Text = dizi[0] + dizi[1];
        }

        string toplam = “”;
        string hesapla()
        {
            dizi = maskedTextBox1.Text.Split(‘:’);
            if (dizi[0].IsNumeric() == true && dizi[1].IsNumeric() == true)
            {
                TimeSpan a;
                DateTime trih = Convert.ToDateTime(dateTimePicker1.Text + ” ” +maskedTextBox1.Text);
                if (trih < pctarih)
                {
                    return “Geçerli Bir Tarih Giriniz!”;
                }
                else
                {
                    a = trih – pctarih;
                    int gun = a.Days;
                    int saat = a.Hours;
                    int dakika = a.Minutes;
                    int saniye = a.Seconds;
                    toplam = gun +”|”+saat + “|” +dakika + “|” +saniye + “|”;
                    return toplam;
                }
            }
            return “Geçerli Bir Tarih Giriniz!”;
        }
        string[] dizi;
        private void btnAyarla_Click(object sender, EventArgs e)
        {
            if (rbKapat.Checked == true || rbReset.Checked == true || rbAudio.Checked == true || rbNote.Checked == true)
            {
                btnIptal.Enabled = true;
                pctarih = DateTime.Now;
                if (hesapla() == “Geçerli Bir Tarih Giriniz!”)
{
                    timer1.Stop();
                    MessageBox.Show(“Geçerli Bir Tarih Giriniz!”, “Hata”, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
else
                {
                    dizi = maskedTextBox1.Text.Split(‘:’);
                    if (dizi[0].IsNumeric() == true && dizi[1].IsNumeric() == true)
                    {
                        timer1.Enabled = true;
                        maskedTextBox1.Enabled = false;
                        btnAyarla.Enabled = false;
                        dateTimePicker1.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(“Geçerli Bir Saat Dilimi Giriniz!”, “Hata”, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
                MessageBox.Show(“Lütfen Bir İşlem Seçiniz!”,”Hata”, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        DateTime pctarih;
        string[] gelenler;
        private void timer1_Tick(object sender, EventArgs e)
        {
            pctarih = DateTime.Now;
            gelenler = hesapla().Split(‘|’);
            if (gelenler[0] == “0” && gelenler[1] == “0” && gelenler[2] == “0” && gelenler[3] == “0”)
{
                toplam = gelenler[1] + ” Saat ” +gelenler[2] + ” Dakika ” +gelenler[3] + ” Saniye Kaldı.”;
                this.Text = toplam;
                notifyIcon1.Text = toplam;
                this.Text = “Pc Kapatma”;
                switch (secenek)
                {
                    case 0:
                        timer1.Stop();
                        Process.Start(“ShutDown”, “/ s”);
                        break;
                    case 1:
                        timer1.Stop();
                        Process.Start(“ShutDown”, “/ r”);
                        break;
                    case 2:
                        timer1.Stop();
                        this.Activate();
                        muzikcalar.URL = txtMuzikYol.Text;
                        break;
                    case 3:
                        timer1.Stop();
                        this.Activate();
                        muzikcalar.URL = Application.StartupPath + @”\melodi.mp3″;
                        MessageBox.Show(rcNote.Text,”Zaman Geldi”, MessageBoxButtons.OK);
                        this.TopMost = true;
                        break;
                }
                btnIptal.Enabled = false;
                btnAyarla.Enabled = true;
                dateTimePicker1.Enabled = true;
                maskedTextBox1.Enabled = true;
            }
else
            {
                if (int.Parse(gelenler[0]) > 0)
                {
                    gelenler[0] = gelenler[0] + ” Gün “;
                    toplam = gelenler[0] + gelenler[1] + ” Saat ” +gelenler[2] + ” Dakika ” +gelenler[3] + ” Saniye Kaldı.”;
                }
                else
                {
                    toplam = gelenler[1] + ” Saat ” +gelenler[2] + ” Dakika ” +gelenler[3] + ” Saniye Kaldı.”;
                }
                this.Text = toplam;
                notifyIcon1.Text = toplam;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            DialogResult a = MessageBox.Show(“İptal Etmek İstediğinize Eminmisiniz ?”,”Dikkat”, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (a == DialogResult.Yes)
            {
                btnIptal.Enabled = false;
                timer1.Stop();
                btnAyarla.Enabled = true;
                maskedTextBox1.Enabled = true;
                dateTimePicker1.Enabled = true;
                this.Text = “Pc Kapatma”;
                muzikcalar.URL = “”;
                this.TopMost = false;
                if (gelenler[0] == “0” && gelenler[1] == “0” && gelenler[2] == “0” && gelenler[3] == “0”)
Process.Start(“ShutDown”, “/ a”);
            }
        }
        int secenek = -1;
        private void rbKapat_CheckedChanged(object sender, EventArgs e)
        {
            btnMuzikSec.Enabled = false;
            rcNote.Enabled = false;
            txtMuzikYol.Enabled = false;
            secenek = 0;
        }

        private void rbReset_CheckedChanged(object sender, EventArgs e)
        {
            btnMuzikSec.Enabled = false;
            rcNote.Enabled = false;
            txtMuzikYol.Enabled = false;
            secenek = 1;
        }

        private void rbAudio_CheckedChanged(object sender, EventArgs e)
        {
            btnMuzikSec.Enabled = true;
            rcNote.Enabled = false;
            txtMuzikYol.Enabled = true;
            secenek = 2;
        }

        private void rbNote_CheckedChanged(object sender, EventArgs e)
        {
            btnMuzikSec.Enabled = false;
            rcNote.Enabled = true;
            txtMuzikYol.Enabled = false;
            secenek = 3;
        }

        private void btnMuzikSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog a = new OpenFileDialog();
            if (a.ShowDialog() == DialogResult.OK)
            {
                txtMuzikYol.Text = a.FileName;
                this.toolTip1.SetToolTip(this.txtMuzikYol, Path.GetFileName(a.FileName));
            }
        }
        int test = 0;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            test++;
            if (test == 1)
            {
                DialogResult a = MessageBox.Show(“Çıkış Yapmak İstediğinize Eminmisiniz ?”, “Uyarı”, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (a == DialogResult.OK)
                    Application.Exit();
                else
                    e.Cancel = true;
            }
            else
            {
                Application.Exit();
            }
        }
    }
    public static class ExtensionManager
    {
        public static bool IsNumeric(this string text)
        {
            foreach (char chr in text)
            {
                if (!Char.IsNumber(chr)) return false;
            }
            return true;
        }
    }
}