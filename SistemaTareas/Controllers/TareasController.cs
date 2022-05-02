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
                                   on tarea.Id_Categoria equals Cat.Id_Categoria
                                   join Est in bd.Estado
                                   on tarea.Id_Estado equals Est.Id_Estado
                                   join Niv in bd.NivelUrgencia
                                   on tarea.Id_NivelUrgencia equals Niv.Id_NivelUrgencia
                                   where tarea.Habilitado == 1
                                   select new TareaCLS
                                   {
                                       IdTarea = tarea.Id_Tarea,
                                       tituloTarea = tarea.Titulo_Tarea,
                                       descripcionTarea = tarea.Descripcion,
                                       fechaInicio = (DateTime)tarea.Fecha_Inicio,
                                       fechaTermino = (DateTime)tarea.Fecha_Termino,
                                       IdCategoria = tarea.Id_Categoria,
                                       IdEstado = tarea.Id_Estado,
                                       IdNivelUrgencia = tarea.Id_NivelUrgencia,
                                       categoria = Cat.Categoria1,
                                       estado = Est.Estado1,
                                       nivelUrgencia = Niv.Nivel


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
                                   Value= estados.Id_Estado.ToString(),
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
                             Value = item.Id_Categoria.ToString(),
                             Text = item.Categoria1
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
                             Value = item.Id_NivelUrgencia.ToString(),
                             Text = item.Nivel
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
            ViewBag.listarCategorias;
            ViewBag.listarEstados;
            ViewBag.listarNivelUrgencia;
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
                oTarea.Titulo_Tarea = oTareasCLS.tituloTarea;
                oTarea.Descripcion = oTareasCLS.descripcionTarea;
                oTarea.Fecha_Inicio = oTareasCLS.fechaInicio;
                oTarea.Fecha_Termino = oTareasCLS.fechaTermino;
                oTarea.Id_Categoria = oTareasCLS.IdCategoria;
                oTarea.Id_Estado = oTareasCLS.IdEstado;
                oTarea.Id_NivelUrgencia = oTareasCLS.IdNivelUrgencia;
                oTarea.Habilitado = 1;
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
              
                Tareas oTareas = bd.Tareas.Where(p => p.Id_Tarea.Equals(id)).First();
                tareasCLS.IdTarea = oTareas.Id_Tarea;
                tareasCLS.tituloTarea = oTareas.Titulo_Tarea;
                tareasCLS.descripcionTarea = oTareas.Descripcion;
                tareasCLS.fechaInicio = oTareas.Fecha_Inicio;
                tareasCLS.fechaTermino =(DateTime)oTareas.Fecha_Termino;
                tareasCLS.IdCategoria = oTareas.Id_Categoria ;
                tareasCLS.IdEstado = oTareas.Id_Estado ;
                tareasCLS.IdNivelUrgencia = oTareas.Id_NivelUrgencia ;
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
                Tareas oTarea = bd.Tareas.Where(p => p.Id_Tarea.Equals(idTarea)).First();
                oTarea.Titulo_Tarea = oTareaCLS.tituloTarea;
                oTarea.Id_Tarea  = oTareaCLS.IdTarea;
                oTarea.Titulo_Tarea = oTareaCLS.tituloTarea ;
                oTarea.Descripcion = oTareaCLS.descripcionTarea;
                oTarea.Fecha_Inicio = oTareaCLS.fechaInicio;
                oTarea.Fecha_Termino =  oTareaCLS.fechaTermino;
                oTarea.Id_Categoria = oTareaCLS.IdCategoria;
                oTarea.Id_Estado = oTareaCLS.IdEstado;
                oTarea.Id_NivelUrgencia = oTareaCLS.IdNivelUrgencia ;
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Eliminar(int IdTarea)
        {
            using (var bd = new TareasDBEntities())
            {
                Tareas ot = bd.Tareas.Where(p => p.Id_Tarea.Equals(IdTarea)).First();
                ot.Habilitado = 0;
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
                               on tarea.Id_Categoria equals Cat.Id_Categoria
                               join Est in bd.Estado
                               on tarea.Id_Estado equals Est.Id_Estado
                               join Niv in bd.NivelUrgencia
                               on tarea.Id_NivelUrgencia equals Niv.Id_NivelUrgencia
                               where tarea.Habilitado == 1
                               select new TareaCLS
                               {
                                   IdTarea = tarea.Id_Tarea,
                                   tituloTarea = tarea.Titulo_Tarea,
                                   descripcionTarea = tarea.Descripcion,
                                   fechaInicio = (DateTime)tarea.Fecha_Inicio,
                                   fechaTermino = (DateTime)tarea.Fecha_Termino,
                                   categoria = Cat.Categoria1,
                                   estado = Est.Estado1,
                                   nivelUrgencia = Niv.Nivel
                                 
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
                               on tarea.Id_Categoria equals Cat.Id_Categoria
                               join Est in bd.Estado
                               on tarea.Id_Estado equals Est.Id_Estado
                               join Niv in bd.NivelUrgencia
                               on tarea.Id_NivelUrgencia equals Niv.Id_NivelUrgencia
                               where Est.Estado1.Equals("Terminada") &&
                               tarea.Habilitado == 1 
                               select new TareaCLS
                               {
                                   IdTarea = tarea.Id_Tarea,
                                   tituloTarea = tarea.Titulo_Tarea,
                                   descripcionTarea = tarea.Descripcion,
                                   fechaInicio = (DateTime)tarea.Fecha_Inicio,
                                   fechaTermino = (DateTime)tarea.Fecha_Termino,
                                   categoria = Cat.Categoria1,
                                   estado = Est.Estado1,
                                   nivelUrgencia = Niv.Nivel
                                   
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
                               on tarea.Id_Categoria equals Cat.Id_Categoria
                               join Est in bd.Estado
                               on tarea.Id_Estado equals Est.Id_Estado
                               join Niv in bd.NivelUrgencia
                               on tarea.Id_NivelUrgencia equals Niv.Id_NivelUrgencia
                               where Est.Estado1.Equals("No terminada") &&
                               tarea.Habilitado == 1
                               select new TareaCLS
                               {
                                   IdTarea = tarea.Id_Tarea,
                                   tituloTarea = tarea.Titulo_Tarea,
                                   descripcionTarea = tarea.Descripcion,
                                   fechaInicio = (DateTime)tarea.Fecha_Inicio,
                                   fechaTermino = (DateTime)tarea.Fecha_Termino,
                                   categoria = Cat.Categoria1,
                                   estado = Est.Estado1,
                                   nivelUrgencia = Niv.Nivel

                               }).ToList();
            }
            return View(listaTareas);
        }
    }
}