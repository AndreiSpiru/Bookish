using Bookish.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Services;

public class BookService
{
    public List<Book> GetAllBooks(int sortBy)
    { 
        var context = new LibraryContext();
        List<Book> books = context.Books.ToList();
        switch (sortBy)
        {
            case 1:
                books.Sort(delegate(Book b1, Book b2) { return b1.Author.CompareTo(b2.Author);});
                break;
            case 2:
                books.Sort(delegate(Book b1, Book b2) { return b1.Title.CompareTo(b2.Title);});
                break;
            case 3:
                books.Sort((delegate(Book b1, Book b2) { return b1.Copies.CompareTo(b2.Copies); }));
                break;
            case 4:
                books.Sort((delegate(Book b1, Book b2) { return b1.AvailableCopies.CompareTo(b2.AvailableCopies); }));
                break;
        }
        return books;

    }

    public void CreateEntry(int copies, string title, string author)
    { 
        var context = new LibraryContext();
        context.Books.Add(new Book(){Copies = copies, Title = title,Author = author, AvailableCopies = copies});
        context.SaveChanges();
    }

    public void DeleteEntryById(int id)
    { 
        var context = new LibraryContext();
        context.Books.Where(b => b.BookId == id).ExecuteDelete();
        context.Relations.Where(b => b.BookId == id).ExecuteDelete();
    }

    public Book GetBookById(int id)
    {
        var context = new LibraryContext();
        return context.Books.Find(id);
    }

    public void EditBookById(int id, int copies, string title, string author)
    {
        var context = new LibraryContext();
        Book book = context.Books.Find(id);
        book.Copies = copies;
        book.Title = title;
        book.Author = author;
        context.SaveChanges();
    }

    public void AddCopyById(int id)
    {
        var context = new LibraryContext();
        Book book = context.Books.Find(id);
        book.Copies++;
        book.AvailableCopies++;
        context.SaveChanges();
    }

    public void RemoveCopyById(int id)
    {
        var context = new LibraryContext();
        Book book = context.Books.Find(id);
        if (book.Copies > 0 && book.AvailableCopies > 0)
        {
            book.Copies--;
            book.AvailableCopies--;
        }
        context.SaveChanges();
    }

    public void CheckoutBook(int bookId, int userId, int checkOutCopies)
    {
        Book book;
        var context = new LibraryContext();
        if (context.Relations.Any(r => (r.BookId == bookId && r.UserId == userId)))
        {
            BorrowRelationModel r = context.Relations.Single(r => (r.BookId == bookId && r.UserId == userId));
            r.Copies += checkOutCopies;
            book = context.Books.Find(bookId);
            book.AvailableCopies -= checkOutCopies;

        }
        else
        {
            context.Relations.Add(new BorrowRelationModel(){Copies = checkOutCopies, BookId = bookId, UserId = userId});
            book = context.Books.Find(bookId);
            book.AvailableCopies -= checkOutCopies;
            
        }
        if (book.AvailableCopies >= 0)
        {
            context.SaveChanges();
        }
    }

    public List<BorrowRelationExtModel> GetRelationsByBookId(int bookId)
    {
        var context = new LibraryContext();
        Book book;
        User user;
        List<BorrowRelationModel> relations = context.Relations.Where(r => (r.BookId == bookId)).ToList();
        List<BorrowRelationExtModel> extendedRelations = new List<BorrowRelationExtModel>();
        foreach (var relation in relations)
        {
            BorrowRelationExtModel extendedRelation = new BorrowRelationExtModel();
            extendedRelation.Copies = relation.Copies;
            extendedRelation.RelationId = relation.RelationId;
            extendedRelation.BookId = relation.BookId;
            extendedRelation.UserId = relation.UserId;
            book = context.Books.Find(bookId);
            extendedRelation.Author = book.Author;
            extendedRelation.Title = book.Title;
            user = context.Users.Find(relation.UserId);
            extendedRelation.FirstName = user.FirstName;
            extendedRelation.LastName = user.LastName;
            extendedRelations.Add(extendedRelation);
        }

        return extendedRelations;
    }
}