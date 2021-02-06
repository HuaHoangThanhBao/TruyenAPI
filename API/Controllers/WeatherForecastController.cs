using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        public WeatherForecastController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            //var domesticAccounts = _repoWrapper.Account.FindByCondition(x => x.AccountType.Equals("Domestic"));
            var truyens = await _repoWrapper.Truyen.GetAllTruyensAsync();
            var tacGias = await _repoWrapper.TacGia.GetAllTacGiasAsync();
            var theLoais = await _repoWrapper.TheLoai.GetAllTheLoaisAsync();
            return new string[] {"SL Truyện: " + truyens.Count().ToString(), "SL Tác giả: " + tacGias.Count().ToString(), "SL thể loại: " + theLoais.Count().ToString()};
        }
    }
}
