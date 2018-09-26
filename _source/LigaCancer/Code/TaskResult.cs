using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Code
{
    //
    // Summary:
    //     Represents the result of an identity operation.
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
