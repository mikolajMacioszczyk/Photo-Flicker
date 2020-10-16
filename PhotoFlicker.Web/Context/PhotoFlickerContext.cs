using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PhotoFlicker.Models;

namespace PhotoFlicker.Web.Context
{
    public class PhotoFlickerContext : DbContext
    {
        public PhotoFlickerContext(DbContextOptions<PhotoFlickerContext> options): base(options)
        {
        }

        public DbSet<Photo> PhotoItems { get; set; }
        public DbSet<Tag> TagItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var tagItems = new List<Tag>()
            {
                new Tag(){Id = 1, Name = "Kanion"},
                new Tag(){Id = 2, Name = "USA"},
                new Tag(){Id = 3, Name = "Zorza"},
            };
            var photoItems = new List<Photo>()
            {
                new Photo(){Id = 1, Path = "https://planetescape.pl//app/uploads/2018/10/Wielki-Kanion-3.jpg", Tags = new List<Tag>(){tagItems[0], tagItems[1]}},
                new Photo(){Id = 2, Path = "https://www.ef.pl/sitecore/__/~/media/universal/pg/8x5/destination/US_US-CA_LAX_1.jpg", Tags = new List<Tag>(){tagItems[1]}},
                new Photo(){Id = 3, Path = "https://i.wpimg.pl/1500x0/d.wpimg.pl/1036153311-705207955/zorza-polarna.jpg", Tags = new List<Tag>(){tagItems[2]}},
                new Photo(){Id = 4, Path = "https://www.banita.travel.pl/wp-content/uploads/2019/10/zorza-polarna-norwegia1-1920x1282.jpg", Tags = new List<Tag>(){tagItems[2]}},
            };
            tagItems[0].MarkedPhotos = new List<Photo>(){photoItems[0], photoItems[1]};
            tagItems[1].MarkedPhotos = new List<Photo>(){photoItems[1]};
            tagItems[2].MarkedPhotos = new List<Photo>(){photoItems[2], photoItems[3]};
            modelBuilder.Entity<Photo>().HasData(photoItems);
            modelBuilder.Entity<Tag>().HasData(tagItems);
            base.OnModelCreating(modelBuilder);
        }
    }
}