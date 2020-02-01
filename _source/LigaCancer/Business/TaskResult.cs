﻿using System.Collections.Generic;

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