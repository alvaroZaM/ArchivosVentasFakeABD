using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArchivosVentasFakeABD.Logica;
using ArchivosVentasFakeABD.Modelos;

namespace ArchivosVentasFakeABD
{
    public partial class Form1 : Form
    {
        private List<string> archivosSeleccionados = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCargarYGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // cadenaConexion a BD
                string cadenaConexion = @"Server=localhost;Database=ventas_fake;Trusted_Connection=True;TrustServerCertificate=True;";

                var logicaVentas = new LogicaVentas(cadenaConexion);

                List<Cliente> clientes = new List<Cliente>();
                List<Producto> productos = new List<Producto>();
                List<Venta> ventas = new List<Venta>();

                
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Multiselect = true;
                    openFileDialog.Filter = "Archivos JSON, XML, TXT|*.json;*.xml;*.txt";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string archivo in openFileDialog.FileNames)
                        {
                            string extension = System.IO.Path.GetExtension(archivo).ToLower();
                            lstArchivos.Items.Add(archivo);


                            switch (extension)
                            {
                                case ".json":
                                    var cargadorJson = new Servicios.CargarJson(archivo);
                                    var resultadoJson = cargadorJson.CargarYProcesar();
                                    clientes.AddRange(resultadoJson.Clientes);
                                    productos.AddRange(resultadoJson.Productos);
                                    ventas.AddRange(resultadoJson.Ventas);
                                    break;

                                case ".xml":
                                    var cargadorXml = new Servicios.CargarXml(archivo);  
                                    var resultadoXml = cargadorXml.CargarYProcesar();
                                    clientes.AddRange(resultadoXml.Clientes);
                                    productos.AddRange(resultadoXml.Productos);
                                    ventas.AddRange(resultadoXml.Ventas);
                                    break;

                                case ".txt":
                                    var cargadorTxt = new Servicios.CargarTxt(archivo); 
                                    var resultadoTxt = cargadorTxt.CargarYProcesar();
                                    clientes.AddRange(resultadoTxt.Clientes);
                                    productos.AddRange(resultadoTxt.Productos);
                                    ventas.AddRange(resultadoTxt.Ventas);
                                    break;

                                default:
                                    MessageBox.Show($"Extensión no soportada: {extension}");
                                    break;
                            }
                        }

                        // Limpiar duplicados globales antes de guardar
                        clientes = clientes.GroupBy(c => c.CodCli).Select(g => g.First()).ToList();
                        productos = productos.GroupBy(p => p.CodProd).Select(g => g.First()).ToList();

                        //limpiar ventas duplicadas basadas en Folio + Cliente + Fecha
                        ventas = ventas
                            .GroupBy(v => new { v.Folio, v.CodCli, v.Fecha })
                            .Select(g => g.First())
                            .ToList();

                        // Se guarda en BD
                        logicaVentas.GuardarDatosEnBaseDeDatos(clientes, productos, ventas);

                        MessageBox.Show("Datos cargados y guardados correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



    }
}