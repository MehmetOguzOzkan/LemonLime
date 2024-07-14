namespace LemonLime.Handlers
{
    public class GenericResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public GenericResponse()
        {
            Errors = new List<string>();
        }
    }
}
