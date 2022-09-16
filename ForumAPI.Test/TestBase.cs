using AutoMapper;
using ForumAPI.Service.Mapping;
namespace ForumAPI.Test
{
    public class TestBase
    {
        public IMapper _mapper;
        public TestBase()
        {
            var mapperConfig = new MapperConfiguration(x => x.AddProfile(new MapProfile()));
            IMapper mapper = mapperConfig.CreateMapper();
            _mapper = mapper;
        }
    }
}
