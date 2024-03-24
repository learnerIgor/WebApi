using Common.Domain;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Newtonsoft.Json;
using Serilog;
using Common.Application.Abstractions;
using MediatR;

namespace Users.Application.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        public DeleteUserCommandHandler(IRepository<ApplicationUser> userRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var deletUser = await _userRepository.SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (deletUser == null)
            {
                Log.Error($"There isn't user with id {request.Id} in DB");
                throw new NotFoundException($"There isn't user with id {request.Id} in DB");
            }
            if (_currentUserService.CurrentUserId != request.Id && !_currentUserService.UserRole.Contains("Admin"))
            {
                Log.Error($"Your account {deletUser.Login} doesn't allow deliting");
                throw new ForbiddenException();
            }

            Log.Information("Deleted user " + JsonConvert.SerializeObject(new { deletUser.Id, deletUser.Login }));

            return await _userRepository.DeleteAsync(deletUser, cancellationToken);
        }
    }
}
