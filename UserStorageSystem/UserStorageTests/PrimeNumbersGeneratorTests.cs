using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage;
using UserStorage.IdentifiersGeneration;

namespace UserStorageTests
{
    [TestClass]
    public class PrimeNumbersGeneratorTests
    {
        [TestMethod]
        public void GenerateNewNumber_CalledSevenTimes_ReturnsTwoThreeFiveSevenElevenThirteenSeventeen()
        {
            var generator = new PrimeNumbersGenerator();
            var result = generator.GenerateNewNumber();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result);
            result = generator.GenerateNewNumber();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
            result = generator.GenerateNewNumber();
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
            result = generator.GenerateNewNumber();
            Assert.IsNotNull(result);
            Assert.AreEqual(7, result);
            result = generator.GenerateNewNumber();
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result);
            result = generator.GenerateNewNumber();
            Assert.IsNotNull(result);
            Assert.AreEqual(13, result);
            result = generator.GenerateNewNumber();
            Assert.IsNotNull(result);
            Assert.AreEqual(17, result);
        }

        [TestMethod]
        public void GenerateNewNumber_CalledTventyFiveTimes_ReturnsLastNumberThatIsNinetySeven()
        {
            var generator = new PrimeNumbersGenerator();
            int result = 0;
            for (int i = 0; i < 25; i++)
            {
                result = generator.GenerateNewNumber();
            }            
            Assert.IsNotNull(result);
            Assert.AreEqual(97, result);
        }

        [TestMethod]
        public void GenerateNewNumber_CalledFiveTimesResetAndCalledTreeTimesMore_ReturnsLastNumberThatIsFive()
        {
            var generator = new PrimeNumbersGenerator();
            int result = 0;
            for (int i = 0; i < 5; i++)
            {
                result = generator.GenerateNewNumber();
            }
            generator.ResetGenerator();
            for (int i = 0; i < 3; i++)
            {
                result = generator.GenerateNewNumber();
            }
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }
    }
}
