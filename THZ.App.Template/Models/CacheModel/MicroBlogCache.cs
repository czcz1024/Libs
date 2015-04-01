namespace THZ.App.Template.Models.CacheModel
{
    using System;

    public class MicroBlogCache
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime AddTime { get; set; }

        public int UserId { get; set; }

        public string Image { get; set; }

        public string Video { get; set; }

        public int VisitType { get; set; }
    }
}