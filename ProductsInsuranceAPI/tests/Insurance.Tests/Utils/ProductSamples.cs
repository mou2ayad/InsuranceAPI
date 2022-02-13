using Insurance.DataProvider.Model;
using Insurance.Tests.Builders;

namespace Insurance.Tests.Utils
{
    public class ProductSamples
    {
        public static Product SamsungGalaxyS10 => ProductBuilder.Create().WithId(827074)
            .WithProductTypeId(ProductTypeSamples.Smartphones).WithPrice(699).Build();

        public static Product AppleIPod  => ProductBuilder.Create().WithId(832845)
            .WithProductTypeId(ProductTypeSamples.MP3Players).WithPrice(229).Build();

        public static Product AppleMacBookPro => ProductBuilder.Create().WithId(861866)
            .WithProductTypeId(ProductTypeSamples.Laptops).WithPrice(1749).Build();

        public static Product SonyCyberShot => ProductBuilder.Create().WithId(836194)
            .WithProductTypeId(ProductTypeSamples.DigitalCameras).WithPrice(1129).Build();

    }
}
