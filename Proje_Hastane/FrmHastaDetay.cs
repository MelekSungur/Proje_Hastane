using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Proje_Hastane
{
    public partial class FrmHastaDetay : Form
    {
        public FrmHastaDetay()
        {
            InitializeComponent();
        }
        public string tc;
        SqlBaglantisi bgl = new SqlBaglantisi();

        private void FrmHastaDetay_Load(object sender, EventArgs e)
        {
            LblTc.Text = tc;
            SqlCommand komut = new SqlCommand("select HastaAd, HastaSoyad From Tbl_Hastalar where HastaTC=@p1",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", LblTc.Text);
            SqlDataReader dr=komut.ExecuteReader();
            while(dr.Read())
            {
                LblAdSoyad.Text = dr[0] +" "+dr[1];
            }
            bgl.baglanti().Close();
            //randevu geçmişi
            DataTable dt= new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * From Tbl_Randevular where HastaTC="+tc,bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            
            //branşları çekme 

            SqlCommand komut2 = new SqlCommand("Select BransAd From Tbl_Branslar",bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read()) 
            {
                CmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();
            dr2.Close();
           
        }

        private void CmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void CmbBrans_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CmbDoktor.Items.Clear();
            SqlCommand komut4 = new SqlCommand("Select DoktorAd,DoktorSoyad From Tbl_Doktorlar Where DoktorBrans=@p1", bgl.baglanti());
            komut4.Parameters.AddWithValue("@p1",CmbBrans.Text);
            SqlDataReader dr3 = komut4.ExecuteReader();
            while (dr3.Read())
            {
                CmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            }
            bgl.baglanti().Close();





            ////Doktor Ad Soyad çekme 
            //CmbDoktor.Items.Clear();
            //SqlCommand komut3 = new SqlCommand("Select DoktorAd, DoktorSoyad From Tbl_Doktorlar where DoktorBrans=@p1", bgl.baglanti());
            //komut3.Parameters.AddWithValue("@p1", CmbBrans.Text);
            //SqlDataReader dr3 = komut3.ExecuteReader();

            //while (dr3.Read())
            //{
            //    CmbDoktor.Items.Add(dr3[0] + " " + dr3[1]);
            //}
            //bgl.baglanti().Close();
        }

        private void CmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt=new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From Tbl_Randevular where RandevuBrans='" + CmbBrans.Text + "' ", bgl.baglanti());
            da.Fill(dt);
            dataGridView2.DataSource= dt;
        }

        private void LnkBilgiDüzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmBilgiDuzenle fr = new FrmBilgiDuzenle();
            fr.TCno = LblTc.Text;
            fr.Show();
           
        }

        private void BtnRandevual_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Update Tbl_Randevular set RandevuDurum=1,HastaTC=@p1,HastaSikayet=@p2 where Randevuid=@p3", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", LblTc.Text);
            komut.Parameters.AddWithValue("@p2", RchSikayet.Text);
            komut.Parameters.AddWithValue("p3", Txtİd.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Randevu Alındı ", "uyarı",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;
            Txtİd.Text = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
        }
    }
}
