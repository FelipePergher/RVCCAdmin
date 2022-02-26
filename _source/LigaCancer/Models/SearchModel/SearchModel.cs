// <copyright file="SearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.Collections.Generic;

namespace RVCC.Models.SearchModel
{
    public class SearchModel
    {
        public string Draw { get; set; }

        public string Start { get; set; }

        public string Length { get; set; }

        public List<SearchModelColumn> Columns { get; set; }

        public List<SearchModelResultOrder> Order { get; set; }

        public SearchModelInputSearch Search { get; set; }
    }
}
