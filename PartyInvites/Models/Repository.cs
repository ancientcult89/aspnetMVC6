namespace PartyInvites.Models
{
    public static class Repository
    {
        private static List<GuestResponse> _responses = new List<GuestResponse>();
        public static List<GuestResponse> Responses => _responses;
        public static void AddResponse(GuestResponse response)
        {
            Console.WriteLine(response);
            Responses.Add(response);
        }
    }
}
