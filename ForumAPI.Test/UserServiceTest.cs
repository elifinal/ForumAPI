using ForumAPI.Contract.UserContract;
using ForumAPI.Data.Abstract;
using ForumAPI.Data.Entity;
using ForumAPI.Service.Abstract;
using ForumAPI.Service.Concrete;
using Moq;

namespace ForumAPI.Test
{
    public class UserServiceTest : TestBase
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userRepository;
        public UserServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(_userRepository.Object, _mapper);
        }

        [Fact]
        public async void AddUserAsyncTest()
        {
            // arrange
            var mockUser = new AddUserContract
            {
                Name = "Elif",
                Surname = "Ýnal",
                Email = "elif@gmail.com",
                Password = "elif",
                ConfirmPassword = "elif"
            };
            var mockDbUser = new User
            {
                Name = "Elif",
                Surname = "Ýnal",
                Email = "x@gmail.com",
                Password = "elif"
            };

            // act
            _userRepository.Setup(x => x.AddAsync(It.IsAny<User>()));
            _userRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(Task.FromResult<User>(null));
            await _userService.AddUserAsync(mockUser);
            // assert

        }
    }
}