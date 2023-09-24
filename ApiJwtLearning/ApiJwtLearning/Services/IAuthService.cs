using ApiJwtLearning.Model;

namespace ApiJwtLearning.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RejesterAsync(RegisterModel model);
        Task<AuthModel> GetTokkenAsync(TokkenRequistModel model);
        Task<String> AddRoleAsync(AddRoleModel model);
    }
}
