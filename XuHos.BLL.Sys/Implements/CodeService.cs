
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;
using XuHos.Resource;

namespace XuHos.BLL.Sys.Implements
{
    public class CodeService
    {
        public static CodeDictionary<string> GetRelationships()
        {
            CodeDictionary<string> ret = new CodeDictionary<string>();
            ret.Add("00", Lang.Myself);
            ret.Add("01", Lang.Spouse);
            ret.Add("02", Lang.Father);
            ret.Add("03", Lang.Mother);
            ret.Add("04", Lang.Son);
            ret.Add("05", Lang.Daughter);
            ret.Add("06", Lang.Other);
            return ret;
        }
        public static CodeDictionary<string> GetGenders()
        {
            CodeDictionary<string> ret = new CodeDictionary<string>();
            ret.Add("0", Lang.Male);
            ret.Add("1", Lang.Female);
            //ret.Add("3", Lang.Unknown);
            return ret;
        }
        public static CodeDictionary<string> GetIDTypes()
        {
            CodeDictionary<string> ret = new CodeDictionary<string>();
            ret.Add("0", Lang.IDCard);
            ret.Add("1", Lang.ResidentsBooklet);
            ret.Add("2", Lang.Passport);
            ret.Add("3", Lang.MilitaryOfficer);
            ret.Add("4", Lang.DriverLicense);
            ret.Add("5", Lang.HKMacaoPass);
            ret.Add("6", Lang.TaiwanPass);
            ret.Add("7", Lang.HMTIDCard);
            ret.Add("99", Lang.Other);
            return ret;
        }
        static CodeDictionary<string> Positions = null;
        public static CodeDictionary<string> GetDoctorTitles()
        {
            if(Positions == null)
            {
                Positions = new CodeDictionary<string>();
                foreach (EnumDoctorTitle type in Enum.GetValues(typeof(EnumDoctorTitle)))
                {
                    Positions.Add(((int)type).ToString(), type.GetEnumDescript());
                }
            }
            return Positions;
        }
        public static CodeDictionary<string> GetDiseases()
        {
            CodeDictionary<string> ret = new CodeDictionary<string>();
            ret.Add("01", "高血压");
            ret.Add("02", "高血脂");
            ret.Add("03", "糖尿病");
            ret.Add("04", "冠心病");
            ret.Add("05", "恶性肿瘤");
            ret.Add("06", "结核病");
            ret.Add("07", "心律失常");
            ret.Add("08", "其他");
            return ret;
        }
        public static CodeDictionary<string> GetUnits()
        {
            CodeDictionary<string> ret = new CodeDictionary<string>();
            ret.Add("01", "支");
            ret.Add("02", "瓶");
            ret.Add("03", "克");
            ret.Add("04", "次");
            return ret;
        }
        public static CodeDictionary<int> GetChargeTypes()
        {
            CodeDictionary<int> ret = new CodeDictionary<int>();
            ret.Add(1, "检验项目");
            ret.Add(2, "检查项目");
            ret.Add(3, "药品明细");
            return ret;
        }

        public static CodeDictionary<int> GetControllerTypes()
        {
            CodeDictionary<int> ret = new CodeDictionary<int>();
            ret.Add(1, "MVC");
            ret.Add(2, "API");
            return ret;
        }

        public static List<SysDrugRouteDto> GetDrugRotes()
        {
            List<SysDrugRouteDto> list = new List<SysDrugRouteDto>();
            list.Add(new SysDrugRouteDto("口服", "KF", "KE"));
            list.Add(new SysDrugRouteDto("含服", "HF", "WE"));
            list.Add(new SysDrugRouteDto("煎服", "JF", "UE"));
            list.Add(new SysDrugRouteDto("外用加棉签", "WYJMQ", "QELST"));
            list.Add(new SysDrugRouteDto("舌下含服", "SXHF", "THWE"));
            list.Add(new SysDrugRouteDto("眼用", "YY", "HE"));
            list.Add(new SysDrugRouteDto("滴耳", "DE", "IB"));
            list.Add(new SysDrugRouteDto("吸入", "XR", "KT"));
            list.Add(new SysDrugRouteDto("滴鼻", "DB", "IT"));
            list.Add(new SysDrugRouteDto("含漱", "HS", "WI"));
            list.Add(new SysDrugRouteDto("外用", "WY", "QE"));
            list.Add(new SysDrugRouteDto("喷喉", "PH", "KK"));
            list.Add(new SysDrugRouteDto("吸氧", "XY", "KR"));
            list.Add(new SysDrugRouteDto("吸痰", "XT", "KU"));
            list.Add(new SysDrugRouteDto("雾化吸入", "WHXR", "FWKT"));
            list.Add(new SysDrugRouteDto("涂口腔", "TKQ", "IKE"));
            list.Add(new SysDrugRouteDto("塞肛", "SG", "PE"));
            list.Add(new SysDrugRouteDto("阴道给药", "YDGY", "BUXA"));
            list.Add(new SysDrugRouteDto("洗口腔", "XKQ", "IKE"));
            return list;
        }
    }
}
