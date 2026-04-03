using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace DataAccessLayer.Entities
{
    [Index(nameof(Email), IsUnique = true)]

    public class Company
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string CompanyNameArabic { get; set; } = null!;
        public string CompanyNameEnglish { get; set; } = null!;
        public string WebsiteUrl { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string? CompanyLogo { get; set; }
    }
}
