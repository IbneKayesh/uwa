using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversalWebApi.ModelsV2
{
    public class UWA_BODY_M
    {
        /// <summary>
        /// SF Method - GET or SET
        /// </summary>
        [Display(Name = "Method")]
        [Required(ErrorMessage = "{0} is required")]
        public string METHOD { get; set; }

        [Display(Name = "Data Body")]
        [Required(ErrorMessage = "{0} is required")]
        public List<UWA_BODY_D> DATA_BODY { get; set; }
    }
}