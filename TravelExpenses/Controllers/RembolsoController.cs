using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelExpenses.Core;
using TravelExpenses.Data;
using TravelExpenses.ViewModels;

namespace TravelExpenses.Controllers
{
    public class rembolsoController : Controller
    {
        private readonly IRembolso _rembolso;
        private readonly ISolicitudes _solicitud;
        private readonly IComprobante _comprobante;
        private readonly IGasto _gastos;
        private readonly IPolitica _politica;
        private readonly IPoliticaDetalle _politicadetalle;
        private readonly IFormaPago _formaPago;
        private readonly ICatProdServSATDA _prodserv;
        private readonly IHostingEnvironment _env;
        
        public List<Comprobante> lstComprobantes;
        
        public rembolsoController(  IRembolso rembolso, 
                                    ISolicitudes solicitud, 
                                    IHostingEnvironment env, 
                                    IComprobante comprobante,
                                    IGasto gastos,
                                    IPolitica politica,
                                    IPoliticaDetalle politicadetalle,
                                    IFormaPago formaPago,
                                    ICatProdServSATDA prodserv
                                    )
        {
            _rembolso = rembolso;
            _solicitud = solicitud;
            _env = env;
            _comprobante = comprobante;
            _gastos = gastos;
            _politica = politica;
            _politicadetalle = politicadetalle;
            _formaPago = formaPago;
            _prodserv = prodserv;
        }

        // GET: Centro Costos
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Lista()
        {
            
            SolicitudesViewModel solicitud = new SolicitudesViewModel();
            solicitud.Solicitudes = _solicitud.ObtenerSolicitudesXEstatus("PorComprobar");
            return View(solicitud);
        }


        // POST: gastos/Create
        [HttpPost]
        [Authorize]
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

        [Authorize]
        public ActionResult Delete(string UUID)
        {
            try
            {
                var FolioSolicitud =  _comprobante.ObtenerComprobantes().Where(x=>x.UUID == UUID).FirstOrDefault().FolioSolicitud;
                _comprobante.Delete(UUID);
                return RedirectToAction("Edit", "Rembolso", new { Folio = FolioSolicitud.ToString() });
            }
            catch
            {
                return View();
            }
        }

        // GET: gastos/Edit/5
        [Authorize]
        public ActionResult Edit(string Folio)
        {
            int FolioSolicitud = 0;
            var rembolso = new RembolsoViewModel();
            if (int.TryParse(Folio, out FolioSolicitud))
            {                 
                rembolso.Comprobantes = new List<Comprobante>();
                rembolso.Comprobantes = _comprobante.ObtenerComprobantes(FolioSolicitud);

                foreach(Comprobante comp in rembolso.Comprobantes)
                { 
                    if (comp.FormaPago == "01" && comp.Conceptos.Where(x => x.DescripcionProdServ.ToUpper().Contains("GASOLINA") || x.DescripcionProdServ.ToUpper().Contains("ALIMENTO")).Count() > 0)
                    {
                        comp.MensajeError = "La forma de pago para Gasolina o Alimenos debe ser diferente de Efectivo";
                    }
                }

                var solicitud = _solicitud.ObtenerSolicitudes().Where(x=>x.Folio == FolioSolicitud).FirstOrDefault();
                rembolso.solicitud = solicitud;
            }
            var _Gasto = _gastos.ObtenerGastos();

            var misGastos = _solicitud.ObtenerGastos(FolioSolicitud);
            rembolso.MisGastos = misGastos;
            return View( rembolso);
        }

