namespace SIMOP_frontend.Models
{
    public class TareaDto
    {
        public int pta_codigo { get; set; }
        public int pta_codproyecto { get; set; }
        public int? pta_codHito { get; set; }
        public string pta_Descripcion { get; set; }
        public string pta_Estado { get; set; }
        public DateTime? pta_FechaInicio { get; set; }
        public DateTime? pta_FechaFin { get; set; }
    }
}
