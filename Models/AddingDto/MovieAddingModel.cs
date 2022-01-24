using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.AddingDto
{
    public class MovieAddingModel
    {
        [MaxLength(200)] public string MovieDescription { get; set; }
        public bool AllowAnonymous { get; set; }
        public int NumberTopic { get; set; }
        public int MaxHighlightPerUser { get; set; }
        public bool OnlyHighlightOnce { get; set; }
    }
}