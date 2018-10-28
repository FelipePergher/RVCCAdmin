namespace LigaCancer.Models
{
    using Code;

    public class BootstrapModel
    {
        public string Id { get; set; }
        public string AreaLabeledId { get; set; }
        public Globals.ModalSize Size { get; set; }
        public string Message { get; set; }
        public string ModalSizeClass
        {
            get
            {
                switch (this.Size)
                {
                    case Globals.ModalSize.Small:
                        return "modal-sm";
                    case Globals.ModalSize.Large:
                        return "modal-lg";
                    case Globals.ModalSize.Medium:
                    default:
                        return "";
                }
            }
        }        
    }
}