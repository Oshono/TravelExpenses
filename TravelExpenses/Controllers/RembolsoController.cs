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
using Microsoft.AspNetCore.Hosting;

namespace TravelExpenses.Controllers
{
    public class rembolsoController : Controller
    {
        private readonly IRembolso _rembolso;
        private readonly ISolicitudes _solicitud;
        private readonly IComprobante _comprobante;
        private readonly IGasto _gastos;
        private readonly IPolitica _politica;
        private readonly IHostingEnvironment _env;
        
        public List<Comprobante> lstComprobantes;
        
        public rembolsoController(  IRembolso rembolso, 
                                    ISolicitudes solicitud, 
                                    IHostingEnvironment env, 
                                    IComprobante comprobante,
                                    IGasto gastos,
                                    IPolitica politica
                                    )
        {
            _rembolso = rembolso;
            _solicitud = solicitud;
            _env = env;
            _comprobante = comprobante;
            _gastos = gastos;
            _politica = politica;
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
        public ActionResult Edit(string Folio)
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

        public ActionResult Details(string UUID)
        {
            var rembolso = new RembolsoViewModel();
            rembolso.Comprobante = _comprobante.ObtenerComprobantesXID(UUID);
            var gastos = _gastos.ObtenerGastos();
            rembolso.Gastos = gastos;
            return View(rembolso);
        }

        [HttpPost]
        public ActionResult Details(RembolsoViewModel rembolso)
        { 
            if (rembolso.Comprobante != null)
            {
                var politicas = _politica.ObtenerPoliticas();

                if (rembolso.Comprobante.Conceptos.Count() > 0)
                {
                    var gastos = rembolso.Comprobante.Conceptos.Select(o => o.IdGasto).Distinct().ToList();
                    foreach (int gasto in gastos)
                    {
                        var Importe = rembolso.Comprobante.Conceptos.Where(x => x.IdGasto == gasto).Sum(x => x.Impuesto + x.Importe);
                        var pol = politicas.Where(x => x.IdGasto == gasto).FirstOrDefault();
                        if (pol != null && decimal.Parse(Importe.ToString()) > pol.ImportePermitido)
                        {
                                var conceptos = rembolso.Comprobante.Conceptos.Where(x => x.IdGasto == gasto);

                                foreach (var obj in conceptos)
                                {
                                    obj.MensajeError = pol.MensajeError;    
                                }
                            var listagastos = _gastos.ObtenerGastos();
                            rembolso.Gastos = listagastos;
                            return View(rembolso);
                        }
                        
                    }
                    
                }
                _rembolso.GuardarComprobante(rembolso.Comprobante);
            }

            return View(rembolso);
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

        
        private int SaveDB(Comprobante comprobante)
        {
            var Id = _rembolso.Guardar(comprobante);

            return Id;
        }

        private bool Exists(IFormFile file)
        {
            var InputFileName = Path.GetFileName(file.FileName).Replace(",", "-");
            InputFileName = InputFileName.Replace(" ", "-");
            var Extension = Path.GetExtension(file.FileName).Replace(".", "");            
            var NombreArchivo = Path.GetFileName(file.FileName);

            var filePath = Path.Combine(_env.ContentRootPath, "UploadFiles", InputFileName);
            return ((System.IO.File.Exists(filePath)) && _rembolso.Exists(NombreArchivo, Extension));
        }

        private List<Comprobante> SaveFile(IFormFile file)
        {
            var comprobante = new List<Comprobante>();
            var InputFileName = Path.GetFileName(file.FileName).Replace(",", "-");
            InputFileName = InputFileName.Replace(" ", "-");
            var filePath = Path.Combine(_env.ContentRootPath, "UploadFiles", InputFileName);

            var Extension = Path.GetExtension(file.FileName);



            if (Extension == ".xml")
            {
                comprobante = CargaXML(file);
            }
            else
            {
                comprobante.Add (new Comprobante());
                comprobante.FirstOrDefault().UUID = Guid.NewGuid().ToString();
                comprobante.FirstOrDefault().Fecha = DateTime.Now;
                comprobante.FirstOrDefault().Folio = "Sin Folio";
                comprobante.FirstOrDefault().RFC = "XXXX000000XXX";
                comprobante.FirstOrDefault().NombreProveedor = "Sin Proveedor";
            }

            comprobante.FirstOrDefault().Archivo = new Archivo();
            comprobante.FirstOrDefault().Archivo.Extension = Extension;
            comprobante.FirstOrDefault().Archivo.NombreArchivo = InputFileName;
            comprobante.FirstOrDefault().Archivo.Ruta = "\\UploadFile\\" + InputFileName;
            comprobante.FirstOrDefault().Archivo.Extension = Extension;
            comprobante.FirstOrDefault().Archivo.Usuario = "b2b8325e-5ff6-4a36-b028-48b1c0f87c6e";
            comprobante.FirstOrDefault().Archivo.FechaAlta = DateTime.Now;
            comprobante.FirstOrDefault().Archivo.UUID = comprobante.FirstOrDefault().UUID;
            

            SaveDB(comprobante.FirstOrDefault());

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return comprobante;
        }

        [HttpPost]
        public  ActionResult  FileUpload(RembolsoViewModel rembolso)
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
                        if (!Exists(formFile))
                        {
                            comprobante = SaveFile(formFile);
                        }
                        else
                        {
                            return Json("ERROR-El archivo ya se encuentra registrado");
                        }
                    }
                } 

