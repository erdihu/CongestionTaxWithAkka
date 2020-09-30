namespace Trangselskatt.Common.Model.Messages
{
    public class MessageResult
    {
        public MessageResult(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; }
        public string Message { get; }
    }
}
