using Microsoft.EntityFrameworkCore;
using Subutai.Repository.SqlRepository.Contexts;

namespace Subutai.Repository.SqlRepository.Tests;

public static class ContextGenerator
{
    public static SubutaiContext Generate()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SubutaiContext>()
            .UseInMemoryDatabase(Guid.NewGuid()
            .ToString());

        return new SubutaiContext(optionsBuilder.Options);
    }
}
