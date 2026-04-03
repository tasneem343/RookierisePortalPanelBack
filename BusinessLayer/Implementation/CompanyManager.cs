using BusinessLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Implementation
{

        public class CompanyManager : ICompanyManager
        {
            private readonly ApplicationDbContext _context;

            public CompanyManager(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Company> CreateCompanyAsync(Company company)
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                return company;
            }

            public async Task SetPasswordAsync(string email, string passwordHash)
            {
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.Email == email);
                if (company == null) throw new KeyNotFoundException("Company not found");

                company.PasswordHash = passwordHash;
                _context.Companies.Update(company);
                await _context.SaveChangesAsync();
            }

            public async Task<Company?> GetByEmailAsync(string email)
            {
                return await _context.Companies.FirstOrDefaultAsync(c => c.Email == email);
            }
        }
    
}
