using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Bloggy.API.Features.Categories;
using Bloggy.API.Entities;
using Bloggy.Tests;
using Shouldly;
using Xunit;
using Bloggy.Tests;
using static Bloggy.Tests.SliceFixture;

namespace Bloggy.Tests.Features.Categories
{
    public class ListAllTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_return_all_categories()
        {
            var architecture = new Category
            {
                Name = "Software Architecture"
            };

            var functional = new Category
            {
                Name = "Functional Programming",
            };

            await InsertAsync(architecture);

            var result = await SendAsync(new ListAllQ.Query());

            result.ShouldNotBeNull();
            result.Categories.Count.ShouldBeGreaterThanOrEqualTo(2);
        }
    }
}
