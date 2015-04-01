namespace THZ.App.Template.Models.DataModel
{
    public class MicroBlogComment
    {
        public int Id { get; set; }

        public UserMicroBlog Blog { get; set; }
    }
}