namespace LigaCancer.Models
{
    public class ModalFooter
    {
        public string SubmitButtonText { get; set; } = "Enviar";
        public string CancelButtonText { get; set; } = "Cancelar";
        public string SubmitButtonId { get; set; } = "btn-submit";
        public string CancelButtonId { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }
    }
}