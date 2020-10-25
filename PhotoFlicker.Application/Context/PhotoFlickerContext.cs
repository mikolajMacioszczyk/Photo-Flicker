using Microsoft.EntityFrameworkCore;
using PhotoFlicker.Models.Models;

namespace PhotoFlicker.Application.Context
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