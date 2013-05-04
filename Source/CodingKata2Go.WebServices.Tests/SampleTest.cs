using NUnit.Framework;

namespace CodingKata2Go.WebServices.Tests
{
    public class SampleClass
    {
        public int Add(int i, int j)
        {
            return i + j;
        }
    }

    [TestFixture]
    public class SampleTest
    {
        public void Test()
        {
            var sample = new SampleClass();

            int result = sample.Add(1, 2);

            Assert.AreEqual(3, result);
        }
    }
}