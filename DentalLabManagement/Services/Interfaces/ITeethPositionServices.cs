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
        public Task<TeethPositionResponse> CreateTeethPosition(TeethPositionRequest teethPositionRequest);
        public Task<IPaginate<TeethPositionResponse>> GetTeethPositions(ToothArch? toothArch, int page, int size);
        public Task<TeethPositionResponse> GetTeethPositionById (int id);
        public Task<TeethPositionResponse> UpdateTeethPosition(int id, UpdateTeethPositionRequest updateTeethPositionRequest);
    }
}
