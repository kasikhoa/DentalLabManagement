using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.TeethPosition
{
    public class UpdateTeethPositionRequest
    {
        [EnumDataType(typeof(ToothArch))]
        public ToothArch ToothArch { get; set; }
        public string? PositionName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

        public void TrimString()
        {
            PositionName = PositionName?.Trim();
            Description = Description?.Trim();
            Image = Image?.Trim();
        }
    }
}
