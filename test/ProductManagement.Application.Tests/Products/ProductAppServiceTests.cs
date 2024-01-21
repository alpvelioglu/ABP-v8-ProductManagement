using ProductManagement.Categories;
using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Xunit;

namespace ProductManagement.Products
{
    public abstract class ProductAppServiceTests<TStartupModule> : ProductManagementApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IProductAppService _productAppService;
        protected ProductAppServiceTests()
        {
            _productAppService = GetRequiredService<IProductAppService>();
        }

        [Fact]
        public async Task Should_Get_Product_List2()
        {
            // Act
            var output = await _productAppService.GetListAsync(new PagedAndSortedResultRequestDto());

            // Assert
            output.TotalCount.ShouldBe(3);
            output.Items.ShouldContain(x => x.Name.Contains("Acme Monochrome Laser Printer"));
        }

		[Fact]
		public async Task Should_Get_Category_Lookup()
		{
			var output = await _productAppService.GetCategoriesAsync();
			output.Items.Count.ShouldBeGreaterThanOrEqualTo(2);
			output.Items.ShouldContain(x => x.Name == "Monitors");
		}

		[Fact]
		public async Task Should_Create_A_Valid_Product()
		{
			var category = await WithUnitOfWorkAsync(
				async () => await GetRequiredService<IRepository<Category, Guid>>().FirstAsync()
			);

			var createProductDto = new CreateUpdateProductDto
			{
				Name = "Tarsus Gaming Laptop 17\"",
				Price = 2999,
				ReleaseDate = DateTime.Now,
				StockState = ProductStockState.InStock,
				CategoryId = category.Id,
				IsFreeCargo = true
			};

			await _productAppService.CreateAsync(createProductDto);

			await WithUnitOfWorkAsync(async () =>
			{
				var product = await GetRequiredService<IRepository<Product, Guid>>()
					.FirstOrDefaultAsync(x => x.Name == createProductDto.Name);

				product.ShouldNotBeNull();
				product.Price.ShouldBe(createProductDto.Price);
				product.StockState.ShouldBe(createProductDto.StockState);
				product.CategoryId.ShouldBe(createProductDto.CategoryId);
				product.IsFreeCargo.ShouldBe(createProductDto.IsFreeCargo);
			});
		}
	}
}
