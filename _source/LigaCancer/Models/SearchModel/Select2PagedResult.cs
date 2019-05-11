using System.Collections.Generic;

namespace LigaCancer.Models.SearchModel
{
    public class Select2PagedResult
    {
        public Select2PagedResult()
        {
            Results = new List<Result>();
            Pagination = new Pagination();
        }

        public List<Result> Results { get; set; }

        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public bool More { get; set; } = false;
    }

    public class Result
    {
        public string Id { get; set; }

        public string Text { get; set; }
    }
}
