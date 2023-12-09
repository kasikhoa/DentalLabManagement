using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.TeethPosition
{
    public class TeethPositionFilter
    {
        public string? positionName { get; set; }
        public ToothArch? toothArch { get; set; }
    }
}
