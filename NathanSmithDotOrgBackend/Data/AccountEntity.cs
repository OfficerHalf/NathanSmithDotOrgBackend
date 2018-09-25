using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NathanSmithDotOrgBackend.Data
{    public class AccountEntity
    {
        [Key]
        public string UserId { get; set; }
        public string Username { get; set; }
        public string SaltyHash { get; set; }
    }
}
