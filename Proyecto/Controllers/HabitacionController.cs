﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.permisos;

namespace WebApplication1.Controllers
{
    [ValidateSession]
    public class HabitacionController : Controller
    {
        //recibir una lista de una api 
        private readonly string _url = "https://63572d5b2712d01e14036ea9.mockapi.io/pruebas4/LoteProducto";
        public async Task<ActionResult> Index()

        {

            //https://63572d5b2712d01e14036ea9.mockapi.io/pruebas4
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var listadoHabitacion = JsonConvert.DeserializeObject<List<HabitacionesViewModel>>(responseString);
                return View(listadoHabitacion);
            }



        }

        
        public async Task<ActionResult> newHabitaciones()
        {


            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var listadoHabitacion = JsonConvert.DeserializeObject<List<HabitacionesViewModel>>(responseString);

               var listadoClinica = listadoHabitacion.ConvertAll(r =>
               {
                   return new SelectListItem()
                   {
                       Text = r.nombre,
                       Value = r.idClinica.ToString(),
                       Selected = false
                   };
               });
                ViewBag.listadoClinica = listadoClinica;

                return View();
            }        
            }
        //agregar a el json
        [HttpPost]
        //siempre debe ser un model
        public async Task<ActionResult> agregarHabitacion(HabitacionesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            using (var http = new HttpClient())
            {
                var habitacionSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(habitacionSerializada, Encoding.UTF8, "application/json");
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
        public async Task<ActionResult> modificarHabitacion(int id)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_url + "/" + id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var habitacion = JsonConvert.DeserializeObject<HabitacionesViewModel>(responseString);
                return View(habitacion);
            }

        }

        //modifica los datos de la bd
        [HttpPost]
        public async Task<ActionResult> modificarHabitacion(HabitacionesViewModel model)
        {
            using (var http = new HttpClient())
            {
                var habitacionSerializada = JsonConvert.SerializeObject(model);
                var content = new StringContent(habitacionSerializada, Encoding.UTF8, "application/json");
                var response = await http.PutAsync(_url + "/" + model.idHabitacion, content);
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
        public async Task<string> eliminarHabitacion(int id)
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