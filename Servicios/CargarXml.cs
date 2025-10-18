using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using ArchivosVentasFakeABD.Modelos;

namespace ArchivosVentasFakeABD.Servicios
{
    public class CargarXml
    {
        private readonly string _rutaArchivoXml;

        public CargarXml(string rutaArchivoXml)
        {
            _rutaArchivoXml = rutaArchivoXml ?? throw new ArgumentNullException(nameof(rutaArchivoXml));
        }

        public (List<Cliente> Clientes, List<Producto> Productos, List<Venta> Ventas) CargarYProcesar()
        {
            if (!File.Exists(_rutaArchivoXml))
                throw new FileNotFoundException("No se encontró el archivo XML.", _rutaArchivoXml);

            var doc = XDocument.Load(_rutaArchivoXml);

            var clientesDict = new Dictionary<string, Cliente>();
            var productosDict = new Dictionary<string, Producto>();
            var ventasList = new List<Venta>();

            foreach (var ventaElem in doc.Root.Elements("Venta"))
            {
                string codCli = ventaElem.Element("CodCli")?.Value;
                string nombreCli = ventaElem.Element("Nombre")?.Value;
                int folio = int.Parse(ventaElem.Element("Folio")?.Value ?? "0");
                string fechaStr = ventaElem.Element("Fecha")?.Value;
                decimal total = decimal.Parse(ventaElem.Element("Total")?.Value ?? "0", CultureInfo.InvariantCulture);

                
                if (!clientesDict.ContainsKey(codCli))
                {
                    clientesDict[codCli] = new Cliente
                    {
                        CodCli = codCli,
                        Nombre = nombreCli,
                        Correo = null // quitarlo
                    };
                }

                var detalles = new List<VentaDetalle>();

                foreach (var prodElem in ventaElem.Element("Productos").Elements("Producto"))
                {
                    string codProd = prodElem.Element("CodProd")?.Value;
                    string descripcion = prodElem.Element("Descripcion")?.Value;
                    int cantidad = int.Parse(prodElem.Element("Cantidad")?.Value ?? "0");
                    decimal importe = decimal.Parse(prodElem.Element("Importe")?.Value ?? "0", CultureInfo.InvariantCulture);
                    decimal subtotal = decimal.Parse(prodElem.Element("Subtotal")?.Value ?? "0", CultureInfo.InvariantCulture);

                    
                    if (!productosDict.ContainsKey(codProd))
                    {
                        productosDict[codProd] = new Producto
                        {
                            CodProd = codProd,
                            Nombre = descripcion,
                            PrecioReferencia = importe
                        };
                    }

                    detalles.Add(new VentaDetalle
                    {
                        CodProd = codProd,
                        Cantidad = cantidad,
                        PrecioUnitario = importe,
                        Subtotal = subtotal
                    });
                }

                ventasList.Add(new Venta
                {
                    Folio = folio,
                    CodCli = codCli,
                    Fecha = DateTime.Parse(fechaStr, CultureInfo.InvariantCulture),
                    Total = total,
                    Detalles = detalles
                });
            }

            return (new List<Cliente>(clientesDict.Values),
                    new List<Producto>(productosDict.Values),
                    ventasList);
        }
    }
}
