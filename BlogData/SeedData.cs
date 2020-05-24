using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogData
{
    /// <summary>
    /// Example to seed data.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Seed the data.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        public static async Task SeedDataAsync()
        {
            using var context = new BlogContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var myBlog = new Blog
            {
                Id = Guid.NewGuid(),
                Url = "https://blog.jeremylikness.com",
                Posts = new List<Post>
                    {
                        new Post
                        {
                            IsActive = true,
                            Title = "First post"
                        },
                        new Post
                        {
                            IsActive = false,
                            Title = "Draft"
                        }
                    }
            };

            context.Add(myBlog);
            await context.SaveChangesAsync();
        }
    }
}
