using System;
namespace spark.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}

