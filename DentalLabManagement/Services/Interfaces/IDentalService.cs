using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.BusinessTier.Payload.Order;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface IDentalService
    {
        Task<DentalResponse> CreateDentalAccount(DentalRequest dentalRequest);
        Task<DentalResponse> GetDentalById(int dentalId);
        Task<IPaginate<DentalResponse>> GetDentals(string? name, DentalStatus? status, int page, int size);
        Task<bool> UpdateDentalInfo(int id, UpdateDentalRequest request);       
        Task<bool> UpdateDentalStatus(int id);
        Task<IPaginate<GetOrdersResponse>> GetOrderDetails(int dentalId, string? InvoiceId, OrderStatus? status,
            OrderPaymentStatus? paymentStatus, int page, int size);
    }
}
