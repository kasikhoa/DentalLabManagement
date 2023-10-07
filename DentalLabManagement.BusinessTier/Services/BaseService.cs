using DentalLabManagement.DataTier.Models;
using DentalLabManagement.DataTier.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;


namespace DentalLabManagement.BusinessTier.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<DentalLabManagementContext> _unitOfWork;
        protected ILogger<T> _logger;

        public BaseService(IUnitOfWork<DentalLabManagementContext> unitOfWork, ILogger<T> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;

        }


        //Use for employee and store manage
    }
}
