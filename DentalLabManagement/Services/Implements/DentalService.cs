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
using AutoMapper;

namespace DentalLabManagement.API.Services.Implements
{
    public class DentalService : BaseService<IDentalService>, IDentalService
    {
        public DentalService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<IDentalService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<DentalResponse> CreateDentalAccount(DentalRequest dentalRequest)
        {
            if (dentalRequest.AccountId < 1) throw new BadHttpRequestException(MessageConstant.Account.EmptyAccountIdMessage);

            Dental newDental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync
                (predicate: x => x.AccountId.Equals(dentalRequest.AccountId)
                ); 
            if (newDental != null) throw new BadHttpRequestException(MessageConstant.Account.AccountExisted);

            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync
                (predicate: x => x.Id.Equals(dentalRequest.AccountId)
                );
            if (account == null) throw new BadHttpRequestException(MessageConstant.Account.AccountNotFoundMessage);

            if (account.Role.Equals(RoleEnum.Dental.GetDescriptionFromEnum()))
            {
                newDental = new Dental()
                {
                    Name = dentalRequest.DentalName,
                    Address = dentalRequest.Address,
                    Status = DentalStatus.Active.GetDescriptionFromEnum(),
                    AccountId = dentalRequest?.AccountId,                  
                };
            } 
            else throw new BadHttpRequestException(MessageConstant.Account.CreateAccountWithWrongRoleMessage);

            await _unitOfWork.GetRepository<Dental>().InsertAsync(newDental);
            bool isSuccefful = await _unitOfWork.CommitAsync() > 0;

            if (!isSuccefful) throw new BadHttpRequestException(MessageConstant.Dental.CreateDentalFailed);
            return new DentalResponse(newDental.Id, newDental.Name, newDental.Address,
                EnumUtil.ParseEnum<DentalStatus>(newDental.Status), account.UserName);
        }

        public async Task<DentalResponse> GetDentalById(int dentalId)
        {
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);
            
            Dental dental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dentalId),
                include: x => x.Include(x => x.Account)
                );
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            return new DentalResponse(dental.Id, dental.Name, dental.Address, 
                EnumUtil.ParseEnum<DentalStatus>(dental.Status), dental.Account?.UserName);

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
                selector: x => new DentalResponse(x.Id, x.Name, x.Address, EnumUtil.ParseEnum<DentalStatus>(x.Status), x.Account.UserName),
                predicate: BuildGetDentalsQuery(searchDentalName, status),
                page: page,
                size: size
                );
            return response;
        }

        public async Task<bool> UpdateDentalInfo(int dentalId, UpdateDentalRequest request)
        {
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);

            Dental updateDental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dentalId));
            if (updateDental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

            request.TrimString();

            updateDental.Name = string.IsNullOrEmpty(request.Name) ? updateDental.Name : request.Name;
            updateDental.Address = string.IsNullOrEmpty(request.Address) ? updateDental.Address : request.Address;
            updateDental.Status = request.Status.GetDescriptionFromEnum();

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
            return isSuccessful;

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

        private Expression<Func<Order, bool>> BuildGetOrdersQuery(int dentalId, string? InvoiceId, 
            OrderStatus? status, OrderPaymentStatus? paymentStatus)
        {
            Expression<Func<Order, bool>> filterQuery = p => true;

            filterQuery = filterQuery.AndAlso(p => p.DentalId.Equals(dentalId));

            if (!string.IsNullOrEmpty(InvoiceId))
            {
                filterQuery = filterQuery.AndAlso(p => p.InvoiceId.Contains(InvoiceId));
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

        public async Task<IPaginate<GetOrdersResponse>> GetOrderDetails(int dentalId, string? InvoiceId, OrderStatus? status, 
            OrderPaymentStatus? paymentStatus, int page, int size)
        {
            page = (page == 0) ? 1 : page;
            size = (size == 0) ? 10 : size;
            if (dentalId < 1) throw new BadHttpRequestException(MessageConstant.Dental.EmptyDentalId);

            Dental dental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(dentalId)
                );
            if (dental == null) throw new BadHttpRequestException(MessageConstant.Dental.DentalNotFoundMessage);

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
                    TeethQuantity = x.TeethQuantity,
                    TotalAmount = x.TotalAmount,
                    Discount = x.Discount,
                    FinalAmount = x.FinalAmount,
                    CreatedDate = x.CreatedDate,
                    CompletedDate = x.CompletedDate,
                    Note = x.Note,
                    PaymentStatus = EnumUtil.ParseEnum<OrderPaymentStatus>(x.PaymentStatus),
                },
                predicate: BuildGetOrdersQuery(dentalId, InvoiceId, status, paymentStatus),
                orderBy: x => x.OrderBy(x => x.InvoiceId),
                page: page,
                size: size
            );
            return orderList;
        }
    }
}
