using NUnit.Framework;
using Shouldly;

namespace Talista.Extensions.Tests
{
	public class ExtensionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Can_Get_IndexedEnumerable_From_List()
        {
	        var source = new List<string> {"one", "two", "three"};

	        var result = source.WithIndex();

	        var expected = new List<(int, string)> {(0, "one"), (1, "two"), (2, "three")};

	        result.ShouldContain(x => expected.Contains(x));
        }

        [Test]
        public void Can_Get_IndexedEnumerable_From_List_OneBased()
        {
	        var source = new List<string> {"one", "two", "three"};

	        var result = source.WithIndex(zeroBased: false);

	        var expected = new List<(int, string)> {(1, "one"), (2, "two"), (3, "three")};

	        result.ShouldContain(x => expected.Contains(x));
        }
    }
}
