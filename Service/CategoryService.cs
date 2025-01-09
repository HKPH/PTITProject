using AutoMapper;
using BookStore.Dtos;
using BookStore.Models;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;

namespace BookStore.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categorys = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categorys);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found.");
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var createdCategory = await _categoryRepository.CreateAsync(category);
            if (createdCategory == null)
            {
                throw new Exception("Failed to create category.");
            }
            return _mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<bool> UpdateAsync(CategoryDto categoryDto, int id)
        {
            var checkCategory = await _categoryRepository.GetByIdAsync(id);
            if (checkCategory == null)
            {
                throw new Exception("Category not found.");
            }

            var category = _mapper.Map<Category>(categoryDto);
            return await _categoryRepository.UpdateAsync(category, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkCategory = await _categoryRepository.GetByIdAsync(id);
            if (checkCategory == null)
            {
                throw new Exception("Category not found.");
            }

            return await _categoryRepository.DeleteAsync(id);
        }
    }
}
