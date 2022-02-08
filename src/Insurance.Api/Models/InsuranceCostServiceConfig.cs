namespace Insurance.Api.Models
{
    public class InsuranceCostServiceConfig
    {
        public bool EnableCaching { set; get; }
        public int ExpireAfterInMinutes { set; get; }
    }
}
