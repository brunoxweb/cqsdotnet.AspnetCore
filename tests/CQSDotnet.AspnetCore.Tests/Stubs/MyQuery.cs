using System.Diagnostics.CodeAnalysis;
using CQSDotnet.Queries.Interfaces;

namespace CQSDotnet.AspnetCore.Tests.Stubs
{
    [ExcludeFromCodeCoverage]
    public class MyQuery : IQuery<MyDto>
    {
    }

    [ExcludeFromCodeCoverage]
    public class MyQueryHandler : IQueryHandler<MyQuery, MyDto>
    {
        public Task<MyDto> HandleAsync(MyQuery query, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new MyDto { Id = 1, Name = "Name" });
        }
    }

    [ExcludeFromCodeCoverage]
    public class MyDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
