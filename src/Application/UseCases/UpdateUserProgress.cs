using src.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Application.UseCases
{
    public class UpdateUserProgress
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserProgress(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(Guid userId, List<string> addSubjectIds, List<string> removeSubjectIds)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} was not found.");
            
            user.SubjectIds ??= new List<string>();

            if (addSubjectIds != null)
            {
                foreach (var subjectId in addSubjectIds)
                {
                    if (!user.SubjectIds.Contains(subjectId))
                        user.SubjectIds.Add(subjectId);
                }
            }

            if (removeSubjectIds != null)
            {
                user.SubjectIds.RemoveAll(subject => removeSubjectIds.Contains(subject));
            }

            await _userRepository.UpdateAsync(user);
        }
    }
}
