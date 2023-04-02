using System.ComponentModel.DataAnnotations;

namespace UniversalWebApi.Areas.Manager.Models
{
    public class DbTableModel
    {
        [Display(Name = "Payload . Table Prefix")]
        [Required(ErrorMessage = "{0} is required")]
        public string PAYLOAD_ID { get; set; }

        [Display(Name = "Module")]
        [Required(ErrorMessage = "{0} is required")]

        [StringLength(maximumLength: 15, ErrorMessage = "{0} meaningful name length is between 4 and 15 char", MinimumLength = 5)]
        public string TABLE_NAME { get; set; }
    }
}