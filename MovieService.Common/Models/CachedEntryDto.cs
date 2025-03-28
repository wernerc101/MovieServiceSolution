using System;

namespace MovieService.Common.Models
{
    public class CachedEntryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string ImdbID { get; set; }
        public DateTime CachedAt { get; set; }
    }
}