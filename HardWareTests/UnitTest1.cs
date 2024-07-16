namespace HardWareTests;

using System.Text;
using FIV.Tle5012b;
using System.Text.Json;
using System.Device.Spi;

[TestClass]
public class HwTestEncoder
{


    private RotaryEncoder encoder;

    [TestInitialize()]
    public void Initialize()
    {
       encoder = new RotaryEncoder();
    }


    [TestMethod]
    public void TestRotaryEncoder()
    {
        //Check encoder instance
        Assert.IsNotNull(encoder);

        //Test encoder
        //Act
        double res;
        bool answer = encoder.Cmd_8021(out res);

        //Assert
        Assert.IsTrue(answer);





 


    }



}