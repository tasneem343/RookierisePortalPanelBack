using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICompanyManager
    {

        Task<Company> CreateCompanyAsync(Company company);

        Task SetPasswordAsync(string email, string passwordHash);

        Task<Company?> GetByEmailAsync(string email);
    }
}
