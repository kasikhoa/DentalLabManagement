using DentalLabManagement.BusinessTier.Constants;
using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.Dental;
using DentalLabManagement.BusinessTier.Services.Interfaces;
using DentalLabManagement.BusinessTier.Utils;
using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Services.Implements
{
    public class DentalService : BaseService<IDentalService>, IDentalService
    {
        public DentalService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<DentalService> logger) : base(unitOfWork, logger)
        {

        }

        public async Task<DentalResponse> CreateDentalAccount(DentalRequest dentalRequest)
        {
            Dental newDental = await _unitOfWork.GetRepository<Dental>().SingleOrDefaultAsync
                (predicate: x => x.AccountId.Equals(dentalRequest.AccountId)); 
            if (newDental != null) throw new HttpRequestException(MessageConstant.Account.AccountExisted);
            Account account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync
                (predicate: x => x.Id.Equals(dentalRequest.AccountId));
            if (account == null) throw new HttpRequestException(MessageConstant.Account.AccountNotFoundMessage);
            if (account.Role.Equals(EnumUtil.GetDescriptionFromEnum(RoleEnum.Dental)))
            {
                newDental = new Dental()
                {
                    Name = dentalRequest.DentalName,
                    Address = dentalRequest.Address,
                    AccountId = dentalRequest.AccountId,
                };
            } else throw new HttpRequestException(MessageConstant.Account.CreateAccountWithWrongRoleMessage);

            await _unitOfWork.GetRepository<Dental>().InsertAsync(newDental);
            bool isSuccefful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccefful) return null;
            return new DentalResponse(newDental.Id, newDental.Name, newDental.Address, newDental.AccountId);

        }
    }
}
