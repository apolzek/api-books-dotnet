using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#region snippet_NewtonsoftJsonImport
using Newtonsoft.Json;
#endregion

namespace BooksApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string BookName { get; set; }

        [Range(0, 1_000_000)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Category { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 1)]
        public string Author { get; set; }
    }
}
