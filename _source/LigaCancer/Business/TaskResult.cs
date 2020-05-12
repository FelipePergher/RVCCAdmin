// <copyright file="TaskResult.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.Collections.Generic;

namespace RVCC.Business
{
    public class TaskResult
    {
        public TaskResult()
        {
            Errors = new List<TaskError>();
        }

        public static TaskResult Success { get; }

        public bool Succeeded { get; set; }

        public List<TaskError> Errors { get; }
    }
}
