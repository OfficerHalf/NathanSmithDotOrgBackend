using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace NathanSmithDotOrgBackend.Data
{    public class AccountEntity
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string SaltyHash { get; set; }
    }
}
