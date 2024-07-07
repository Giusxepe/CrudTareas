using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaTareas.Models;
namespace SistemaTareas.Controllers
{
    public class TareasController : Controller
    {
        // GET: Tareas
        public ActionResult Index()
        {
            List<TareaCLS> listaTareas = new List<TareaCLS>();
            try
            {
                ListarCombos();

                using (var bd = new TareasDBEntities())
                {
                    listaTareas = (from tarea in bd.Tareas
                                   join Cat in bd.Categoria
                                   on tarea.IdCategoria equals Cat.IdCategoria
                                   join Est in bd.Estado
                                   on tarea.IdEstado equals Est.IdEstado
                                   join Niv in bd.NivelUrgencia
                                   on tarea.IdNivelUrgencia equals Niv.IdNUrgencia
                                   where tarea.Habilitada == true
                                   select new TareaCLS
                                   {
                                       IdTarea = tarea.IdTarea,
                                       tituloTarea = tarea.TituloTarea,
                                       descripcionTarea = tarea.Descripcion,
                                       fechaInicio = (DateTime)tarea.FechaInicio,
                                       fechaTermino = (DateTime)tarea.FechaTermino,
                                       IdCategoria = tarea.IdCategoria,
                                       IdEstado = tarea.IdEstado,
                                       IdNivelUrgencia = tarea.IdNivelUrgencia,
                                       categoria = Cat.NombreCategoria,
                                       estado = Est.Estado1,
                                       nivelUrgencia = Niv.NivelUrgencia1


                                   }).ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Mensaje.", ex);
            }

            return View(listaTareas);
        }

       
        public void ListarEstados()
        {
            List<SelectListItem> listarEstados = null;

            using (var bd = new TareasDBEntities())
            {
                listarEstados = (from estados in bd.Estado
                               select new SelectListItem
                               {
                                   Value= estados.IdEstado.ToString(),
                                   Text = estados.Estado1
                               }).ToList();
                listarEstados.Insert(0, new SelectListItem { Text = "-- Seleccione un estado --", Value = "" });
                ViewBag.listarEstados = listarEstados;
            }

        }


        
        public void ListarCategorias()
        {

            List<SelectListItem> listarCategorias = null;
            using (var bd = new TareasDBEntities())
            {
                listarCategorias = (from item in bd.Categoria
                         select new SelectListItem
                         {
                             Value = item.IdCategoria.ToString(),
                             Text = item.NombreCategoria
                         }).ToList();
                listarCategorias.Insert(0, new SelectListItem { Text = "Seleccione una categoria", Value = "" });
                ViewBag.listarCategorias = listarCategorias;
            }

        }

       
        public void ListarNivelUrgencia()
        {
            List<SelectListItem> listarNivelUrgencia = null;
            using (var bd = new TareasDBEntities())
            {
                listarNivelUrgencia = (from item in bd.NivelUrgencia
                         select new SelectListItem
                         {
                             Value = item.IdNUrgencia.ToString(),
                             Text = item.NivelUrgencia1
                         }).ToList();
                listarNivelUrgencia.Insert(0, new SelectListItem { Text = "Seleccione el nivel de urgencia", Value = "" });
                ViewBag.ListarNivelUrgencia = listarNivelUrgencia;
            }

        }

        public void ListarCombos()
        {
            ListarEstados();
            ListarCategorias();
            ListarNivelUrgencia();
        //    ViewBag.listarCategorias;
        //    ViewBag.listarEstados;
        //    ViewBag.listarNivelUrgencia;
        }

        public ActionResult Agregar()
        {
            ListarCombos();
         

            return View();
        }

        [HttpPost]
        public ActionResult Agregar(TareaCLS oTareasCLS )
        {
            if(!ModelState.IsValid)
            {
                ListarCombos();
                return View(oTareasCLS);
            }
            using (var bd = new TareasDBEntities())
            {
                Tareas oTarea = new Tareas();
                oTarea.TituloTarea = oTareasCLS.tituloTarea;
                oTarea.Descripcion = oTareasCLS.descripcionTarea;
                oTarea.FechaInicio = oTareasCLS.fechaInicio;
                oTarea.FechaTermino = oTareasCLS.fechaTermino;
                oTarea.IdCategoria = oTareasCLS.IdCategoria;
                oTarea.IdEstado = oTareasCLS.IdEstado;
                oTarea.IdNivelUrgencia = oTareasCLS.IdNivelUrgencia;
                oTarea.Habilitada = true;
                bd.Tareas.Add(oTarea);
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            ListarCombos();
            TareaCLS tareasCLS = new TareaCLS();

            using (var bd = new TareasDBEntities())
            {
              
                Tareas oTareas = bd.Tareas.Where(p => p.IdTarea.Equals(id)).First();
                tareasCLS.IdTarea = oTareas.IdTarea;
                tareasCLS.tituloTarea = oTareas.TituloTarea;
                tareasCLS.descripcionTarea = oTareas.Descripcion;
                tareasCLS.fechaInicio = oTareas.FechaInicio;
                tareasCLS.fechaTermino =(DateTime)oTareas.FechaTermino;
                tareasCLS.IdCategoria = oTareas.IdCategoria ;
                tareasCLS.IdEstado = oTareas.IdEstado ;
                tareasCLS.IdNivelUrgencia = oTareas.IdNivelUrgencia ;
            }
            return View(tareasCLS);
        }

        [HttpPost]
        public ActionResult Editar(TareaCLS oTareaCLS)
        {
            if (!ModelState.IsValid)
            {
                return View(oTareaCLS);
            }

            int idTarea = oTareaCLS.IdTarea;

            using(var bd = new TareasDBEntities())
            {
                Tareas oTarea = bd.Tareas.Where(p => p.IdTarea.Equals(idTarea)).First();
                oTarea.TituloTarea = oTareaCLS.tituloTarea;
                oTarea.IdTarea  = oTareaCLS.IdTarea;
                oTarea.TituloTarea = oTareaCLS.tituloTarea ;
                oTarea.Descripcion = oTareaCLS.descripcionTarea;
                oTarea.FechaInicio = oTareaCLS.fechaInicio;
                oTarea.FechaTermino =  oTareaCLS.fechaTermino;
                oTarea.IdCategoria = oTareaCLS.IdCategoria;
                oTarea.IdEstado = oTareaCLS.IdEstado;
                oTarea.IdNivelUrgencia = oTareaCLS.IdNivelUrgencia ;
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Eliminar(int IdTarea)
        {
            using (var bd = new TareasDBEntities())
            {
                Tareas ot = bd.Tareas.Where(p => p.IdTarea.Equals(IdTarea)).First();
                ot.Habilitada = false;
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

   
        public ActionResult listarTodasLasTareas()
        {
            List<TareaCLS> listaTareas = null;
            using (var bd = new TareasDBEntities())
            {
                listaTareas = (from tarea in bd.Tareas
                               join Cat in bd.Categoria
                               on tarea.IdCategoria equals Cat.IdCategoria
                               join Est in bd.Estado
                               on tarea.IdEstado equals Est.IdEstado
                               join Niv in bd.NivelUrgencia
                               on tarea.IdNivelUrgencia equals Niv.IdNUrgencia
                               where tarea.Habilitada == true
                               select new TareaCLS
                               {
                                   IdTarea = tarea.IdTarea,
                                   tituloTarea = tarea.TituloTarea,
                                   descripcionTarea = tarea.Descripcion,
                                   fechaInicio = (DateTime)tarea.FechaInicio,
                                   fechaTermino = (DateTime)tarea.FechaTermino,
                                   categoria = Cat.NombreCategoria,
                                   estado = Est.Estado1,
                                   nivelUrgencia = Niv.NivelUrgencia1
                                 
                               }).ToList();
            }

            return View(listaTareas);
        }

        public ActionResult listarTareasTerminadas()
        {
            List<TareaCLS> listaTareas = null;
            using (var bd = new TareasDBEntities())
            {
                listaTareas = (from tarea in bd.Tareas
                               join Cat in bd.Categoria
                               on tarea.IdCategoria equals Cat.IdCategoria
                               join Est in bd.Estado
                               on tarea.IdEstado equals Est.IdEstado
                               join Niv in bd.NivelUrgencia
                               on tarea.IdNivelUrgencia equals Niv.IdNUrgencia
                               where Est.Estado1.Equals("Terminada") &&
                               tarea.Habilitada == true
                               select new TareaCLS
                               {
                                   IdTarea = tarea.IdTarea,
                                   tituloTarea = tarea.TituloTarea,
                                   descripcionTarea = tarea.Descripcion,
                                   fechaInicio = (DateTime)tarea.FechaInicio,
                                   fechaTermino = (DateTime)tarea.FechaTermino,
                                   categoria = Cat.NombreCategoria,
                                   estado = Est.Estado1,
                                   nivelUrgencia = Niv.NivelUrgencia1
                                   
                               }).ToList();
            }
            return View(listaTareas);
        }

        public ActionResult listarTareasNoTerminadas()
        {
            List<TareaCLS> listaTareas = null;
            using (var bd = new TareasDBEntities())
            {
                listaTareas = (from tarea in bd.Tareas
                               join Cat in bd.Categoria
                               on tarea.IdCategoria equals Cat.IdCategoria
                               join Est in bd.Estado
                               on tarea.IdEstado equals Est.IdEstado
                               join Niv in bd.NivelUrgencia
                               on tarea.IdNivelUrgencia equals Niv.IdNUrgencia
                               where Est.Estado1.Equals("No terminada") &&
                               tarea.Habilitada == true
                               select new TareaCLS
                               {
                                   IdTarea = tarea.IdTarea,
                                   tituloTarea = tarea.TituloTarea,
                                   descripcionTarea = tarea.Descripcion,
                                   fechaInicio = (DateTime)tarea.FechaInicio,
                                   fechaTermino = (DateTime)tarea.FechaTermino,
                                   categoria = Cat.NombreCategoria,
                                   estado = Est.Estado1,
                                   nivelUrgencia = Niv.NivelUrgencia1

                               }).ToList();
            }
            return View(listaTareas);
        }
    }
}