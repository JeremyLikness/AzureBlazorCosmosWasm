using Microsoft.EntityFrameworkCore;

namespace BlogData
{
    /// <summary>
    /// Post information.
    /// </summary>
    [Owned]
    public class Post
    {
        /// <summary>
        /// The title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// <c>true</c> when the post is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
