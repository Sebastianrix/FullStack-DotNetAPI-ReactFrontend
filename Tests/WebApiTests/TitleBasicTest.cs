using cit13_portfolioproject2.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tests.WebApiTests
{
    public class TitleBasicTest
    {
        private const string TitleBasicApi = "http://localhost:5002/api/title";
        private static readonly HttpClient client = new HttpClient();
        [Fact]
        public async Task ApiTitleBasic_GetWithValidTConst_OkAndData()
        {
            string TConst = "tt1375666";
            var (titleBasic, statusCode) = await HelperTest.GetObject($"{TitleBasicApi}/{TConst}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.NotNull(titleBasic);
            Assert.Equal("Inception", titleBasic?["primaryTitle"]?.ToString());
        }
    }
}
