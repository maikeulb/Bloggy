using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Bloggy.API.Features.Comments;
using Bloggy.API.Entities;
using Bloggy.Tests;
using Shouldly;
using Xunit;
using static Bloggy.Tests.SliceFixture;

namespace Bloggy.Tests.Features.Comments
{
    public class ListAllCategoreisTests : SliceFixture
    {
        [Fact]
        public async Task Should_return_all_comments()
        {

            var first = new Comment ()
            {
                PostId = 4,
                AuthorId = 2,
                Body = "amazing",
            };

            var second = new Comment ()
            {
                PostId = 4,
                AuthorId = 2,
                Body = "amazing",
            };

            await InsertAsync(first, second);

            var result = await SendAsync(new ListAll.Query());

            result.ShouldNotBeNull();
        }
    }
}
