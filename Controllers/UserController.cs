using System.Diagnostics;
using Bookish.Mappers;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Services;
using Microsoft.EntityFrameworkCore;

namespace Userish.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly UserMapper _mapper;
    private readonly UserService _service;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
        _mapper = new UserMapper();
        _service = new UserService();

    }

    public IActionResult UserPage(int sortBy)
    {
        List<User> users = _service.GetAllUsers(sortBy);
        UserPageModel model = _mapper.GetUserPageModel(users);
        return View(model);

    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(string firstname, string lastname)
    {
        _service.CreateEntry(firstname, lastname);
        return RedirectToAction("UserPage");
    }
    
    public IActionResult Delete(int id)
    {
        _service.DeleteEntryById(id);
        return RedirectToAction("UserPage");
    }
    
    public IActionResult Edit(int id)
    {
        using (var context = new LibraryContext())
        {
            return View(context.Users.Find(id));
        }
    }
    
    [HttpPost]
    public IActionResult Edit(int id, string firstName, string lastName)
    {
        _service.EditUserById(id, firstName, lastName);
        return RedirectToAction("UserPage");
    }
    
    public IActionResult CheckIn(int userId)
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult CheckIn(int bookId, int userId, int checkInCopies)
    {
        _service.CheckinBook(bookId, userId, checkInCopies);
        return RedirectToAction("UserPage");
    }
    
    public IActionResult BorrowedBooks(int userId)
    {
        List<BorrowRelationModel> relations = _service.GetRelationsByUserId(userId);
        BorrowedBooksPageModel model = _mapper.GetBorrowedBooksPageModel(relations);
        return View(model);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}