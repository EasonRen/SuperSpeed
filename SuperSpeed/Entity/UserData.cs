using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSpeed
{
    class UserData
    {
        //{"CommitGcid":"","Message":"文件名中包含违规内容，无法添加到高速通道","Result":509,"SubId":5}
        public const string COMMIT_GCID = "CommitGcid";
        public const string MESSAGE = "Message";
        public const string RESULT = "Result";
        public const string SUB_ID = "SubId";

        public string CommitGcid { get; set; }
        public string Message { get; set; }
        public int Result { get; set; }
        public string SubId { get; set; }
    }
}
