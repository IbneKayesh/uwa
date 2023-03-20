using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniversalWebApi.Models;

namespace UniversalWebApi.ModelsV2
{
    public class UWA_BODY_D
    {
        /// <summary>
        /// Table Name
        /// </summary>
        public string RESULT_TABLE { get; set; }
        /// <summary>
        /// SQL File Id
        /// tableName.sql_id
        /// </summary>
        [Required(ErrorMessage = "{0} is required")]
        public string RESOURCE { get; set; }

        [Display(Name = "Parameters")]
        [Required(ErrorMessage = "{0} is required")]
        public List<PARAM> PARAM_LIST { get; set; }
    }
}