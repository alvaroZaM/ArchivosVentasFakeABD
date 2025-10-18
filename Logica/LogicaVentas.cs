using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ArchivosVentasFakeABD.Modelos;

namespace ArchivosVentasFakeABD.Logica
{
    public class LogicaVentas
    {
        private readonly string _cadenaConexion;

        public LogicaVentas(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion ?? throw new ArgumentNullException(nameof(cadenaConexion));
        }

        public void GuardarDatosEnBaseDeDatos(List<Cliente> clientes, List<Producto> productos, List<Venta> ventas)
        {
            using (var conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();

                var transaction = conexion.BeginTransaction();

                try
                {
                    // Insertar Clientes
                    foreach (var cliente in clientes)
                    {
                        string sql = "IF NOT EXISTS (SELECT 1 FROM Clientes WHERE CodCli = @CodCli) " +
                                     "INSERT INTO Clientes (CodCli, Nombre) VALUES (@CodCli, @Nombre)";
                        using (var cmd = new SqlCommand(sql, conexion, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CodCli", cliente.CodCli);
                            cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Insertar Productos
                    foreach (var producto in productos)
                    {
                        string sql = "IF NOT EXISTS (SELECT 1 FROM Productos WHERE CodProd = @CodProd) " +
                                     "INSERT INTO Productos (CodProd, Nombre, Precio) VALUES (@CodProd, @Nombre, @Precio)";
                        using (var cmd = new SqlCommand(sql, conexion, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CodProd", producto.CodProd);
                            cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                            cmd.Parameters.AddWithValue("@Precio", producto.PrecioReferencia);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Insertar Ventas y Detalles
                    foreach (var venta in ventas)
                    {
                        int clienteId = ObtenerIdCliente(conexion, transaction, venta.CodCli);

                        string sqlVenta = "INSERT INTO Ventas (Folio, ClienteID, Fecha, Total) " +
                                          "VALUES (@Folio, @ClienteID, @Fecha, @Total); " +
                                          "SELECT SCOPE_IDENTITY();";

                        int ventaId;
                        using (var cmd = new SqlCommand(sqlVenta, conexion, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Folio", venta.Folio);
                            cmd.Parameters.AddWithValue("@ClienteID", clienteId);
                            cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                            cmd.Parameters.AddWithValue("@Total", venta.Total);

                            ventaId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        foreach (var detalle in venta.Detalles)
                        {
                            int productoId = ObtenerIdProducto(conexion, transaction, detalle.CodProd);

                            string sqlDetalle = "INSERT INTO VentasDetalle (VentaID, ProductoID, Cantidad, PrecioUnitario, Subtotal) " +
                                                "VALUES (@VentaID, @ProductoID, @Cantidad, @PrecioUnitario, @Subtotal)";
                            using (var cmd = new SqlCommand(sqlDetalle, conexion, transaction))
                            {
                                cmd.Parameters.AddWithValue("@VentaID", ventaId);
                                cmd.Parameters.AddWithValue("@ProductoID", productoId);
                                cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                                cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                                cmd.Parameters.AddWithValue("@Subtotal", detalle.Subtotal);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error al guardar datos: " + ex.Message);
                    throw;
                }
            }
        }

        private int ObtenerIdCliente(SqlConnection conexion, SqlTransaction tx, string codCli)
        {
            string sql = "SELECT ClienteID FROM Clientes WHERE CodCli = @CodCli";
            using (var cmd = new SqlCommand(sql, conexion, tx))
            {
                cmd.Parameters.AddWithValue("@CodCli", codCli);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int ObtenerIdProducto(SqlConnection conexion, SqlTransaction tx, string codProd)
        {
            string sql = "SELECT ProductoID FROM Productos WHERE CodProd = @CodProd";
            using (var cmd = new SqlCommand(sql, conexion, tx))
            {
                cmd.Parameters.AddWithValue("@CodProd", codProd);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
