namespace THZ.App.Template.Database
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;

    using THZ.App.Template.Models.DataModel;

    using Uninf.Data.EntityFramework;

    public class DatabaseContext:DbContextBase
    {
         //此处写入需要的DbSet<T>
        public DatabaseContext()
            : base("name=connstr")
        {
            
        }

        public DbSet<UserMicroBlog> MicroBlogs { get; set; }

        public DbSet<MicroBlogComment> Comments { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserMicroBlog>().ToTable("Microblog");

            modelBuilder.Entity<UserMicroBlog>().HasKey(x => x.Id);

            modelBuilder.Entity<UserMicroBlog>()
                .Property(x => x.Id).HasColumnName("blogId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}