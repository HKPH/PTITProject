using BookStore.Application.Dtos;
using BookStore.Application.Service.Interface;

namespace BookStore.Application.Service.Interface
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDto>> GetAllAsync();
        Task<PublisherDto> GetByIdAsync(int id);
        Task<PublisherDto> CreateAsync(PublisherDto publisherDto);
        Task<bool> UpdateAsync(PublisherDto publisherDto, int id);
        Task<bool> DeleteAsync(int id);
    }
}
