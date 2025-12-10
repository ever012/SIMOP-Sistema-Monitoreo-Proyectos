namespace SIMOP_frontend.Models
{
    public class PresupuestoDto
    {
        public int ppre_codigo { get; set; }
        public int ppre_codproyecto { get; set; }
        public decimal ppre_monto { get; set; }
        public string ppre_descripcion { get; set; }
        public DateTime? ppre_fecha { get; set; }
    }
}
