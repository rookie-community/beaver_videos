using Volo.Abp.Application.Dtos;

namespace Beaver.Books;

public class AuthorLookupDto : EntityDto<Guid>
{
    public string Name { get; set; }
}
