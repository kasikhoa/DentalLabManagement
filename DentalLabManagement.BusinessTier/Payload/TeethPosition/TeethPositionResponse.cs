using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.TeethPosition
{
    public class TeethPositionResponse
    {
        public int Id { get; set; }
        public ToothArch ToothArch { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }

        public TeethPositionResponse() { }

        public TeethPositionResponse(int id, ToothArch toothArch, string positionName, string description)
        {
            Id = id;
            ToothArch = toothArch;
            PositionName = positionName;
            Description = description;
        }
    }
}
