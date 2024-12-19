using cit13_portfolioproject2.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tests.WebApiTests
{
    public class SearchControllerTest
    {
        private const string SearchApi = "http://localhost:5002/api/search";
        private static readonly HttpClient client = new HttpClient();
        [Fact]
        public async Task ApiSearch_GetWithValidNameSearch_OkAndSearch()
        {
            string searchName = "Reynolds";
            var (searchResult, statusCode) = await HelperTest.GetObject($"{SearchApi}/name/{searchName}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.NotNull(searchResult);
            int numberOfItems = searchResult?["numberOfItems"]?.GetValue<int>() ?? 0;
            Assert.Equal(168, numberOfItems);
        }

        [Fact]
        public async Task ApiSearch_GetWithValidTitleSearch_OkAndSearch()
        {
            string searchTitle = "Deadpool";
            var (searchResult, statusCode) = await HelperTest.GetObject($"{SearchApi}/title/{searchTitle}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.NotNull(searchResult);
            int numberOfItems = searchResult?["numberOfItems"]?.GetValue<int>() ?? 0;
            Assert.Equal(12, numberOfItems);
        }
    }
}
