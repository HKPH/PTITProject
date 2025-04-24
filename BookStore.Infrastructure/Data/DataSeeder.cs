using BookStore.Application.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(BookStoreContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Publishers.AnyAsync() || await context.Categories.AnyAsync() || await context.Books.AnyAsync())
            {
                return;
            }

            var publishers = new List<Publisher>
            {
                new Publisher { Name = "Penguin Books", Active = true },
                new Publisher { Name = "HarperCollins", Active = true },
                new Publisher { Name = "Random House", Active = true }
            };
            await context.Publishers.AddRangeAsync(publishers);
            await context.SaveChangesAsync();

            var categories = new List<Category>
            {
                new Category { Name = "Fiction", Active = true },
                new Category { Name = "Non-Fiction", Active = true },
                new Category { Name = "Science Fiction", Active = true }
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var books = new List<Book>
            {
                new Book
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    PublisherId = publishers[0].Id,
                    Price = 9.99m,
                    PublicationDate = new DateTime(1925, 4, 10),
                    Active = true,
                    StockQuantity = 100,
                    Description = "A tale of wealth, love, and the American Dream.",
                    Image = "great_gatsby.jpg",
                    Categories = new List<Category> { categories[0] }
                },
                new Book
                {
                    Title = "Sapiens",
                    Author = "Yuval Noah Harari",
                    PublisherId = publishers[1].Id,
                    Price = 14.99m,
                    PublicationDate = new DateTime(2011, 9, 4),
                    Active = true,
                    StockQuantity = 50,
                    Description = "A history of humankind from Stone Age to present.",
                    Image = "sapiens.jpg",
                    Categories = new List<Category> { categories[1] }
                },
                new Book
                {
                    Title = "Dune",
                    Author = "Frank Herbert",
                    PublisherId = publishers[2].Id,
                    Price = 12.99m,
                    PublicationDate = new DateTime(1965, 8, 1),
                    Active = true,
                    StockQuantity = 75,
                    Description = "An epic sci-fi saga on the planet Arrakis.",
                    Image = "dune.jpg",
                    Categories = new List<Category> { categories[2] }
                },
                new Book
                {
                    Title = "1984",
                    Author = "George Orwell",
                    PublisherId = publishers[0].Id,
                    Price = 10.99m,
                    PublicationDate = new DateTime(1949, 6, 8),
                    Active = true,
                    StockQuantity = 80,
                    Description = "A dystopian novel of surveillance and control.",
                    Image = "1984.jpg",
                    Categories = new List<Category> { categories[0], categories[2] }
                },
                new Book
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    PublisherId = publishers[1].Id,
                    Price = 7.99m,
                    PublicationDate = new DateTime(1813, 1, 28),
                    Active = true,
                    StockQuantity = 90,
                    Description = "A classic romance of love and society.",
                    Image = "pride_prejudice.jpg",
                    Categories = new List<Category> { categories[0] }
                },
                new Book
                {
                    Title = "Educated",
                    Author = "Tara Westover",
                    PublisherId = publishers[2].Id,
                    Price = 13.49m,
                    PublicationDate = new DateTime(2018, 2, 20),
                    Active = true,
                    StockQuantity = 45,
                    Description = "A memoir of self-education and discovery.",
                    Image = "educated.jpg",
                    Categories = new List<Category> { categories[1] }
                },
                new Book
                {
                    Title = "Foundation",
                    Author = "Isaac Asimov",
                    PublisherId = publishers[0].Id,
                    Price = 11.99m,
                    PublicationDate = new DateTime(1951, 5, 1),
                    Active = true,
                    StockQuantity = 70,
                    Description = "A sci-fi tale of a galactic empire’s fall.",
                    Image = "foundation.jpg",
                    Categories = new List<Category> { categories[2] }
                },
                new Book
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    PublisherId = publishers[1].Id,
                    Price = 9.49m,
                    PublicationDate = new DateTime(1960, 7, 11),
                    Active = true,
                    StockQuantity = 85,
                    Description = "A story of justice and morality in the South.",
                    Image = "mockingbird.jpg",
                    Categories = new List<Category> { categories[0] }
                },
                new Book
                {
                    Title = "Becoming",
                    Author = "Michelle Obama",
                    PublisherId = publishers[2].Id,
                    Price = 15.99m,
                    PublicationDate = new DateTime(2018, 11, 13),
                    Active = true,
                    StockQuantity = 55,
                    Description = "A memoir by the former First Lady.",
                    Image = "becoming.jpg",
                    Categories = new List<Category> { categories[1] }
                },
                new Book
                {
                    Title = "Neuromancer",
                    Author = "William Gibson",
                    PublisherId = publishers[0].Id,
                    Price = 10.49m,
                    PublicationDate = new DateTime(1984, 7, 1),
                    Active = true,
                    StockQuantity = 65,
                    Description = "A cyberpunk novel of hacking and AI.",
                    Image = "neuromancer.jpg",
                    Categories = new List<Category> { categories[2] }
                },
                new Book
                {
                    Title = "The Hobbit",
                    Author = "J.R.R. Tolkien",
                    PublisherId = publishers[1].Id,
                    Price = 11.49m,
                    PublicationDate = new DateTime(1937, 9, 21),
                    Active = true,
                    StockQuantity = 95,
                    Description = "A fantasy adventure with Bilbo Baggins.",
                    Image = "hobbit.jpg",
                    Categories = new List<Category> { categories[0] }
                },
                new Book
                {
                    Title = "The Alchemist",
                    Author = "Paulo Coelho",
                    PublisherId = publishers[2].Id,
                    Price = 8.49m,
                    PublicationDate = new DateTime(1988, 1, 1),
                    Active = true,
                    StockQuantity = 70,
                    Description = "A tale of following one’s destiny.",
                    Image = "alchemist.jpg",
                    Categories = new List<Category> { categories[0] }
                },
                new Book
                {
                    Title = "Thinking, Fast and Slow",
                    Author = "Daniel Kahneman",
                    PublisherId = publishers[0].Id,
                    Price = 14.49m,
                    PublicationDate = new DateTime(2011, 10, 25),
                    Active = true,
                    StockQuantity = 40,
                    Description = "An exploration of human decision-making.",
                    Image = "thinking_fast_slow.jpg",
                    Categories = new List<Category> { categories[1] }
                },
                new Book
                {
                    Title = "Hyperion",
                    Author = "Dan Simmons",
                    PublisherId = publishers[1].Id,
                    Price = 12.49m,
                    PublicationDate = new DateTime(1989, 5, 26),
                    Active = true,
                    StockQuantity = 60,
                    Description = "A sci-fi epic of pilgrimage and mystery.",
                    Image = "hyperion.jpg",
                    Categories = new List<Category> { categories[2] }
                },
                new Book
                {
                    Title = "Brave New World",
                    Author = "Aldous Huxley",
                    PublisherId = publishers[2].Id,
                    Price = 9.99m,
                    PublicationDate = new DateTime(1932, 1, 1),
                    Active = true,
                    StockQuantity = 80,
                    Description = "A dystopian vision of a controlled society.",
                    Image = "brave_new_world.jpg",
                    Categories = new List<Category> { categories[0], categories[2] }
                },
                new Book
                {
                    Title = "The Road",
                    Author = "Cormac McCarthy",
                    PublisherId = publishers[0].Id,
                    Price = 10.99m,
                    PublicationDate = new DateTime(2006, 9, 26),
                    Active = true,
                    StockQuantity = 50,
                    Description = "A post-apocalyptic father-son journey.",
                    Image = "the_road.jpg",
                    Categories = new List<Category> { categories[0] }
                },
                new Book
                {
                    Title = "Atomic Habits",
                    Author = "James Clear",
                    PublisherId = publishers[1].Id,
                    Price = 13.99m,
                    PublicationDate = new DateTime(2018, 10, 16),
                    Active = true,
                    StockQuantity = 65,
                    Description = "A guide to building and breaking habits.",
                    Image = "atomic_habits.jpg",
                    Categories = new List<Category> { categories[1] }
                },
                new Book
                {
                    Title = "Snow Crash",
                    Author = "Neal Stephenson",
                    PublisherId = publishers[2].Id,
                    Price = 11.99m,
                    PublicationDate = new DateTime(1992, 6, 1),
                    Active = true,
                    StockQuantity = 55,
                    Description = "A cyberpunk adventure in a futuristic world.",
                    Image = "snow_crash.jpg",
                    Categories = new List<Category> { categories[2] }
                },
                new Book
                {
                    Title = "The Name of the Wind",
                    Author = "Patrick Rothfuss",
                    PublisherId = publishers[0].Id,
                    Price = 12.99m,
                    PublicationDate = new DateTime(2007, 3, 27),
                    Active = true,
                    StockQuantity = 70,
                    Description = "A fantasy epic of Kvothe’s life.",
                    Image = "name_of_the_wind.jpg",
                    Categories = new List<Category> { categories[0] }
                },
                new Book
                {
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    PublisherId = publishers[1].Id,
                    Price = 8.99m,
                    PublicationDate = new DateTime(1951, 7, 16),
                    Active = true,
                    StockQuantity = 60,
                    Description = "A story of teenage rebellion.",
                    Image = "catcher_rye.jpg",
                    Categories = new List<Category> { categories[0] }
                }
            };
            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();
        }    
    
    }
}