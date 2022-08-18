namespace Platform
{
    public class QueryStringMiddleware
    {
        private RequestDelegate? _next;
        public QueryStringMiddleware(){ }

        public QueryStringMiddleware(RequestDelegate nextDelegate)
        {
            _next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get && context.Request.Query["custom"] == "true")
            {
                if(!context.Response.HasStarted)
                    context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Class-based Middlware \n");
            }
            if(_next != null)
                await _next(context);
        }
    }
}
