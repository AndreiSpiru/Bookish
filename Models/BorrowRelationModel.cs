using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Models;

public class BorrowRelationModel
{
    [Key]
    public int RelationId { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public int Copies { get; set; }
    
}