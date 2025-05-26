namespace EasyResponse.Abstractions;

public interface IResponseHandler
{
    void HandleResponse(IResponse response);
}
