namespace Database.Repository;

public class TadeoTDatabaseException : Exception
{
    public TadeoTDatabaseException(string message) : base(message) { }
    public TadeoTDatabaseException(string message, Exception innerException) : base(message, innerException) { }
}
