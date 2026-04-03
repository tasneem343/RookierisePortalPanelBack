using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.DTOS
{
    public class CompanySignUpDto
    {
        public string CompanyNameArabic { get; set; } = null!;
        public string CompanyNameEnglish { get; set; } = null!;
        public string WebsiteUrl { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
        public string? CompanyLogo { get; set; }

    }
}
