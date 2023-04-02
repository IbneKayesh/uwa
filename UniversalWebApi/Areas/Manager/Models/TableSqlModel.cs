using System.ComponentModel.DataAnnotations;

namespace UniversalWebApi.Areas.Manager.Models
{
    public class TableSqlModel
    {
        [Display(Name = "Module")]
        [Required(ErrorMessage = "{0} is required")]
        public string TABLE_NAME { get; set; }

        [Display(Name = "Parameter")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 42, ErrorMessage = "{0} meaningful name length is between 14 and 4000 char", MinimumLength = 4)]
        public string ID { get; set; }

        [Display(Name = "Query")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 4000, ErrorMessage = "{0} meaningful name length is between 14 and 4000 char", MinimumLength = 14)]
        public string SQL { get; set; }
    }
}