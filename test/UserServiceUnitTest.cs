using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.Services;
using DataLibrary.ViewModels;
using Moq;

namespace test
{
    public class Tests
    {
        UserService _userservice;

        [SetUp]
        public void Setup()
        {
            var appUserDal = new Mock<IUserDAL>();
            IEnumerable<IUser> userList = new List<IUser>
            {
                new User {
                    UserID = 1,
                    Email = "ashley.kanhye@ceridian.com",
                    Password = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4",
                    FirstName = "nikheel",
                    LastName = "kanye", 
                    NIC = "K271101002337C",
                    IsActive=1,
                    PhoneNo=57961026,
                    ManagerID=1,
                },
                new User {
                    UserID = 2,
                    Email = "hansleylouis.eleonore@ceridian.com",
                    Password = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4",
                    FirstName = "hansley",
                    LastName = "eleonore",
                    NIC = "H271101002337C",
                    IsActive=1,
                    PhoneNo=53931153,
                    ManagerID=1,
                },
                new User {
                    UserID = 3,
                    Email = "aditya.chandoo@ceridian.com",
                    Password = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4",
                    FirstName = "aditya",
                    LastName = "sooklal",
                    NIC = "A2711930023C",
                    IsActive=1,
                    PhoneNo=57931152,
                    ManagerID=1,
                },
            };




            appUserDal.Setup(s=> s.GetAll()).Returns(Task.FromResult(userList));
            _userservice = new UserService(appUserDal.Object, null, null, null);


            appUserDal.Setup(s => s.Find(It.IsAny<string>()))
                .Returns((string inputEmail) =>
                {
                    return userList.Where(user => user.Email == inputEmail).FirstOrDefault();
                });

            appUserDal.Setup(s=> s.GetTotalNumberOfUserRecords()).Returns(Task.FromResult(userList.Count()));

            appUserDal.Setup(s => s.CheckUserExists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns((string inputEmail,string NIC , int phoneNo) =>
            {
                return userList.Any(user => user.Email == inputEmail) || userList.Any(user=>user.NIC == NIC) || userList.Any(user=>user.PhoneNo == phoneNo);
            });
            
            
        }

        [Test]
        [TestCase("ashley.kanhye@ceridian.com","1234")]
        [TestCase("aditya.chandoo@ceridian.com", "1234")]
        [TestCase("hansleylouis.eleonore@ceridian.com", "1234")]
        public void TestLoginSuccess(string userName,string password)
        {
            var inputUser = new LoginUserViewModel();
            inputUser.Email = userName;
            inputUser.Password = password;
            var loginIUserResponse = _userservice.Authenticate(inputUser);
            Assert.AreEqual(inputUser.Email, loginIUserResponse.Email);
        }

        [Test]
        [TestCase("ashley.kanhye@ceridian.com", "dsfod")]
        [TestCase("aditya.chandoo@ceridian.com", "safadf")]
        [TestCase("hansleylouis.eleonore@ceridian.com", "asdfdsaf")]
        public void TestLoginUnsucessful(string userName,string password)
        {
            var inputUser = new LoginUserViewModel();
            inputUser.Email = userName;
            inputUser.Password = password;
            var loginIUserResponse = _userservice.Authenticate(inputUser);
            Assert.AreEqual(null, loginIUserResponse);
        }



        [Test]
        public async Task TestGetTotalNumberOfUserRecords()
        {
            int numofRecords = await _userservice.GetTotalNumberOfUserRecords();
            Assert.AreEqual(3, numofRecords);
        }

        [Test]
        [TestCase("ashley.kanhye@ceridian.com","dsfdsaf",2345678)]
        [TestCase("dfdsfdsaf","sdfdsafdsafsa",57961026)]
        [TestCase("dsfdsfasdf", "A2711930023C",3434)]
        public void TestCheckUserExist(string inputEmail,string NIC, int PhoneNo)
        {
            var user = new CheckUserExistViewModel()
            {
                Email = inputEmail,
                PhoneNo = PhoneNo,
                NIC = NIC
            };
            var result = _userservice.CheckUserExist(user);
            Assert.AreEqual(true, result);
        }


        [Test]
        [TestCase("lenovo.thinkpad@ceridian.com", "k43432435324", 57931151)]
        public void TestCheckUserDoesNotExist(string inputEmail, string NIC, int PhoneNo)
        {
            var user = new CheckUserExistViewModel()
            {
                Email = inputEmail,
                PhoneNo = PhoneNo,
                NIC = NIC
            };
            var result = _userservice.CheckUserExist(user);
            Assert.AreEqual(false, result);
        }



    }
}