using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 体验记录（调第三方API）
/// </summary>
namespace KMEHosp.DTO.Platform
{
    
    public class MessageResult
    {
        public int PagesCount { get; set; }
        public int Status { get; set; } //0正常，1异常
        public string Msg { get; set; }
    }

    public class ApiListResult<T> : MessageResult
    {
        public int Total { get; set; }

        public List<T> Data { get; set; }
    }

    public class ApiModelResult<T> : MessageResult
    {
        public T Data { get; set; }
    }

    public class Questionnaires
    {
        public string score { get; set; }
        public string type { get; set; }
    }

    public class ExaminedRecords
    {
        public int ExamId { get; set; }

        public int PersonId { get; set; }

        public string ExamNo { get; set; }

        public DateTime ExamDate { get; set; }

        public string ExamType { get; set; }

        public string VisitWay { get; set; }

        public string Doctor { get; set; }

        public string CreateDate { get; set; }

        public string CreateBy { get; set; }

        public string UpdateDate { get; set; }

        public string UpdateBy { get; set; }

        public int Status { get; set; }


    }

    public class PersonInfo
    {
        public int PersonId { get; set; }
        public string PersonNo { get; set; }  // "150727197702091748",
        public string RecordNo { get; set; }  // null,
        public string Name { get; set; }  // "dan",
        public string Gender { get; set; }  // "1",
        public string BirthDate { get; set; }  // "19900101",
        public string Country { get; set; }  // null,
        public string Nationality { get; set; }  // null,
        public string MarriageStatus { get; set; }  // null,
        public string IDType { get; set; }  // "01",
        public string IDNumber { get; set; }  // "150727197702091748",
        public string Phone { get; set; }  // "13131111111",
        public string ContactName { get; set; }  // null,
        public string ContactPhone { get; set; }  // null,
        public string EmailAddress { get; set; }  // null,
        public string CensusRegisterFlag { get; set; }  // null,
        public string CensusAddressCode { get; set; }  // null,
        public string CensusAddressName { get; set; }  // "广东省深圳市福田区hunansheng",
        public string CensusPostCode { get; set; }  // null,
        public string CurrentAddressCode { get; set; }  // null,
        public string CurrentAddressName { get; set; }  // null,
        public string CurrentPostCode { get; set; }  // null,
        public string Company { get; set; }  // null,
        public string HireDate { get; set; }  // null,
        public string OccupationClass { get; set; }  // null,
        public string EducationLevel { get; set; }  // null,
        public string InsuranceType { get; set; }  // null,
        public string InsuranceTypeName { get; set; }  // null,
        public string PayMethod { get; set; }  // null,
        public string ABOType { get; set; }  // null,
        public string RHType { get; set; }  // null,
        public string AllergyHistory { get; set; }  // null,
        public string RiskFactors { get; set; }  // null,
        public string DisabilityStatus { get; set; }  // null,
        public string Community { get; set; }  // null,
        public string CommunityContact { get; set; }  // null,
        public string CommunityContactPhone { get; set; }  // null,
        public string ResponsibleOrganization { get; set; }  // null,
        public string ResponsibleOrganizationID { get; set; }  // null,
        public string ResponsibleDoctor { get; set; }  // null,
        public string ResponsibleDoctorPhone { get; set; }  // null,
        public string ArchiveDate { get; set; }  // null,
        public string SourceType { get; set; }  // null,
        public string Remark { get; set; }  // "123456789A3A"


    }

    public class ExaminedItem
    {
        public int ResultId { get; set; }  //10359,
        public int ExamId { get; set; }  //1444,
        public int ItemId { get; set; }  //566,
        public string ItemCode { get; set; }  //null,
        public string Name { get; set; }  //"体温（℃）",
        public string Result { get; set; }  //"37.3",
        public string CreateDate { get; set; }  //"2016-06-06T13:28:54",
        public string CreateBy { get; set; }  //null,
        public string UpdateDate { get; set; }  //null,
        public string UpdateBy { get; set; }  //null,
        public string Remark { get; set; }  //null,
        public string LabTestResults { get; set; }  //null

    }
}
