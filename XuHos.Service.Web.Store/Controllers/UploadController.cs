using XuHos.BLL;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.Common.Cache.Keys;
using XuHos.Common.Config.Sections;
using XuHos.Common.Enum;
using XuHos.DTO;
using XuHos.DTO.Common;
using XuHos.DTO.Platform;
using XuHos.Entity;
using XuHos.Extensions;
using XuHos.Service.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ThoughtWorks.QRCode.Codec;

namespace XuHos.Service.WebStore.Controllers
{    
    public class UploadController : ApiBaseController
    {

        static XuHos.Common.Config.Sections.IMGStore config = null;

        public UploadController()
        {
            if (config == null)
            {
                //文件存储配置
                config = SysConfigService.Get<XuHos.Common.Config.Sections.IMGStore>();
            }
        }



        #region  私有
        async Task<int> GetWavTotalSecond(string FileMD5)
        {
            #region 获取文件长度

            SysFileIndexService fileService = new SysFileIndexService(CurrentOperatorUserID);
            var index = fileService.Single<XuHos.Entity.SysFileIndex>(a => a.MD5 == FileMD5);

            if (index != null)
            {
                //文件存储配置
                var config = SysConfigService.Get<XuHos.Common.Config.Sections.IMGStore>();

                using (var filestream = await XuHos.Common.Storage.Manager.Instance.OpenFile("Audios", index.FileUrl))
                {
                    return Convert.ToInt32(XuHos.Common.Utility.AudioHelper.TotalSeconds(filestream));
                }
            }
            else
            {
                return 0;
            }
            #endregion

        }

        #endregion

        #region 上传

        /**
        * @api {POST} /Upload/Image S0101/上传/图片
        * @apiGroup Store
        * @apiVersion 0.0.1
        * @apiDescription 上传/图片
        * @apiPermission 已登录
        * @apiHeader {String} apptoken appToken
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写  
        * @apiParam {String} [Md5] 文件哈希值（使用秒传功能）
        * @apiParam {String} [AccessKey] 访问密码
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {string} Data 图片访问地址
        * @apiSuccessExample {json} 返回样例:
        *{
            "Data": {
                "UrlPrefix": "http://www.kmwlyy.com",
                "FileName": "{userPath}/2016/08/31/c48490d87fb74c16b7027c3757f989d3.jpg",
                "MD5": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
            },
            "Total": 0,
            "Status": 0,
            "Msg": "操作成功"
        }
        **/
        [ApiOperateNotTrack]
        [Route("~/Upload/Image")]
        [HttpPost]
        public async Task<ApiResult> UploadImage(string Md5 = "", string AccessKey = "")
        {
            return await Upload("images", PreMD5: Md5, AccessKey: AccessKey);
        }

