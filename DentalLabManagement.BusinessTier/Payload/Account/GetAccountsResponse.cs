﻿using DentalLabManagement.BusinessTier.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalLabManagement.BusinessTier.Payload.Account
{
    public class GetAccountsResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public RoleEnum Role { get; set; }
        public string? CurrentStage { get; set; }
        public AccountStatus Status { get; set; }
        public GetAccountsResponse(int id, string username, string name, RoleEnum role, string? currentStage, AccountStatus status)
        {
            Id = id;
            Username = username;
            Name = name;
            Role = role;
            CurrentStage = currentStage;
            Status = status;
        }
    }

}
