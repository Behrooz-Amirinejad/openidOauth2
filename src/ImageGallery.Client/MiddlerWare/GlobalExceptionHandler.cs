
namespace ImageGallery.Client.MiddlerWare;

public class GlobalExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
	

			if((context.Response.StatusCode == StatusCodes.Status403Forbidden) ||
               (context.Response.StatusCode == StatusCodes.Status403Forbidden) ||
			   (context.Response.StatusCode == StatusCodes.Status403Forbidden) )
			{
				context.Response.Redirect("/Authentication/logout");
			}
			await next(context);


       
    }
}
