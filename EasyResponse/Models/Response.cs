using EasyResponse.Abstractions;

namespace EasyResponse.Models
{
    public record Response(int Status, string? Message) : IResponse
    {
        public const int Success = 0;
        public const int Fail = 1;
        public const int Warning = 2;
    }

    public record Response<T>(int Status, string? Message, T Data) : Response(Status, Message)
    {
        public Response<O> As<O>(Func<T, O> converter) => new(Status, Message, converter(Data));
    }

    public record ErrorResponse(int Status, string? Message, Exception? Error) : Response(Status, Message), IErrorResponse;

    public record FullResponse<T>(int Status, string? Message, T Data, Exception? Error) : Response<T>(Status, Message, Data), IErrorResponse
    {
        public new FullResponse<O> As<O>(Func<T, O> converter) => new(Status, Message, converter(Data), Error);
        public async Task<FullResponse<O>> As<O>(Func<T, CancellationToken, Task<O>> converter, CancellationToken ct) => new(Status, Message, await converter(Data, ct), Error);
    }
}
