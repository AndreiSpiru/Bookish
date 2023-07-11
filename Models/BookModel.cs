using System.ComponentModel.DataAnnotations.Schema;

namespace Bookish.Models;

public class Book
{
    public int BookId { get; set; }
    public int Copies { get; set; }
    
    public int AvailableCopies { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
}

