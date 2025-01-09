using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Dtos;
using BookStore.Models;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
            return bookDtos;
        }

        public async Task<BookDto> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new Exception("Book not found.");
            }
            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }

        public async Task<BookDto> CreateAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);

            var createdBook = await _bookRepository.CreateAsync(book);
            if (createdBook == null)
            {
                throw new Exception("Failed to create book.");
            }
            return _mapper.Map<BookDto>(createdBook);
        }

        public async Task<bool> UpdateAsync(BookDto bookDto, int id)
        {
            var checkBook = await _bookRepository.GetByIdAsync(id);
            if (checkBook == null)
            {
                throw new Exception("Book not found.");
            }

            var book = _mapper.Map<Book>(bookDto);
            return await _bookRepository.UpdateAsync(book, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkBook = await _bookRepository.GetByIdAsync(id);
            if (checkBook == null)
            {
                throw new Exception("Book not found.");
            }

            return await _bookRepository.DeactivateBookAsync(id);
        }

        public async Task<PaginatedList<BookDto>> GetAllAsync(int page, int pageSize, string? category = null, string? sortBy = null, bool isDescending = false, string? searchTerm = null)
        {
            var pageList = await _bookRepository.GetAllAsync(page, pageSize, category, sortBy, isDescending, searchTerm);
            var bookDtos =  _mapper.Map<List<BookDto>>(pageList.Items);
            return new PaginatedList<BookDto>(
                bookDtos, pageList.TotalCount, pageList.PageIndex, pageList.PageSize);

        }

        public async Task<IEnumerable<CategoryDto>> GetCategoryByBookId(int id)
        {
            var categories = await _bookRepository.GetCategoryByBookId(id);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<bool> JoinCategoryToBook(int bookId, int categoryId)
        {
            var check = await _bookRepository.JoinCategoryToBook(bookId, categoryId);
            if (check == false)
            {
                throw new Exception("Cant join category to book");
            }

            return check;
        }
    }
}