        /**
        * @api {POST} /Upload/ImageByBase64 S0102/上传/图片(Base64)
        * @apiGroup Store
        * @apiVersion 0.0.1
        * @apiDescription 上传/图片
        * @apiPermission 已登录
        * @apiHeader {String} apptoken appToken
        * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
        * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
        * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写  
        * @apiParam {String} [Md5] 文件哈希值（使用秒传功能）
        * @apiParam {String} [AccessKey] 访问密码
        * @apiParam {String} Content 图片Base64内容
        * @apiSuccess (Response) {String} Msg 提示信息 
        * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
        * @apiSuccess (Response) {string} Data 图片访问地址
        * @apiSuccessExample {json} 返回样例:
        *{
            "Data": {
                "UrlPrefix": "http://www.kmwlyy.com",
                "FileName": "{userPath}/2016/08/31/c48490d87fb74c16b7027c3757f989d3.jpg",
                "MD5": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
            },
            "Total": 0,
            "Status": 0,
            "Msg": "操作成功"
        }
        **/
        [Route("~/Upload/ImageByBase64")]
        [ApiOperateNotTrack]
        [HttpPost]
        public async Task<ApiResult> UploadImageByBase64([FromBody]BLL.Sys.DTOs.Request.RequestUploadImageByBase64DTO request)
        {
            byte[] byteData = Convert.FromBase64String(request.Content.Replace("data:image/png;base64", ""));

            var FileType = "Images";

            try
            {

                //文件存储配置
                var config = SysConfigService.Get<XuHos.Common.Config.Sections.IMGStore>();

                SysFileIndex fileIndex = null;

                var FileStream = new System.IO.MemoryStream(byteData);

                var FileMD5 = XuHos.Common.Utility.HashHelper.ComputeMD5(FileStream);

                //通过缓存来判断文件是否已经上传了     
                var CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<SysFileIndex>(StringCacheKeyType.SysFileIndex, FileMD5);

                //缓存中获取文件索引
                fileIndex = CacheKey.FromCache();

                //文件已经存在
                if (fileIndex != null)
                {
                    //返回上传结果
                    return new ResponseUploadFileDTO
                    {
                        UrlPrefix = config.UrlPrefix,
                        FileName = string.Format("{0}/{1}", fileIndex.FileType, fileIndex.FileUrl),
                        MD5 = fileIndex.MD5,
                        FileSize = fileIndex.FileSize,
                        FileSeq = request.FileSeq
                    }.ToApiResultForObject();
                }
                //文件不存在的才保存到服务器
                else
                {
                    SysFileIndexService fileService = new SysFileIndexService(CurrentOperatorUserID);

                    //文件不存在
                    if (fileService.Exists(a => a.MD5 == FileMD5))
                    {
                        fileIndex = fileService.Single<SysFileIndex>(a => a.MD5 == FileMD5);

                        if (fileIndex != null)
                        {
                            //设置缓存
                            fileIndex.ToCache(CacheKey, TimeSpan.FromHours(2));

                            var result = new ResponseUploadFileDTO
                            {
                                UrlPrefix = config.UrlPrefix,
                                FileName = string.Format("{0}/{1}", fileIndex.FileType, fileIndex.FileUrl),
                                MD5 = fileIndex.MD5,
                                FileSize = fileIndex.FileSize,
                                FileSeq = request.FileSeq
                            };

                            return result.ToApiResultForObject();
                        }
                        else
                        {
                            return EnumApiStatus.BizError.ToApiResultForApiStatus();
                        }
                    }
                    else
                    {
                        //文件名
                        var FileUrl = Guid.NewGuid().ToString("N") + ".jpg";

                        var FileSize = FileStream.Length;

                        await XuHos.Common.Storage.Manager.Instance.WriteFile(FileType, FileUrl, FileStream);

                        fileIndex = new SysFileIndex()
                        {
                            MD5 = FileMD5,
                            FileType = FileType,
                            FileUrl = FileUrl,
                            FileSize = FileSize,
                            Remark = "",
                            AccessKey = string.IsNullOrEmpty(request.AccessKey) ? "" : request.AccessKey
                        };

                        var result = new ResponseUploadFileDTO
                        {
                            UrlPrefix = config.UrlPrefix,
                            FileName = string.Format("{0}/{1}", FileType, FileUrl),
                            MD5 = FileMD5,
                            FileSize = FileSize,
                            FileSeq = request.FileSeq,
                            AccessKey = request.AccessKey
                        };

                        var SaveFlag = false;

                        //文件不存在
                        if (!fileService.Exists(a => a.MD5 == FileMD5))
                        {
                            try
                            {
                                //添加记录
                                SaveFlag = fileService.Insert(fileIndex);
                            }
                            //主键冲突异常
                            catch (ConstraintException ex)
                            {
                                SaveFlag = true;
                            }
                        }
                        else
                        {
                            SaveFlag = true;
                        }

                        //保存成功
                        if (SaveFlag)
                        {

                            //设置缓存
                            fileIndex.ToCache(CacheKey, TimeSpan.FromHours(2));

                            return result.ToApiResultForObject();
                        }
                        else
                        {
                            return EnumApiStatus.BizError.ToApiResultForApiStatus();

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += System.Environment.NewLine + ex.InnerException.Message;

                    if (ex.InnerException.InnerException != null)
                    {
                        msg += System.Environment.NewLine + ex.InnerException.InnerException.Message;
                    }

                }
                LogHelper.WriteError(ex);

                return EnumApiStatus.BizError.ToApiResultForApiStatus("上传失败，错误:" + msg);
            }
        }

        /**
          * @api {POST} /Upload/File S0103/上传/文件
          * @apiGroup Store
          * @apiVersion 0.0.1
          * @apiDescription 上传/文件
          * @apiPermission 已登录
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写  
          * @apiParam {String} [Md5] 文件哈希值（使用秒传功能）
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {string} Data 图片访问地址
          * @apiSuccessExample {json} 返回样例:
          *{
              "Data": {
                  "UrlPrefix": "http://www.kmwlyy.com",
                  "FileName": "{userPath}/2016/08/31/c48490d87fb74c16b7027c3757f989d3.jpg",
                  "MD5": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
              },
              "Total": 0,
              "Status": 0,
              "Msg": "操作成功"
          }
          **/
        [Route("~/Upload/File")]
        [ApiOperateNotTrack]
        [HttpPost]
        public async Task<ApiResult> UploadFile(string Md5 = "", string AccessKey = "", string Name = "")
        {
            return await Upload("Files", PreMD5: Md5, AccessKey: AccessKey, Name: Name);
        }

        /**
          * @api {POST} /Upload/Audio S0104/上传/语音
          * @apiGroup Store
          * @apiVersion 0.0.1
          * @apiDescription 上传/文件 
          * @apiPermission 已登录
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写  
          * @apiParam {String} [Md5] 文件哈希值（使用秒传功能）
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {string} Data 图片访问地址
          * @apiSuccessExample {json} 返回样例:
          *{
              "Data": {
                  "UrlPrefix": "http://www.kmwlyy.com",
                  "FileName": "{userPath}/2016/08/31/c48490d87fb74c16b7027c3757f989d3.jpg",
                  "MD5": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
              },
              "Total": 0,
              "Status": 0,
              "Msg": "操作成功"
          }
          **/
        /// <summary>
        /// 上传语音
        /// </summary>
        /// <param name="Md5"></param>
        /// <returns></returns>
        [Route("~/Upload/Audio")]
        [ApiOperateNotTrack]
        [HttpPost]
        public async Task<ApiResult> UploadAudio(string Md5 = "", string AccessKey = "")
        {
            var result = await Upload("Audios", PreMD5: Md5, AccessKey: AccessKey);
            var uploadFileReturn = result.Data as ResponseUploadFileDTO;
            result.Data = new ResponseUploadAudioDTO(uploadFileReturn, await GetWavTotalSecond(uploadFileReturn.MD5));
            return result;
        }

        /**
          * @api {POST} /Upload/Video S0105/上传/视频
          * @apiGroup Store
          * @apiVersion 0.0.1
          * @apiDescription 上传/文件 
          * @apiPermission 已登录
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写  
          * @apiParam {String} [Md5] 文件哈希值（使用秒传功能）
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {string} Data 图片访问地址
          * @apiSuccessExample {json} 返回样例:
          *{
              "Data": {
                  "UrlPrefix": "http://www.kmwlyy.com",
                  "FileName": "{userPath}/2016/08/31/c48490d87fb74c16b7027c3757f989d3.jpg",
                  "MD5": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
              },
              "Total": 0,
              "Status": 0,
              "Msg": "操作成功"
          }
          **/
        /// <summary>
        /// 上传视频
        /// </summary>
        /// <param name="Md5"></param>
        /// <returns></returns>
        [Route("~/Upload/Video")]
        [ApiOperateNotTrack]
        [HttpPost]
        public async Task<ApiResult> UploadVideo(string Md5 = "", string AccessKey = "")
        {
            return await Upload("Videos", PreMD5: Md5, AccessKey: AccessKey);
        }

        /**
          * @api {POST} /Upload/DCM S0106/上传/影像
          * @apiGroup Store
          * @apiVersion 0.0.1
          * @apiDescription 上传/影像 
          * @apiPermission 已登录
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写  
          * @apiParam {String} [Md5] 文件哈希值（使用秒传功能）
          * @apiSuccess (Response) {String} Msg 提示信息 
          * @apiSuccess (Response) {int} Status 0 代表无错误 1代表有错误
          * @apiSuccess (Response) {string} Data 图片访问地址
          * @apiSuccessExample {json} 返回样例:
          *{
              "Data": {
                  "UrlPrefix": "http://www.kmwlyy.com",
                  "FileName": "{userPath}/2016/08/31/c48490d87fb74c16b7027c3757f989d3.jpg",
                  "MD5": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
              },
              "Total": 0,
              "Status": 0,
              "Msg": "操作成功"
          }
          **/
        [Route("~/Upload/DCM")]
        [ApiOperateNotTrack]
        [HttpPost]
        public async Task<ApiResult> UploadDCM(string Md5 = "", string AccessKey = "")
        {
            var res = await Upload("dcm", "", PreMD5: Md5, AccessKey: AccessKey);
            if (res.Status != 0)
                return res;

            var data = res.Data as ResponseUploadFileDTO;
            var result = data.Map<ResponseUploadFileDTO, DcmDTO>();

            #region //获取文件路径
            string filePath = "";
            //缓存中获取文件索引
            var CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<SysFileIndex>(StringCacheKeyType.SysFileIndex, result.MD5);

            SysFileIndex fileIndex = CacheKey.FromCache();

            if (fileIndex != null)
                filePath = fileIndex.FileUrl;
            else
            {
                //从数据库中获取文件索引
                var file = new SysFileIndexService(CurrentOperatorUserID).Single<SysFileIndex>(a => a.MD5 == result.MD5);
                if (file == null)
                    filePath = file.FileUrl;
            }
            if (string.IsNullOrEmpty(filePath))
                return EnumApiStatus.BizError.ToApiResultForApiStatus();
            #endregion

            #region//解析DCM文件参数
            var config = SysConfigService.Get<IMGStore>();
            var fileFullPath = Path.Combine(config.DirectorRootPath, filePath);
            Dictionary<string, string> tags = DCMHelper.TagRead(fileFullPath);
            string caseId = null; //病例号
            string studyId = null; //检查号
            string stuUid = null;
            if (tags != null)
            {
                tags.TryGetValue("0010,0020", out caseId);
                tags.TryGetValue("0008,0050", out studyId);
                tags.TryGetValue("0020,000d", out stuUid);
            }
            result.CaseID = caseId != null ? caseId.Trim() : "";
            result.StudyID = studyId != null ? studyId.Trim() : "";
            result.StuUID = stuUid != null ? stuUid.TrimEnd('\0').Trim() : "";
            #endregion

            return result.ToApiResultForObject();

        }

        /// <summary>
        /// 上传文件
        
        /// 日期：2016年9月28日
        /// </summary>
        /// <param name="FileType">文件类型</param>
        /// <param name="Directory">文件保存路径</param>
        /// <param name="PreMD5">预处理Md5哈希值</param>
        /// <returns></returns>
        public async Task<ApiResult> Upload(
            string FileType = "images",
            string Directory = null,
            string PreMD5 = "",
            string AccessKey = "", 
            string Name = "")
        {


            // 检查是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return EnumApiStatus.BizError.ToApiResultForApiStatus("UnsupportedMediaType");
            }

            try
            {
                //文件存储配置
                var config = SysConfigService.Get<XuHos.Common.Config.Sections.IMGStore>();

                SysFileIndex fileIndex = null;

                //通过缓存来判断文件是否已经上传了
                XuHos.Common.Cache.Keys.EntityCacheKey<SysFileIndex> CacheKey;

                //预处理文件Hash，客户端进行哈希值计算
                if (PreMD5 != "")
                {
                    CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<SysFileIndex>(StringCacheKeyType.SysFileIndex, PreMD5);
                    //缓存中获取文件索引
                    fileIndex = CacheKey.FromCache();
                }

                //文件已经存在，则使用妙传（不需要保存直接返回之前已经上传的问题）
                if (fileIndex != null)
                {
                    //返回上传结果
                    return new ResponseUploadFileDTO
                    {
                        UrlPrefix = config.UrlPrefix,
                        FileName = string.IsNullOrEmpty(Name) ? string.Format("{0}/{1}", FileType, fileIndex.FileUrl) : Name,
                        MD5 = fileIndex.MD5,
                        FileSize = fileIndex.FileSize,
                        FileSeq = fileIndex.MD5,
                        AccessKey = fileIndex.AccessKey
                    }.ToApiResultForObject();
                }
                else
                {
                    var httpPostFile = HttpContext.Current.Request.Files[0];

                    var FileStream = httpPostFile.InputStream;

                    var RequestFileName = httpPostFile.FileName;

                    FileStream.Seek(0, SeekOrigin.Begin);

                    var FileMD5 = XuHos.Common.Utility.HashHelper.ComputeMD5(FileStream);

                    //扩展名
                    var FileUrl = FileMD5 + Path.GetExtension(RequestFileName);

                    var FileSize = FileStream.Length;

                    //通过缓存来判断文件是否已经上传了                    
                    CacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<SysFileIndex>(StringCacheKeyType.SysFileIndex, FileMD5);

                    //缓存中获取文件索引
                    fileIndex = CacheKey.FromCache();

                    //文件已经存在
                    if (fileIndex != null)
                    {
                        //返回上传结果
                        return new ResponseUploadFileDTO
                        {
                            UrlPrefix = config.UrlPrefix,
                            FileName = string.IsNullOrEmpty(Name) ? string.Format("{0}/{1}", fileIndex.FileType, fileIndex.FileUrl) : Name,
                            MD5 = fileIndex.MD5,
                            FileSize = fileIndex.FileSize,
                            FileSeq = fileIndex.MD5,
                            AccessKey = fileIndex.AccessKey
                        }.ToApiResultForObject();
                    }
                    //文件不存在的才保存到服务器
                    else
                    {
                        await XuHos.Common.Storage.Manager.Instance.WriteFile(FileType, FileUrl, FileStream);

                        SysFileIndexService fileService = new SysFileIndexService(CurrentOperatorUserID);
                        fileIndex = new SysFileIndex()
                        {
                            MD5 = FileMD5,
                            FileType = FileType,
                            FileUrl = FileUrl,
                            FileSize = FileSize,
                            Remark = string.IsNullOrEmpty(Name) ? FileUrl : Name,
                            AccessKey = AccessKey
                        };

                        var result = new ResponseUploadFileDTO
                        {
                            UrlPrefix = config.UrlPrefix,
                            FileName = string.IsNullOrEmpty(Name) ? string.Format("{0}/{1}", FileType, FileUrl) : Name,
                            MD5 = FileMD5,
                            FileSize = FileSize,
                            FileSeq = fileIndex.MD5,
                            AccessKey = fileIndex.AccessKey
                        };

                        var SaveFlag = false;

                        //文件不存在
                        if (!fileService.Exists(a => a.MD5 == FileMD5))
                        {
                            try
                            {
                                //添加记录
                                SaveFlag = fileService.Insert(fileIndex);
                            }
                            //主键冲突异常
                            catch (ConstraintException ex)
                            {
                                SaveFlag = true;
                            }
                        }
                        else
                        {
                            fileService.Update(fileIndex);
                            SaveFlag = true;
                        }

                        //保存成功
                        if (SaveFlag)
                        {

                            //设置缓存
                            fileIndex.ToCache(CacheKey, TimeSpan.FromHours(2));

                            return result.ToApiResultForObject();
                        }
                        else
                        {
                            return EnumApiStatus.BizError.ToApiResultForApiStatus();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += System.Environment.NewLine + ex.InnerException.Message;

                    if (ex.InnerException.InnerException != null)
                    {
                        msg += System.Environment.NewLine + ex.InnerException.InnerException.Message;
                    }

                }
                LogHelper.WriteError(ex);

                return EnumApiStatus.BizError.ToApiResultForApiStatus("上传失败，错误:" + msg);
            }

        }



        /**
          * @api {Get} /GetAccessSignature S0107/获取读取访问签名
          * @apiGroup Store
          * @apiVersion 0.0.1
          * @apiDescription 获取读取访问签名 
          * @apiPermission 已登录
          * @apiHeader {String} apptoken appToken
          * @apiHeader {String} noncestr 随机数，每次调用接口不能重复，长度10到40的字母或数字组成
          * @apiHeader {String} usertoken 登录用户token，用户未登录时传空
          * @apiHeader {String} sign  apptoken=@apptoken&noncestr=@noncestr&usertoken=@userToken&appkey=@appkey MD5加密后转成大写  
          * @apiParam {String} ResourceID 文件唯一标识
          * @apiParam {String} AccessKey 文件唯一标识
          * @apiParam {String} [AccessStartTime] 文件唯一标识
          * @apiParam {String} [AccessExpiryTime] 文件唯一标识
          * @apiParamExample {json} 请求样例：
            ?ResourceID=MD5&AccessKey=XXXXXX&AccessStartTime=2017-08-01&AccessExpiryTime=2017-08-02
          * @apiSuccessExample {json} 返回样例:
          https://cnd.kmwlyy.com/files/xxxxx
          **/
        [Route("~/GetAccessSignatureUrl")]
        [ApiOperateNotTrack]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        [HttpGet]
        public ApiResult GetAccessSignatureUrl(
            string ResourceID = "", 
            string AccessKey = "", 
            DateTime? AccessStartTime = null,
            DateTime? AccessExpiryTime = null)
        {
            var fileService = new SysFileIndexService("");

            #region 从缓存中获取，如果不存在则重建缓存              

            var fileIndexCacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<SysFileIndex>(StringCacheKeyType.SysFileIndex, ResourceID);

            //获取缓存中的数据
            var fileIndex = fileIndexCacheKey.FromCache();

            //从数据库中重建索引
            if (fileIndex == null)
            {
                if (ResourceID.Length == 32)
                {
                    fileIndex = fileService.Single<SysFileIndex>(a => a.MD5 == ResourceID);
                }
                else
                {
                    fileIndex = fileService.Single<SysFileIndex>(a => a.FileUrl == ResourceID);
                }

                if (fileIndex != null)
                {
                    fileIndex.ToCache(fileIndexCacheKey, TimeSpan.FromHours(2));
                }
            }
            #endregion

            //验证访问权限
            if (fileIndex != null && (fileIndex.AccessKey == AccessKey || string.IsNullOrEmpty(fileIndex.AccessKey)))
            {
                var signUrl = XuHos.Common.Storage.Manager.Instance.GetReadAccessSignature(
                    fileIndex.FileType,
                    fileIndex.FileUrl,
                    AccessStartTime,
                    AccessExpiryTime);

                return signUrl.ToApiResultForObject();
                //return EnumApiStatus.BizOK.ToApiResultForApiStatus(signUrl);
            }
            else
            {
                return EnumApiStatus.ApiUserUnauthorized.ToApiResultForApiStatus("","AccessKey 与 ResourceID 不匹配");
            }
        }
        #endregion

        #region 下载


        [HttpGet]
        [Route("~/QRCode")]
        [ApiOperateNotTrack]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        public async Task<HttpResponseMessage> QRCode(string data)
        {

            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(data, Encoding.Default);        
            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            result.Content = new StreamContent(ms);
            return result;
        }

        [HttpGet]
        [Route("~/{FileType}/{ResourceID}")]
        [Route("~/{FileType}/{ResourceID}.{ResizeCMD}")]
        [ApiOperateNotTrack]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        public async Task<HttpResponseMessage> Download(
            string FileType,
            string ResourceID,
            string ResizeCMD = "")
        {
            return await Task.Run<HttpResponseMessage>(async () =>
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                SysFileIndexService fileService = new SysFileIndexService(CurrentOperatorUserID);

                SysFileIndex fileIndex = null;

                #region 修正命令
                if (!string.IsNullOrEmpty(ResizeCMD) && !ResizeCMD.ToUpper().Contains("X"))
                {
                    ResourceID = ResourceID + "." + ResizeCMD;
                    ResizeCMD = "";
                }
                #endregion

                ResourceID = ResourceID.ToUpper();

                #region 从缓存中获取，如果不存在则重建缓存              

                var fileIndexCacheKey = new XuHos.Common.Cache.Keys.EntityCacheKey<SysFileIndex>(StringCacheKeyType.SysFileIndex, ResourceID);

                //获取缓存中的数据
                fileIndex = fileIndexCacheKey.FromCache();

                //从数据库中重建索引
                if (fileIndex == null)
                {
                    if (ResourceID.Length == 32)
                    {
                        fileIndex = fileService.Single<SysFileIndex>(a => a.MD5 == ResourceID && a.FileType == FileType);
                    }
                    else
                    {
                        fileIndex = fileService.Single<SysFileIndex>(a => a.FileUrl == ResourceID && a.FileType == FileType);
                    }

                    if (fileIndex != null)
                    {
                        fileIndex.ToCache(fileIndexCacheKey, TimeSpan.FromHours(2));
                    }
                }
                #endregion

                if (fileIndex != null)
                {
                    string etag = string.Format("\"{0}\"", fileIndex.MD5);
                    var tag = Request.Headers.IfNoneMatch.FirstOrDefault();
                    if (Request.Headers.IfModifiedSince.HasValue && tag != null && tag.Tag == etag)
                    {
                        result.StatusCode = HttpStatusCode.NotModified;
                    }
                    else
                    {
                        #region 重新读取文件                      

                        if (XuHos.Common.Storage.Manager.Instance.Exists(FileType, fileIndex.FileUrl))
                        {
                            #region 文件存在

                            //不需要缩放
                            if (!string.IsNullOrEmpty(ResizeCMD) && FileType.ToLower() == "images")
                            {
                                //80X80 统一成小写  Exists 的时候 不区分大小，但是OpenFile确区分大小写
                                ResizeCMD = ResizeCMD.ToLower();
                                //E:\XXX\XXX.80X80.jpg
                                var resizeImgOtputPath = string.Format("{0}.{1}", fileIndex.FileUrl, ResizeCMD);

                                if (XuHos.Common.Storage.Manager.Instance.Exists(FileType, resizeImgOtputPath))
                                {
                                    var stream = await XuHos.Common.Storage.Manager.Instance.OpenFile(FileType,resizeImgOtputPath);
                                    result.Content = new StreamContent(stream);
                                }
                                else
                                {
                                    var stream = await XuHos.Common.Storage.Manager.Instance.OpenFile(FileType, fileIndex.FileUrl);

                                    using (ImageMagick.MagickImage image = new ImageMagick.MagickImage(stream))
                                    {
                                        ImageMagick.MagickGeometry size = new ImageMagick.MagickGeometry(ResizeCMD);

                                        image.Resize(size);

                                        var resizeStream = new System.IO.MemoryStream();

                                        image.Write(resizeStream);

                                        await XuHos.Common.Storage.Manager.Instance.WriteFile(FileType, resizeImgOtputPath, resizeStream);

                                        result.Content = new ByteArrayContent(image.ToByteArray());
                                    }
                                }

                            }
                            else
                            {
                                var stream = await XuHos.Common.Storage.Manager.Instance.OpenFile(FileType, fileIndex.FileUrl);
                                result.Content = new StreamContent(stream);
                            }

                            #endregion

                            result.Content.Headers.ContentType = new MediaTypeHeaderValue("Store/octet-stream");
                            result.Content.Headers.Add("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(fileIndex.Remark) + "\"");
                            result.Headers.ETag = new EntityTagHeaderValue(etag);
                            result.Headers.CacheControl = new CacheControlHeaderValue();
                            result.Headers.CacheControl.Public = true;
                            result.Headers.CacheControl.MaxAge = TimeSpan.FromHours(480);
                            result.Content.Headers.Expires = DateTimeOffset.Now.AddDays(20);
                            result.Content.Headers.LastModified = fileIndex.CreateTime;
                        }
                        else
                        {
                            result.StatusCode = HttpStatusCode.NotFound;
                        }
                        #endregion
                    }
                }
                else
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                }

                return result;

            });
        }

        /// <summary>
        /// 获取文件内容(兼容老版本文件访问方式)    
        
        /// 日期：2016年9月28日
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="Prefix">路径前缀</param>
        /// <returns></returns>
        [HttpGet]
        [Route(@"~/{FileType}/{*FilePath:regex(.*[.jpg|.png|.gif|.jpeg])}")]
        [ApiOperateNotTrack]
        [IgnoreAuthenticate]
        [IgnoreUserAuthenticate]
        public async Task<HttpResponseMessage> DownloadByAbsouteFilePath(string FileType, string FilePath = "")
        {
            return await Task.Run<HttpResponseMessage>(async () =>
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                #region 获取解析
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(?<FilePath>.*[.jpg|.png|.gif|.jpeg|.cab|.apk])");
                var match = reg.Match(FilePath);
                if (match.Success)
                {
                    FilePath = match.Groups["FilePath"].Value;
                }

                reg = new System.Text.RegularExpressions.Regex(@"[0-9]+X[0-9]+");
                match = reg.Match(FilePath);
                var ResizeCMD = "";
                if (match.Success)
                {
                    ResizeCMD = match.Value;
                }
                #endregion

                if (XuHos.Common.Storage.Manager.Instance.Exists(FileType, FilePath))
                {
                    //不需要缩放
                    if (!string.IsNullOrEmpty(ResizeCMD))
                    {

                        //E:\XXX\XXX.80X80.jpg
                        var resizeImgOtputPath = string.Format("{0}.{1}", FilePath, ResizeCMD);

                        if (XuHos.Common.Storage.Manager.Instance.Exists(FileType, resizeImgOtputPath))
                        {
                            result.Content = new StreamContent(await XuHos.Common.Storage.Manager.Instance.OpenFile(FileType, resizeImgOtputPath));
                        }
                        else
                        {
                            var stream = await XuHos.Common.Storage.Manager.Instance.OpenFile(FileType, FilePath);

                            using (ImageMagick.MagickImage image = new ImageMagick.MagickImage(stream))
                            {
                                ImageMagick.MagickGeometry size = new ImageMagick.MagickGeometry(ResizeCMD);

                                image.Resize(size);

                                var resizeStream = new System.IO.MemoryStream();

                                image.Write(resizeStream);

                                await XuHos.Common.Storage.Manager.Instance.WriteFile(FileType, resizeImgOtputPath, resizeStream);

                                result.Content = new ByteArrayContent(image.ToByteArray());
                            }
                        }
                    }
                    else
                    {
                        result.Content = new StreamContent(await XuHos.Common.Storage.Manager.Instance.OpenFile(FileType, FilePath));
                    }


                    result.Headers.CacheControl = new CacheControlHeaderValue();
                    result.Headers.CacheControl.Public = true;
                    result.Headers.CacheControl.MaxAge = TimeSpan.FromHours(480);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("Store/octet-stream");
                    result.Content.Headers.Expires = DateTimeOffset.Now.AddDays(20);

                }
                else
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                }
                return result;

            });
        }
        #endregion

    }
    
}


