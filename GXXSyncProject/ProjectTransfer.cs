using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GXXSyncProject
{
    /// <summary>
    /// 项目信息转化器
    /// 需要实现的功能包括如下
    /// **
    /// 1. 根据接收的项目属性条目编号获取项目相关信息
    /// 2. 根据项目属性条目，获取对应的ppm space id，以及 hiden的task id
    /// 3. 根据hidden的task id，更新项目信息（特殊的把项目成员写入到多行文本框中，并且同步资源到其他模块中）
    /// </summary>
 public   class ProjectTransfer
    {
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="PIItemID">项目信息任务id</param>
        /// <returns>返回项目信息实体类</returns>
        public static Project LoadProjectInfo(int PIItemID)
        {
            Project project = new Project();
            using (GXX_DS_0312Entities dbcontext= new GXX_DS_0312Entities())
            {
                try
                {
                    //名称，描述
                    var bug = (from c in dbcontext.Bug where c.ProjectID == 502 && c.BugID == PIItemID select c).SingleOrDefault();
                    {
                        project.ProjectTtile = bug.BugTitle;//名称
                        project.ProjectDesc = bug.ProblemDescription;//描述
                    }
                    var customFields2 = (from fields2 in dbcontext.CustomerFieldTrackExt2 where fields2.ProjectID == 502 && fields2.IssueID == PIItemID select fields2);
                    {
                        project.ProjectCode = (from a in customFields2 where a.PageNumber == 12 select a.Custom_8).SingleOrDefault();//项目编码
                        project.ProductCode = (from a in customFields2 where a.PageNumber == 13 select a.Custom_1).SingleOrDefault();//产品代码
                        project.ProductName = (from a in customFields2 where a.PageNumber == 13 select a.Custom_2).SingleOrDefault();//产品名称
                        project.BelongTo = (from a in customFields2 where a.PageNumber == 13 select a.Custom_4).SingleOrDefault();//所属单位
                        project.ProjectStatus = (from a in customFields2 where a.PageNumber == 14 select a.Custom_3).SingleOrDefault();//项目状态
                        project.PlanStartDate = (from a in customFields2 where a.PageNumber == 14 select a.Custom_5).SingleOrDefault();//计划开始时间
                        project.PlanFinishDate = (from a in customFields2 where a.PageNumber == 14 select a.Custom_6).SingleOrDefault();//计划完成时间
                        project.ProjectLevel = (from a in customFields2 where a.PageNumber == 13 select a.Custom_6).SingleOrDefault();//项目级别
                        project.ProjectManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_4).SingleOrDefault();//项目经理
                        project.DevWay = (from a in customFields2 where a.PageNumber == 14 select a.Custom_2).SingleOrDefault();// 开发方式
                        project.ProductManager = (from a in customFields2 where a.PageNumber == 6 select a.Custom_3).SingleOrDefault();// 产品经理(PM)
                        project.ContractMoney = (from a in customFields2 where a.PageNumber == 15 select a.Custom_4).SingleOrDefault();// 合同额（万）
                        project.DevModle = (from a in customFields2 where a.PageNumber == 15 select a.Custom_2).SingleOrDefault();// 开发模式
                        project.DevManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_7).SingleOrDefault();//开发负责人
                        project.ScoreMoney = (from a in customFields2 where a.PageNumber == 14 select a.Custom_1).SingleOrDefault();//贡献值（万）
                        project.ProjectType = (from a in customFields2 where a.PageNumber == 13 select a.Custom_5).SingleOrDefault();//项目类型
                        project.TestManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_8).SingleOrDefault();//测试负责人
                        project.TransfterProject = (from a in customFields2 where a.PageNumber == 13 select a.Custom_7).SingleOrDefault();//转产项目
                        project.Level = (from a in customFields2 where a.PageNumber == 15 select a.Custom_1).SingleOrDefault();//重要程度
                        project.PatchDeliver = (from a in customFields2 where a.PageNumber == 13 select a.Custom_8).SingleOrDefault();//批量发货
                        project.ProjectMembers = (from a in customFields2 where a.PageNumber == 15 select a.Custom_5).SingleOrDefault();//项目团队成员(下拉框)
                        project.ProjectGole = (from a in customFields2 where a.PageNumber == 9 select a.Custom_10).SingleOrDefault();//项目目标
                        project.ProjectMembersText = (from a in customFields2 where a.PageNumber == 15 select a.Custom_5).SingleOrDefault();//项目团队成员(多行文本框)

                    }

                    return project;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
              

            }
        }
        /// <summary>
        /// 获取项目空间编号
        /// </summary>
        /// <param name="PIItemID">项目信息任务ID</param>
        /// <returns>返回父项目空间编号</returns>
        private static int GetParentSpaceID(int PIItemID)
        {
            //编写根据条目ID获取父空间目录
            return 24288;
        }
        //public static string UpdateProjectInfo(int PIItemID)
        //{
        //    Project project = LoadProjectInfo(PIItemID);
        //    project.ProjectSpaceID = GetParentSpaceID(PIItemID);
        //    using (GXX_DS_0312Entities dbcontext = new GXX_DS_0312Entities())
        //    {
        //        //名称，描述
        //        var bug = (from c in dbcontext.Bug where c.ProjectID == 502 && c.SubProjectID == -(1500000001 + project.ProjectSpaceID) select c).SingleOrDefault();
        //        {
        //            project.HiddenTaskID =  bug.BugID;
        //        }

        //        if (project.HiddenTaskID > 0 && project.ProjectSpaceID > 0)
        //        {

        //            return project.ToString();
        //        }
               
                

        //    }
                
          
        //}
    }
}