                return Json(comprobante);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private List<Comprobante>  CargaXML(IFormFile formFile)
        {
            var result = new StringBuilder();
            var comprobante = new List<Comprobante>();
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }

            XNamespace nsCFDi = "http://www.sat.gob.mx/cfd/3";
            XNamespace tfd = "http://www.sat.gob.mx/TimbreFiscalDigital";
            XDocument archivoXML = XDocument.Parse(result.ToString());


            comprobante = (from e in archivoXML.Elements(nsCFDi + "Comprobante")
                               let r = e.Element(nsCFDi + "Emisor")
                               let i = e.Element(nsCFDi + "Impuestos")
                               let c = e.Element(nsCFDi + "Complemento").Element(tfd + "TimbreFiscalDigital")
                               select new Comprobante
                               {
                                   UUID = (string)c.Attribute("UUID"),
                                   Folio = (string)e.Attribute("Folio"),
                                   Fecha = (DateTime)e.Attribute("Fecha"),
                                   Moneda = (string)e.Attribute("Moneda"),
                                   SubTotal = (float)e.Attribute("SubTotal"),
                                   Total = (float)e.Attribute("Total"),
                                   Impuestos = (float)i.Attribute("TotalImpuestosTrasladados"),
                                   RFC = (string)r.Attribute("Rfc"),
                                   NombreProveedor = (string)r.Attribute("Nombre"),
                                   RegimenFiscal = (string)r.Attribute("RegimenFiscal"),

                               }                           
                           ).ToList();

            
            List<Concepto> listaConceptos = new List<Concepto>();
            
            comprobante[0].Conceptos = listaConceptos;
            var conceptos = from c in archivoXML.Descendants()
                            where c.Name.LocalName == "Conceptos"
                            select c.Elements();

            foreach (IEnumerable<XElement> item in conceptos)
            {
                foreach (var conceptoXML in item)
                {
                    Concepto concepto = new Concepto();
                    concepto.UUID = comprobante.FirstOrDefault().UUID;
                    concepto.Cantidad = float.Parse(conceptoXML.Attribute("Cantidad").Value);
                    concepto.ClaveProdServ = conceptoXML.Attribute("ClaveProdServ").Value;                    
                    if ( conceptoXML.Attribute("Base") != null)
                    {
                        concepto.Base = float.Parse(conceptoXML.Attribute("Base").Value);
                    }
                    concepto.Cantidad = float.Parse(conceptoXML.Attribute("ClaveProdServ").Value);
                    concepto.ClaveUnidad = conceptoXML.Attribute("ClaveUnidad").Value;
                    concepto.Descripcion = conceptoXML.Attribute("Descripcion").Value;
                    concepto.Importe = float.Parse(conceptoXML.Attribute("Importe").Value);
                    if (conceptoXML.Attribute("Impuesto") != null)
                    {
                        concepto.Impuesto = float.Parse(conceptoXML.Attribute("Impuesto").Value);
                    }
                    concepto.NoIdentificacion = conceptoXML.Attribute("NoIdentificacion").Value;
                    if ( conceptoXML.Attribute("TasaOCuota") != null)
                    {
                        concepto.TasaOCuota = float.Parse(conceptoXML.Attribute("TasaOCuota").Value);
                    }
                    if ( conceptoXML.Attribute("TipoFactor") != null)
                    {
                        concepto.TipoFactor = conceptoXML.Attribute("TipoFactor").Value;
                    }
                    concepto.Unidad = conceptoXML.Attribute("Unidad").Value;
                    concepto.ValorUnitario = float.Parse(conceptoXML.Attribute("ValorUnitario").Value);
                    listaConceptos.Add(concepto);
                }
            }

            return comprobante;
        }

    }
}