using Grpc.Core;
using UserManagementService;
using src.Application.UseCases;

namespace src.Services
{
    public class ProgressGrpcService : ProgressService.ProgressServiceBase
    {
        private readonly GetUserProgress _getUserProgress;
        private readonly UpdateUserProgress _updateUserProgress;

        public ProgressGrpcService(GetUserProgress getUserProgress, UpdateUserProgress updateUserProgress)
        {
            _getUserProgress = getUserProgress;
            _updateUserProgress = updateUserProgress;
        }

        public override async Task<ProgressResponse> GetUserProgress(ProgressRequest request, ServerCallContext context)
        {
            var subjectIds = await _getUserProgress.ExecuteAsync(Guid.Parse(request.UserId));
            return new ProgressResponse { SubjectIds = { subjectIds } };
        }

        public override async Task<UpdateProgressResponse> UpdateUserProgress(UpdateProgressRequest request, ServerCallContext context)
        {
            await _updateUserProgress.ExecuteAsync(
                Guid.Parse(request.UserId),
                request.AddSubjectIds.ToList(),
                request.RemoveSubjectIds.ToList()
            );

            return new UpdateProgressResponse { Success = true };
        }
    }
}
