namespace Insurance.Storage
{
    public class InsuranceSurchargeRate
    {
        public InsuranceSurchargeRate(int productTypeId, double insuranceValue, string insuranceLevel)
        {
            ProductTypeId = productTypeId;
            InsuranceValue = insuranceValue;
            ChargingLevel = insuranceLevel;
        }

        public int ProductTypeId { set; get; }

        public double InsuranceValue { set; get; }

        public string ChargingLevel { set; get; }

    }
}
