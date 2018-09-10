using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KahveAkademisi.Entities.DbModels
{
    public class ConvertedPayout
    {
        [Key]
        public int Id { get; set; }
        public string PaidPrice { get; set; }
        public string IyziCommissionRateAmount { get; set; }
        public string IyziCommissionFee { get; set; }
        public string BlockageRateAmountMerchant { get; set; }
        public string BlockageRateAmountSubMerchant { get; set; }
        public string SubMerchantPayoutAmount { get; set; }
        public string MerchantPayoutAmount { get; set; }
        public string IyziConversionRate { get; set; }
        public string IyziConversionRateAmount { get; set; }
        public string Currency { get; set; }

    }
}
