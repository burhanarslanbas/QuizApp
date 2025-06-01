using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using QuizApp.Domain.Entities;
using QuizApp.Domain.Entities.Common;
using QuizApp.Domain.Entities.Identity;
using QuizApp.Persistence.Configurations;

namespace QuizApp.Persistence.Contexts
{
    public class QuizAppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) : base(options)
        { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<QuestionRepo> QuestionRepos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Entity konfigürasyonlarını uygula
            // builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new QuizConfiguration());
            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new OptionConfiguration());
            builder.ApplyConfiguration(new QuizResultConfiguration());
            builder.ApplyConfiguration(new UserAnswerConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
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
                    _ => DateTime.UtcNow
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
            
            // Varsayılan olarak Development
            var environment = "Development";
            // Komut satırı argümanlarından environment'ı bul
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "--environment" && !string.IsNullOrEmpty(args[i + 1]))
                {
                    environment = args[i + 1];
                    break;
                }
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Presentation", "QuizApp.API")))
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                options => options.EnableRetryOnFailure());

            return new QuizAppDbContext(optionsBuilder.Options);
        }
    }
}
