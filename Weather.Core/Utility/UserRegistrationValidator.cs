using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Core.DTOs;

namespace Weather.Core.Utility
{
    public class UserRegistrationValidator : AbstractValidator<RegistrationDTO>
    {
        public UserRegistrationValidator()
        {
            RuleFor(RegistrationDTO => RegistrationDTO.Password)
                .Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("Passwords do not match");
            RuleFor(RegistrationDTO => RegistrationDTO.FirstName)
                .HumanName();
            RuleFor(RegistrationDTO => RegistrationDTO.LastName)
                .HumanName();
            RuleFor(RegistrationDTO => RegistrationDTO.PhoneNumber)
                .PhoneNumber();
            RuleFor(RegistrationDTO => RegistrationDTO.Email)
                .EmailAddress();
        }
    }
}
