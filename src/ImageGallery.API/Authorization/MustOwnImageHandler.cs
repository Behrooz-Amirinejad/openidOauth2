using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization;

public class MustOwnImageHandler : AuthorizationHandler<MustOwnImageRequirment>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGalleryRepository _galleryRepository;

    public MustOwnImageHandler(IHttpContextAccessor httpContextAccessor  , IGalleryRepository galleryRepository )
    {
        if (httpContextAccessor is null)
        {
            throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        if (galleryRepository is null)
        {
            throw new ArgumentNullException(nameof(galleryRepository));
        }

        this._httpContextAccessor = httpContextAccessor;
        this._galleryRepository = galleryRepository;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnImageRequirment requirement)
    {
        var imageId = _httpContextAccessor.HttpContext?
                                                .GetRouteValue("id")?
                                                .ToString();

        if (!Guid.TryParse(imageId, out var imageIdAsGuid))
        {
            context.Fail();
            return;
        }

        //get the sub claim
        var ownerId = context.User.Claims.FirstOrDefault(x => x.Type =="sub")?.Value;
        if (ownerId == null)
        {
            context.Fail();
            return;
        }


        if(!await _galleryRepository.IsImageOwnerAsync(imageIdAsGuid , ownerId))
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}
