using DentalLabManagement.BusinessTier.Enums;
using DentalLabManagement.BusinessTier.Payload.TeethPosition;
using DentalLabManagement.DataTier.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.API.Services.Interfaces
{
    public interface ITeethPositionServices
    {
        Task<TeethPositionResponse> CreateTeethPosition(TeethPositionRequest teethPositionRequest);
        Task<IPaginate<TeethPositionResponse>> GetTeethPositions(TeethPositionFilter filter, int page, int size);
        Task<TeethPositionResponse> GetTeethPositionById (int id);
        Task<bool> UpdateTeethPosition(int id, UpdateTeethPositionRequest request);
    }
}
