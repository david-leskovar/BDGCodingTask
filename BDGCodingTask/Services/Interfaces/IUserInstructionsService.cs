using BDGCodingTask.Domain.Entities;
using BDGCodingTask.Domain.Enums;

namespace BDGCodingTask.Services.Interfaces
{
    public interface IUserInstructionsService
    {
        public (List<UserInstruction>,UserInstructionCompletion) GetUserInstructions(string orderType, decimal amount, List<Exchange> exchanges);
    }
}
