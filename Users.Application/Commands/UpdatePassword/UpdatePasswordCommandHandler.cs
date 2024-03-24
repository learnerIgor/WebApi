using AutoMapper;
using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Application.Utils;
using Newtonsoft.Json;
using Serilog;
using Users.Application.Dto;
using Common.Application.Abstractions;
using MediatR;

namespace Users.Application.Commands.UpdatePassword
{
    public class UpdatePasswordCommandHandler: IRequestHandler<UpdatePasswordCommand,GetUserDto>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdatePasswordCommandHandler(IRepository<ApplicationUser> userRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<GetUserDto> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (user == null)
            {
                Log.Error($"There isn't user with id {request.Id} in DB");
                throw new NotFoundException($"There isn't user with id {request.Id} in DB");
            }
            if (_currentUserService.CurrentUserId != request.Id && !_currentUserService.UserRole.Contains("Admin"))
            {
                Log.Error($"Your account {user.Login} doesn't allow editing");
                throw new ForbiddenException();
            }

            request.PasswordHash = PasswordHasher.HashPassword(request.PasswordHash);
            _mapper.Map(request, user);
            Log.Information("Updated user password " + JsonConvert.SerializeObject(user.Login));

            return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(user, cancellationToken));
        }
    }
}
