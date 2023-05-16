using System;
using Microsoft.EntityFrameworkCore;

namespace getQuote.DAO
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {}
    }
}

