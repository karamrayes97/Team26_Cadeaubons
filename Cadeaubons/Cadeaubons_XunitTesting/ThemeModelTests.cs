using Cadeaubons_Domain.Model;
using System;
using Xunit;

namespace Cadeaubons_XunitTesting
{
    public class ThemeModelTests
    {
        // ===== Id tests =====

        [Fact]
        public void Id_ShouldAcceptZero()
        {
            var theme = new Theme();
            theme.Id = 0;
            Assert.Equal(0, theme.Id);
        }

        [Fact]
        public void Id_ShouldAcceptPositiveValue()
        {
            var theme = new Theme();
            theme.Id = 42;
            Assert.Equal(42, theme.Id);
        }

        [Fact]
        public void Id_ShouldThrow_WhenNegative()
        {
            var theme = new Theme();
            Assert.Throws<ArgumentException>(() => theme.Id = -1);
        }

        // ===== Name tests =====

        [Fact]
        public void Name_ShouldBeSet_WhenValid()
        {
            var theme = new Theme();
            theme.Name = "Christmas";
            Assert.Equal("Christmas", theme.Name);
        }

        [Fact]
        public void Name_ShouldBeTrimmed()
        {
            var theme = new Theme();
            theme.Name = "  Birthday  ";
            Assert.Equal("Birthday", theme.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Name_ShouldThrow_WhenInvalid(string name)
        {
            var theme = new Theme();
            Assert.Throws<ArgumentException>(() => theme.Name = name);
        }

        // ===== IconPath tests =====

        [Fact]
        public void IconPath_ShouldBeSet_WhenValid()
        {
            var theme = new Theme();
            theme.IconPath = "🎁";
            Assert.Equal("🎁", theme.IconPath);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void IconPath_ShouldThrow_WhenInvalid(string iconPath)
        {
            var theme = new Theme();
            Assert.Throws<ArgumentException>(() => theme.IconPath = iconPath);
        }

        // ===== PrimaryColor tests =====

        [Fact]
        public void PrimaryColor_ShouldBeSet_WhenValid()
        {
            var theme = new Theme();
            theme.PrimaryColor = "#FF5733";
            Assert.Equal("#FF5733", theme.PrimaryColor);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void PrimaryColor_ShouldThrow_WhenInvalid(string color)
        {
            var theme = new Theme();
            Assert.Throws<ArgumentException>(() => theme.PrimaryColor = color);
        }

        // ===== Description tests =====

        [Fact]
        public void Description_ShouldAcceptEmptyValue()
        {
            var theme = new Theme();
            theme.Description = "";
            Assert.Equal("", theme.Description);
        }

        [Fact]
        public void Description_ShouldHaveDefaultEmptyValue()
        {
            var theme = new Theme();
            Assert.Equal(string.Empty, theme.Description);
        }

        // ===== Equals tests =====

        [Fact]
        public void Equals_ShouldBeTrue_WhenSameName()
        {
            var t1 = new Theme { Name = "Christmas", IconPath = "🎄", PrimaryColor = "#FF0000" };
            var t2 = new Theme { Name = "Christmas", IconPath = "🎁", PrimaryColor = "#00FF00" };

            Assert.Equal(t1, t2);
        }

        [Fact]
        public void Equals_ShouldBeTrue_WhenSameNameDifferentCase()
        {
            var t1 = new Theme { Name = "Christmas", IconPath = "🎄", PrimaryColor = "#FF0000" };
            var t2 = new Theme { Name = "CHRISTMAS", IconPath = "🎄", PrimaryColor = "#FF0000" };

            Assert.Equal(t1, t2);
        }

        [Fact]
        public void Equals_ShouldBeFalse_WhenDifferentName()
        {
            var t1 = new Theme { Name = "Christmas", IconPath = "🎄", PrimaryColor = "#FF0000" };
            var t2 = new Theme { Name = "Birthday", IconPath = "🎄", PrimaryColor = "#FF0000" };

            Assert.NotEqual(t1, t2);
        }
    }
}