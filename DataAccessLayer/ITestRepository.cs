using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public interface ITestRepository
    {
        IEnumerable<Truyen> GetAllTruyens();
    }
}
