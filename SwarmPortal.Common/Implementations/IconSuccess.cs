namespace SwarmPortal.Common
{
    public class IconSuccess
    {
        public bool IsSuccess { get; init; }
        public Stream? IconStream { get; init; }
        private static IconSuccess _failureCase = new IconSuccess(false, null);
        private IconSuccess(bool success, Stream? iconStream)
        {
            IsSuccess = success;
            IconStream = iconStream;
        }
        public static IconSuccess Failure => _failureCase;
        public static IconSuccess Success(Stream iconStream) => new IconSuccess(true, iconStream);
    }
}