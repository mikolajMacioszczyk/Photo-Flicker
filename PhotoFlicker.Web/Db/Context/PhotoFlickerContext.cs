using Microsoft.EntityFrameworkCore;
using PhotoFlicker.Models;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Web.Db.Context
{
    public class PhotoFlickerContext : DbContext
    {
        public PhotoFlickerContext(DbContextOptions<PhotoFlickerContext> options): base(options)
        {
        }

        public DbSet<Photo> PhotoItems { get; set; }
        public DbSet<Tag> TagItems { get; set; }
    }
}