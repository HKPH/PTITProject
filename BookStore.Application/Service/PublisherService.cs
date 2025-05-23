﻿using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Interface.Service;

namespace BookStore.Application.Service
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;

        public PublisherService(IPublisherRepository publisherRepository, IMapper mapper)
        {
            _publisherRepository = publisherRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PublisherDto>> GetAllAsync()
        {
            var publishers = await _publisherRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
        }

        public async Task<PublisherDto> GetByIdAsync(int id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);
            if (publisher == null)
            {
                throw new Exception("Publisher not found.");
            }
            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto> CreateAsync(PublisherDto publisherDto)
        {
            var publisher = _mapper.Map<Publisher>(publisherDto);
            var createdPublisher = await _publisherRepository.CreateAsync(publisher);
            if (createdPublisher == null)
            {
                throw new Exception("Failed to create Publisher.");
            }
            return _mapper.Map<PublisherDto>(createdPublisher);
        }

        public async Task<bool> UpdateAsync(PublisherDto publisherDto, int id)
        {
            var checkPublisher = await _publisherRepository.GetByIdAsync(id);
            if (checkPublisher == null)
            {
                throw new Exception("Publisher not found.");
            }

            var publisher = _mapper.Map<Publisher>(publisherDto);
            return await _publisherRepository.UpdateAsync(publisher, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkPublisher = await _publisherRepository.GetByIdAsync(id);
            if (checkPublisher == null)
            {
                throw new Exception("Publisher not found.");
            }

            return await _publisherRepository.DeleteAsync(id);
        }
    }
}
