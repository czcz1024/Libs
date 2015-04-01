namespace THZ.App.Template.Models.Events
{
    using System;

    public class MicroBlogAdded
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string BlogContent { get; set; }

        public string MroblogPic { get; set; }

        public string MroblogVideo { get; set; }

        public int VisitRole { get; set; }

        public DateTime AddDate { get; set; }
    }
}