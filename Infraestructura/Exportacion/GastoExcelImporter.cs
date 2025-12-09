using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.Interfaces.Infraestructura;
using OfficeOpenXml;

namespace Infraestructura.Exportacion
{
    public class GastoExcelImporter : IGastoImport
    {
        public async Task<List<GastoCreateDTO>> ImportarExcel(Stream archivoStream, Guid usuarioId)
        {


            var gastosImportados = new List<GastoCreateDTO>();

            using (var package = new ExcelPackage(archivoStream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Primer hoja
                int rowCount = worksheet.Dimension.Rows;

                // Asumimos que la fila 1 son encabezados: 
                // A: Encabezado, B: Monto, C: Fecha (YYYY-MM-DD), D: Descripcion
                // NOTA: Para Categoria y MetodoDePago, en un caso real necesitarías 
                // buscar sus IDs por nombre. Aquí dejaremos IDs dummy o null para que 
                // el servicio lo maneje o el usuario lo seleccione después.

                for (int row = 2; row <= rowCount; row++)
                {
                    var encabezado = worksheet.Cells[row, 1].Value?.ToString();
                    var montoObj = worksheet.Cells[row, 2].Value;
                    var fechaObj = worksheet.Cells[row, 3].Value;
                    var descripcion = worksheet.Cells[row, 4].Value?.ToString();

                    if (string.IsNullOrEmpty(encabezado) || montoObj == null)
                        continue;

                    decimal monto = Convert.ToDecimal(montoObj);

                    // Parseo de fecha simple
                    DateOnly fecha = DateOnly.FromDateTime(DateTime.Now);
                    if (fechaObj != null && DateTime.TryParse(fechaObj.ToString(), out DateTime fTemp))
                    {
                        fecha = DateOnly.FromDateTime(fTemp);
                    }

                    var dto = new GastoCreateDTO
                    {
                        Encabezado = encabezado,
                        Monto = monto, // El setter es private en tu DTO, asegúrate de que se pueda setear o usa constructor
                        UsuarioId = usuarioId,
                        Descripcion = descripcion,
                        IsFechaActual = false,
                        Fecha = fecha,
                        // IDs Temporales o Default, la lógica de negocio debería validar esto
                        CategoriaId = Guid.Empty,
                        MetodoDePagoId = Guid.Empty
                    };

                    // Nota: Como 'Monto' tiene setter privado en tu código provisto, 
                    // necesitarás ajustar el DTO o usar reflexión. 
                    // Asumiré aquí que cambiarás el setter a public o internal.

                    gastosImportados.Add(dto);
                }
            }

            return await Task.FromResult(gastosImportados);
        }
    }
}
