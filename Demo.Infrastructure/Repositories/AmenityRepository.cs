using Demo.Application.Common.Interfaces;
using Demo.Domain.Entities;
using Demo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Repositories
{
    public class AmenityRepository : GenaricRepository<Amenity>, IAmenityRepository
    {
        private readonly WhiteLagoonDbContext _dbContext;

        public AmenityRepository(WhiteLagoonDbContext dbContext) : base(dbContext)
        {
            _dbContext=dbContext;
        }

        public void Update(Amenity entity)
        {
            _dbContext.Amenities.Update(entity);
        }
    }
}
