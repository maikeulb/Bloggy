using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using FluentValidation;
using Bloggy.API.Data;
using Bloggy.API.Infrastructure;
using Bloggy.API.Services;
using Bloggy.API.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Bloggy.API.Features.Tags
{
    public class ListAllQ
    {
        public class Query : IRequest<Model>
        {
            public List<string> Tags { get; set; }
        }

        public class Model
        {
            public List<string> Tags { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Model> HandleCore (Query message)
            {
                return new Model { Tags = await _context.Tags.OrderBy(t => t.Name).Select(t=> t.Name).AsNoTracking().ToListAsync() };
            }
        }
    }
}
