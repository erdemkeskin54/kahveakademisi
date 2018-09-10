using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KahveAkademisi.Entities.Infrastructure
{
    public class Enums
    {
        public enum MessageNumber
        {
            Bos = 200,


            UrunEklendi = 1000,
            UrunEklenemedi = 1001,
            UrunGuncellendi = 1002,
            UrunGuncellenemedi = 1003,
            UrunSilindi = 1004,
            UrunSilinemedi = 1005,
            UrunBulundu = 1006,
            UrunBulunamadi = 1007,
            UrunGetirilemedi=1008,




            KayitOlmaBasarisiz = 2000,
            KayitOlmaBasarili = 2001,
            EmailZatenKayitli = 2002,
            KayitOlmaBilgileriYanlis = 2003,
            TelefonNumarasiUygunDegil = 2004,
            TekrarSmsIcınDahaZamanVar = 2005,
            TelefonNumarasiBulunamadi = 2006,
            DogrulamaKoduUyusmadi = 2007,




            KategoriEklendi = 3000,
            KategoriEklenemedi = 3001,
            KategoriGuncellendi = 3002,
            KategoriGuncellenemedi = 3003,
            KategoriSilindi = 3004,
            KategoriSilinemedi = 3005,
            KategoriBulundu = 3006,
            KategoriBulunamadi = 3007,




            TokenOlusturulamadi = 4001,
            KullaniciAdiVeSifreUyusmadi = 4002,
            KullaniciOlusturulamadi = 4003,
            BuTelefonNumarasiZatenKayitli = 4004,
            KullaniciBilgileriYanlis = 4005,
            KullaniciBulunamadi=4006,




            AdresBulunamadi=5001,
            AdresGetirirkenHata=5002,
            AdresEklenemedi=5003,





            SiparislerGetirilemedi =6001,
            SiparisSayisiGetirilemedi=6002,
            SiparisBilgileriYanlis=6003,
            SiparisEklenemedi=6004,




            SepetGetirilemedi=7001,
            SepetSilinemedi=7002,
            SepetBulunamadi=7003,
            SepetinizBos=7004,



            OdemeBasarisiz =8001,



            ParametrelerHatali =9998,
            BeklenmedikHata = 9999,

        }


        public enum Language : int
        {
            English = 1,
            Turkish = 2
        }

        public enum AmountType : int
        {
            Null=0,
            Piece = 1,
            Weight = 2,
            Box = 3,
            Sack = 4
        }

        public enum ProductStatus : int
        {
            Disabled = 0,
            Enabled = 1,
            InRequest = 2


        }

        public enum OrderStatus : int
        {
            NotApproved = 0,
            Approved = 1,
            Preparing = 2,
            Prepared = 3,
            UploadedToCar = 4,
            UploadedToMotor = 5,
            Delivered = 6,
            Canceled = 7,
            WaitingForApproval=8,
            Active=9,
        }

        public enum CommentType : int
        {
            PositiveComment=1,
            NegativeComment=2
        }

        public enum ProductSize : int
        { 
            Null=0,
            Small=1,
            Medium=2,
            Large=3
        }

        public enum PaymentMethod : int
        {
            CreditCard = 1,
            PaymentAtTheDoor =2,  
            Remittance=3
        }
    }
}
