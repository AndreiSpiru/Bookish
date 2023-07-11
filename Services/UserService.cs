using Bookish.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Services;

public class UserService
{
    public List<User> GetAllUsers(int sortBy)
    {
        var context = new LibraryContext();
        List<User> users = context.Users.ToList();
        switch (sortBy)
        {
            case 1:
                users.Sort(delegate(User u1, User u2) { return u1.FirstName.CompareTo(u2.FirstName);});
                break;
            case 2:
                users.Sort(delegate(User u1, User u2) { return u1.LastName.CompareTo(u2.LastName);});
                break;
        }

        return users;
    }
    
    public void CreateEntry(string firstname, string lastname)
    { 
        var context = new LibraryContext();
        
        context.Users.Add(new User(){FirstName = firstname, LastName = lastname});
        context.SaveChanges();
        
    }

    public void DeleteEntryById(int id)
    {
        var context = new LibraryContext();
        
        context.Users.Where(b => b.UserId == id).ExecuteDelete();
        if (context.Relations.Any(r => r.UserId == id))
        {
            List<BorrowRelationModel> relations = context.Relations.Where(r => r.UserId == id).ToList();
            foreach (var relation in relations)
            {
                CheckinBook(relation.BookId, relation.UserId, relation.Copies);
            }
        }
        context.Relations.Where(b => b.UserId == id).ExecuteDelete();
    }

    public void EditUserById(int id, string firstName, string lastName)
    {
        var context = new LibraryContext();
        User user = context.Users.Find(id);
        user.FirstName = firstName;
        user.LastName = lastName;
        context.SaveChanges();
        
    }
    
    public void CheckinBook(int bookId, int userId, int checkInCopies)
    {
        Book book;
        var context = new LibraryContext();
        if (context.Relations.Any(r => (r.BookId == bookId && r.UserId == userId)))
        {
            BorrowRelationModel r = context.Relations.Single(r => (r.BookId == bookId && r.UserId == userId));
            if (r.Copies >= checkInCopies)
            {
                r.Copies -= checkInCopies;
                book = context.Books.Find(bookId);
                book.AvailableCopies += checkInCopies;
                context.SaveChanges();
                if (r.Copies == 0)
                {
                    context.Relations.Where(r => (r.BookId == bookId && r.UserId == userId)).ExecuteDelete();
                }
            }
        }
    }
    
    public List<BorrowRelationModel> GetRelationsByUserId(int userId)
    {
        var context = new LibraryContext();
        return context.Relations.Where(r => (r.UserId == userId)).ToList();
    }
}