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
            Results = new List<Select2Result>();
            Pagination = new Select2Pagination();
        }

        public List<Select2Result> Results { get; set; }

        public Select2Pagination Pagination { get; set; }
    }
}
