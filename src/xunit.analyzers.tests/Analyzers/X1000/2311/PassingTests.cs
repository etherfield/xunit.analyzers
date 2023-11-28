namespace Xunit.Analyzers.Analyzers.X1000;

[Collection("missing-collection-definition")]
public class PassingTests
{
	private readonly DatabaseFixture? _fixture = null;

	[Fact]
	public void TestOne()
	{
		Assert.Null(_fixture);
	}
}
