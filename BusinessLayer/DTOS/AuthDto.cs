using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.DTOS
{
    public class AuthDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
