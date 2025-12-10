namespace SIMOP_frontend.Models
{
    public class ProyectoDto
    {
        public int pro_codigo { get; set; }
        public string pro_nombre { get; set; }
        public string pro_descripcion { get; set; }
        public int pro_codCategoria { get; set; }
        public string pro_ubicacion { get; set; }
        public decimal pro_presupuestoTotal { get; set; }
        public DateTime? pro_fechaInicio { get; set; }
        public DateTime? pro_fechaFinEstimada { get; set; }
        public DateTime? pro_fechaCreacion { get; set; }
        public DateTime? pro_fechaActualizacion { get; set; }
    }
}
