using Bookish.Models;

namespace Bookish.Mappers;

public class UserMapper
{
    public UserPageModel GetUserPageModel(List<User> users)
    {
        return new UserPageModel{Users = users};
    }
    
    public BorrowedBooksPageModel GetBorrowedBooksPageModel(List<BorrowRelationModel> relations)
    {
        return new BorrowedBooksPageModel { Relations = relations };
    }
}