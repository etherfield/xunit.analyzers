using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Verify = CSharpVerifier<Xunit.Analyzers.AssertIsTypeShouldUseGenericOverloadType>;

public class AssertIsTypeShouldUseGenericOverloadTypeTests
{
#if NETCOREAPP  // static abstract methods are only supported on .NET, not .NET Framework
	public class StaticAbstractInterfaceMethods
	{
		const string methodCode = "static abstract void Method();";
		const string codeTemplate = @"
using Xunit;

public interface IParentClass  {{
	{0}
}}

public interface IClass : IParentClass {{
    {1}
}}

public class Class : IClass {{
    public static void Method() {{ }}
}}

public abstract class TestClass {{
    [Fact]
    public void TestMethod() {{
        var data = new Class();

        Assert.IsAssignableFrom(typeof(IClass), data);
    }}
}}";

		[Fact]
		public async void DoesNotFindWarning_ForStaticAbstractInterfaceMembers()
		{
			string source = string.Format(codeTemplate, string.Empty, methodCode);

			await Verify.VerifyAnalyzer(LanguageVersion.Preview, source);
		}

		[Fact]
		public async void DoesNotFindWarning_ForNestedStaticAbstractInterfaceMembers()
		{
			string source = string.Format(codeTemplate, methodCode, string.Empty);

			await Verify.VerifyAnalyzer(LanguageVersion.Preview, source);
		}

		[Theory]
		[InlineData("static", "", "{ }")]
		[InlineData("", "abstract", ";")]
		public async void FindsWarning_ForNotStaticAbstractInterfaceMembers(string staticModifier, string abstractModifier, string methodBody)
		{
			string source = $@"
using Xunit;

public interface IClass {{
    {staticModifier} {abstractModifier} void Method() {methodBody}
}}

public class Class : IClass {{
    public {staticModifier} void Method() {{ }}
}}

public abstract class TestClass {{
    [Fact]
    public void TestMethod() {{
        var data = new Class();

        Assert.IsAssignableFrom(typeof(IClass), data);
    }}
}}";

			var expected =
				Verify
					.Diagnostic()
					.WithSpan(17, 9, 17, 54)
					.WithArguments("IClass");

			await Verify.VerifyAnalyzer(LanguageVersion.CSharp8, source, expected);
		}
	}
#endif
}
