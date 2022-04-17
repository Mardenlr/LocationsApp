﻿using LocationsApp.DataAccess.Models.Base;
using LocationsApp.DataAccess.Utils;
using System.ComponentModel.DataAnnotations;

namespace LocationsApp.DataAccess.Models
{
    public class StateModel : BaseModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = Messages.FieldRequired)]
        [MaxLength(10, ErrorMessage = Messages.MaximumCharactersAllowed)]
        [Display(Name = "Short Code")]
        public string ShortCode { get; set; }


        [Required(ErrorMessage = Messages.FieldRequired)]
        [MaxLength(100, ErrorMessage = Messages.MaximumCharactersAllowed)]
        public string Name { get; set; }


        [Required(ErrorMessage = Messages.FieldRequired)]
        [MaxLength(100, ErrorMessage = Messages.MaximumCharactersAllowed)]
        public string Capital { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = Messages.FieldRequired)]
        [Required(ErrorMessage = Messages.FieldRequired)]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public string Country { get; set; }
    }
}
