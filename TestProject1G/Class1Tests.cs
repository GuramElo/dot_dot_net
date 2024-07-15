using ConsoleApp1;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reddit;
using Reddit.Models;
using Reddit.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1G
{
   
    public class Class1Tests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(12)]
        public void Class1_isPositive_ReturnBool(int num)
        {
            Class1 instance = new Class1();
            instance.isPositive(num).Should().BeTrue();
            //Assert.True(instance.isPositive(num));

        }
        [Fact]
        public async void Class1_unrelatedTest()
        {
            using var context = CreateContext();
            var repository = new PostsRepository(context);
            var pagedPosts = await repository.GetPosts(page: 1, pageSize: 2, searchTerm: null, SortTerm: null);

            Assert.Equal(2, pagedPosts.Items.Count);
            Assert.Equal(3, pagedPosts.TotalCount);
        }
        private ApplicationDbContext CreateContext()
        {
            var dbName = Guid.NewGuid().ToString();     // give unqie name to the database, so that different tests don't interfere with each other
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Posts.AddRange(
                new Post { Id = 1, Title = "First Post", Content = "First post content", Upvotes = 10, Downvotes = 2 }, // 10 / 12 = 0.8333333333333333
                new Post { Id = 2, Title = "Second Post", Content = "Second post content", Upvotes = 5, Downvotes = 1 },// 5 /6
                new Post { Id = 3, Title = "Third Post", Content = "Third post content", Upvotes = 20, Downvotes = 5 } // 20 / 25 = 0.8
            );

            context.SaveChanges();
            return context;
        }

    }
}
