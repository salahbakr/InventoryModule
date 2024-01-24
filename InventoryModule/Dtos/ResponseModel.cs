namespace InventoryModule.Dtos
{
    public class ResponseModel<T> where T : class
    {
        public string? Message { get; set; }
        public string? Error { get; set; }
        public bool Success => Error == null;
        public T? Data { get; set; }
    }
}
