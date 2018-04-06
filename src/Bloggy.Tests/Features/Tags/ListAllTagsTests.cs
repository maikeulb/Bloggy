using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Bloggy.API.Features.Tags;
using Bloggy.API.Entities;
using Bloggy.Tests;
using Shouldly;
using Xunit;
using static Bloggy.Tests.SliceFixture;

namespace Bloggy.Tests.Features.Tags
{
    public class ListAllTagsTests : SliceFixture
    {
        [Fact]
        public async Task Should_return_all_tags()
        {
            var tag = new Tag
            {
                Name = "DDD"
            };

            var python = new Tag
            {
                Name = "Python"
            };

            await InsertAsync(tag, python);

            var result = await SendAsync(new ListAllQ.Query());

            result.ShouldNotBeNull();
            result.Tags.Count.ShouldBeGreaterThanOrEqualTo(2);
        }
    }
}
