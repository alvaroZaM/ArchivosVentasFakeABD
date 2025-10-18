namespace ArchivosVentasFakeABD.Modelos
{


    public class Venta
    {
        public int VentaID { get; set; } 
        public int Folio { get; set; }
        public int ClienteID { get; set; }
        public string CodCli { get; set; } 
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<VentaDetalle> Detalles { get; set; }
    }

}

