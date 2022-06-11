using FC.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        // [Fact(NomeQueVaiAparecerNo_TestExplorer)]
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            var validData = new
            {
                Name = "Category name",
                Description = "Category description"
            };
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithActive(bool IsActive)
        {
            var validData = new
            {
                Name = "Category name",
                Description = "Category description"
            };
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description, IsActive);
            var datetimeAfter = DateTime.Now;

            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.Equal(IsActive, category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = () => new DomainEntity.Category(name!, "Category Description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenDescriptionIsEmpty(string description)
        {
            Action action = () => new DomainEntity.Category("Category Name", description);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThen3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ca")]
        public void InstantiateErrorWhenNameIsLessThen3Characters(string InvalidName)
        {
            Action action = () => new DomainEntity.Category(InvalidName, "Category Description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsMoreThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsMoreThen255Characters()
        {
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category(invalidName, "Category Description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less or equal 255 caracters long", exception.Message);
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThen10_000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category("Category Name", invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should be less or equal 10.000 caracters long", exception.Message);
        }
    }
}
