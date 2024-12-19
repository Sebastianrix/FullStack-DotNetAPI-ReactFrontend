using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;
using cit13_portfolioproject2.Tests;
using Newtonsoft.Json.Linq;

namespace Tests.WebApiTests
{
    public class UserControllerTests
    {
        private const string UsersApi = "http://localhost:5002/api/v3/user";
        private static readonly HttpClient client = new HttpClient();

        /* /api/user */

        [Fact]
        public async Task ApiUsers_GetUserWithValidId_OkAndUserDetails()
        {
            // Creating a user for test
            var newUser = new
            {
                Username = "Bree",
                Password = "password123",
                Email = "normal@example.com",
                Role = "user",
                Name = "Bree Sunshiner"
            };
            await HelperTest.PostData($"{UsersApi}/register", newUser);
            string username = "Bree";
            var (_user, _statusCode) = await HelperTest.GetObject($"{UsersApi}/username/{username}");



            string? id = _user?["id"]?.ToString();
            if (id != null)
            {

                var (user, statusCode) = await HelperTest.GetObject($"{UsersApi}/{id}");
                Assert.Equal(HttpStatusCode.OK, statusCode);
                Assert.NotNull(user);
                Assert.Equal(id, user?["id"]?.ToString());

                //Clean up after test
                await HelperTest.DeleteData($"{UsersApi}/{id}");
            }
        }

        [Fact]
        public async Task ApiUsers_GetUserWithInvalidId_NotFound()
        {
            int userId = 999;
            var (_, statusCode) = await HelperTest.GetObject($"{UsersApi}/{userId}");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task ApiUsers_GetUserByUsernameWithValidUsername_OkAndUserDetails()
        {
            // Creating a user for test
            var newUser = new
            {
                Username = "john_doe",
                Password = "password123",
                Email = "todelete@example.com",
                Role = "user",
                Name = "testter"
            };
            await HelperTest.PostData($"{UsersApi}/register", newUser);



            string username = "john_doe";
            var (user, statusCode) = await HelperTest.GetObject($"{UsersApi}/username/{username}");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.NotNull(user);
            Assert.Equal(username, user?.Value("username"));

            /// Clean up after test
            string? id = user?["id"]?.ToString();
            if (id != null)
            {
                await HelperTest.DeleteData($"{UsersApi}/{id}");
            }
        }

        [Fact]
        public async Task ApiUsers_GetUserByUsernameWithInvalidUsername_NotFound()
        {
            string username = "invaliduser";
            var (_, statusCode) = await HelperTest.GetObject($"{UsersApi}/username/{username}");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task ApiUsers_RegisterUser_ValidData_Created()
        {
            var newUser = new
            {
                Username = "newusertest",
                Password = "password123",
                Email = "newuser@example.com",
                Name = "test",
                Role = "user",
            };
            var (user, statusCode) = await HelperTest.PostData($"{UsersApi}/register", newUser);

            Assert.Equal(HttpStatusCode.Created, statusCode);
            Assert.NotNull(user);
            Assert.Equal(newUser.Username, user?.Value("username"));
            Assert.Equal(newUser.Email, user?.Value("email"));

            // Clean up after test
            string? id = user?["id"]?.ToString();
            if (id != null)
            {
                await HelperTest.DeleteData($"{UsersApi}/{id}");
            }
        }

        [Fact]
        public async Task ApiUsers_RegisterUser_InvalidData_BadRequest()
        {
            var invalidUser = new
            {
                Username = "",
                Password = "123",
                Email = "invalidemail",
                Role = "invalidrole",
                Name = "Perry"

            };
            var (_, statusCode) = await HelperTest.PostData($"{UsersApi}/register", invalidUser);

            Assert.Equal(HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task ApiUsers_DeleteUserWithValidId_NoContent()
        {
            var newUser = new
            {
                Username = "login_test_user",
                Password = "loginPassword123",
                Email = "login_test@example.com",
                Role = "admin",
                Name = "Test User"
            };

            var loginCredentials = new
            {
                UserName = "login_test_user",
                Password = "loginPassword123",
                Email = "login_test@example.com"
            };

            var (user, _) = await HelperTest.PostData($"{UsersApi}/register", newUser);

            var (loginResponseContent, loginStatusCode) = await HelperTest.PutData($"{UsersApi}", loginCredentials);
            var token = loginResponseContent["token"]?.ToString();
            HelperTest.SetAuthorizationHeader(token);

           

            // delete user
            string? id = user?["id"]?.ToString();

            if (id != null)
            {
                var statusCode = await HelperTest.DeleteData($"{UsersApi}/{id}");
                Assert.Equal(HttpStatusCode.NoContent, statusCode);
            }

        }

        [Fact]
        public async Task ApiUsers_DeleteUserWithInvalidId_NotFound()
        {
            var newUser = new
            {
                Username = "login_test_user",
                Password = "loginPassword123",
                Email = "login_test@example.com",
                Role = "admin",
                Name = "Test User"
            };
            await HelperTest.PostData($"{UsersApi}/register", newUser);

            var loginCredentials = new
            {
                UserName = "login_test_user",
                Password = "loginPassword123",
                Email = "login_test@example.com"
            };

            var (loginResponseContent, loginStatusCode) = await HelperTest.PutData($"{UsersApi}", loginCredentials);
            var token = loginResponseContent["token"]?.ToString();
            HelperTest.SetAuthorizationHeader(token);
      
            var statusCode = await HelperTest.DeleteData($"{UsersApi}/999");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);

            // Cleanup
            string? id = (await HelperTest.GetObject($"{UsersApi}/username/{newUser.Username}")).Item1?["id"]?.ToString();
            if (id != null)
            {
                await HelperTest.DeleteData($"{UsersApi}/{id}");
            }


        }

        [Fact]
        public async Task ApiUsers_Login_ValidCredentials_ReturnsToken()
        {
            var newUser = new
            {
                Username = "login_test_user",
                Password = "loginPassword123",
                Email = "login_test@example.com",
                Role = "admin",
                Name = "Test User"
            };
            await HelperTest.PostData($"{UsersApi}/register", newUser);

            var loginCredentials = new
            {
                UserName = "login_test_user",
                Password = "loginPassword123",
                Email = "login_test@example.com"
            };

            var (responseContent, statusCode) = await HelperTest.PutData($"{UsersApi}", loginCredentials);

            // Check if login was successful and a JWT token is returned
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.NotNull(responseContent);
            Assert.True(responseContent?["token"] != null);

            // Cleanup
            string? id = (await HelperTest.GetObject($"{UsersApi}/username/{newUser.Username}")).Item1?["id"]?.ToString();
            if (id != null)
            {
                await HelperTest.DeleteData($"{UsersApi}/{id}");
            }
        }


    }
}