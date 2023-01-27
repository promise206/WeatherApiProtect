using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Enums;
using Weather.Core.DTOs;
using Weather.Core.Interfaces;
using Weather.Core.Utilities;
using Weather.Domain.Entities;

namespace Weather.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IDigitTokenService _digitTokenService;
        public AuthService(IServiceProvider provider)
        {
            _userManager = provider.GetRequiredService<UserManager<User>>();
            _tokenService = provider.GetRequiredService<ITokenService>();
            _mapper = provider.GetRequiredService<IMapper>();
            _digitTokenService = provider.GetRequiredService<IDigitTokenService>();
        }

        public async Task<ResponseDto<CredentialResponseDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User does not exist", (int)HttpStatusCode.NotFound);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return ResponseDto<CredentialResponseDTO>.Fail("Invalid user credential", (int)HttpStatusCode.BadRequest);
            }

            if (!user.EmailConfirmed)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User's account is not confirmed", (int)HttpStatusCode.BadRequest);
            }
            else if (!user.IsActive)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User's account is deactivated", (int)HttpStatusCode.BadRequest);
            }

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); //sets refresh token for 7 days
            var credentialResponse = new CredentialResponseDTO()
            {
                Id = user.Id,
                Token = await _tokenService.GenerateToken(user),
                RefreshToken = user.RefreshToken
            };

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
               // _logger.Information("User successfully logged in");
                return ResponseDto<CredentialResponseDTO>.Success("Login successful", credentialResponse);
            }
            return ResponseDto<CredentialResponseDTO>.Fail("Failed to login user", (int)HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<RegistrationResponseDTO>> Register(RegistrationDTO userDetails)
        {
            var checkEmail = await _userManager.FindByEmailAsync(userDetails.Email);
            if (checkEmail != null)
            {
                return ResponseDto<RegistrationResponseDTO>.Fail("Email already Exists", (int)HttpStatusCode.BadRequest);
            }
            var userModel = _mapper.Map<User>(userDetails);
            await _userManager.CreateAsync(userModel, userDetails.Password);
            await _userManager.AddToRoleAsync(userModel, UserRole.Visitor.ToString());

            var purpose = UserManager<User>.ConfirmEmailTokenPurpose;
            string token = await _digitTokenService.GenerateAsync(purpose, _userManager, userModel);

            var confirmEmailDetails = new ConfirmEmailDTO() { EmailAddress = userModel.Email ,Token= token };

            var confirmEmail = await ConfirmEmail(confirmEmailDetails);
            if (confirmEmail != null)
            {
                return ResponseDto<RegistrationResponseDTO>.Success("Registration Successful and Email Confirmed",
                new RegistrationResponseDTO { Id = userModel.Id, Email = userModel.Email, },
                (int)HttpStatusCode.Created);
            }
            else
            {
                return ResponseDto<RegistrationResponseDTO>.Fail("Email Confirmation not successful", (int)HttpStatusCode.Unauthorized);
            }
        }

        public async Task<ResponseDto<string>> ConfirmEmail(ConfirmEmailDTO confirmEmailDTO)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailDTO.EmailAddress);
            if (user == null)
            {
                return ResponseDto<string>.Fail("User not found", (int)HttpStatusCode.NotFound);
            }
            var purpose = UserManager<User>.ConfirmEmailTokenPurpose;
            var result = await _digitTokenService.ValidateAsync(purpose, confirmEmailDTO.Token, _userManager, user);
            if (result)
            {
                user.EmailConfirmed = true;
                user.IsActive = true;
                var update = await _userManager.UpdateAsync(user);
                if (update.Succeeded)
                {
                    return ResponseDto<string>.Success("Email Confirmation successful", user.Id, (int)HttpStatusCode.OK);
                }
            }   
            return ResponseDto<string>.Fail("Email Confirmation not successful", (int)HttpStatusCode.Unauthorized);
        }
    }
}
