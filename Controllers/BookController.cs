using System.Diagnostics;
using Bookish.Mappers;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Services;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Controllers;

public class BookController : Controller
{
    private readonly ILogger<BookController> _logger;
    private readonly BookMapper _mapper;
    private readonly BookService _service;

    public BookController(ILogger<BookController> logger)
    {
        _logger = logger;
        _mapper = new BookMapper();
        _service = new BookService();
    }

    public IActionResult BookPage(int sortBy)
    {
        
        List<Book> books = _service.GetAllBooks(sortBy);
        BookPageModel model = _mapper.GetBookPageModel(books);
        

        return View(model);
    }
    // GET : BookPage
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(int copies, string title, string author)
    {
        _service.CreateEntry(copies, title, author);
        return RedirectToAction("BookPage");
    }
    
    public IActionResult Delete(int id)
    {
        _service.DeleteEntryById(id);
        return RedirectToAction("BookPage");
    }
    
    public IActionResult Edit(int id)
    {
        Book book = _service.GetBookById(id);
        return View(book);
        
    }
    
    [HttpPost]
    public IActionResult Edit(int id, int copies, string title, string author)
    {
        _service.EditBookById(id, copies, title, author);
        return RedirectToAction("BookPage");
    }
    
    public IActionResult AddCopy(int id)
    {
        _service.AddCopyById(id);
        return RedirectToAction("BookPage");
    }
    
    public IActionResult RemoveCopy(int id)
    {
        _service.RemoveCopyById(id);
        return RedirectToAction("BookPage");
    }

    public IActionResult CheckOut(int bookId)
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult CheckOut(int bookId, int userId, int checkOutCopies)
    {
        _service.CheckoutBook(bookId, userId, checkOutCopies);
        return RedirectToAction("BookPage");
    }

    public IActionResult Borrowers(int bookId)
    {
        List<BorrowRelationModel> relations = _service.GetRelationsByBookId(bookId);
        BorrowersPageModel model = _mapper.GetBorrowersPageModel(relations);
        return View(model);
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    
}