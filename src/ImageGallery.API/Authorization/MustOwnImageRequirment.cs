using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization;

public class MustOwnImageRequirment : IAuthorizationRequirement
{
    public MustOwnImageRequirment()
    {
            
    }
}
