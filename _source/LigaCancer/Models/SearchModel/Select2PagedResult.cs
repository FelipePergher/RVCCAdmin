// <copyright file="Select2PagedResult.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.Collections.Generic;

namespace RVCC.Models.SearchModel
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
