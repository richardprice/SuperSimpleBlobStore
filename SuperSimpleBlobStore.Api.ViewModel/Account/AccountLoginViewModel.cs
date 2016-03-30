using FluentValidation;

namespace SuperSimpleBlobStore.Api.ViewModel
{
    public class AccountLoginViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class AccountLoginViewModelValidator : AbstractValidator<AccountLoginViewModel>
    {
        public AccountLoginViewModelValidator()
        {
            RuleFor(q => q.email).NotEmpty().WithMessage("Email address must not be empty");
            RuleFor(q => q.password).NotEmpty().WithMessage("Password must not be empty");
        }
    }
}
