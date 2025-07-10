using ClientXETL.Services.SearchIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class SearchIndexTests
{
    [Fact]
    public void Constructor_ShouldCreateIndexFromData()
    {
        // Arrange
        var data = new List<TestItem>
        {
            new TestItem { Id = 1, Name = "Item1" },
            new TestItem { Id = 2, Name = "Item2" },
            new TestItem { Id = 3, Name = "Item3" }
        };

        // Act
        var index = new SearchIndex<int, TestItem>(data, item => item.Id);

        // Assert
        Assert.True(index.ContainsKey(1));
        Assert.True(index.ContainsKey(2));
        Assert.True(index.ContainsKey(3));
        Assert.False(index.ContainsKey(999)); // Invalid key
    }

    [Fact]
    public void Constructor_WithDuplicateKeys_ShouldThrowException()
    {
        // Arrange
        var data = new List<TestItem>
        {
            new TestItem { Id = 1, Name = "Item1" },
            new TestItem { Id = 1, Name = "Item2" } // Duplicate key
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new SearchIndex<int, TestItem>(data, item => item.Id));
    }

    [Fact]
    public void ContainsKey_ValidKey_ShouldReturnTrue()
    {
        // Arrange
        var data = new List<TestItem>
        {
            new TestItem { Id = 1, Name = "Item1" },
            new TestItem { Id = 2, Name = "Item2" }
        };

        var index = new SearchIndex<int, TestItem>(data, item => item.Id);

        // Act & Assert
        Assert.True(index.ContainsKey(1));
        Assert.True(index.ContainsKey(2));
    }

    [Fact]
    public void ContainsKey_InvalidKey_ShouldReturnFalse()
    {
        // Arrange
        var data = new List<TestItem>
        {
            new TestItem { Id = 1, Name = "Item1" },
            new TestItem { Id = 2, Name = "Item2" }
        };

        var index = new SearchIndex<int, TestItem>(data, item => item.Id);

        // Act & Assert
        Assert.False(index.ContainsKey(999)); // Invalid key
        Assert.False(index.ContainsKey(-1)); // Invalid key
    }

    private class TestItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}