using FC.Codeflix.Catalog.Domain.Entity;
using Moq;
using FluentAssertions;
using UsuCases = FC.Codeflix.Catalog.Application.UseCases.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    public class CreateCategoryTest
    {
        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var repositoryMock = new Mock<IcategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
                );

            var input = new Category(
                "Category Name",
                "Category Description",
                true
                );

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify( repository => repository.Create(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
            
            unitOfWorkMock.Verify( uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);

            output.Should().BeNotNull();
            output.Name.Should().Be("Category Name");
            output.Description.Should().Be("Category Description");
            output.IsActived.Should().BeTrue();
            (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != default(DateTime)).Should().BeTrue();
        }
    }
}
