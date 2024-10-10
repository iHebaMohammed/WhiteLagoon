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
    public class VillaRepository : GenaricRepository<Villa>, IVillaRepository
    {
        private readonly WhiteLagoonDbContext _db;

        public VillaRepository(WhiteLagoonDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Villa entity)
        {
            _db.Villas.Update(entity);
        }
    }
}
