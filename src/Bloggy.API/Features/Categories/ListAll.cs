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

namespace Bloggy.API.Features.Categories
{
    public class ListAllQ
    {
        public class Query : IRequest<Model>
        {
            public List<string> Categories { get; set; }
        }

        public class Model
        {
            public List<string> Categories { get; set; }
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
                return new Model { Categories = await _context.Categories.OrderBy(c => c.Name).Select(c=> c.Name).AsNoTracking().ToListAsync() };
            }
        }
    }
}
