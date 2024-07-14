namespace LemonLime.Handlers
{
    public static class ResponseHandler
    {
        public static GenericResponse<T> CreateSuccessResponse<T>(T data, string message = "Request successful")
        {
            return new GenericResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static GenericResponse<T> CreateErrorResponse<T>(string errorMessage, List<string> errors = null)
        {
            return new GenericResponse<T>
            {
                Success = false,
                Message = errorMessage,
                Errors = errors ?? new List<string>()
            };
        }
    }
}
