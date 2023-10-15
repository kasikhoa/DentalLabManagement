using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.TeethPosition
{
    public class TeethPositionRequest
    {
        public ToothArch ToothArch { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
    }
}
