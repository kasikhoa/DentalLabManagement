using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.API.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Paginate;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DentalLabManagement.API.Extensions;
using DentalLabManagement.BusinessTier.Payload.Order;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DentalLabManagement.BusinessTier.Payload.Payment;
using DentalLabManagement.BusinessTier.Payload.Product;
using DentalLabManagement.BusinessTier.Payload.TeethPosition;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;

namespace DentalLabManagement.API.Services.Implements
{
    public class DentalService : BaseService<IDentalService>, IDentalService
    {
        public DentalService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<DentalService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<DentalResponse> CreateDentalAccount(DentalRequest dentalRequest)
        {
            if (dentalRequest.AccountId < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Dental newDental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync
                (predicate: x => x.AccountId.Equals(dentalRequest.AccountId)); 
            if (newDental != null) throw new BadHttpRequestException(MessageConstant.Account.AccountExisted);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync
                (predicate: x => x.Id.Equals(dentalRequest.AccountId));
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            if (account.Role.Equals(RoleEnum.Dental.GetDescriptionFromEnum()))
            {
                newDental = new Dental()
                {
                    Name = dentalRequest.DentalName,
                    Address = dentalRequest.Address,
                    Status = DentalStatus.Active.GetDescriptionFromEnum(),
                    AccountId = dentalRequest.AccountId,                  
                };
            } 
            else throw new BadHttpRequestException(MessageConstant.Account.CreateAccountWithWrongRoleMessage);

            await _unitOfWork.GetRepository<Dental>().InsertAsync(newDental);
            bool isSuccefful = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccefful) throw new BadHttpRequestException(MessageConstant.Dental.CreateDentalFailed);
            return new DentalResponse(newDental.Id, newDental.Name, newDental.Address,
                EnumUtil.ParseEnum<DentalStatus>(newDental.Status), newDental.AccountId);
        }

        public async Task<DentalAccountResponse> GetAccountDentalById(int dentalId)
        {
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);
            
            Dental dental = await _unitOfWork.GetRepository<Dental>()
                .SingleOrDefaultAsync(predicate: x => x.Id.Equals(dentalId));
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            Account dentalAccount = await _unitOfWork.GetRepository<Account>().
                SingleOrDefaultAsync(predicate: x => x.Id.Equals(dental.AccountId));
            if (dentalAccount == null) throw new BadHttpRequestException(MessageConstant.Dental.AccountDentalNotFoundMessage);

            return new DentalAccountResponse(dental.Id, dental.Name, dental.Address, dentalAccount.UserName, 
                EnumUtil.ParseEnum<AccountStatus>(dentalAccount.Status));

        }
   
        private Expression<Func<Dental, bool>> BuildGetDentalsQuery(string? searchDentalName, DentalStatus? status)
        {
            Expression<Func<Dental, bool>> filterQuery = x => true; 

            if (!string.IsNullOrEmpty(searchDentalName))
            {
                filterQuery = filterQuery.AndAlso(x => x.Name.Contains(searchDentalName));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(x => x.Status.Equals(status.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }


        public async Task<IPaginate<DentalResponse>> GetDentals(string? searchDentalName, DentalStatus? status, int page, int size)
        {
            searchDentalName = searchDentalName?.Trim().ToLower();
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            IPaginate<DentalResponse> response = await _unitOfWork.GetRepository<Dental>().GetPagingListAsync(
                selector: x => new DentalResponse(x.Id, x.Name, x.Address, EnumUtil.ParseEnum<DentalStatus>(x.Status), x.AccountId),
                predicate: BuildGetDentalsQuery(searchDentalName, status),
                page: page,
                size: size
                );
            return response;
        }

        public async Task<DentalResponse> UpdateDentalInfo(int dentalId, UpdateDentalRequest updateDentalRequest)
        {
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);

            Dental updateDental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dentalId));
            if (updateDental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            updateDentalRequest.TrimString();

            updateDental.Name = string.IsNullOrEmpty(updateDentalRequest.Name) ? updateDental.Name : updateDentalRequest.Name;
            updateDental.Address = string.IsNullOrEmpty(updateDentalRequest.Address) ? updateDental.Address : updateDentalRequest.Address;
            updateDental.Status = updateDentalRequest.Status.GetDescriptionFromEnum();

            Account dentalAccount = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(updateDental.AccountId));
            if (dentalAccount != null && updateDental.Status.Equals(DentalStatus.Active.GetDescriptionFromEnum()))
            {
                dentalAccount.Status = AccountStatus.Activate.GetDescriptionFromEnum();
            }
            if (dentalAccount != null && updateDental.Status.Equals(DentalStatus.Inactive.GetDescriptionFromEnum()))
            {
                dentalAccount.Status = AccountStatus.Deactivate.GetDescriptionFromEnum();
            }

            _unitOfWork.GetRepository<Dental>().UpdateAsync(updateDental);
            _unitOfWork.GetRepository<Account>().UpdateAsync(dentalAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful) throw new BadHttpRequestException(MessageConstant.Dental.UpdateDentalFailedMessage);
            return new DentalResponse(updateDental.Id, updateDental.Name, updateDental.Address, 
                EnumUtil.ParseEnum<DentalStatus>(updateDental.Status), updateDental.AccountId);

        }

