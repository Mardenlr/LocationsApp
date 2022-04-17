using LocationsApp.DataAccess.Models.Base;
using LocationsApp.DataAccess.Utils;
using System.ComponentModel.DataAnnotations;

namespace LocationsApp.DataAccess.Models
{
    public class CountryModel : BaseModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = Messages.FieldRequired)]
        [MaxLength(100, ErrorMessage = Messages.MaximumCharactersAllowed)]
        public string Name { get; set; }
    }
}
