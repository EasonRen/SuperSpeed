using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSpeed
{
    class Speed
    {
        
        public const string LOCAL_TASK_ID = "LocalTaskId";
        public const string ACCELERATE_TASK_ID = "AccelerateTaskId";
        public const string LOCAL_SUB_FILE_INDEX = "LocalSubFileIndex";
        public const string USER_DATA = "UserData";

        public string LocalTaskId { get; set; }
        public string AccelerateTaskId { get; set; }
        public string LocalSubFileIndex { get; set; }
        public string UserData { get; set; }    

    }
}
