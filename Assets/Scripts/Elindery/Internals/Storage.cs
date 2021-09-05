namespace Elindery.Internals
{
    public static class Storage
    {
        public static string ClientID { get; private set; } = "";

        public static void ChangeCliendID(string newClientID)
        {
            ClientID = newClientID;
        }
    }
}
