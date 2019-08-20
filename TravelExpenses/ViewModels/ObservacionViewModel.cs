using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Core;

namespace TravelExpenses.ViewModels
{
    public class ObservacionViewModel
    {
        public IEnumerable<Observacion> ListObservacion { get; set; }
        public Observacion Observacion { get; set; }
        public int Operacion { get; set; }
        public IEnumerable<Comprobante> Comprobantes { get; set; }
        public Comprobante Comprobante { get; set; }
        public IEnumerable<Gastos> Gastos { get; set; }
        public IEnumerable<Gasto> _GastosA { get; set; }
        public IEnumerable<Solicitud> Solicitudes { get; set; }
        public Solicitud Solicitud { get; set; }
        public int IdTipoSolicitud { get; set; }
        public IEnumerable<Destinos> Destinos { get; set; }
        public IEnumerable<Moneda> Monedas { get; set; }
        public Gasto Gasto { get; set; }
        public Concepto Concepto { get; set; }
        public string Error { get; set; }
        public IEnumerable<Estatus> Estatuses { get; set; }
        public Estatus Estatus { get; set; }
        public IEnumerable<Comentarios> comentarios { get; set; }
        public Comentarios comentario { get; set; }
        
        public IEnumerable<Archivo> Archivos { get; set; }
        public Archivo miArchivo { get; set; }
        public Solicitud solicitud { get; set; }
        public IEnumerable<Gasto> MisGastos { get; set; }
        public bool DetallesAsociados { get; set; }

         public string Array { get; set; }

    }
}