        [Authorize]
        public ActionResult Details(string UUID)
        {
            if (!string.IsNullOrEmpty(UUID))
            { 
                var rembolso = new RembolsoViewModel();
                var polDetalle = _politicadetalle.ObtenerDetalles();
                rembolso.Comprobante = _comprobante.ObtenerComprobantesXID(UUID);

           

                rembolso.Comprobante.ComprobanteXML = rembolso.Comprobante.Archivos.FirstOrDefault().Extension.Contains("xml");
                var gastos = _gastos.ObtenerGastos();

                var result = gastos.Where(a => polDetalle.Any(b => a.IdGasto == b.IdGasto));


                rembolso.Gastos = result;
                return View(rembolso);
            }
            else
                return RedirectToAction("Lista", "Rembolso");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Details(RembolsoViewModel rembolso)
        { 
            if (rembolso.Comprobante != null)
            {
                var politicasdetalle = _politicadetalle.ObtenerDetalles();
                var listagastos = _gastos.ObtenerGastos();
                rembolso.Gastos = listagastos;

                if (rembolso.Concepto != null && rembolso.Concepto.Descripcion != string.Empty)
                {

                    if (rembolso.Comprobante.Conceptos == null)
                    { 
                        rembolso.Comprobante.Conceptos = new List<Concepto>();                        
                    }
                    rembolso.Concepto.CantidadComprobada = rembolso.Concepto.Importe;
                    rembolso.Comprobante.Conceptos.Add(rembolso.Concepto);
                    rembolso.Concepto = new Concepto();
                }

                if (rembolso.Comprobante.Conceptos.Count() > 0)
                {
                    var gastos = rembolso.Comprobante.Conceptos.Select(o => o.IdGasto).Distinct().ToList();
                    foreach (int gasto in gastos)
                    {
                        var Importe = rembolso.Comprobante.Conceptos.Where(x => x.IdGasto == gasto).Sum(x => x.Impuesto + x.Importe);
                        var poldetalle = politicasdetalle.Where(x => x.IdGasto == gasto).FirstOrDefault();
                        
                        if (poldetalle == null)
                        {
                            rembolso.Error = "Selecciono gastos que no cuentan con politica";
                            return View(rembolso);
                        }
                        
                        if (decimal.Parse(Importe.ToString()) > poldetalle.ImportePermitido)
                        {
                                var conceptos = rembolso.Comprobante.Conceptos.Where(x => x.IdGasto == gasto);
                                var politica = _politica.ObtenerPoliticas().Where(x => x.IdPolitica == poldetalle.IdPolitica).FirstOrDefault();

                                foreach (var obj in conceptos)
                                {
                                    obj.MensajeError = politica.MensajeError;    
                                }
                        }                        
                    }

                    
                    _rembolso.GuardarComprobante(rembolso.Comprobante);

                }
                
            }

            return RedirectToAction("Details", "Rembolso", new { UUID = rembolso.Comprobante.UUID });
        }
        // POST: Empresa/Edit/5
        [HttpPost]
        [Authorize]
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
            var NombreArchivo = Path.GetFileNameWithoutExtension(file.FileName);

            var filePath = Path.Combine(_env.ContentRootPath, "UploadFiles", InputFileName);
            return ((System.IO.File.Exists(filePath)) && _rembolso.Exists(NombreArchivo, Extension));
        }

        private List<Comprobante> SaveFile(IFormFile file, int FolioSolicitud)
        {
            var comprobante = new List<Comprobante>();
            var InputFileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(",", "-");
            InputFileName = InputFileName.Replace(" ", "-") + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_env.ContentRootPath, "UploadFiles", InputFileName);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var Extension = Path.GetExtension(file.FileName);

            if (Extension == ".xml")
            {
                comprobante = CargaXML(file);
                foreach (Comprobante comp in comprobante)
                {
                    comp.FolioSolicitud = FolioSolicitud;
                }
            }
            else
            {

                //Generar solicitud
                var miSolicitud = new Solicitud();
                
                miSolicitud.FechaSolicitud = DateTime.Now;
                miSolicitud.IdTipoSolicitud = 2;
                miSolicitud.Departamento = "TI";
                miSolicitud.Empresa = "CAP000101";  //Leer empresa de Usuario
                miSolicitud.ImporteSolicitado = 0;
                miSolicitud.ImporteComprobado = 0;
                miSolicitud.Estatus = "PorComprobar";
                miSolicitud.IdEstado = 0;
                miSolicitud.Id = userId;
                miSolicitud.RFC = "CAP000101";
                miSolicitud.ClaveMoneda = "MXN";


                var miFolioSolicitud = _solicitud.InsertarSolicitud(miSolicitud);
                FolioSolicitud = miFolioSolicitud;


                comprobante.Add (new Comprobante());
                comprobante.FirstOrDefault().UUID = Guid.NewGuid().ToString();
                comprobante.FirstOrDefault().Fecha = DateTime.Now;
                comprobante.FirstOrDefault().Folio = "Sin Folio";
                comprobante.FirstOrDefault().RFC = "XXXX000000XXX";
                comprobante.FirstOrDefault().NombreProveedor = "Sin Proveedor";
                comprobante.FirstOrDefault().FolioSolicitud = miFolioSolicitud;
                comprobante.FirstOrDefault().Impuestos = 0;

            }

