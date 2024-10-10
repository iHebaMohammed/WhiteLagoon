using Demo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Common.Interfaces
{
    public interface IVillaRepository : IGenaricRepository<Villa>
    {
        void Update(Villa entity);
    }
}
