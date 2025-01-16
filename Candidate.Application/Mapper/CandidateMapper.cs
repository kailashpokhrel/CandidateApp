using Candidate.Application.Models;
using Candidate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate.Application.Mapper
{
    public class CandidateMapper
    {
        public static CandidateDto ToDto(CandidateProfile entity)
        {
            return new CandidateDto
            {
                CandidateId = entity.CandidateId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber= entity.PhoneNumber,
                BestTimeToCall = entity.BestTimeToCall,
                LinkedInProfileUrl = entity.LinkedInProfileUrl,
                GitHubProfileUrl = entity.GitHubProfileUrl,
                Comment = entity.Comment,
                CreatedAt = entity.CreatedAt,
                UpdatedAt= entity.UpdatedAt
    


            };
        }

        public static CandidateProfile ToEntity(CandidateModel model)
        {
            return new CandidateProfile
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BestTimeToCall= model.BestTimeToCall,
                LinkedInProfileUrl= model.LinkedInProfileUrl,
                GitHubProfileUrl= model.GitHubProfileUrl,
                Comment = model.Comment,
                CreatedAt = model.CreatedAt,
                UpdatedAt= model.UpdatedAt

            };
        }
    }
}
