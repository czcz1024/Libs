namespace THZ.App.Template.Models.DataModel
{
    using System;
    using System.Collections.Generic;

    public class UserMicroBlog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string BlogContent { get; set; }

        public string MroblogPic { get; set; }

        public string MroblogVideo { get; set; }

        public int VisitRole { get; set; }

        public DateTime AddDate { get; set; }

        public int CommentsCount { get; set; }

        public int ShareCount { get; set; }

        public ICollection<MicroBlogComment> Comments { get; set; }
    }
}