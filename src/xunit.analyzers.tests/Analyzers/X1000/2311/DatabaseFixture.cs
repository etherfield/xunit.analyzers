using System;

public class DatabaseFixture : IDisposable
{
	public int Value { get; } = new Random().Next(1000);

	public DatabaseFixture()
	{
		// ... initialize data in the test database ...
	}

	public void Dispose()
	{
		// ... clean up test data from the database ...
	}
}
