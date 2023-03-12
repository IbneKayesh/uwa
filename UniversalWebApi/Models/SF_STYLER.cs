using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversalWebApi.Models
{
    public class SF_STYLER
    {
        /// <summary>
        /// SF Name [DB-SQL]
        /// </summary>
        [Display(Name = "SF Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string SF_NAME { get; set; }

        /// <summary>
        /// Data Table Return
        /// </summary>
        [Display(Name = "SF Return Datatable")]
        [Required(ErrorMessage = "{0} is required")]
        public string SF_RETN { get; set; }

        /// <summary>
        /// SF Parameter List 
        /// </summary>
        [Display(Name = "Parameter List")]
        [Required(ErrorMessage = "{0} is required")]
        public List<PARAM> PARAM_LIST { get; set; }
    }
}