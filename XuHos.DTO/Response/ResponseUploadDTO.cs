using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XuHos.Entity;
namespace XuHos.DTO
{ 
    public class ResponseUploadFileDTO
    {
        public string UrlPrefix { get; set; }

        public string FileName { get; set; }

        public string MD5 { get; set; }

        public long FileSize { get; set; }

        public string FileSeq { get; set; }        
        public string AccessKey { get; set; }
    }

    public class ResponseUploadAudioDTO: ResponseUploadFileDTO
    {
        public ResponseUploadAudioDTO(ResponseUploadFileDTO file,int Second)
        {
            this.FileName = file.FileName;
            this.FileSize = file.FileSize;
            this.MD5 = file.MD5;
            this.UrlPrefix = file.UrlPrefix;
            this.Second = Second;
        }


        public int Second { get; set; }
    }

}
