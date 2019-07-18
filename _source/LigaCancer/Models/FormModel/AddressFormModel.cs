using LigaCancer.Code;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class AddressFormModel
    {
        [Display(Name = "Rua")]
        [Required(ErrorMessage = "Este campo é Obrigatório!")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        public string Neighborhood { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Display(Name = "Número")]
        public string HouseNumber { get; set; }

        [Display(Name = "Complemento")]
        public string Complement { get; set; }

        [Display(Name = "Observação")]
        public string ObservationAddress { get; set; }

        [Display(Name = "Tipo de Residência")]
        public Globals.ResidenceType? ResidenceType { get; set; }

        [Display(Name = "Valor Mensal Residência")]
        public string MonthlyAmmountResidence { get; set; }
    }
}
