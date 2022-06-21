using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTest
    {
        private CategoryTestFixture _categoryTestFixture;

        public CategoryTest(CategoryTestFixture categoryTestFixture)
        {
            _categoryTestFixture = categoryTestFixture;
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            var validData = _categoryTestFixture.GetValidCategory();
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);
            category.Id.Should().NotBe(default(Guid));
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            category.IsActive.Should().BeTrue();

            (category.CreatedAt >= datetimeBefore).Should().BeTrue();
            (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithActive(bool IsActive)
        {
            var validData = _categoryTestFixture.GetValidCategory();
            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validData.Name, validData.Description, IsActive);
            var datetimeAfter = DateTime.Now;

            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);
            category.Id.Should().NotBe(default(Guid));
            category.CreatedAt.Should().NotBe(default(DateTime));
            category.IsActive.Should().Be(IsActive);

            (category.CreatedAt >= datetimeBefore).Should().BeTrue();
            (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = () => new DomainEntity.Category(name!, "Category Description");

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"Name should not be null or empty");
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNullorEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenDescriptionIsNullorEmpty(string description)
        {
            Action action = () => new DomainEntity.Category("Category Name", description);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Description should not be null or empty");
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
        
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characters long");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThen255Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 caracters long");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThen10_000Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{nameof(validCategory.Description)} should be less or equal 10000 caracters long");
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validData = _categoryTestFixture.GetValidCategory();
            var category = new DomainEntity.Category(validData.Name, validData.Description, false);

            category.Activate();

            category.IsActive.Should().BeTrue();
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validData = _categoryTestFixture.GetValidCategory();
            var category = new DomainEntity.Category(validData.Name, validData.Description, true);

            category.Deactivate();

            category.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var categoryUpdate = _categoryTestFixture.GetValidCategory();
            var category = _categoryTestFixture.GetValidCategory();

            category.Update(categoryUpdate.Name, categoryUpdate.Description);

            category.Name.Should().Be(categoryUpdate.Name);
            category.Description.Should().Be(categoryUpdate.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var categoryUpdate = _categoryTestFixture.GetValidCategory();
            var category = _categoryTestFixture.GetValidCategory();
            var currentDescription = category.Description;

            category.Update(categoryUpdate.Name);

            category.Name.Should().Be(categoryUpdate.Name);
            category.Description.Should().Be(currentDescription);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = _categoryTestFixture.GetValidCategory();

            Action action = () => category.Update(name!);

            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should not be null or empty");
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThen3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ca")]
        public void UpdateErrorWhenNameIsLessThen3Characters(string InvalidName)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            Action action = () => new DomainEntity.Category(InvalidName, validCategory.Description);
            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should be at least 3 characters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThen255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThen255Characters()
        {
            var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
            action.Should().Throw<EntityValidationException>()
                .WithMessage("Name should be less or equal 255 caracters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThen10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThen10_000Characters()
        {
            var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();
            while (invalidDescription.Length <= 10_000)
                invalidDescription += $" {_categoryTestFixture.Faker.Commerce.ProductDescription}";
            var category = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(category.Name, invalidDescription);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{nameof(category.Description)} should be less or equal 10000 caracters long");
        }
    }
}
