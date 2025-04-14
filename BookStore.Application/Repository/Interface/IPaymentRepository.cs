using BookStore.Application.Repository.Interface;
using BookStore.Domain.Entities;
namespace BookStore.Application.Repository.Interface
{
    public interface IPaymentRepository: IRepository<Payment>
    {
    }
}
