using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;

namespace SyncProjectInfo
{
    public class ProjectTransfer
    {
        /// <summary>
        /// 获取项目的信息
        /// </summary>
        /// <param name="PIItemID">项目属性任务编号</param>
        /// <returns>返回项目信息</returns>
        /// 
      
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
                        project.ProjectSpaceID = bug.SubProjectID;
                    }
                    if (project.ProjectSpaceID > 0)
                    {
                        var hiddensubprojectid = -(1500000001 + project.ProjectSpaceID);
                        var hiddenbug = (from c in dbcontext.Bug where c.ProjectID == 502 && c.SubProjectID == hiddensubprojectid select c).SingleOrDefault();
                        {
                            project.HiddenTaskID = hiddenbug.BugID;//项目的属性tab所属的隐藏的任务编号
                        }
                    }
                    else
                        return null;//未获取到项目属性隐藏的任务编号
                   
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
                        project.ProjectMembers = (from a in customFields2 where a.PageNumber == 15 select a.Custom_5).SingleOrDefault();//项目团队成员(下拉框文本)
                       
                        project.ProjectMemberList = (from members in dbcontext.BugSelectionInfo where members.ProjectID == 502 && members.BugID == PIItemID && members.FieldID == 12505 select members.FieldSelectionID).ToList<int>();  //项目团队成员下拉列表
                        if (project.ProjectMemberList.Count > 0)
                        {
                            project.SelectedProjecMembers = (from persons in dbcontext.LogIn join list in project.ProjectMemberList on persons.PersonID equals list select persons).ToList<LogIn>();
                            project.ProjectMembersText = string.Join(",", (from member in project.SelectedProjecMembers select member.FName).ToArray()); 
                        }
                       project.ProjectGole = (from a in customFields2 where a.PageNumber == 9 select a.Custom_10).SingleOrDefault();//项目目标
                        //project.ProjectMembersText = (from a in customFields2 where a.PageNumber == 15 select a.Custom_5).SingleOrDefault();//项目团队成员(多行文本框)
               

                    }

                    return project;
                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }
        }

        public static int UpdateProjectInfo(int PIItemID)
        {
            Project project = LoadProjectInfo(PIItemID);
            using (GXX_DS_0312Entities dbcontext = new GXX_DS_0312Entities())
            {
                //更新项目的信息
                //PageNumber=1001 
                CustomerFieldTrackExt2 pageNum1001 = new CustomerFieldTrackExt2()
            {
                ProjectID = 502,
                IssueID = project.HiddenTaskID,
                PageNumber=1001,
                Custom_3 = project.ProjectCode,
                Custom_2 = project.ProjectTtile,
                Custom_9 = project.ProjectStatus,
                Custom_7 = project.ProjectLevel,
                Custom_8 = project.DevWay,
                Custom_6 = project.ProjectType,
                Custom_11 = project.ProjectGole,
                Custom_12 = project.ProjectDesc
            };
                
                //PageNumber=1002 
                CustomerFieldTrackExt2 pageNum1002 = new CustomerFieldTrackExt2()
            {
                ProjectID = 502,
                IssueID = project.HiddenTaskID,
                PageNumber=1002,
                Custom_4 = project.ProductName,
                Custom_2=project.PlanStartDate,
                Custom_3=project.PlanFinishDate,
                Custom_1=project.ProjectManager,
                Custom_5=project.ProductManager,
                Custom_9=project.ContractMoney,
                Custom_7=project.DevManager,
                Custom_8=project.TestManager
            };


                //PageNumber=1003
                CustomerFieldTrackExt2 pageNum1003 = new CustomerFieldTrackExt2()
            {
                ProjectID = 502,
                IssueID = project.HiddenTaskID,
                PageNumber=1003,
                Custom_2 = project.ProductName,
                Custom_9=project.BelongTo,
                Custom_4=project.DevModle,
                Custom_7=project.ScoreMoney,
                Custom_5=project.TransfterProject,
                Custom_3=project.Level,
                Custom_6=project.PatchDeliver
            };

                //项目团队成员多行文本框 pagenumber=15 
                CustomerFieldTrackExt2 membertext = new CustomerFieldTrackExt2()
                {
                    ProjectID = 502,
                    IssueID =PIItemID,
                    PageNumber = 15,
                    Custom_5 = project.ProjectMembersText
                };

                UpdateMemberList(PIItemID);//比较当前项目的已选成员列表和最新的成员列表，进行资源的更新

                dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1001).State = EntityState.Modified;
                dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1002).State = EntityState.Modified;
                dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1003).State = EntityState.Modified;
                dbcontext.Entry<CustomerFieldTrackExt2>(membertext).State = EntityState.Modified;
                return  dbcontext.SaveChanges();
            
            }
        }

        /// <summary>
        /// 比较当前项目的已选成员列表和最新的成员列表，进行资源的更新
        /// </summary>
        private static void UpdateMemberList(int PIItemID)
        {
            Project project = LoadProjectInfo(PIItemID);
            //最新选择的项目成员列表
          List<int> newMemberList=  project.ProjectMemberList;
            List<int> currentMemberList = new List<int>();
            using (GXX_DS_0312Entities dbcontext= new GXX_DS_0312Entities())
            {
                //当前项目成员列表
                 currentMemberList = (from c in dbcontext.SubProjectOwners where c.ProjectID == 502 && c.SubProjectID == project.ProjectSpaceID select c.TeamMemberID).ToList<int>();
            

            //获取是新增的成员，遍历插入
            List<int> addedList = newMemberList.Except(currentMemberList).ToList();

            List<int> deletedList = currentMemberList.Except(newMemberList).ToList();
             string sql = string.Empty;
            foreach (var item in addedList)
            {
                    sql = "insert into SubProjectOwners(ProjectID,SubProjectID,TeamMemberID,Unit) values (@ProjectID,@SubProjectID,@TeamMemberID,@Unit)";
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@ProjectID",502),
                        new SqlParameter("@SubProjectID",project.ProjectSpaceID),
                        new SqlParameter("@TeamMemberID",item),
                        new SqlParameter("@Unit",100)
                    };
                    dbcontext.Database.ExecuteSqlCommand(sql, param);
            }

                foreach (var item in deletedList)
                {
                    sql = "delete from SubProjectOwners where ProjectID=@ProjectID and SubProjectID=@SubProjectID and TeamMemberID=@TeamMemberID ";
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@ProjectID",502),
                        new SqlParameter("@SubProjectID",project.ProjectSpaceID),
                        new SqlParameter("@TeamMemberID",item)
                    };
                    dbcontext.Database.ExecuteSqlCommand(sql, param);
                }

            }
        }
    }
}
