﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.permisos;
using static System.Net.WebRequestMethods;

namespace WebApplication1.Controllers
{
    [ValidateSession]
    public class ProductoController : Controller
    {
        private readonly string _urlProductos = "https://apiclinica.azurewebsites.net/api/Productos";
        private readonly string _urlMarcas = "https://apiclinica.azurewebsites.net/api/Marcas";
        private readonly string _urlLote = "https://apiclinica.azurewebsites.net/api/LoteProducto";
        private readonly string _urlClinica = "https://apiclinica.azurewebsites.net/api/Clinicas";
        // GET: Productos
        public async Task<ActionResult> Index()
        {
            using (var _http = new HttpClient())
            {
                var response = await _http.GetAsync(_urlProductos);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var listadoProductos = JsonConvert.DeserializeObject<List<ProductosViewModel>>(responseString);
                return View(listadoProductos);
            }

        }
        public async Task<ActionResult> newProducto()
        {
            using (var _http = new HttpClient())
            {
                var responseMarcas = await _http.GetAsync(_urlMarcas);
                if (responseMarcas.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseClinicas = await _http.GetAsync(_urlClinica);
                if (responseClinicas.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseLotes = await _http.GetAsync(_urlLote);
                if (responseLotes.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseStringMarcas = await responseMarcas.Content.ReadAsStringAsync();
                var listMarcas = JsonConvert.DeserializeObject<List<TblMarca>>(responseStringMarcas);

                var responseStringClinica = await responseClinicas.Content.ReadAsStringAsync();
                var listClinicas = JsonConvert.DeserializeObject<List<TblClinica>>(responseStringClinica);

                var responseStringLotes = await responseLotes.Content.ReadAsStringAsync();
                var listadoLotes = JsonConvert.DeserializeObject<List<TblLoteProducto>>(responseStringLotes);
                var listadoLote = listadoLotes.ConvertAll(r =>
                {
                    return new SelectListItem()
                    {
                        Text = r.Descripcion,
                        Value = r.IdLoteProducto.ToString(),
                        Selected = false
                    };
                });
                var listClinica = listClinicas.ConvertAll(r =>
                {
                    return new SelectListItem()
                    {
                        Text = r.Nombre,
                        Value = r.IdClinica.ToString(),
                        Selected = false
                    };
                });
                var listMarca = listMarcas.ConvertAll(r =>
                {
                    return new SelectListItem()
                    {
                        Text = r.Marca,
                        Value = r.IdMarca.ToString(),
                        Selected = false
                    };
                });
                ViewBag.listadoLoteProducto= listadoLote;
                ViewBag.listadoClinica= listClinica;
                ViewBag.listadoMarca= listMarca;
                return View();
            }
           
        }


        [HttpPost]
        public async Task<ActionResult> AddProduct (ProductosViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
                
            string rutaSitio = Server.MapPath("~/images/Products/");
            string pathImagen = Path.Combine(rutaSitio + model.ImagenFile.FileName);
            model.ImagenFile.SaveAs(pathImagen);
            using (var http = new HttpClient())
            {
                var oProducto = new TblProducto();
                oProducto.IdLoteProducto = model.IdLoteProducto;
                oProducto.IdClinica = model.IdClinica;
                oProducto.IdMarca = model.IdMarca;
                oProducto.Nombre = model.Nombre;
                oProducto.Descripcion = model.Descripcion;
                oProducto.Precio = model.Precio;
                oProducto.Existencia = model.Existencia;
                oProducto.Imagen = "/images/Products/" + model.ImagenFile.FileName;
                
                var productSerializer = JsonConvert.SerializeObject(oProducto);
                var content = new StringContent(productSerializer, Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_urlProductos, content);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
<<<<<<< HEAD
                return RedirectToAction("Index");
            }   
=======
            };
            var listadoLoteProducto = dataLoteProducto.ConvertAll(r =>
            {
                return new SelectListItem()
                {
                    Text = r.Descripcion,
                    Value = r.IdLoteProductos.ToString(),
                    Selected = false
                };
            });
            ViewBag.listadoLoteProducto = listadoLoteProducto;

            var dataClinica = new List<Clinica>()
            {
                new Clinica()
                {
                    idClinica=1,
                    nombre="Clinica 1",
                    direccion = "Clinica 1"
                },
                new Clinica()
                {
                    idClinica=1,
                    nombre="Clinica 2",
                    direccion = "Clinica 2"
                }
            };
            var listadoClinica = dataClinica.ConvertAll(r =>
            {
                return new SelectListItem()
                {
                    Text = r.nombre + "," + r.direccion,
                    Value = r.idClinica.ToString(),
                    Selected = false
                };
            });
            ViewBag.listadoClinica = listadoClinica;

            var dataMarca = new List<Marcas>()
            {
                new Marcas()
                {
                    IdMarca=1,
                    Marca="Marca 1"
                },
                new Marcas()
                {
                    IdMarca=2,
                    Marca="Marca 2"
                },
                new Marcas()
                {
                    IdMarca=3,
                    Marca="Marca 3"
                }
            };
            var listadoMarca = dataMarca.ConvertAll(r =>
            {
                return new SelectListItem()
                {
                    Text = r.Marca,
                    Value = r.IdMarca.ToString(),
                    Selected = false
                };
            });
            ViewBag.listadoMarca = listadoMarca;
            return View();
>>>>>>> 18857b6bb0833709fb4ab1c219a7f8f5bc7055d6
        }

        public async Task<ActionResult> ActualizarProducto(int? id)
        {
            using (var _http = new HttpClient())
            {
                var response = await _http.GetAsync(_urlProductos+"/"+id);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseString = await response.Content.ReadAsStringAsync();
                var ProductoSeleccionado = JsonConvert.DeserializeObject<ProductosViewModel>(responseString);


                var responseMarcas = await _http.GetAsync(_urlMarcas);
                if (responseMarcas.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseClinicas = await _http.GetAsync(_urlClinica);
                if (responseClinicas.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseLotes = await _http.GetAsync(_urlLote);
                if (responseLotes.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return View("Error");
                }
                var responseStringMarcas = await responseMarcas.Content.ReadAsStringAsync();
                var listMarcas = JsonConvert.DeserializeObject<List<TblMarca>>(responseStringMarcas);

                var responseStringClinica = await responseClinicas.Content.ReadAsStringAsync();
                var listClinicas = JsonConvert.DeserializeObject<List<TblClinica>>(responseStringClinica);

                var responseStringLotes = await responseLotes.Content.ReadAsStringAsync();
                var listadoLotes = JsonConvert.DeserializeObject<List<TblLoteProducto>>(responseStringLotes);
                var listadoLote = listadoLotes.ConvertAll(r =>
                {
                    return new SelectListItem()
                    {
                        Text = r.Descripcion,
                        Value = r.IdLoteProducto.ToString(),
                        Selected = false
                    };
                });
                var listClinica = listClinicas.ConvertAll(r =>
                {
                    return new SelectListItem()
                    {
                        Text = r.Nombre,
                        Value = r.IdClinica.ToString(),
                        Selected = false
                    };
                });
                var listMarca = listMarcas.ConvertAll(r =>
                {
                    return new SelectListItem()
                    {
                        Text = r.Marca,
                        Value = r.IdMarca.ToString(),
                        Selected = false
                    };
                });
                ViewBag.listadoLoteProducto = listadoLote;
                ViewBag.listadoClinica = listClinica;
                ViewBag.listadoMarca = listMarca;
                return View(ProductoSeleccionado);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ModificarProducto(ProductosViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            if (model.ImagenFile != null)
            {
                string rutaSitio = Server.MapPath("~/images/Products/");
                string pathImagen = Path.Combine(rutaSitio + model.ImagenFile.FileName);
                model.ImagenFile.SaveAs(pathImagen);
                using (var _http = new HttpClient())
                {
                    var productos = new TblProducto();
                    productos.IdProducto = model.IdProducto;
                    productos.Nombre = model.Nombre;
                    productos.IdLoteProducto = model.IdLoteProducto;
                    productos.IdClinica = model.IdClinica;
                    productos.IdMarca = model.IdMarca;
                    productos.Descripcion = model.Descripcion;
                    productos.Precio = model.Precio;
                    productos.Existencia = model.Existencia;
                    productos.Imagen = "/images/Products/" + model.ImagenFile.FileName;
                    var ProductoSerializado = JsonConvert.SerializeObject(productos);
                    var content = new StringContent(ProductoSerializado, Encoding.UTF8, "application/json");
                    var response = await _http.PutAsync(_urlProductos + "/" + model.IdProducto, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        return View("Error");
                    }
                    return RedirectToAction("Index");
                }
            }
            //si es un producto a modificar y la imagen no se modifica
            else
            {
                using (var _http = new HttpClient())
                {
                    var productos = new TblProducto();
                    productos.IdProducto = model.IdProducto;
                    productos.Nombre = model.Nombre;
                    productos.IdLoteProducto = model.IdLoteProducto;
                    productos.IdClinica = model.IdClinica;
                    productos.IdMarca = model.IdMarca;
                    productos.Descripcion = model.Descripcion;
                    productos.Precio = model.Precio;
                    productos.Existencia = model.Existencia;
                    productos.Imagen = model.Imagen;
                    var ProductoSerializado = JsonConvert.SerializeObject(productos);
                    var content = new StringContent(ProductoSerializado, Encoding.UTF8, "application/json");
                    var response = await _http.PutAsync(_urlProductos + "/" + model.IdProducto, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        return View("Error");
                    }
                    return RedirectToAction("Index");
                }
            }
        }

        public async Task<ActionResult> EliminarProducto(int? id)
        {
            using (var _http = new HttpClient())
            {
                var response = await _http.DeleteAsync(_urlProductos + "/" + id);
                if (!response.IsSuccessStatusCode)
                {
                    return View("Error");
                }
                return RedirectToAction("Index");
            }
        }

    }
}