        public async Task<bool> UpdateDentalStatus(int id)
        {
            if (id < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);

            Dental dental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id));
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            dental.Status = DentalStatus.Inactive.GetDescriptionFromEnum();

            Account accountDental = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dental.AccountId));

            if (accountDental != null && dental.Status.Equals(DentalStatus.Inactive.GetDescriptionFromEnum()))
            {
                accountDental.Status = AccountStatus.Deactivate.GetDescriptionFromEnum();
            }
            _unitOfWork.GetRepository<Dental>().UpdateAsync(dental);
            _unitOfWork.GetRepository<Account>().UpdateAsync(accountDental);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        private Expression<Func<Order, bool>> BuildGetOrdersQuery(string? InvoiceId, OrderMode? mode, OrderStatus? status, OrderPaymentStatus? paymentStatus)
        {
            Expression<Func<Order, bool>> filterQuery = p => true;

            if (!string.IsNullOrEmpty(InvoiceId))
            {
                filterQuery = filterQuery.AndAlso(p => p.InvoiceId.Contains(InvoiceId));
            }

            if (mode != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Mode.Equals(mode.GetDescriptionFromEnum()));
            }

            if (status != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.Status.Equals(status.GetDescriptionFromEnum()));
            }

            if (paymentStatus != null)
            {
                filterQuery = filterQuery.AndAlso(p => p.PaymentStatus.Equals(paymentStatus.GetDescriptionFromEnum()));
            }

            return filterQuery;
        }

        public async Task<IPaginate<GetOrdersResponse>> GetOrderDetails(int dentalId, string? InvoiceId, OrderMode? mode, OrderStatus? status, 
            OrderPaymentStatus? paymentStatus, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);
            Dental dental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dentalId)
                );
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            Order order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: x => x.DentalId.Equals(dentalId)
                );
            if (order == null) throw new BadHttpRequestException(MessageConstant.Order.EmptyOrderMessage);

            string updateBy = (order.UpdatedByNavigation != null) ? order.UpdatedByNavigation.FullName : null;

            IPaginate<GetOrdersResponse> orderList = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: x => new GetOrdersResponse()
                {
                    Id = x.Id,
                    InvoiceId = x.InvoiceId,
                    DentalName = x.Dental.Name,
                    DentistName = x.DentistName,
                    DentistNote = x.DentistNote,
                    PatientName = x.PatientName,
                    PatientGender = EnumUtil.ParseEnum<PatientGender>(x.PatientGender),
                    PatientPhoneNumber = x.PatientPhoneNumber,
                    Status = EnumUtil.ParseEnum<OrderStatus>(x.Status),
                    Mode = EnumUtil.ParseEnum<OrderMode>(x.Mode),
                    TeethQuantity = x.TeethQuantity,
                    TotalAmount = x.TotalAmount,
                    Discount = x.Discount,
                    FinalAmount = x.FinalAmount,
                    CreatedDate = x.CreatedDate,
                    CompletedDate = x.CompletedDate,
                    UpdatedBy = x.UpdatedByNavigation.FullName,
                    Note = x.Note,
                    PaymentStatus = EnumUtil.ParseEnum<OrderPaymentStatus>(x.PaymentStatus),
                },
                predicate: BuildGetOrdersQuery(InvoiceId, mode, status, paymentStatus),
                orderBy: x => x.OrderBy(x => x.InvoiceId),
                page: page,
                size: size
            );
            return orderList;
        }
    }
}
