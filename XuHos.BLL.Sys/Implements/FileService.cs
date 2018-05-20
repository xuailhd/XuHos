using System.Linq;
using XuHos.Common.Enum;
using XuHos.DAL.EF;
using XuHos.Entity;

namespace XuHos.BLL.Sys.Implements
{
    public class FileService : BLL.Common.CommonBaseService<UserFile>
    {
        public FileService() : base("")
        {

        }

        public UserFile GetFile(string UserID, string OutID, int? FileType = null)
        {
            using (var db = new DBEntities())
            {
                var userRole = (from roleMap in db.UserRoleMaps
                                join role in db.UserRoles on roleMap.RoleID equals role.RoleID
                                where roleMap.UserID == UserID && (role.RoleType == EnumRoleType.DrugOper || role.RoleType == EnumRoleType.DrugManage || role.RoleType == EnumRoleType.DrugTreatment)
                                select role).FirstOrDefault();

                var query = db.UserFiles.Where(t => t.OutID == OutID);

                if (FileType.HasValue)
                {
                    query = query.Where(x => x.FileType == FileType.Value);
                }
                query = from userFile in query
                        join file in db.SysFileIndexs on userFile.ResourceID equals file.MD5
                        where userFile.UserID == UserID
                        orderby file.FileSize descending    //一个预约多个处方，会生成多张，选择最大的那张
                        select userFile;

                return query.FirstOrDefault();
            }
        }
    

        public UserFile GetFile(string OutID, int FileType)
        {
            using (var db = new DBEntities())
            {
                var query = db.UserFiles.Where(x => x.OutID == OutID && x.FileType == FileType);
                return query.FirstOrDefault();
            }
        }
    }
}
