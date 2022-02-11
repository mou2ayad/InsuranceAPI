using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Services;
using Insurance.Tests.Services;

namespace Insurance.Tests.Builders
{
    public class InsuranceStrategiesHelper
    {
        public static InsurancePriceRule[] BuildInsurancePriceRules()
        {
            return new[]
            {
                InsurancePriceRuleBuilder.IfPrice()
                    .LessThan(500)
                    .ThenInsuranceValueIs(0)
                    .Build(),

                InsurancePriceRuleBuilder.IfPrice()
                    .MoreThanOrEqual(500).And().LessThan(2000)
                    .ThenInsuranceValueIs(1000)
                    .Build(),

                InsurancePriceRuleBuilder.IfPrice()
                    .MoreThanOrEqual(2000)
                    .ThenInsuranceValueIs(2000)
                    .Build()
            };
        }
        public static IEnumerable<IInsuranceStrategy> GetStrategies() =>
            new List<IInsuranceStrategy>
            {
                TestableInsurancePriceStrategy.Create()
                    .WithRules(BuildInsurancePriceRules()),
                new InsuranceProductTypeStrategy(MockedInsuranceSurchargeRatesRepository.Create(500))
            };
    }
}
