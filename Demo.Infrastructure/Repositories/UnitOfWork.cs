using Demo.Application.Common.Interfaces;
using Demo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WhiteLagoonDbContext _dbContext;

        public IVillaRepository Villa { get; private set; }
        public IVillaNumberRepository VillaNumber { get; private set; }
        public IAmenityRepository Amenity { get; private set; }

        public UnitOfWork(WhiteLagoonDbContext dbContext)
        {
            _dbContext=dbContext;

            Villa = new VillaRepository(_dbContext);
            VillaNumber = new VillaNumberRepository(_dbContext);
            Amenity = new AmenityRepository(_dbContext);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
