using FluentAssertions;
using Moq;
using MotoDispatch.Application.UseCases.Motorcycle.Common;
using MotoDispatch.Domain.SeedWork.SearchableRepository;
using DomainEntity = MotoDispatch.Domain.Entity;
using UseCase = MotoDispatch.Application.UseCases.Motorcycle.ListMotorcycles;

namespace MotoDispatch.UnitTests.Application.Motorcycle.ListMotorcycles;

[Collection(nameof(ListMotorcyclesTestFixture))]
public class ListDeliveryDriversTest(ListMotorcyclesTestFixture fixture)
{
    [Fact(DisplayName = nameof(List))]
    [Trait("Application", "ListMotorcycles - Use Cases")]
    public async Task List()
    {
        var motorcyclesExampleList = fixture.GetExampleMotorcyclesList();
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();

        var input = fixture.GetExampleInput();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Motorcycle>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Motorcycle>) motorcyclesExampleList,
            total: new Random().Next(50, 200)
        );

        motorcycleRepositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                               && searchInput.PerPage == input.PerPage
                               && searchInput.Search == input.Search
                               && searchInput.OrderBy == input.Sort
                               && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);
        var useCase = new UseCase.ListMotorcycles(motorcycleRepositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        ((List<MotorcycleModelOutput>) output.Items).ForEach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Year.Should().Be(repositoryCategory!.Year);
            outputItem.LicensePlate.Should().Be(repositoryCategory!.LicensePlate);
            outputItem.Model.Should().Be(repositoryCategory!.Model);
            outputItem.CreatedAt.Should().Be(repositoryCategory!.CreatedAt);
        });

        motorcycleRepositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                               && searchInput.PerPage == input.PerPage
                               && searchInput.Search == input.Search
                               && searchInput.OrderBy == input.Sort
                               && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(ListOkWhenEmpty))]
    [Trait("Application", "ListMotorcycles - Use Cases")]
    public async Task ListOkWhenEmpty()
    {
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();
        var input = fixture.GetExampleInput();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Motorcycle>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<DomainEntity.Motorcycle>().AsReadOnly(),
            total: 0
        );

        motorcycleRepositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                               && searchInput.PerPage == input.PerPage
                               && searchInput.Search == input.Search
                               && searchInput.OrderBy == input.Sort
                               && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);
        var useCase = new UseCase.ListMotorcycles(motorcycleRepositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);

        motorcycleRepositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                               && searchInput.PerPage == input.PerPage
                               && searchInput.Search == input.Search
                               && searchInput.OrderBy == input.Sort
                               && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
    [Trait("Application", "ListMotorcycles - Use Cases")]
    [MemberData(
        nameof(ListMotorcyclesTestDataGenerator.GetInputsWithoutAllParameter),
        parameters: 14,
        MemberType = typeof(ListMotorcyclesTestDataGenerator)
    )]
    public async Task ListInputWithoutAllParameters(
        UseCase.ListMotorcyclesInput input)
    {
        var motorcyclesExampleList = fixture.GetExampleMotorcyclesList();
        var motorcycleRepositoryMock = fixture.GetMotorcycleRepositoryMock();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Motorcycle>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Motorcycle>) motorcyclesExampleList,
            total: new Random().Next(50, 200)
        );

        motorcycleRepositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                               && searchInput.PerPage == input.PerPage
                               && searchInput.Search == input.Search
                               && searchInput.OrderBy == input.Sort
                               && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);
        var useCase = new UseCase.ListMotorcycles(motorcycleRepositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

        ((List<MotorcycleModelOutput>) output.Items).ForEach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Year.Should().Be(repositoryCategory!.Year);
            outputItem.LicensePlate.Should().Be(repositoryCategory!.LicensePlate);
            outputItem.Model.Should().Be(repositoryCategory!.Model);
            outputItem.CreatedAt.Should().Be(repositoryCategory!.CreatedAt);
        });

        motorcycleRepositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                               && searchInput.PerPage == input.PerPage
                               && searchInput.Search == input.Search
                               && searchInput.OrderBy == input.Sort
                               && searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}