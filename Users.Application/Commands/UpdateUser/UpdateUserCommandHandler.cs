using AutoMapper;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Newtonsoft.Json;
using Serilog;
using Users.Application.Dto;
using Common.Application.Abstractions;
using MediatR;

namespace Users.Application.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GetUserDto>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IRepository<ApplicationUser> userRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<GetUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var updateUser = await _userRepository.SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (updateUser == null)
            {
                Log.Error($"There isn't user with id {request.Id} in DB");
                throw new NotFoundException($"There isn't user with id {request.Id} in DB");
            }
            if (_currentUserService.CurrentUserId != request.Id && !_currentUserService.UserRole.Contains("Admin"))
            {
                Log.Error($"Your account {updateUser.Login} doesn't allow editing");
                throw new ForbiddenException();
            }

            _mapper.Map(request, updateUser);
            Log.Information("Updated user " + JsonConvert.SerializeObject(request.Login));

            return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(updateUser, cancellationToken));
        }
    }
}
