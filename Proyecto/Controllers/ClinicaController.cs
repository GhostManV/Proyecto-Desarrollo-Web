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
    public class ClinicaController : Controller
    {
        //recibir una lista de una api 
<<<<<<< HEAD
        private readonly string _url = "https://apiclinica.azurewebsites.net/api/Clinicas";
=======
        private readonly string _url = "https://63572b429243cf412f942721.mockapi.io/prueba3/Clinica";
>>>>>>> 18857b6bb0833709fb4ab1c219a7f8f5bc7055d6
        public async Task<ActionResult> Index()

        {

            //https://63572b429243cf412f942721.mockapi.io/prueba3/Clinica
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
<<<<<<< HEAD
                var listadoClinica = JsonConvert.DeserializeObject<List<TblClinica>>(responseString);
=======
                var listadoClinica = JsonConvert.DeserializeObject<List<Clinica>>(responseString);
>>>>>>> 18857b6bb0833709fb4ab1c219a7f8f5bc7055d6
                return View(listadoClinica);
            }



        }
        public ActionResult newClinic()
        {
            return View();
        }
        //agregar a el json
        [HttpPost]
        //siempre debe ser un model
<<<<<<< HEAD
        public async Task<ActionResult> agregarClinica(TblClinica model)
=======
        public async Task<ActionResult> agregarClinica(Clinica model)
>>>>>>> 18857b6bb0833709fb4ab1c219a7f8f5bc7055d6
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            using (var http = new HttpClient())
            {
                var clinicaSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(clinicaSerializada, Encoding.UTF8, "application/json");
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
        public async Task<ActionResult> modificarClinica(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url + "/" + id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
<<<<<<< HEAD
                var clinica = JsonConvert.DeserializeObject<TblClinica>(responseString);
=======
                var clinica = JsonConvert.DeserializeObject<Clinica>(responseString);
>>>>>>> 18857b6bb0833709fb4ab1c219a7f8f5bc7055d6
                return View(clinica);
            }

        }

        //modifica los datos de la bd
        [HttpPost]
<<<<<<< HEAD
        public async Task<ActionResult> modificarClinica(TblClinica model)
=======
        public async Task<ActionResult> modificarClinica(Clinica model)
>>>>>>> 18857b6bb0833709fb4ab1c219a7f8f5bc7055d6
        {
            using (var http = new HttpClient())
            {
                var clinicaSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(clinicaSerializada, Encoding.UTF8, "application/json");
<<<<<<< HEAD
                var response = await http.PutAsync(_url + "/" + model.IdClinica, content);
=======
                var response = await http.PutAsync(_url + "/" + model.idClinica, content);
>>>>>>> 18857b6bb0833709fb4ab1c219a7f8f5bc7055d6
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
        public async Task<string> eliminarClinica(int id)
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