namespace TadeoT.Database {
    public class TadeoTArgumentNullException : Exception {
        public TadeoTArgumentNullException(string message) : base(message) { }
        public TadeoTArgumentNullException(string message, Exception innerException) : base(message, innerException) { }
    }

}
