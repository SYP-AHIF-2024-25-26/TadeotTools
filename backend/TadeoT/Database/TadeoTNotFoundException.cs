namespace TadeoT.Database;
public class TadeoTNotFoundException: Exception {
    public TadeoTNotFoundException(string message) : base(message) { }
    public TadeoTNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}