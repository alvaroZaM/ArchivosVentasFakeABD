using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ArchivosVentasFakeABD.Modelos;

namespace ArchivosVentasFakeABD.Servicios
{
    public class CargarTxt
    {
        private readonly string _rutaArchivoTxt;

        public CargarTxt(string rutaArchivoTxt)
        {
            _rutaArchivoTxt = rutaArchivoTxt ?? throw new ArgumentNullException(nameof(rutaArchivoTxt));
        }

        public (List<Cliente> Clientes, List<Producto> Productos, List<Venta> Ventas) CargarYProcesar()
        {
            if (!File.Exists(_rutaArchivoTxt))
                throw new FileNotFoundException("No se encontró el archivo TXT.", _rutaArchivoTxt);

            var clientesDict = new Dictionary<string, Cliente>();
            var productosDict = new Dictionary<string, Producto>();
            var ventasDict = new Dictionary<int, Venta>(); 

            var lineas = File.ReadAllLines(_rutaArchivoTxt);

            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                var columnas = linea.Split('|');
                if (columnas.Length != 9)
                    throw new FormatException($"La línea no tiene el formato esperado: {linea}");

                
                int folio = int.Parse(columnas[0]);
                DateTime fecha = DateTime.ParseExact(columnas[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                string codCli = columnas[2];
                string nombreCliente = columnas[3];
                int cantidad = int.Parse(columnas[4]);
                string codProd = columnas[5];
                string nombreProducto = columnas[6];
                decimal precioUnitario = decimal.Parse(columnas[7], CultureInfo.InvariantCulture);
                decimal subtotal = decimal.Parse(columnas[8], CultureInfo.InvariantCulture);

                
                if (!clientesDict.ContainsKey(codCli))
                {
                    clientesDict[codCli] = new Cliente
                    {
                        CodCli = codCli,
                        Nombre = nombreCliente,
                        Correo = null // quitar
                    };
                }

                
                if (!productosDict.ContainsKey(codProd))
                {
                    productosDict[codProd] = new Producto
                    {
                        CodProd = codProd,
                        Nombre = nombreProducto,
                        PrecioReferencia = precioUnitario
                    };
                }

                
                if (!ventasDict.ContainsKey(folio))
                {
                    ventasDict[folio] = new Venta
                    {
                        Folio = folio,
                        CodCli = codCli,
                        Fecha = fecha,
                        Detalles = new List<VentaDetalle>()
                    };
                }

                var detalle = new VentaDetalle
                {
                    CodProd = codProd,
                    Cantidad = cantidad,
                    PrecioUnitario = precioUnitario,
                    Subtotal = subtotal
                };

                ventasDict[folio].Detalles.Add(detalle);
            }

            
            foreach (var venta in ventasDict.Values)
            {
                decimal total = 0;
                foreach (var det in venta.Detalles)
                    total += det.Subtotal;
                venta.Total = total;
            }

            return (new List<Cliente>(clientesDict.Values), new List<Producto>(productosDict.Values), new List<Venta>(ventasDict.Values));
        }
    }
}
