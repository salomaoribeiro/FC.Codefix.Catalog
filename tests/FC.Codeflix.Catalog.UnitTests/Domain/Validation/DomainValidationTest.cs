using Bogus;
using FluentAssertions;
using FC.Codeflix.Catalog.Domain.Validation;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        //Nao pode ser null
        public Faker Faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
            var valor = Faker.Commerce.ProductName();

            Action action = () => DomainValidation.NotNull(valor, fieldName);
            action.Should().NotThrow();
        }

        //null or empty
        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
            string? valor = null;

            Action action = () => DomainValidation.NotNull(valor, fieldName);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should not be null");
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void NotNullOrEmptyThrowWhenEmpty(string? target)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should not be null or empty");
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk()
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            string target = Faker.Commerce.ProductName();
            Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(MinLengthThrowWhenLessThen))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThenMinimal), parameters: 10)]
        public void MinLengthThrowWhenLessThen(string target, int minLength)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
            
            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be at least {minLength} characters long");
        }

        [Theory(DisplayName = nameof(MinLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreatherThenMinimal), parameters: 10)]
        public void MinLengthOk(string target, int minLength)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreaterThen))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreatherThenMax), parameters: 10)]
        public void MaxLengthThrowWhenGreaterThen(string target, int maxLength)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should().Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be less or equal {maxLength} caracters long");
        }

        [Theory(DisplayName = nameof(MaxLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThenMax), parameters: 10)]
        public void MaxLengthOk(string target, int maxLength)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesSmallerThenMinimal(int numberOfTests = 5)
        {
            var Faker = new Faker();

            for (int i = 0; i < numberOfTests; i++)
            {
                var exemplo = Faker.Commerce.ProductName();
                var minLength = exemplo.Length + (new Random()).Next(1, 20);

                yield return new object[] { exemplo, minLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreatherThenMinimal(int numberOfTests = 5)
        {
            var Faker = new Faker();

            // Teste para ver o comportamento com a string com mínimo no tamanho do passado 
            yield return new object[] { "123456", 6 };

            for (int i = 0; i < numberOfTests; i++)
            {
                var exemplo = Faker.Commerce.ProductName();
                var minLength = exemplo.Length - (new Random()).Next(1, exemplo.Length);

                yield return new object[] { exemplo, minLength };
            }
        }


        public static IEnumerable<object[]> GetValuesSmallerThenMax(int numberOfTests = 5)
        {
            var Faker = new Faker();

            yield return new object[] { "123456", 6 };

            for (int i = 0; i < numberOfTests; i++)
            {
                var exemplo = Faker.Commerce.ProductName();
                var maxLength = exemplo.Length + (new Random()).Next(0, 20);

                yield return new object[] { exemplo, maxLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreatherThenMax(int numberOfTests = 5)
        {
            var Faker = new Faker();

            for (int i = 0; i < numberOfTests; i++)
            {
                var exemplo = Faker.Commerce.ProductName();
                var maxLength = exemplo.Length - (new Random()).Next(1, exemplo.Length);

                yield return new object[] { exemplo, maxLength };
            }
        }

    }
}
