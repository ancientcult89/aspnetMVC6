using Microsoft.AspNetCore.Http.Features;

namespace Platform
{
    public class ConsentMiddleware
    {
        private RequestDelegate _next;

        public ConsentMiddleware(RequestDelegate nextDelegate)
        { 
            _next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/consent")
            {
                ITrackingConsentFeature? consentFeature = context.Features.Get<ITrackingConsentFeature>();
                if (consentFeature != null)
                {
                    if (!consentFeature.HasConsent)
                        consentFeature.GrantConsent();
                    else
                        consentFeature.WithdrawConsent();
                    await context.Response.WriteAsync(consentFeature.HasConsent ? "Consent Granted \n" : "Consent Withdrawn\n");
                }
            }
            else
                await _next(context);
        }
    }
}
