using AutoMapper;
using BookStore.Dtos;
using BookStore.Models;
using BookStore.Repository;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using BookStore.Services;

namespace BookStore.Service
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public RatingService(IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }
        public async Task<PaginatedList<RatingDto>> GetRatingByBookIdAsync(
            int bookId,
            int page,
            int pageSize,
            int? ratingValue = null,
            bool sortByDescending = false)
        {
            var pageList = await _ratingRepository.GetRatingsByBookIdAsync(
                bookId, page, pageSize, ratingValue, sortByDescending);
            var ratingDtos = _mapper.Map<List<RatingDto>>(pageList.Items);
            return new PaginatedList<RatingDto>(
                ratingDtos, pageList.TotalCount, pageList.PageIndex, pageList.PageSize);

        }
        public async Task<IEnumerable<RatingDto>> GetAllAsync()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<RatingDto> GetByIdAsync(int id)
        {
            var rating = await _ratingRepository.GetByIdAsync(id);
            if (rating == null)
            {
                throw new Exception("Rating not found.");
            }
            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<RatingDto> CreateAsync(RatingDto ratingDto)
        {
            var rating = _mapper.Map<Rating>(ratingDto);
            var createdRating = await _ratingRepository.CreateAsync(rating);
            if (createdRating == null)
            {
                throw new Exception("Failed to create Rating.");
            }
            return _mapper.Map<RatingDto>(createdRating);
        }

        public async Task<bool> UpdateAsync(RatingDto ratingDto, int id)
        {
            var checkRating = await _ratingRepository.GetByIdAsync(id);
            if (checkRating == null)
            {
                throw new Exception("Rating not found.");
            }

            var rating = _mapper.Map<Rating>(ratingDto);
            return await _ratingRepository.UpdateAsync(rating, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkRating = await _ratingRepository.GetByIdAsync(id);
            if (checkRating == null)
            {
                throw new Exception("Rating not found.");
            }

            return await _ratingRepository.DeleteAsync(id);
        }
        public async Task<Dictionary<int, int>> GetRatingCountByBookIdAsync(int bookId)
        {
            return await _ratingRepository.GetRatingCountByBookIdAsync(bookId);
        }
    }
}