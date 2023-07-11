using Bookish.Models;

namespace Bookish.Mappers;

public class BookMapper
{
    public BookPageModel GetBookPageModel(List<Book> books)
    {
        return new BookPageModel{ Books = books };
    }

    public BorrowersPageModel GetBorrowersPageModel(List<BorrowRelationExtModel> relations)
    {
        return new BorrowersPageModel { Relations = relations };
    }
}