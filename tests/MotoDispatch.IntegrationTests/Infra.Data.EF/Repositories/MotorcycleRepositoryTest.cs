using FluentAssertions;
using MotoDispatch.Application.Exceptions;
using MotoDispatch.Domain.Entity;
using MotoDispatch.Domain.SeedWork.SearchableRepository;
using MotoDispatch.Infra.Data.EF;
using Repository = MotoDispatch.Infra.Data.EF.Repositories;

namespace MotoDispatch.IntegrationTests.Infra.Data.EF.Repositories;

[Collection(nameof(MotorcycleRepositoryTestFixture))]
public class MotorcycleRepositoryTest(MotorcycleRepositoryTestFixture fixture)
{
    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    public async Task Insert()
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();
        var exampleMotorcycle = fixture.GetExampleMotorcycle();
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);

        await motorcycleRepository.Insert(exampleMotorcycle, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbMotorcycle = await (fixture.CreateDbContext(true))
            .Motorcycles.FindAsync(exampleMotorcycle.Id);
        dbMotorcycle.Should().NotBeNull();
        dbMotorcycle!.Year.Should().Be(exampleMotorcycle.Year);
        dbMotorcycle.LicensePlate.Should().Be(exampleMotorcycle.LicensePlate);
        dbMotorcycle.Model.Should().Be(exampleMotorcycle.Model);
        dbMotorcycle.CreatedAt.Should().Be(exampleMotorcycle.CreatedAt);
    }
    
    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    public async Task Get()
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();
        var exampleMotorcycle = fixture.GetExampleMotorcycle();
        var exampleMotorcyclesList = fixture.GetExampleMotorcyclesList(15);
        exampleMotorcyclesList.Add(exampleMotorcycle);
        await dbContext.AddRangeAsync(exampleMotorcyclesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var motorcycleRepository = new Repository.MotorcycleRepository(
            fixture.CreateDbContext(true)
        );

        var dbMotorcycle = await motorcycleRepository.Get(
            exampleMotorcycle.Id,
            CancellationToken.None);

        dbMotorcycle.Should().NotBeNull();
        dbMotorcycle.Id.Should().Be(exampleMotorcycle.Id);
        dbMotorcycle!.Year.Should().Be(exampleMotorcycle.Year);
        dbMotorcycle.LicensePlate.Should().Be(exampleMotorcycle.LicensePlate);
        dbMotorcycle.Model.Should().Be(exampleMotorcycle.Model);
        dbMotorcycle.CreatedAt.Should().Be(exampleMotorcycle.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();
        var exampleId = Guid.NewGuid();
        await dbContext.AddRangeAsync(fixture.GetExampleMotorcyclesList(15));
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);

        var task = async () => await motorcycleRepository.Get(
            exampleId,
            CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Motorcycle '{exampleId}' not found.");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    public async Task Update()
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();
        var exampleMotorcycle = fixture.GetExampleMotorcycle();
        var newMotorValues = fixture.GetExampleMotorcycle();
        var exampleMotorcycleList = fixture.GetExampleMotorcyclesList(15);
        exampleMotorcycleList.Add(exampleMotorcycle);
        await dbContext.AddRangeAsync(exampleMotorcycleList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);

        exampleMotorcycle.Update(
            newMotorValues.LicensePlate,
            newMotorValues.Year,
            newMotorValues.Model
        );
        await motorcycleRepository.Update(exampleMotorcycle, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbMotorcycle = await (fixture.CreateDbContext(true))
            .Motorcycles.FindAsync(exampleMotorcycle.Id);
        dbMotorcycle.Should().NotBeNull();
        dbMotorcycle!.Year.Should().Be(exampleMotorcycle.Year);
        dbMotorcycle.Id.Should().Be(exampleMotorcycle.Id);
        dbMotorcycle.LicensePlate.Should().Be(exampleMotorcycle.LicensePlate);
        dbMotorcycle.Model.Should().Be(exampleMotorcycle.Model);
        dbMotorcycle.CreatedAt.Should().Be(exampleMotorcycle.CreatedAt);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    public async Task Delete()
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();
        var exampleMotorcycle = fixture.GetExampleMotorcycle();
        var exampleMotorcycleList = fixture.GetExampleMotorcyclesList(15);
        exampleMotorcycleList.Add(exampleMotorcycle);
        await dbContext.AddRangeAsync(exampleMotorcycleList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);

        await motorcycleRepository.Delete(exampleMotorcycle, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbMotorcycle = await (fixture.CreateDbContext(true))
            .Motorcycles.FindAsync(exampleMotorcycle.Id);
        dbMotorcycle.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ListByIds))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    public async Task ListByIds()
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();
        var exampleMotorcyclesList = fixture.GetExampleMotorcyclesList(15);

        List<Guid> motorcyclesIdsToGet = Enumerable.Range(1, 3).Select(_ =>
        {
            int indexToGet = (new Random()).Next(0, exampleMotorcyclesList.Count - 1);
            return exampleMotorcyclesList[indexToGet].Id;
        }).Distinct().ToList();
        await dbContext.AddRangeAsync(exampleMotorcyclesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);

        IReadOnlyList<Motorcycle> motorcyclesList = await motorcycleRepository
            .GetListByIds(motorcyclesIdsToGet, CancellationToken.None);

        motorcyclesList.Should().NotBeNull();
        motorcyclesList.Should().HaveCount(motorcyclesIdsToGet.Count);

        foreach (Motorcycle outputItem in motorcyclesList)
        {
            var exampleItem = exampleMotorcyclesList.Find(
                motorcycle => motorcycle.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Year.Should().Be(exampleItem!.Year);
            outputItem.LicensePlate.Should().Be(exampleItem.LicensePlate);
            outputItem.Model.Should().Be(exampleItem.Model);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    public async Task SearchReturnsListAndTotal()
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();
        var exampleMotorcyclesList = fixture.GetExampleMotorcyclesList(15);

        await dbContext.AddRangeAsync(exampleMotorcyclesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        var output = await motorcycleRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleMotorcyclesList.Count);
        output.Items.Should().HaveCount(exampleMotorcyclesList.Count);

        foreach (Motorcycle outputItem in output.Items)
        {
            var exampleItem = exampleMotorcyclesList.Find(
                motorcycle => motorcycle.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Year.Should().Be(exampleItem!.Year);
            outputItem.LicensePlate.Should().Be(exampleItem.LicensePlate);
            outputItem.Model.Should().Be(exampleItem.Model);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceIsEmpty))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    public async Task SearchReturnsEmptyWhenPersistenceIsEmpty()
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        var output = await motorcycleRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnsPaginated(
        int quantityMotorcyclesToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
    )
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();

        var exampleMotorcyclesList =
            fixture.GetExampleMotorcyclesList(quantityMotorcyclesToGenerate);
        await dbContext.AddRangeAsync(exampleMotorcyclesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);
        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

        var output = await motorcycleRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(quantityMotorcyclesToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (Motorcycle outputItem in output.Items)
        {
            var exampleItem = exampleMotorcyclesList.Find(
                motorcycle => motorcycle.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Year.Should().Be(exampleItem!.Year);
            outputItem.LicensePlate.Should().Be(exampleItem.LicensePlate);
            outputItem.Model.Should().Be(exampleItem.Model);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    [InlineData("AAA-1234", 1, 5, 1, 1)]
    [InlineData("BBB-5678", 1, 5, 1, 1)]
    [InlineData("CCC-9101", 1, 5, 1, 1)]
    [InlineData("DDD-2345", 1, 5, 1, 1)]
    [InlineData("DDD-2345", 2, 5, 0, 1)]
    [InlineData("EEE-6789", 1, 2, 1, 1)]
    [InlineData("FFF-0123", 1, 3, 1, 1)]
    [InlineData("GGG-4567", 1, 5, 0, 0)]
    [InlineData("HHH-8910", 1, 5, 0, 0)]
    public async Task SearchByText(
        string search,
        int page,
        int perPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityTotalItems
    )
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();

        var exampleMotorcyclesList = new List<Motorcycle>
        {
            new Motorcycle(2020, "AAA-1234", "Model X"),
            new Motorcycle(2019, "BBB-5678", "Model Y"),
            new Motorcycle(2018, "CCC-9101", "Model Z"),
            new Motorcycle(2020, "DDD-2345", "Model A"),
            new Motorcycle(2021, "EEE-6789", "Model B"),
            new Motorcycle(2017, "FFF-0123", "Model C"),
        };
        await dbContext.AddRangeAsync(exampleMotorcyclesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);
        var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

        var output = await motorcycleRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);

        foreach (Motorcycle outputItem in output.Items)
        {
            var exampleItem = exampleMotorcyclesList.Find(
                motorcycle => motorcycle.Id == outputItem.Id
            );
            exampleItem.Should().NotBeNull();
            outputItem.Year.Should().Be(exampleItem!.Year);
            outputItem.LicensePlate.Should().Be(exampleItem.LicensePlate);
            outputItem.Model.Should().Be(exampleItem.Model);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchOrdered))]
    [Trait("Integration/Infra.Data", "MotorcycleRepository - Repositories")]
    [InlineData("licensePlate", "asc")]
    [InlineData("licensePlate", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task SearchOrdered(
        string orderBy,
        string order
    )
    {
        MotoDispatchDbContext dbContext = fixture.CreateDbContext();

        var exampleMotorcyclesList =
            fixture.GetExampleMotorcyclesList(10);
        await dbContext.AddRangeAsync(exampleMotorcyclesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var motorcycleRepository = new Repository.MotorcycleRepository(dbContext);
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

        var output = await motorcycleRepository.Search(searchInput, CancellationToken.None);

        var expectedOrderedList = fixture.CloneMotorcyclesListOrdered(
            exampleMotorcyclesList,
            orderBy,
            searchOrder
        );
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleMotorcyclesList.Count);
        output.Items.Should().HaveCount(exampleMotorcyclesList.Count);

        for (int indice = 0; indice < expectedOrderedList.Count; indice++)
        {
            var expectedItem = expectedOrderedList[indice];
            var outputItem = output.Items[indice];
            expectedItem.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Year.Should().Be(expectedItem!.Year);
            outputItem.Id.Should().Be(expectedItem.Id);
            outputItem.LicensePlate.Should().Be(expectedItem.LicensePlate);
            outputItem.Model.Should().Be(expectedItem.Model);
            outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
    }
}