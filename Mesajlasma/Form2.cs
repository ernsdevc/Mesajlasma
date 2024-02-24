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

namespace Mesajlasma
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=MACHINEX;Initial Catalog=DBMesajlasma;Integrated Security=True");
        public string numara;

        void GelenKutusu()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT g.Ad+' '+g.Soyad AS 'Gönderen',m.Baslik AS 'Başlık',Icerik AS 'Mesaj' FROM TBLMesajlar AS m" +
                " INNER JOIN TBLKisiler AS g ON g.Numara = m.Gonderen WHERE Alici=@p1", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@p1", lblNumara.Text);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        void GidenKutusu()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT a.Ad+' '+a.Soyad AS 'Alıcı',m.Baslik AS 'Başlık',Icerik AS 'Mesaj' FROM TBLMesajlar AS m" +
                " INNER JOIN TBLKisiler AS a ON a.Numara = m.Alici WHERE Gonderen=@p1", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@p1", lblNumara.Text);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            lblNumara.Text = numara;
            GelenKutusu();
            GidenKutusu();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT Ad,Soyad FROM TBLKisiler WHERE Numara=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", lblNumara.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                lblAdSoyad.Text = dr[0].ToString() + ' ' + dr[1].ToString();
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO TBLMesajlar (Gonderen,Alici,Baslik,Icerik) VALUES(@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", numara);
            komut.Parameters.AddWithValue("@p2", maskedTextBox1.Text);
            komut.Parameters.AddWithValue("@p3", textBox1.Text);
            komut.Parameters.AddWithValue("@p4", richTextBox1.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Mesajınız iletildi.");
            GidenKutusu();
        }
    }
}
