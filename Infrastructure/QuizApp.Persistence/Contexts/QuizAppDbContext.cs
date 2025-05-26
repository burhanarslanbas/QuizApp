using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QuizApp.Domain.Entities;
using QuizApp.Domain.Entities.Common;
using QuizApp.Persistence.Configurations;

namespace QuizApp.Persistence.Contexts
{
    public class QuizAppDbContext : DbContext
    {
        public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) : base(options)
        { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QuestionRepo> QuestionRepos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Entity konfigürasyonlarını uygula
            builder.ApplyConfiguration(new QuizConfiguration());
            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new OptionConfiguration());
            builder.ApplyConfiguration(new UserAnswerConfiguration());
            builder.ApplyConfiguration(new QuizResultConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new QuestionRepoConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // ChangeTracker : Entityler üzerinden yapılan değişikliklerin ya da yeni eklenen verinin yakalanmasını sağlayan propertydir.
            // Update operasyonlarında Track edilen verileri yakalayıp elde etmemizi sağlar.

            var models = ChangeTracker.Entries<BaseEntity>();

            foreach (var model in models)
            {
                // Buradaki _ ifadesi discard edilen bir değişkendir. Yani bu değişkeni kullanmayacağız.
                _ = model.State switch
                {
                    EntityState.Added => model.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => model.Entity.UpdatedDate = DateTime.UtcNow,
                    EntityState.Deleted => model.Entity.DeletedDate = DateTime.UtcNow,
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
    public class QuizAppDbContextFactory : IDesignTimeDbContextFactory<QuizAppDbContext>
    {
        public QuizAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuizAppDbContext>();
            optionsBuilder.UseSqlServer("Server=BURHAN;Database=QuizAppDB;Trusted_Connection=True;TrustServerCertificate=True;",
                options => options.EnableRetryOnFailure());

            return new QuizAppDbContext(optionsBuilder.Options);
        }
    }
}
