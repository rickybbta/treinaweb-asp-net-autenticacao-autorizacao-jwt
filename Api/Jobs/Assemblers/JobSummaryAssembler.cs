using System.Security.Claims;
using TWJobs.Api.Common.Assemblers;
using TWJobs.Api.Common.Dtos;
using TWJobs.Api.Jobs.Dtos;

namespace TWJobs.Api.Jobs.Assemblers;

public class JobSummaryAssembler : IAssembler<JobSummaryResponse>
{
    private readonly LinkGenerator _linkGenerator;

    public JobSummaryAssembler(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    public JobSummaryResponse ToResource(JobSummaryResponse resource, HttpContext context)
    {
        var selfLink = new LinkResponse(
            _linkGenerator.GetUriByName(context, "FindJobById", new { Id = resource.Id }),
            "GET",
            "self"
        );
        var updateLink = new LinkResponse(
            _linkGenerator.GetUriByName(context, "UpdateJobById", new { Id = resource.Id }),
            "PUT",
            "update"
        );
        var deleteLink = new LinkResponse(
            _linkGenerator.GetUriByName(context, "DeleteJobById", new { Id = resource.Id }),
            "DELETE",
            "delete"
        );
        var role = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "";
        resource.AddLinksIf(role=="Admin", updateLink, deleteLink);
        resource.AddLink(selfLink);
        return resource;
    }
}