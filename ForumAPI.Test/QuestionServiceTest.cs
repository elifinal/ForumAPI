using ForumAPI.Cache.Interfaces;
using ForumAPI.Contract.AnswerContract;
using ForumAPI.Contract.QuestionContract;
using ForumAPI.Contract.UserContract;
using ForumAPI.Data.Abstract;
using ForumAPI.Data.Entity;
using ForumAPI.Service.Abstract;
using ForumAPI.Service.Concrete;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NuGet.Frameworks;

namespace ForumAPI.Test
{
    public class QuestionServiceTest : TestBase
    {
        private readonly IQuestionService _questionService;
        private readonly Mock<IQuestionRepository> _questionRepository;
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IFavoriteRepository> _favoriteRepository;
        private readonly Mock<IFavoriteCache> _favoriteCache;
        private readonly Mock<IVoteCache> _voteCache;
        private readonly Mock<IQuestionDetailCache> _questionDetailCache;
        public QuestionServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(_userRepository.Object, _mapper);
            _questionRepository = new Mock<IQuestionRepository>();
            _favoriteRepository = new Mock<IFavoriteRepository>();
            _favoriteCache = new Mock<IFavoriteCache>();
            _voteCache = new Mock<IVoteCache>();
            _questionDetailCache = new Mock<IQuestionDetailCache>();
            _questionService = new QuestionService(_questionRepository.Object, _mapper, _favoriteRepository.Object, _userRepository.Object, _favoriteCache.Object, _questionDetailCache.Object, _voteCache.Object);
        }

        [Fact]
        public async void GetQuestionDetail_Returns_QuestionDetailResponseContract_When_QuestionExist()
        {
            int id = 5;
            int UserId = 5;
            bool mockIsFavorite = false;
            int mockVote = 50;
            var mockQuestion = new QuestionDetailResponseContract
            {
                Answer = 1,
                Category = "elif",
                Content = "bilmembilmembilmembilmem",
                Title = "bilmem",
                View = 50,
                Favorite = 20,
                Vote = 0,
                IsFavorite = false,
                User = new UserResponseContract
                {
                    Name = "Elif",
                    Surname = "Elif"
                },
                AnswerResponse = new List<AnswerResponseContract>()
                {
                    new AnswerResponseContract
                    {
                        Content = "yanıtyanıtyanıtyanıtyanıtyanıt",
                        User = new UserResponseContract
                        {
                            Name = "gizem",
                            Surname = "gizem"
                        }

                    }
                },


            };
            _questionDetailCache.Setup(c => c.GetQuestionsWithDetail(It.IsAny<int>())).Returns(Task.FromResult(mockQuestion));
            _favoriteCache.Setup(f => f.CheckFav(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(mockIsFavorite));
            _voteCache.Setup(v => v.GetNumberOfVotes(It.IsAny<int>())).Returns(Task.FromResult(mockVote));

            var responseData = await _questionService.GetQuestionsWithDetail(id, UserId);

            Assert.NotNull(responseData);
        }

        [Fact]
        public async void GetNewestQuestions_Returns_PaginationResponseContract_When_HasQuestions()
        {
            var mockPagination = new PaginationContract
            {
                Page = 1,
                PageSize = 10,
            };
            var mockPaginationResponse = new PaginationResponseContract<GetAllQuestionsContract>
            {
                Data = new List<GetAllQuestionsContract>()
                {
                    new GetAllQuestionsContract
                    {
                        Answer = 10,
                        Category = "elif",
                        Content = "bilmembilmembilmembilmem",
                        Title = "bilmem",
                        View=50,
                        Vote=30,
                        User = new UserResponseContract
                        {
                            Name = "Elif",
                            Surname ="Elif"
                        },
                    }
                },
                Pagination = new PaginationContract
                {
                    Page = 1,
                    PageSize = 10,
                    TotalData = 20,
                    TotalPage = 2,
                }
            };

            _questionRepository.Setup(x => x.GetNewestQuestions(mockPagination)).Returns(Task.FromResult(mockPaginationResponse));

            var responseData = await _questionService.GetNewestQuestions(mockPagination);

            Assert.NotNull(responseData);
        }
    }
}
