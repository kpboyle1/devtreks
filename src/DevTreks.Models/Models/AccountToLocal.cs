using System;
using System.Collections.Generic;

namespace DevTreks.Models
{
    public partial class AccountToLocal
    {
        public AccountToLocal() { }
        public AccountToLocal(bool init)
        {
            this.PKId = 0;
            this.LocalName = string.Empty;
            this.LocalDesc = string.Empty;
            this.UnitGroupId = 0;
            this.UnitGroup = string.Empty;
            this.CurrencyGroupId = 0;
            this.CurrencyGroup = string.Empty;
            this.RealRateId = 0;
            this.RealRate = 0;
            this.NominalRateId = 0;
            this.NominalRate = 0;
            this.DataSourceTechId = 0;
            this.DataSourceTech = string.Empty;
            this.GeoCodeTechId = 0;
            this.GeoCodeTech = string.Empty;
            this.DataSourcePriceId = 0;
            this.DataSourcePrice = string.Empty;
            this.GeoCodePriceId = 0;
            this.GeoCodePrice = string.Empty;
            this.RatingGroupId = 0;
            this.RatingGroup = string.Empty;
            this.AccountId = 0;
            this.IsDefaultLinkedView = false;
            this.Account = new Account();
        }
        public AccountToLocal(AccountToLocal copyLocal)
        {
            this.PKId = copyLocal.PKId;
            this.LocalName = copyLocal.LocalName;
            this.LocalDesc = copyLocal.LocalDesc;
            this.UnitGroupId = copyLocal.UnitGroupId;
            this.UnitGroup = copyLocal.UnitGroup;
            this.CurrencyGroupId = copyLocal.CurrencyGroupId;
            this.CurrencyGroup = copyLocal.CurrencyGroup;
            this.RealRateId = copyLocal.RealRateId;
            this.RealRate = copyLocal.RealRate;
            this.NominalRateId = copyLocal.NominalRateId;
            this.NominalRate = copyLocal.NominalRate;
            this.DataSourceTechId = copyLocal.DataSourceTechId;
            this.DataSourceTech = copyLocal.DataSourceTech;
            this.GeoCodeTechId = copyLocal.GeoCodeTechId;
            this.GeoCodeTech = copyLocal.GeoCodeTech;
            this.DataSourcePriceId = copyLocal.DataSourcePriceId;
            this.DataSourcePrice = copyLocal.DataSourcePrice;
            this.GeoCodePriceId = copyLocal.GeoCodePriceId;
            this.GeoCodePrice = copyLocal.GeoCodePrice;
            this.RatingGroupId = copyLocal.RatingGroupId;
            this.RatingGroup = copyLocal.RatingGroup;
            this.AccountId = copyLocal.AccountId;
            this.IsDefaultLinkedView = copyLocal.IsDefaultLinkedView;
            this.Account = new Account();
        }
        public int PKId { get; set; }
        public string LocalName { get; set; }
        public string LocalDesc { get; set; }
        public int UnitGroupId { get; set; }
        public string UnitGroup { get; set; }
        public int CurrencyGroupId { get; set; }
        public string CurrencyGroup { get; set; }
        public int RealRateId { get; set; }
        public float RealRate { get; set; }
        public int NominalRateId { get; set; }
        public float NominalRate { get; set; }
        public int DataSourceTechId { get; set; }
        public string DataSourceTech { get; set; }
        public int GeoCodeTechId { get; set; }
        public string GeoCodeTech { get; set; }
        public int DataSourcePriceId { get; set; }
        public string DataSourcePrice { get; set; }
        public int GeoCodePriceId { get; set; }
        public string GeoCodePrice { get; set; }
        public int RatingGroupId { get; set; }
        public string RatingGroup { get; set; }
        public int AccountId { get; set; }
        public bool IsDefaultLinkedView { get; set; }

        public virtual Account Account { get; set; }
    }
}
