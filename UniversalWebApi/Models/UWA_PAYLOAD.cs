using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversalWebApi.Models
{
    public class UWA_PAYLOAD
    {
        /// <summary>
        /// Database Id
        /// </summary>
        [Display(Name = "Payload")]
        [Required(ErrorMessage = "{0} is required")]
        public string PAYLOAD_ID { get; set; }

        /// <summary>
        /// Database connection string
        /// </summary>
        [Display(Name = "Master DB Connection String")]
        [Required(ErrorMessage = "{0} is required")]
        public string DB_CONNECTION { get; set; }

        /// <summary>
        /// Descriptions of database
        /// </summary>
        /// 
        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} is required")]
        public string DB_DESC { get; set; }

        /// <summary>
        /// SQL file table
        /// </summary>
        /// 
        [Display(Name = "Table Prefix")]
        [Required(ErrorMessage = "{0} is required")]
        public string PAYLOAD_TABLE { get; set; }

    }
}