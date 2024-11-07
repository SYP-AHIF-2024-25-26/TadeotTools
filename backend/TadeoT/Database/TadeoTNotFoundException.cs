namespace TadeoT.Database;
public class TadeoTNotFoundException: TadeoTDatabaseException {
    public TadeoTNotFoundException(string message) : base(message) { }
    public TadeoTNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
