using Insurance.Api.Services;

namespace Insurance.Tests.Builders
{
    public class InsurancePriceRuleBuilder
    {
        private readonly InsurancePriceRule _insurancePriceRule ;

        private InsurancePriceRuleBuilder() => _insurancePriceRule = new InsurancePriceRule();
        public static InsurancePriceRuleBuilder IfPrice() => new();
        public InsurancePriceRuleBuilder And() => this;

        public InsurancePriceRuleBuilder LessThanOrEqual(double value)
        {
            _insurancePriceRule.LessThanOrEqual = value;
            return this;
        }
        public InsurancePriceRuleBuilder LessThan(double value)
        {
            _insurancePriceRule.LessThan = value;
            return this;
        }
        public InsurancePriceRuleBuilder MoreThan(double value)
        {
            _insurancePriceRule.MoreThan = value;
            return this;
        }
        public InsurancePriceRuleBuilder MoreThanOrEqual(double value)
        {
            _insurancePriceRule.MoreThanOrEqual = value;
            return this;
        }
        public InsurancePriceRuleBuilder ThenInsuranceValueIs(double insuranceValue)
        {
            _insurancePriceRule.InsuranceValue = insuranceValue;
            return this;
        }
        public InsurancePriceRule Build()=> _insurancePriceRule;
    }
}