using EasyResponse.Abstractions;

namespace EasyResponse.Extensions
{
    public static class TaskUtilities
    {
        public static async Task FireSafeAsync(this Task task, IErrorHandler? handler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }

        public static async Task<TOutput?> FireSafeAsync<TOutput>(this Task<TOutput> task, IErrorHandler? handler = null)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
            return default;
        }

        public static T Fire<T>(this T response, IResponseHandler handler) where T : IResponse
        {
            handler.HandleResponse(response);
            return response;
        }

        public static T Fire<T>(this IResponse<T> response, IResponseHandler handler) where T : IResponse<T>
        {
            handler.HandleResponse(response);
            return response.Data;
        }

        public static async void Forget(this Task task) => await task;
    }
}