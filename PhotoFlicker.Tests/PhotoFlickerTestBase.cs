using System;
using Microsoft.EntityFrameworkCore;
using PhotoFlicker.Web.Db.Context;

namespace PhotoFlicker.Tests
{
    public class PhotoFlickerTestBase
    {
        protected PhotoFlickerContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<PhotoFlickerContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            
            var context = new PhotoFlickerContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        protected void DisposeContext(PhotoFlickerContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}