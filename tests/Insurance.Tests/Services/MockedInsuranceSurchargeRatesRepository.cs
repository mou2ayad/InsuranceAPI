using Insurance.Api.Contracts;
using Insurance.Tests.Utils;
using Moq;

namespace Insurance.Tests.Services
{
    public class MockedInsuranceSurchargeRatesRepository
    {
        public static IInsuranceSurchargeRatesRepository Create(int surchargeRate)
        {
            Mock<IInsuranceSurchargeRatesRepository> moqRepository = new();
            moqRepository.Setup(e => e.GetSurchargeRateOnItemLevel(It.IsIn(ProductTypeSamples.Smartphones, ProductTypeSamples.Laptops))).Returns(surchargeRate);
            moqRepository.Setup(e => e.GetSurchargeRateOnOrderLevel(ProductTypeSamples.DigitalCameras)).Returns(surchargeRate);

            return moqRepository.Object;
        }
    }
}
