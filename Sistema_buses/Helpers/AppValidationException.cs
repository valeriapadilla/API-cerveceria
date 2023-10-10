namespace Sistema_buses.Helpers
{
    public class AppValidationException : Exception
    {
        public AppValidationException(string message) : base(message) { }
    }
}
