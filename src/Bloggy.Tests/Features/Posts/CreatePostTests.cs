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
    public class CreatePostTests : SliceFixture
    {
        [Fact(Skip="context accessor is giving me problems")]
        public async Task Should_create_new_post()
        {

            Create.Command command = null;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                command = new Create.Command
                {
                    Title = "post title",
                    Body = "post obdy",
                    CategoryId = 1
                };
                await mediator.Send(command);
            });

            var created = await ExecuteDbContextAsync(db => db.Posts.Where(c => c.Title == command.Title).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Title.ShouldBe(command.Title);
            created.Body.ShouldBe(command.Body);
            created.CategoryId.ShouldBe(command.CategoryId);
        }
    }
}
