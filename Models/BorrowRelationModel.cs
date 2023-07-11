using System.ComponentModel.DataAnnotations;

namespace Bookish.Models;

public class BorrowRelationModel
{
    [Key]
    public int RelationId { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public int Copies { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}