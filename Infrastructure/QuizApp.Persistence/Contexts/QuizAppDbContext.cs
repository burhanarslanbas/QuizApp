using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using QuizApp.Domain.Entities;
using QuizApp.Domain.Entities.Identity;
using QuizApp.Persistence.Configurations;
using QuizApp.Persistence.Interceptors;

namespace QuizApp.Persistence.Contexts
{
    public class QuizAppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        private readonly BaseEntitySaveChangesInterceptor _baseEntitySaveChangesInterceptor;

        public QuizAppDbContext(
            DbContextOptions<QuizAppDbContext> options,
            BaseEntitySaveChangesInterceptor baseEntitySaveChangesInterceptor)
            : base(options)
        {
            _baseEntitySaveChangesInterceptor = baseEntitySaveChangesInterceptor;
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<QuestionRepo> QuestionRepos { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_baseEntitySaveChangesInterceptor);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Entity Configurations
            builder.ApplyConfiguration(new QuizConfiguration());
            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new OptionConfiguration());
            builder.ApplyConfiguration(new QuizResultConfiguration());
            builder.ApplyConfiguration(new UserAnswerConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new QuestionRepoConfiguration());
            builder.ApplyConfiguration(new QuizQuestionConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }
    }

    public class QuizAppDbContextFactory : IDesignTimeDbContextFactory<QuizAppDbContext>
    {
        public QuizAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuizAppDbContext>();

            var environment = "Development";
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

            var connectionString = environment == "Development"
                ? configuration.GetConnectionString("LocalConnection")
                : configuration.GetConnectionString("AzureConnection");

            optionsBuilder.UseSqlServer(connectionString,
                options => options.EnableRetryOnFailure());

            var interceptor = new BaseEntitySaveChangesInterceptor();
            return new QuizAppDbContext(optionsBuilder.Options, interceptor);
        }
    }
}
