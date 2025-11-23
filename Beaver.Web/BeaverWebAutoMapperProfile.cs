using AutoMapper;
using Beaver.Books;

namespace Beaver
{
    public class BeaverWebAutoMapperProfile: Profile
    {
        public BeaverWebAutoMapperProfile()
        {
            CreateMap<BookDto, CreateUpdateBookDto>();
        }
    }
}
