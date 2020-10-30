using System.ComponentModel.DataAnnotations;

namespace PhotoFlicker.Models.ViewModels
{
    public class UrlAndPlanTextViewModel
    {
        [Required]
        public string Url { get; set; }

        public string Text { get; set; }
    }
}