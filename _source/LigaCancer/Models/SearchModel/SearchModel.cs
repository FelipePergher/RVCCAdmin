using System.Collections.Generic;

namespace LigaCancer.Models.SearchModel
{
    public class SearchModel
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public List<Column> Columns { get; set; }
        public List<ResultOrder> Order { get; set; }
        public InputSearch Search { get; set; }
    }

    public class Column
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }

        public InputSearch Search { get; set; }
    }

    public class ResultOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }


    public class InputSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
}
