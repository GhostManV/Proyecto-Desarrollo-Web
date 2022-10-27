﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.permisos;

namespace WebApplication1.Controllers
{
    [ValidateSession]
    public class PacientesController : Controller
    {
        //recibir una lista de una api 
        private readonly string _url = "https://apiclinica.azurewebsites.net/api/Pacientes";
        public async Task<ActionResult> Index()

        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var listadoPacientes = JsonConvert.DeserializeObject<List<TblPaciente>>(responseString);
                return View(listadoPacientes);
            }



        }
        public ActionResult newPaciente()
        {
            return View();
        }
        //agregar a el json
        [HttpPost]
        //siempre debe ser un model
        public async Task<ActionResult> agregarPacientes(TblPaciente model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            using (var http = new HttpClient())
            {
                var pacienteSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(pacienteSerializada, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }

        //trae la vista con los datos cargados
        [HttpGet]
        [Route("modificar/(id)")]
        public async Task<ActionResult> modificarPaciente(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url + "/" + id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var pacientes = JsonConvert.DeserializeObject<TblPaciente>(responseString);
                return View(pacientes);
            }

        }

        //modifica los datos de la bd
        [HttpPost]
        public async Task<ActionResult> modificarPaciente(TblPaciente model)
        {
            using (var http = new HttpClient())
            {
                var pacienteSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(pacienteSerializada, Encoding.UTF8, "application/json");
                var response = await http.PutAsync(_url + "/" + model.IdPaciente, content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }

        }
        //elimina los datos de la bd
        [HttpGet]
        [Route("eliminar/(id)")]
        public async Task<string> eliminarPaciente(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.DeleteAsync(_url + "/" + id);
                if (!response.IsSuccessStatusCode)
                {
                    return "Error";
                }
                return "Exito";
            }
        }

    }
}