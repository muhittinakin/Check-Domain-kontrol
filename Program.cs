using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoğrulaMail
{
     class Program
    {
        //private const string ConnectionString = @"Server=??;Database=??;User Id=makin;Password=??;";
        private const string ConnectionString = @"Data Source=localhost;Initial Catalog=makin;Integrated Security=True";


        static void Main(string[] args)
        {
            hataliMailleriBul();
        
            Console.WriteLine("Bitti");
            Console.ReadLine();
        }

        public static List<Musteri> GetirMusteriler()
        {

            SqlConnection con = new SqlConnection(ConnectionString);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("SELECT  MusteriNo,KullaniciAd,Mail,Telefon,SonGirisTarih  FROM Kullanici WHERE MusteriNo IS NOT NULL AND MusteriNo <> ''", con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<Musteri> musteriler = new List<Musteri>();

            while (reader.Read())
            {
                Musteri musteri = new Musteri
                {
                    MusteriNo = reader["MusteriNo"].ToString(),
                    KullaniciAd = reader["KullaniciAd"].ToString(),
                    Mail = reader["Mail"].ToString(),

                };
                musteriler.Add(musteri);
            }
            reader.Close();
            con.Close();

            return musteriler;
        }
        public static void hataliMailleriBul()
        {
            List<Musteri> musterliList = GetirMusteriler();

            foreach (var musteri in musterliList)
            {

                if (new EmailAddressAttribute().IsValid(musteri.Mail) == false)
                {
                    Console.WriteLine("HATALI YAZIM->" + musteri.MusteriNo + " " + musteri.KullaniciAd + " " + musteri.Mail);
                }
                else
                {
                    if (checkDomain(musteri.Mail) == false)
                    {
                        Console.WriteLine("HATALI DOMAİN ->" + musteri.MusteriNo + " " + musteri.KullaniciAd + " " + musteri.Mail);
                    }
                }

            }
        }


        public static bool checkDomain(string domainUrl)
        {
            bool _result = false;
            try
            {

                string host = "www." + new System.Net.Mail.MailAddress(domainUrl).Host;
                Socket _checkDomain = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                _checkDomain.Connect(host, 80);
                _checkDomain.Close();
                _result = true;

            }
            catch (Exception)
            {
                if (new System.Net.Mail.MailAddress(domainUrl).Host != "windowslive.com")
                {
                    _result = false;
                }
                else
                {
                    _result = true;

                }
            }

            if (_result == false)
            {
                try
                {

                    string host = new System.Net.Mail.MailAddress(domainUrl).Host;
                    Socket _checkDomain = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    _checkDomain.Connect(host, 80);
                    _checkDomain.Close();
                    _result = true;

                }
                catch (Exception)
                {
                    if (new System.Net.Mail.MailAddress(domainUrl).Host != "windowslive.com")
                    {
                        _result = false;
                    }
                    else
                    {
                        _result = true;

                    }
                }
            }

            return _result;
        }
      
    }
}