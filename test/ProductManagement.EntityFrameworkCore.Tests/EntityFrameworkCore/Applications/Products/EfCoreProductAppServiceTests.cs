using ProductManagement.Products;
using ProductManagement.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductManagement.EntityFrameworkCore.Applications.Products
{
    [Collection(ProductManagementTestConsts.CollectionDefinitionName)]
    public class EfCoreProductAppServiceTests : ProductAppServiceTests<ProductManagementEntityFrameworkCoreTestModule>
    {
    }
}
