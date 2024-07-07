using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaTareas.Models
{
    public class TareaCLS { 


        [Display(Name = "Numero tarea")]
        public int IdTarea { get; set; }
        [Display(Name ="Titulo")]
        public string tituloTarea { get; set; }

        [Display(Name = "Descripción")]
        public string descripcionTarea { get; set; }


        [Display(Name = "Fecha  de inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true )]
        public DateTime fechaInicio { get; set; }

        [Display(Name = "Fecha de terminacion")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaTermino { get; set; }
        [Display(Name = "Categoria")]
        public int? IdCategoria { get; set; }
        [Display(Name = "Estado")]
        public int? IdEstado { get; set; }
        [Display(Name = "Nivel de urgencia")]
        public int? IdNivelUrgencia { get; set; }

        public bool habilitado { get; set; }

        //propiedades adicionales

        
        public string categoria { get; set; }
       
        public string estado { get; set; }
       
        public string nivelUrgencia { get; set; }
    }
}