using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Api.Models
{
    public class InsuranceSurchargeRateDao
    {
        public int ProductTypeId { set; get; }

        public double InsuranceValue { set; get; }

        public string ChargingLevel { set; get; }
    }
}
