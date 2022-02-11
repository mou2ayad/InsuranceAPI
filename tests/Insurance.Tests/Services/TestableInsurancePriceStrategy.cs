using System.Collections.Generic;
using Insurance.Api.Services;
using Microsoft.Extensions.Configuration;

namespace Insurance.Tests.Services
{
    public class TestableInsurancePriceStrategy : InsurancePriceStrategy
    {
        private TestableInsurancePriceStrategy() : base(new ConfigurationBuilder().Build())
        {
        }
        private TestableInsurancePriceStrategy(IConfiguration config) : base(config)
        {
        }

        public static TestableInsurancePriceStrategy Create(IConfiguration config = null) =>
            config == null ? new() : new(config);

        public TestableInsurancePriceStrategy WithRules(params InsurancePriceRule[] insurancePriceRules)
        {
            _rules.Clear();
            _rules.AddRange(insurancePriceRules);
            return this;
        }

        public List<InsurancePriceRule> GetRules() => _rules;

    }
}
