namespace ArchivosVentasFakeABD.Modelos
{




    public class VentaDetalle
    {
        public int DetalleID { get; set; }
        public int VentaID { get; set; }
        public int ProductoID { get; set; }
        public string CodProd { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }  // Precio usado en la venta
        public decimal Subtotal { get; set; }
    }
}


