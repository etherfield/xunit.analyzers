namespace Xunit.Analyzers.Analyzers.X1000;

[Collection("missing-collection-definition")]
public class FailingTests
{
	private readonly DatabaseFixture? _fixture = null;

	public FailingTests(DatabaseFixture fixture) // The following constructor parameters did not have matching fixture data: DatabaseFixture fixture
	{
		_fixture = fixture;
	}

	[Fact]
	public void TestOne()
	{
		Assert.Null(_fixture);
	}
}
