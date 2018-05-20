using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Integration.ShortMessage.YunPian
{

    public  class ResCls
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string count { get; set; }
        public string fee { get; set; }
        public string unit { get; set; }
        public string sid { get; set; }

    }
    public class Result
    {
        public bool success;
        public int statusCode;
        public string responseText;
        public Exception e;
        public ResCls data;

        public Result(int statusCode,string responseText,Exception e=null)
        {
            if (statusCode == 200)
            {
                this.success = true;
            }

            else if(statusCode == 400)
            {
                this.success = false;
            }
            else
            {
                this.success = false;
            }
            this.responseText = responseText;
            try
            {
                data = JsonConvert.DeserializeObject<ResCls>(responseText);
            }
            catch
            {

            }

            this.statusCode = statusCode;
            this.e = e;
            
        }
        public override string ToString()
        {
            //重写需要的输出。
            return "[success:" + success + ",statusCode:" + statusCode + ",responseText:" + responseText + "]";
        }

    }
}
