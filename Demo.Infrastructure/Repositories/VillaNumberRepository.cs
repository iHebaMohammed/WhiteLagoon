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
    public class VillaNumberRepository : GenaricRepository<VillaNumber>, IVillaNumberRepository
    {
        private readonly WhiteLagoonDbContext _db;

        public VillaNumberRepository(WhiteLagoonDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(VillaNumber entity)
        {
            _db.VillaNumbers.Update(entity);
        }
    }
}
