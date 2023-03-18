using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversalWebApi.Models
{
    public class UWA_BODY
    {
        /// <summary>
        /// SQL File Id
        /// tableName.sql_id
        /// </summary>
        [Required(ErrorMessage = "{0} is required")]
        public string RESOURCE { get; set; }
        /// <summary>
        /// SF Method - GET or SET
        /// </summary>
        [Display(Name = "Method")]
        [Required(ErrorMessage = "{0} is required")]
        public string METHOD { get; set; }

        [Display(Name = "Parameters")]
        [Required(ErrorMessage = "{0} is required")]
        public List<PARAM> PARAM_LIST { get; set; }
    }
}