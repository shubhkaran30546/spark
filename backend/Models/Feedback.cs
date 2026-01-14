using System;
namespace spark.Models
{
    /// <summary>
    /// User feedback entry submitted via the site.
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// Feedback identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The feedback content provided by the user.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// When the feedback was submitted (UTC).
        /// </summary>
        public DateTime SubmittedAt { get; set; }
    }
}

