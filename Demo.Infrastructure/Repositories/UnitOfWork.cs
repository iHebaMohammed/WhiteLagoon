using Demo.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IVillaRepository Villa => throw new NotImplementedException();

        public IVillaNumberRepository VillaNumber => throw new NotImplementedException();

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
