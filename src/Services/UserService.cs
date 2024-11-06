using Grpc.Core;
using UserManagementService;
using src.Application.UseCases;
using src.Infrastructure.Messaging;
using src.Domain.Events;

namespace src.Services
{
    public class UserGrpcService : UserService.UserServiceBase
    {
        private readonly GetUserProfile _getUserProfile;
        private readonly UpdateUserProfile _updateUserProfile;

        public UserGrpcService(GetUserProfile getUserProfile, UpdateUserProfile updateUserProfile)
        {
            _getUserProfile = getUserProfile;
            _updateUserProfile = updateUserProfile;
        }

        public override async Task<UserResponse> GetUserProfile(UserRequest request, ServerCallContext context)
        {
            var user = await _getUserProfile.ExecuteAsync(Guid.Parse(request.UserId));
            return new UserResponse
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                FirstLastName = user.FirstLastName,
                SecondLastName = user.SecondLastName,
                Rut = user.RUT,
                Email = user.Email
            };
        }

        public override async Task<UpdateProfileResponse> UpdateUserProfile(UpdateProfileRequest request, ServerCallContext context)
        {
            await _updateUserProfile.ExecuteAsync(Guid.Parse(request.UserId), request.Name, request.FirstLastName, request.SecondLastName);
            return new UpdateProfileResponse { Success = true };
        }
    }
}
