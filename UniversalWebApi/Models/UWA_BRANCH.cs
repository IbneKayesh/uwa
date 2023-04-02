using System.ComponentModel.DataAnnotations;

namespace UniversalWebApi.Models
{
    public class UWA_BRANCH
    {
        /// <summary>
        /// Database Id
        /// </summary>
        [Display(Name = "Payload")]
        [Required(ErrorMessage = "{0} is required")]
        public string PAYLOAD_ID { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [Display(Name = "Branch")]
        [Required(ErrorMessage = "{0} is required")]
        public string BRANCH_ID { get; set; }

        /// <summary>
        /// User Descriptions
        /// </summary>
        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} is required")]
        public string BRANCH_DESC { get; set; }

        /// <summary>
        /// 0: No Auth, 1: Single Auth, 2: Multiple Auth
        /// </summary>
        [Display(Name = "Autorization")]
        [Required(ErrorMessage = "{0} is required")]
        public int BRANCH_STATUS { get; set; }
    }
}