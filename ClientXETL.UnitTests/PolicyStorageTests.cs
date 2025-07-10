using System.Text;
using ClientXETL.Models;
using ClientXETL.Services.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class PolicyStorageTests
{
    private static PolicyStorage CreatePolicyStorage(out Mock<ILogger<PolicyStorage>> loggerMock)
    {
        loggerMock = new Mock<ILogger<PolicyStorage>>();
        return new PolicyStorage(loggerMock.Object);
    }

    [Fact]
    public async Task LoadAsync_ValidInput_AddsPolicies()
    {
        // Arrange
        var input = "1\tAuto Insurance\n2\tHome Insurance\n3\tTravel Insurance\n";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        var policyStorage = CreatePolicyStorage(out var _);

        // Act
        await policyStorage.LoadAsync(stream, CancellationToken.None);

        // Assert
        Assert.Collection(policyStorage.Policies,
            p => Assert.Equal(new { ID = 1, PolicyName = "Auto Insurance" }, new { p.ID, p.PolicyName }),
            p => Assert.Equal(new { ID = 2, PolicyName = "Home Insurance" }, new { p.ID, p.PolicyName }),
            p => Assert.Equal(new { ID = 3, PolicyName = "Travel Insurance" }, new { p.ID, p.PolicyName })
        );
    }

    [Fact]
    public async Task LoadAsync_InvalidInput_ThrowsFormatException()
    {
        // Arrange
        var input = "1\tAuto Insurance\nINVALID\tMissing Fields\n3\tTravel Insurance\n";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        var policyStorage = CreatePolicyStorage(out var _);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => policyStorage.LoadAsync(stream, CancellationToken.None));
    }

    [Fact]
    public async Task LoadAsync_EmptyStream_NoPoliciesAdded()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));

        var policyStorage = CreatePolicyStorage(out var _);

        // Act
        await policyStorage.LoadAsync(stream, CancellationToken.None);

        // Assert
        Assert.Empty(policyStorage.Policies);
    }

    [Fact]
    public async Task LoadAsync_CancellationRequested_StopsProcessing()
    {
        // Arrange
        var input = "1\tAuto Insurance\n2\tHome Insurance\n3\tTravel Insurance\n";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        var policyStorage = CreatePolicyStorage(out var _);
        using var cts = new CancellationTokenSource();

        await cts.CancelAsync(); // Cancel the token immediately

        // Act
        await policyStorage.LoadAsync(stream, cts.Token);

        // Assert
        Assert.Empty(policyStorage.Policies); // No items should be processed
    }

    [Fact]
    public async Task LoadAsync_EmptyPolicyName_ThrowsFormatException()
    {
        // Arrange
        var input = "1\t\n2\tHome Insurance\n3\tTravel Insurance\n";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        var policyStorage = CreatePolicyStorage(out var _);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => policyStorage.LoadAsync(stream, CancellationToken.None));
    }

    [Fact]
    public void Policies_IsInitiallyEmpty()
    {
        // Arrange
        var policyStorage = CreatePolicyStorage(out var _);

        // Act & Assert
        Assert.Empty(policyStorage.Policies);
    }
}