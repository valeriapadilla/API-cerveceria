namespace Sistema_buses.Helpers
{
    public class DbOperationException : Exception
    {
        public DbOperationException(string message) : base(message) { }
    }
}
