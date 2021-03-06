﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SyncProjectInfo
{
    [Serializable]
  public  class Project
    {
        [DisplayName("项目编码")]
        public string ProjectCode { get; set; } //项目编码
        [DisplayName("项目名称")]
        public string ProjectTtile { get; set; }//项目名称
        [DisplayName("产品代码")]
        public string ProductCode { get; set; } //产品代码
        [DisplayName("产品名称")]
        public string ProductName { get; set; }//产品名称
        [DisplayName("所属单位")]
        public string BelongTo { get; set; }//所属单位
        [DisplayName("项目状态")]
        public string ProjectStatus { get; set; }//项目状态
        [DisplayName("计划开始时间")]
        public string PlanStartDate { get; set; }
        [DisplayName("计划完成时间")]
        public string PlanFinishDate { get; set; }
        [DisplayName("项目级别")]
        public string ProjectLevel { get; set; }
        [DisplayName("项目经理")]
        public string ProjectManager { get; set; } //项目经理
  
        public int ProjectManagerID { get; set; }//项目经理编号

        [DisplayName("开发方式")]
        public string DevWay { get; set; }
        [DisplayName("产品经理")]
        public string ProductManager { get; set; }//产品经理
        public int ProductManagerID { get; set; }//产品经理编号

        [DisplayName("合同额（万）")]
        public string ContractMoney { get; set; }
        [DisplayName("开发模式")]
        public string DevModle { get; set; }
        [DisplayName("开发负责人")]
        public string DevManager { get; set; }//开发负责人
     
        public int DevManagerID { get; set; }//开发负责人编号
        public List<int> DevManagerIDs { get; set; }
        [DisplayName("贡献值")]
        public string ScoreMoney { get; set; }//贡献值
        [DisplayName("项目类型")]
        public string ProjectType { get; set; }
        [DisplayName("测试负责人")]
        public string TestManager { get; set; }//测试负责人
        public int TestManagerID { get; set; }//测试负责人编号
        public List<int> TestManagerIDs { get; set; }

        [DisplayName("需求分析师")]
        public string RequirementAnalyst { get; set; }
        public List<int> RequirementAnalystIDs { get; set; }


        [DisplayName("开发工程师")]
        public string DevEngineer { get; set; }
        public List<int> DevEngineerIDs { get; set; }


        [DisplayName("测试工程师")]
        public string TestEngineer { get; set; }
        public List<int> TestEngineerIDs { get; set; }

        [DisplayName("质量管理员")]
        public string QualityManager { get; set; }
        public List<int> QualityManagerIDs { get; set; }

        [DisplayName("配置管理员")]
        public string ConfigManager { get; set; }
        public List<int> ConfigManagerIDs { get; set; }


        [DisplayName("转产项目")]
        public string TransfterProject { get; set; }//转产项目
        [DisplayName("重要程度")]
        public string Level { get; set; }//重要程度
        [DisplayName("批量发货")]
        public string PatchDeliver { get; set; }//批量发货
        [DisplayName("其他成员")]
        public string ProjectMembers { get; set; } //项目下拉成员的文本(其他成员)
        public List<int> ProjectMemberList { get; set ; }
        public List<LogIn> SelectedProjecMembers;


        public string ProjectResourceText { get; set; } //项目资源（合并所有人员）
        public List<int> ProjectResourceIDs { get; set; }
        public List<LogIn> ProjectResourceMembers;


        [DisplayName("项目目标")]
        public string ProjectGole { get; set; }//项目目标
        [DisplayName("项目描述")]
        public string ProjectDesc { get; set; }//项目描述
        [DisplayName("项目团队成员文本框")]
        public string ProjectMembersText { get; set; }//项目团队成员文本框
        [DisplayName("商机编号")]
        public string ShangJiID { get; set; }//商机编号
        [DisplayName("结束时间")]
        public string EndDate { get; set; }//结束时间


        public int HiddenTaskID { get; set; }
        [DisplayName("当前Techexcel的项目编号")]
        public int? ProjectSpaceID { get; set; }

        public string ProjectSpaceTitle { get; set; }

        //新增的5个同步字段
        [DisplayName("预转产时间")]
        public string PlanTransferTime { get; set; }
        [DisplayName("实际转产时间")]
        public string ActualTransferTime { get; set; }
        [DisplayName("预退市时间")]
        public string PlanDelistTime { get; set; }
        [DisplayName("实际退市时间")]
        public string ActualDelistTime { get; set; }
        [DisplayName("ERP编号")]
        public string ERPCode { get; set; }

        public Project()
        {
            SelectedProjecMembers = new List<LogIn>();
            ProjectMemberList = new List<int>();
            DevManagerIDs = new List<int>();
            TestManagerIDs = new List<int>();
            RequirementAnalystIDs = new List<int>();
            DevEngineerIDs = new List<int>();
            TestEngineerIDs = new List<int>();
            QualityManagerIDs = new List<int>();
            ConfigManagerIDs = new List<int>();
            ProjectResourceIDs = new List<int>();
            ProjectResourceMembers = new List<LogIn>();
        }



        public override string ToString()
        {
            return $"当前更新的项目信息是: 项目编码" + this.ProjectCode + "...其他信息待补充";
        }
    }
}
