namespace EasyResponse.Abstractions
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}