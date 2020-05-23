using Microsoft.EntityFrameworkCore;

namespace BlogData
{
    /// <summary>
    /// Data access for the <see cref="Blog"/> and <see cref="Post"/> classes.
    /// </summary>
    public class BlogContext : DbContext
    {
        /// <summary>
        /// Database name.
        /// </summary>
        public static readonly string MyBlogs = nameof(MyBlogs).ToLower();

        /// <summary>
        /// The list of <see cref="Blog"/> entities.
        /// </summary>
        public DbSet<Blog> Blogs { get; set; }

        /// <summary>
        /// Creates an instance of the <see cref="BlogContext"/> class.
        /// </summary>
        public BlogContext() : base()
        {

        }

        /// <summary>
        /// Creates an instance of the <see cref="BlogContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{BlogContext}"/> to configure the context.</param>
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
    }
}
