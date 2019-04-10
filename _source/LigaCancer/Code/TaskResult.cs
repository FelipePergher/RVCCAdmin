using System.Collections.Generic;

namespace LigaCancer.Code
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

        public int Id { get; set; }

    }
}
