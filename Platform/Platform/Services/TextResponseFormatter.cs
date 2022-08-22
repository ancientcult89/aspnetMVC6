namespace Platform.Services
{
    public class TextResponseFormatter : IResponseFormatter
    {
        private int responseCounter = 0;
        private static TextResponseFormatter? shared;
        public async Task Format(HttpContext context, string content)
        {
            await context.Response.WriteAsync($"Respose {++responseCounter}:\n{content}");
        }

        //singleton pattern simple realisation
        public static TextResponseFormatter Singleton
        {
            get
            {
                if (shared == null)
                    shared = new TextResponseFormatter();
                return shared;
            }
        }
    }
}
