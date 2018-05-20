using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace XuHos.DTO
{
    public class RequestUserBuyRecipe
    {
        [Required]
        public string OPDRegisterID { get; set; }

        [Required]
        public List<string> RecipeNos { get; set; }


        /// 收货人详细信息
        /// </summary>
        public XuHos.DTO.Platform.OrderConsigneeDTO Consignee { get; set; }
    }
}
