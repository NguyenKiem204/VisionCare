using AutoMapper;
using MediatR;
using VisionCare.Application.DTOs.Common;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Interfaces;

namespace VisionCare.Application.Queries.Users;

public class SearchUsersQuery : IRequest<PagedResult<UserDto>>
{
    public string? Keyword { get; set; }
    public int? RoleId { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}

public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, PagedResult<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SearchUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<UserDto>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var (items, totalCount) = await _userRepository.SearchAsync(
            request.Keyword,
            request.RoleId,
            request.Status,
            request.Page,
            request.PageSize,
            request.SortBy,
            request.Desc
        );

        return new PagedResult<UserDto>
        {
            Items = _mapper.Map<IEnumerable<UserDto>>(items),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}


