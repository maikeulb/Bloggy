using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Bloggy.API.Features.Posts;
using Bloggy.API.Entities;
using Bloggy.Tests;
using Shouldly;
using Xunit;
using static Bloggy.Tests.SliceFixture;

namespace Bloggy.Tests.Features.Posts
{
    public class ListAllCategoreisTests : SliceFixture
    {
        [Fact]
        public async Task Should_return_all_posts()
        {
            var first = new Post
            {
                CategoryId = 2,
                AuthorId = 1,
                Title = "dolor lorem",
                Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                CreatedDate = new DateTime (2018, 4, 30),
            };

            var second = new Post
            {
                CategoryId = 1,
                AuthorId = 2,
                Title = "dolor lorem",
                Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                CreatedDate = new DateTime (2018, 1, 30),
            };

            await InsertAsync(first, second);

            var result = await SendAsync(new ListAll.Query());

            result.ShouldNotBeNull();
        }
    }
}
