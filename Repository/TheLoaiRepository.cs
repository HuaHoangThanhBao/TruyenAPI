using Contracts;
using CoreLibrary;
using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class TheLoaiRepository : RepositoryBase<TheLoai>, ITheLoaiRepository
    {
        public TheLoaiRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<TheLoai> AccountsByOwner(int ownerId)
        {
            return FindByCondition(a => a.TheLoaiID.Equals(ownerId)).ToList();
        }
    }
}
