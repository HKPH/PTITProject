using BookStore.Application.Dtos;


namespace BookStore.Application.Service.Interface
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto> GetByIdAsync(int id);
        Task<PaymentDto> CreateAsync(PaymentDto paymentDto);
        Task<bool> UpdateAsync(PaymentDto paymentDto, int id);
        Task<bool> DeleteAsync(int id);
    }
}
