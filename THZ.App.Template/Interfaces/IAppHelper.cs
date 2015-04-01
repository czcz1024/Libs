namespace THZ.App.Template.Interfaces
{
    using System.Collections;
    using System.Collections.Generic;

    using THZ.App.Template.Models.Commands;
    using THZ.App.Template.Models.ViewModel;

    public interface IAppHelper
    {
        string Test();

        bool SameInstance(ITHZHelper thz1);

        int Add(AddMicroBlog cmd);

        void Delete(DeleteMicroBlog cmd);

        IEnumerable<MyMicroBlogViewModel> MyBlogs(int uid, out long all, int page = 1, int pagesize = 10);

        IEnumerable<UserMicroBlogViewModel> UserBlogs(int uid,int? visitor, out long all, int page = 1, int pagesize = 10);

        IEnumerable<FriendMicroBlogViewModel> FriendBlogs(int uid, int page, int i, out long all);

        IEnumerable<FriendMicroBlogViewModel> All(int page, int i, out long all);
    }
}