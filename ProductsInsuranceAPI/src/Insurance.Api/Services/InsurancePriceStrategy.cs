using System.Collections.Generic;
using System.Linq;
using Insurance.Api.Contracts;
using Insurance.DataProvider.Model;
using Microsoft.Extensions.Configuration;

namespace Insurance.Api.Services
{
    public class InsurancePriceStrategy : IInsuranceStrategy
    {
        protected readonly List<InsurancePriceRule> _rules;

        public InsurancePriceStrategy(IConfiguration configuration)
        {
            _rules = new List<InsurancePriceRule>();
            LoadInsurancePriceRule(configuration);
        }

        public double GetInsuranceValue(Product product)
        {
            var insuranceValue= _rules.FirstOrDefault(rule => rule.MatchRule(product.SalesPrice))?.InsuranceValue;
            return insuranceValue ?? 0;
        }

        protected void LoadInsurancePriceRule(IConfiguration configuration)
        {
            var insurancePriceRules = configuration.GetSection("InsurancePriceRules").Get<InsurancePriceRule[]>();
            if (insurancePriceRules != null)
                _rules.AddRange(insurancePriceRules);
        }
    }

    public class InsurancePriceRule
    {
        public double? LessThanOrEqual { set; get; }
        public double? LessThan { set; get; }
        public double? MoreThan { set; get; }
        public double? MoreThanOrEqual { set; get; }
        public bool MatchRule(double productPrice)
        {
            if (LessThanOrEqual.HasValue && productPrice > LessThanOrEqual.Value) return false;
            if (LessThan.HasValue && productPrice >= LessThan.Value) return false;
            if (MoreThanOrEqual.HasValue && productPrice < MoreThanOrEqual.Value) return false;
            if (MoreThan.HasValue && productPrice <= MoreThan.Value) return false;
            return true;
        }
        public double InsuranceValue { set; get; }

    }
}
