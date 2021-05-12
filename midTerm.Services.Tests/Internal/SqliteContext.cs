using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using midTerm.Data;
using midTerm.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midTerm.Services.Tests.Internal
{
    public abstract class SqliteContext
    : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory";
        private readonly SqliteConnection _connection;
        protected readonly MidTermDbContext DbContext;

        protected SqliteContext(bool withData = false)
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            DbContext = new MidTermDbContext(CreateOptions());
            _connection.Open();
            DbContext.Database.EnsureCreated();
            
            if(withData)
                SeedData(DbContext);
        }

        private DbContextOptions<MidTermDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<MidTermDbContext>()
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseSqlite(_connection)
                .Options;
        }

        private void SeedData(MidTermDbContext dbContext)
        {
                var options = new List<Option>
                {
                    new Option
                    {
                        Id = 1,
                        Text = "Option1"
                    },
                    new Option
                    {
                         Id = 2,
                        Text = "Option2"
                    },
                    new Option
                    {
                        Id = 3,
                        Text = "Option3"
                    }
                };

                var questions = new List<Question>
                {
                    new Question
                    {
                        Id = 1,
                        Description = "Question1"
                    },
                    new Question
                    {
                       Id = 2,
                        Description = "Question2"
                    },
                    new Question
                    {
                        Id = 3,
                        Description = "Question3"
                    },
                    new Question
                    {
                       Id = 4,
                        Description = "Question4"
                    },
                };
               

                dbContext.AddRange(options);
                dbContext.AddRange(questions);
                dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _connection.Close();
            _connection?.Dispose();
            DbContext?.Dispose();
        }
    }
}

