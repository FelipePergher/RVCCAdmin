namespace RVCC.Business
{

    public class TaskError
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return $"Code: {Code} => {Description}";
        }
    }
}
