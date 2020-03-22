using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncProjectInfo
{
  public  class Project
    {
        public string ProjectCode { get; set; } //项目编码
        public string ProjectTtile { get; set; }//项目名称
        public string ProductCode { get; set; } //产品代码
        public string ProductName { get; set; }//产品名称
        public string BelongTo { get; set; }//所属单位
        public string ProjectStatus { get; set; }//项目状态
        public string PlanStartDate { get; set; }
        public string PlanFinishDate { get; set; }
        public string ProjectLevel { get; set; }
        public string ProjectManager { get; set; }
        public string DevWay { get; set; }
        public string ProductManager { get; set; }
        public string ContractMoney { get; set; }
        public string DevModle { get; set; }
        public string DevManager { get; set; }
        public string ScoreMoney { get; set; }//贡献值
        public string ProjectType { get; set; }
        public string TestManager { get; set; }
        public string TransfterProject { get; set; }//转产项目
        public string Level { get; set; }//重要程度
        public string PatchDeliver { get; set; }//批量发货
        public string ProjectMembers { get; set; } //项目下拉成员的文本
        public List<int> ProjectMemberList { get; set ; }
        //public List<LogIn> SelectedProjecMembers;
        public string ProjectGole { get; set; }//项目目标
        public string ProjectDesc { get; set; }//项目描述
        public string ProjectMembersText { get; set; }//项目团队成员文本框



        public int? HiddenTaskID { get; set; }
        public int? ProjectSpaceID { get; set; }
        
        public Project()
        {
           // SelectedProjecMembers = new List<LogIn>();
            ProjectMemberList = new List<int>();
        }

        public override string ToString()
        {
            return $"当前更新的项目信息是: 项目编码" + this.ProjectCode + "...其他信息待补充";
        }
    }
}
