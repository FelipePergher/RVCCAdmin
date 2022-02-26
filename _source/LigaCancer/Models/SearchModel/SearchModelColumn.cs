// <copyright file="SearchModelColumn.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Models.SearchModel;

public class SearchModelColumn
{
    public string Data { get; set; }

    public string Name { get; set; }

    public bool Searchable { get; set; }

    public bool Orderable { get; set; }

    public SearchModelInputSearch Search { get; set; }
}