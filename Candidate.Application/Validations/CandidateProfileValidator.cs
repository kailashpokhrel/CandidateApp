using Candidate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Candidate.Application.Models;

namespace Candidate.Application.Validations
{
    public class CandidateProfileValidator : AbstractValidator<CandidateModel>
    {
        public CandidateProfileValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(1, 100).WithMessage("First name cannot be longer than 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(1, 100).WithMessage("Last name cannot be longer than 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.")
                .Length(1, 255).WithMessage("Email cannot be longer than 255 characters.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Free text comment is required.")
                .Length(1, 500).WithMessage("Comment cannot be longer than 500 characters.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10}$").WithMessage("Phone number should be exactly 10 digits.");

            RuleFor(x => x.BestTimeToCall)
                .Length(1, 50).WithMessage("Best time to call cannot be longer than 50 characters.");

            RuleFor(x => x.LinkedInProfileUrl)
                .Matches(@"^(http[s]?:\/\/)?([a-zA-Z0-9]+\.)+[a-zA-Z]{2,}\/.*$").WithMessage("Invalid LinkedIn URL format.");

            RuleFor(x => x.GitHubProfileUrl)
                .Matches(@"^(http[s]?:\/\/)?([a-zA-Z0-9]+\.)+[a-zA-Z]{2,}\/.*$").WithMessage("Invalid GitHub URL format.");
        }
    }
}