            comprobante.FirstOrDefault().Archivos = new List<Archivo>();
            comprobante.FirstOrDefault().Archivos.Add(new Archivo()) ;
            comprobante.FirstOrDefault().Archivos.FirstOrDefault().Extension = Extension;
            comprobante.FirstOrDefault().Archivos.FirstOrDefault().NombreArchivo = InputFileName;
            comprobante.FirstOrDefault().Archivos.FirstOrDefault().Ruta = "\\UploadFile\\" + InputFileName;
            comprobante.FirstOrDefault().Archivos.FirstOrDefault().Extension = Extension;
            comprobante.FirstOrDefault().Archivos.FirstOrDefault().Usuario = userId;
            comprobante.FirstOrDefault().Archivos.FirstOrDefault().FechaAlta = DateTime.Now;
            comprobante.FirstOrDefault().Archivos.FirstOrDefault().UUID = comprobante.FirstOrDefault().UUID;
            

            SaveDB(comprobante.FirstOrDefault());

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            comprobante = _comprobante.ObtenerComprobantes(FolioSolicitud);

            return comprobante;
        }

        [HttpPost]
        [Authorize]
        public  ActionResult  FileUpload(RembolsoViewModel rembolso)
        {
            try
            { 
                IFormFileCollection files = Request.Form.Files;
                //// full path to file in temp location
                var filePath = Path.GetTempFileName();
                var comprobantes = new List<Comprobante>();
                

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        if (!Exists(formFile))
                        {
                            comprobantes = SaveFile(formFile, rembolso.solicitud.Folio);
                            rembolso.Comprobantes = comprobantes;
                        }
                        else
                        {
                            return Json("ERROR-El archivo ya se encuentra registrado");
                        }
                    }
                }

                return Json(comprobantes);                
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Download(string UUID)
        {
            var comprobante =_comprobante.ObtenerComprobantesXID(UUID);            

            string filename = comprobante.Archivos.FirstOrDefault().NombreArchivo;
            if (filename == null)
                return Content("Archivo no encontrado");

            var path = Path.Combine(
                           _env.ContentRootPath,
                           "UploadFiles", filename);
            
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".xml", "text/xml"},
            };
        }

        //[HttpPost]
        //[Authorize]
        //public ActionResult EnviarReembolso(RembolsoViewModel rembolso)
        //{
        //    _solicitud.ActualizarEstatus(rembolso.Comprobante.FolioSolicitud, "Comprobada");
        //    return Redirect("/rembolso/Lista");
        //}

        
        [Authorize]
        public ActionResult EnviarReembolso(int FolioSolicitud)
        {
            _solicitud.ActualizarEstatus(FolioSolicitud, "Comprobada");
            return Redirect("/rembolso/Lista");
        }

        private List<Comprobante>  CargaXML(IFormFile formFile)
        {
            try
            {
                var result = new StringBuilder();
                var comprobante = new List<Comprobante>();
                var catProdServ = _prodserv.ObtenerCatalogo();
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
                                   FormaPago = (string)e.Attribute("FormaPago"),

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
                        if (conceptoXML.Attribute("Base") != null)
                        {
                            concepto.Base = float.Parse(conceptoXML.Attribute("Base").Value);
                        }
                        concepto.ClaveUnidad = conceptoXML.Attribute("ClaveUnidad").Value;
                        concepto.Descripcion = conceptoXML.Attribute("Descripcion").Value;

                        var catprodserv = catProdServ.FirstOrDefault(x => x.ClaveProdServ == concepto.ClaveProdServ);

                        if (catprodserv != null)
                        {
                            concepto.DescripcionProdServ = catprodserv.Descripcion;

                            
                            if ((concepto.DescripcionProdServ.ToUpper().Contains("GASOLINA") || concepto.DescripcionProdServ.ToUpper().Contains("ALIMENTO")))
                            {
                                foreach (Comprobante comp in comprobante)
                                {
                                    if (comp.FormaPago == "01")
                                    {
                                        comp.MensajeError = "La forma de pago para Gasolina o Alimenos debe ser diferente de Efectivo";
                                    }
                                }
                                
                            }

                        }
                        concepto.Importe = float.Parse(conceptoXML.Attribute("Importe").Value);
                        concepto.CantidadComprobada = concepto.Importe;
                        if (conceptoXML.Attribute("Impuesto") != null)
                        {
                            concepto.Impuesto = float.Parse(conceptoXML.Attribute("Impuesto").Value);
                        }
                        concepto.NoIdentificacion = conceptoXML.Attribute("NoIdentificacion").Value;
                        if (conceptoXML.Attribute("TasaOCuota") != null)
                        {
                            concepto.TasaOCuota = float.Parse(conceptoXML.Attribute("TasaOCuota").Value);
                        }
                        if (conceptoXML.Attribute("TipoFactor") != null)
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
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}