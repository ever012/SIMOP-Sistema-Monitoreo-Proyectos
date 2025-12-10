namespace SIMOP_frontend.Models
{
    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public T data { get; set; }
        public string mensaje { get; set; }
    }

}
