namespace Platform.Services
{
    //type broker pattern
    public static class TypeBroker
    {
        private static IResponseFormatter formatter = new HtmlResponseFormatter();

        public static IResponseFormatter Formatter => formatter;
    }
}
