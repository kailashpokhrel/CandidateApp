using Candidate.Application.DTOS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate.Application.Commands
{
    internal class CreateCandidateCommand: IRequest<CandidateCreateUpdateDto>
    {
        public CandidateCreateUpdateDto CandidateDto { get; set; }
    }
}
