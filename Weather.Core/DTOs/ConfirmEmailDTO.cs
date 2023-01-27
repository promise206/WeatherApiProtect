using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Core.DTOs
{
    public class ConfirmEmailDTO
    {

        public string EmailAddress { get; set; }
        public string Token { get; set; }
    }
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDTO>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Email address is required")
                .EmailAddress().WithMessage("A valid email is required");
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
        }
    }
}
