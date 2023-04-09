using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Response
{
    public class ResponseModel
    {
        public HttpStatusCode? Code { get; set; }
        public string? Message { get; set; }
        public string[] Errors { get; set; }
        public bool Success { get; set; } = true;
        public long? ExecTime { get; set; }

        public object Data { get; set; }

        public ResponseModel()
        {

        }

        public ResponseModel(string message, string[] errors, bool success, HttpStatusCode code, long? execTime, object data)
        {
            Code = code;
            Message = message;
            Success = success;
            Data = data;
            ExecTime = execTime;
            Errors = errors;
        }
    }
}
