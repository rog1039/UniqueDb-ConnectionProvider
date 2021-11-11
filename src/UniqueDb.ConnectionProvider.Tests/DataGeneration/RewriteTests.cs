using System;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class RewriteTests
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void RewriteNumericColumnNames()
    {
        var numericalRewriter = new RewriteNumericalName();
        var propertyName      = "2000";
        var expectedName      = "_2000";

        numericalRewriter.ShouldRewrite(propertyName).Should().BeTrue();
        var rewrittenName = numericalRewriter.Rewrite(propertyName);

        rewrittenName.Should().Be(expectedName);
        Console.WriteLine(rewrittenName);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void RewriteMultiWordColumnNames()
    {
        var multiWordRewriter = new RewriteMultiWordName();
        var propertyName      = "Hi There";
        var expectedName      = "Hi_There";

        multiWordRewriter.ShouldRewrite(propertyName).Should().BeTrue();
        var rewrittenName = multiWordRewriter.Rewrite(propertyName);

        rewrittenName.Should().Be(expectedName);
        Console.WriteLine(rewrittenName);
    }
}