using AutoMapper;
using BookStore.Application.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BookStore.Application.Service;
using BookStore.Application.Entities;
using BookStore.Application.Dtos;

namespace BookStore.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepository = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookService = new BookService(_bookRepository.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task GetAllAsync_ReturnsMappedBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Price = 10 },
                new Book { Id = 2, Title = "Book 2", Price = 20 }
            };
            var bookDtos = new List<BookDto>
            {
                new BookDto { Id = 1, Title = "Book 1", Price = 10 },
                new BookDto { Id = 2, Title = "Book 2", Price = 20 }
            };
            _bookRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(It.IsAny<IEnumerable<Book>>())).Returns(bookDtos);
            // Act
            var result = await _bookService.GetAllAsync();
            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Book 1", result.First().Title);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsMappedBookDto()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book 1", Price = 10 };
            var bookDto = new BookDto { Id = 1, Title = "Book 1", Price = 10 };
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(bookDto);
            // Act
            var result = await _bookService.GetByIdAsync(1);
            // Assert
            Assert.Equal("Book 1", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ThrowsException()
        {
            // Arrange
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Book)null);
            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _bookService.GetByIdAsync(1));

            Assert.Equal("Book not found.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ValidDto_ReturnsCreatedBookDto()
        {
            // Arrange
            var bookDto = new BookDto { Title = "Book 1", Price = 10 };
            var book = new Book { Id = 1, Title = "Book 1", Price = 10 };
            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookDto>())).Returns(book);
            _bookRepository.Setup(repo => repo.CreateAsync(book)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(bookDto);
            // Act
            var result = await _bookService.CreateAsync(bookDto);
            // Assert
            Assert.Equal("Book 1", result.Title);
        }

        [Fact]
        public async Task CreateAsync_CreateReturnsNull_ThrowsException()

        {
            // Arrange
            var bookDto = new BookDto { Title = "Book 1", Price = 10 };
            var book = new Book { Id = 1, Title = "Book 1", Price = 10 };
            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookDto>())).Returns(book);
            _bookRepository.Setup(repo => repo.CreateAsync(book)).ReturnsAsync((Book)null); // Lỗi tạo book
            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _bookService.CreateAsync(bookDto));
            Assert.Equal("Failed to create book.", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_ValidBook_ReturnsTrue()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Title = "Book 1", Price = 10 };
            var book = new Book { Id = 1, Title = "Book 1", Price = 10 };
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<Book>(It.IsAny<BookDto>())).Returns(book);
            _bookRepository.Setup(repo => repo.UpdateAsync(book, 1)).ReturnsAsync(true);
            // Act
            var result = await _bookService.UpdateAsync(bookDto, 1);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_BookNotFound_ThrowsException()
        {
            // Arrange
            var bookDto = new BookDto { Id = 1, Title = "Book 1", Price = 10 };
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Book)null);
            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _bookService.UpdateAsync(bookDto, 1));
            Assert.Equal("Book not found.", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book 1", Price = 10 };
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(book);
            _bookRepository.Setup(repo => repo.DeactivateBookAsync(1)).ReturnsAsync(true);
            // Act
            var result = await _bookService.DeleteAsync(1);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_BookNotFound_ThrowsException()
        {
            // Arrange
            _bookRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Book?) null);
            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _bookService.DeleteAsync(1));
            Assert.Equal("Book not found.", ex.Message);
        }

        [Fact]
        public async Task GetCategoryByBookId_ValidBookId_ReturnsCategoryDtos()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Category 1" },
                new CategoryDto { Id = 2, Name = "Category 2" }
            };
            _bookRepository.Setup(repo => repo.GetCategoryByBookId(1)).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>())).Returns(categoryDtos);
            // Act
            var result = await _bookService.GetCategoryByBookId(1);
            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_WithPagination_ReturnsPaginatedBookDtos()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var category = "Fiction";
            var sortBy = "Title";
            var isDescending = false;
            var searchTerm = "book";

            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1" },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2" }
            };

            var paginatedBooks = new PaginatedList<Book>(books, totalCount: 50, pageIndex: page, pageSize: pageSize);

            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author
            }).ToList();

            _bookRepository.Setup(repo => repo.GetAllAsync(page, pageSize, category, sortBy, isDescending, searchTerm))
                           .ReturnsAsync(paginatedBooks);

            _mapperMock.Setup(m => m.Map<List<BookDto>>(books))
                   .Returns(bookDtos);

            // Act
            var result = await _bookService.GetAllAsync(page, pageSize, category, sortBy, isDescending, searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(50, result.TotalCount);
            Assert.Equal(page, result.PageIndex);
            Assert.Equal(pageSize, result.PageSize);
            Assert.True(result.HasNextPage);
            Assert.False(result.HasPreviousPage);
        }



    }
}
