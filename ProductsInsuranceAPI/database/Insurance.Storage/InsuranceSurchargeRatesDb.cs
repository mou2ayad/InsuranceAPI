using System.Collections.Generic;

namespace Insurance.Storage
{
    public static class InsuranceSurchargeRatesDb
    {
        private const int Smartphones = 21;
        private const int Laptops = 32;
        private const int DigitalCameras = 12;

        public static readonly List<InsuranceSurchargeRate> Db = new()
        {
            new InsuranceSurchargeRate(Smartphones, 500,"item"),
            new InsuranceSurchargeRate(Laptops, 500, "item"),
            new InsuranceSurchargeRate(DigitalCameras, 500, "order"),
        };

    }
}
