using System.Text;
using ClientXETL.Models;
using ClientXETL.Services.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class RiskStorageTests
{
    private static RiskStorage CreateRiskStorage(out Mock<ILogger<RiskStorage>> loggerMock)
    {
        loggerMock = new Mock<ILogger<RiskStorage>>();
        return new RiskStorage(loggerMock.Object);
    }

    [Fact]
    public async Task LoadAsync_ValidInput_AddsRisks()
    {
        // Arrange
        var input =
            "1\tFire\tFire\t101\t123 Elm Street\tClientA\t40.7128\t-74.0060\n" +
            "2\tTheft\tWindstorm\t102\t456 Pine Avenue\tClientB\t37.7749\t-122.4194\n" +
            "3\tFlood\tFlood\t103\t789 Maple Lane\tClientC\t41.8781\t-87.6298\n";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        var riskStorage = CreateRiskStorage(out var _);

        // Act
        await riskStorage.LoadAsync(stream, CancellationToken.None);

        // Assert
        Assert.Collection(riskStorage.Risks,
            r => Assert.Equal(new { ID = 1, RiskName = "Fire", Peril = Peril.Fire, PolicyID = 101, ClientID = "ClientA" },
                              new { r.ID, r.RiskName, r.Peril, r.PolicyID, r.ClientID }),
            r => Assert.Equal(new { ID = 2, RiskName = "Theft", Peril = Peril.Windstorm, PolicyID = 102, ClientID = "ClientB" },
                              new { r.ID, r.RiskName, r.Peril, r.PolicyID, r.ClientID }),
            r => Assert.Equal(new { ID = 3, RiskName = "Flood", Peril = Peril.Flood, PolicyID = 103, ClientID = "ClientC" },
                              new { r.ID, r.RiskName, r.Peril, r.PolicyID, r.ClientID })
        );
    }

    [Fact]
    public async Task LoadAsync_InvalidInput_ThrowsFormatException()
    {
        // Arrange
        var input =
            "1\tFire Damage\tINVALID_PERIL\t101\t123 Elm Street\tClientA\t40.7128\t-74.0060\n" +
            "2\tTheft\tTheft\tINVALID_POLICYID\t456 Pine Avenue\tClientB\t37.7749\t-122.4194\n";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        var riskStorage = CreateRiskStorage(out var _);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => riskStorage.LoadAsync(stream, CancellationToken.None));
    }

    [Fact]
    public async Task LoadAsync_EmptyStream_NoRisksAdded()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(string.Empty));
        var riskStorage = CreateRiskStorage(out var _);

        // Act
        await riskStorage.LoadAsync(stream, CancellationToken.None);

        // Assert
        Assert.Empty(riskStorage.Risks);
    }

    [Fact]
    public async Task LoadAsync_CancellationRequested_StopsProcessing()
    {
        // Arrange
        var input =
            "1\tFire Damage\tFire\t101\t123 Elm Street\tClientA\t40.7128\t-74.0060\n" +
            "2\tTheft\tTheft\t102\t456 Pine Avenue\tClientB\t37.7749\t-122.4194\n" +
            "3\tFlooding\tFlood\t103\t789 Maple Lane\tClientC\t41.8781\t-87.6298\n";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        var riskStorage = CreateRiskStorage(out var _);
        using var cts = new CancellationTokenSource();

        cts.Cancel(); // Cancel immediately

        // Act
        await riskStorage.LoadAsync(stream, cts.Token);

        // Assert
        Assert.Empty(riskStorage.Risks); // No items should be processed
    }

    [Fact]
    public async Task LoadAsync_EmptyRiskName_ThrowsFormatException()
    {
        // Arrange
        var input = "1\t\tFire\t101\t123 Elm Street\tClientA\t40.7128\t-74.0060\n";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        var riskStorage = CreateRiskStorage(out var _);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => riskStorage.LoadAsync(stream, CancellationToken.None));
    }

    [Fact]
    public async Task LoadAsync_InvalidLatitudeLongitude_ThrowsFormatException()
    {
        // Arrange
        var input =
            "1\tFire Damage\tFire\t101\t123 Elm Street\tClientA\tINVALID_LATITUDE\tINVALID_LONGITUDE\n";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        var riskStorage = CreateRiskStorage(out var _);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => riskStorage.LoadAsync(stream, CancellationToken.None));
    }

    [Fact]
    public void Risks_IsInitiallyEmpty()
    {
        // Arrange
        var riskStorage = CreateRiskStorage(out var _);

        // Act & Assert
        Assert.Empty(riskStorage.Risks);
    }
}