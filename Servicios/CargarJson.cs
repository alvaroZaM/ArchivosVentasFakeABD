using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using ArchivosVentasFakeABD.Modelos;

namespace ArchivosVentasFakeABD.Servicios
{
    public class CargarJson
    {
        private readonly string _rutaArchivoJson;

        public CargarJson(string rutaArchivoJson)
        {
            _rutaArchivoJson = rutaArchivoJson ?? throw new ArgumentNullException(nameof(rutaArchivoJson));
        }

        public (List<Cliente> Clientes, List<Producto> Productos, List<Venta> Ventas) CargarYProcesar()
        {
            if (!File.Exists(_rutaArchivoJson))
                throw new FileNotFoundException("No se encontró el archivo JSON.", _rutaArchivoJson);

            string contenidoJson = File.ReadAllText(_rutaArchivoJson);

            
            var ventasJson = JsonSerializer.Deserialize<List<VentaJson>>(contenidoJson);

            // Diccionarios para evitar duplicados
            var clientesDict = new Dictionary<string, Cliente>();   // CodCli
            var productosDict = new Dictionary<string, Producto>(); // CodProd
            var ventasList = new List<Venta>();

            foreach (var vj in ventasJson)
            {
                
                if (!clientesDict.ContainsKey(vj.CodCli))
                {
                    clientesDict[vj.CodCli] = new Cliente
                    {
                        CodCli = vj.CodCli,
                        Nombre = vj.Nombre,
                        Correo = null // quitar
                    };
                }

                
                var venta = new Venta
                {
                    Folio = vj.Folio,
                    CodCli = vj.CodCli,
                    Fecha = DateTime.Parse(vj.Fecha, CultureInfo.InvariantCulture),
                    Total = vj.Total,
                    Detalles = new List<VentaDetalle>()
                };

               
                foreach (var pj in vj.Productos)
                {
                    if (!productosDict.ContainsKey(pj.CodProd))
                    {
                        productosDict[pj.CodProd] = new Producto
                        {
                            CodProd = pj.CodProd,
                            Nombre = pj.Descripcion,
                            PrecioReferencia = pj.Importe
                        };
                    }

                    var detalle = new VentaDetalle
                    {
                        CodProd = pj.CodProd,
                        Cantidad = pj.Cantidad,
                        PrecioUnitario = pj.Importe,
                        Subtotal = pj.Subtotal
                    };

                    venta.Detalles.Add(detalle);
                }

                ventasList.Add(venta);
            }

            return (new List<Cliente>(clientesDict.Values), new List<Producto>(productosDict.Values), ventasList);
        }

        private class VentaJson
        {
            public int Folio { get; set; }
            public string Fecha { get; set; }
            public string CodCli { get; set; }
            public string Nombre { get; set; }
            public List<ProductoJson> Productos { get; set; }
            public decimal Total { get; set; }
        }

        private class ProductoJson
        {
            public string CodProd { get; set; }
            public string Descripcion { get; set; }
            public int Cantidad { get; set; }
            public decimal Importe { get; set; }
            public decimal Subtotal { get; set; }
        }
    }
}
