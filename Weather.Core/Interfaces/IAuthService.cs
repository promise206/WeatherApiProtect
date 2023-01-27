using Weather.Core.DTOs;
using Weather.Core.Utilities;

namespace Weather.Core.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<CredentialResponseDTO>> Login(LoginDTO model);
        Task<ResponseDto<RegistrationResponseDTO>> Register(RegistrationDTO userDetails);
        Task<ResponseDto<string>> ConfirmEmail(ConfirmEmailDTO confirmEmailDTO);
    }
}