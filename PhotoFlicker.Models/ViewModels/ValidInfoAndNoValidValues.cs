using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoFlicker.Models.ViewModels
{
    public class ValidInfoAndNoValidValues<T>
    {
        [Required]
        public bool IsValid { get; set; }

        public IEnumerable<T> NoValid { get; set; }
    }
}