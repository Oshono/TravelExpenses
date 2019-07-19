using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.ViewModels;
using System.Web;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using System.Text;


namespace TravelExpenses.Controllers
{
    public class rembolsoController : Controller
    {
        private readonly IRembolso _rembolso;
        private readonly ISolicitudes _solicitud;

        public rembolsoController(IRembolso rembolso, ISolicitudes solicitud)
        {
            _rembolso = rembolso;
            _solicitud = solicitud;
        }

        // GET: Centro Costos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            
            SolicitudesViewModel solicitud = new SolicitudesViewModel();
            solicitud.Solicitudes = _solicitud.ObtenerSolicitudesXEstatus("PorComprobar");
            return View(solicitud);
        }


        // POST: gastos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: gastos/Edit/5
        public ActionResult Edit(string ClaveCentroCosto)
        {
            var rembolso = new RembolsoViewModel();
            rembolso.Comprobantes = new List<Comprobante>();
            //var centroModel = new CentroCostoViewModel();
            //centroModel.CentroCosto = new CentroCosto();
            //if (!string.IsNullOrEmpty(ClaveCentroCosto))
            //{
            //    var centro = _centro.ObtenerCentroCostos()
            //                .Where(x => x.ClaveCentroCosto == ClaveCentroCosto)
            //                .FirstOrDefault();
            //    centroModel.CentroCosto = centro;
            //}
            return View( rembolso);
        }

        // POST: Empresa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CentroCostoViewModel centroCostoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }             
            try
            {
                //_centro.Guardar(centroCostoModel.CentroCosto);
            }
            catch
            {

            }

            return Redirect("/CentroCosto/Lista");
        }

        [HttpPost]
        public  ActionResult  FileUpload(IEnumerable<IFormFile> filex)
        {
            try
            { 
                IFormFileCollection files = Request.Form.Files;
                //// full path to file in temp location
                var filePath = Path.GetTempFileName();
                var comprobante = new List<Comprobante>();

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {

                        var result = new StringBuilder();
                        using (var reader = new StreamReader(formFile.OpenReadStream()))
                        {
                            while (reader.Peek() >= 0)
                                result.AppendLine(reader.ReadLine());
                        }

                         


                        XNamespace nsCFDi = "http://www.sat.gob.mx/cfd/3"; // para que pueda identificar el prefijo CFDI
                            XDocument archivoXML = XDocument.Parse (result.ToString()); // selecciona y abre la factura electrónica xml

                            
                         
                            comprobante = (from e in archivoXML.Elements(nsCFDi + "Comprobante")
                                               let r = e.Element(nsCFDi + "Emisor")
                                               select new Comprobante
                                               {
                                                   Folio = (string)e.Attribute("Folio"),
                                                   Fecha = (string)e.Attribute("Folio"),
                                                   Moneda = (string)e.Attribute("Moneda"),
                                                   SubTotal = (float)e.Attribute("SubTotal"),
                                                   Total = (float)e.Attribute("Total"),
                                                   Impuestos = (float)e.Attribute("Total") - (float)e.Attribute("SubTotal"),
                                                   RFC = (string)r.Attribute("Rfc"),
                                                   NombreProveedor = (string)r.Attribute("Nombre"),
                                                   RegimenFiscal = (string)r.Attribute("RegimenFiscal"),

                                               }).ToList();

                       



                    }
                } 

                return Json(comprobante);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
 

    }
}