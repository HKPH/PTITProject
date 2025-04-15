using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.Entities;
using BookStore.Application.Interface.Repository;
using BookStore.Application.Interface.Service;

namespace BookStore.Application.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> GetByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                throw new Exception("Payment not found.");
            }
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<PaymentDto> CreateAsync(PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            var createdPayment = await _paymentRepository.CreateAsync(payment);
            if (createdPayment == null)
            {
                throw new Exception("Failed to create Payment.");
            }
            return _mapper.Map<PaymentDto>(createdPayment);
        }

        public async Task<bool> UpdateAsync(PaymentDto paymentDto, int id)
        {
            var checkPayment = await _paymentRepository.GetByIdAsync(id);
            if (checkPayment == null)
            {
                throw new Exception("Payment not found.");
            }

            var payment = _mapper.Map<Payment>(paymentDto);
            return await _paymentRepository.UpdateAsync(payment, id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var checkPayment = await _paymentRepository.GetByIdAsync(id);
            if (checkPayment == null)
            {
                throw new Exception("Payment not found.");
            }

            return await _paymentRepository.DeleteAsync(id);
        }

    }
}
