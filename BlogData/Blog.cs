using System;
using System.Collections.Generic;

namespace BlogData
{
    /// <summary>
    /// A blog entry
    /// </summary>
    public class Blog
    {
        /// <summary>
        /// Creates a new instance of <see cref="Blog"/>.
        /// </summary>
        public Blog()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the blog.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The blog URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// <see cref="List{Post}"/> related to the <see cref="Blog"/>.
        /// </summary>
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
