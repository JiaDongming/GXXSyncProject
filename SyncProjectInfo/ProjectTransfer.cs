using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

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
                    var customFields= (from fields in dbcontext.CustomerFieldTrackExt where fields.ProjectID==502 && fields.BugID==PIItemID select fields);
                    var bugselectionInfo = (from items in dbcontext.BugSelectionInfo where items.ProjectID == 502 && items.BugID == PIItemID select items);
                    {
                        project.ProjectCode = (from a in customFields2 where a.PageNumber == 12 select a.Custom_8).SingleOrDefault();//项目编码
                        project.ProductCode = (from a in customFields2 where a.PageNumber == 13 select a.Custom_1).SingleOrDefault();//产品代码
                        project.ProductName = (from a in customFields2 where a.PageNumber == 13 select a.Custom_2).SingleOrDefault();//产品名称
                        project.BelongTo = (from a in customFields2 where a.PageNumber == 13 select a.Custom_4).SingleOrDefault();//所属单位
                        project.ProjectStatus = (from a in customFields2 where a.PageNumber == 14 select a.Custom_3).SingleOrDefault();//项目状态
                        project.PlanStartDate = (from a in customFields2 where a.PageNumber == 14 select a.Custom_5).SingleOrDefault();//计划开始时间
                        project.PlanFinishDate = (from a in customFields2 where a.PageNumber == 14 select a.Custom_6).SingleOrDefault();//计划完成时间
                        project.ProjectLevel = (from a in customFields2 where a.PageNumber == 13 select a.Custom_6).SingleOrDefault();//项目级别
                        //project.ProjectManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_4).SingleOrDefault();//项目经理
                        project.ProjectManager = (from a in customFields   select a.Custom_2).SingleOrDefault();//项目经理文本
                        project.ProjectManagerID = (from id in bugselectionInfo where id.FieldID == 2 select id.FieldSelectionID).FirstOrDefault();//项目经理编号 FieldID=2

                        project.DevWay = (from a in customFields2 where a.PageNumber == 14 select a.Custom_2).SingleOrDefault();// 开发方式

                        project.ProductManager = (from a in customFields2 where a.PageNumber == 6 select a.Custom_3).SingleOrDefault();// 产品经理(PM)
                        project.ProductManagerID = (from id in bugselectionInfo where id.FieldID == 1603 select id.FieldSelectionID).FirstOrDefault();//产品经理编号 FieldID=1603

                        project.ContractMoney = (from a in customFields2 where a.PageNumber == 15 select a.Custom_4).SingleOrDefault();// 合同额（万）
                        project.DevModle = (from a in customFields2 where a.PageNumber == 15 select a.Custom_2).SingleOrDefault();// 开发模式

                        project.DevManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_7).SingleOrDefault();//开发负责人
                        project.DevManagerID = (from id in bugselectionInfo where id.FieldID == 12407 select id.FieldSelectionID).FirstOrDefault();//开发负责人编号 FieldID=12407

                        project.ScoreMoney = (from a in customFields2 where a.PageNumber == 14 select a.Custom_1).SingleOrDefault();//贡献值（万）
                        project.ProjectType = (from a in customFields2 where a.PageNumber == 13 select a.Custom_5).SingleOrDefault();//项目类型

                        project.TestManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_8).SingleOrDefault();//测试负责人
                        project.TestManagerID = (from id in bugselectionInfo where id.FieldID == 12408 select id.FieldSelectionID).FirstOrDefault();//测试负责人编号 FieldID=12408

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

                    LogHelper.Error(ex.Message,ex);
                    return null;
                }


            }
        }

        private static bool CheckSyncOrNot(int PIItemID)
        {
            Project project = LoadProjectInfo(PIItemID);
            using (GXX_DS_0312Entities dbcontext= new GXX_DS_0312Entities())
            {
                int minID = (from c in dbcontext.Bug where c.ProjectID == 502 && c.CrntBugTypeID == 227 && c.SubProjectID == project.ProjectSpaceID select c.BugID).Min<int>();
                if (minID == PIItemID)
                {
                    return true;
                }
                else
                    return false;
            }
    
        }
        /// <summary>
        /// 更新项目的属性页以及资源
        /// </summary>
        /// <param name="PIItemID"></param>
        /// <returns></returns>
        public static int UpdateProjectInfo(int PIItemID)
        {

            LogHelper.WriteLog("开始检测是否满足同步条件");

           bool isNeedSync = CheckSyncOrNot(PIItemID);
            if (isNeedSync)
            {
                LogHelper.WriteLog("满足同步条件！");
                Project project = LoadProjectInfo(PIItemID);
                if (project ==null)
                {
                    LogHelper.WriteLog("待同步的对象为空");
                    return 0;
                }
                else
                {
                    LogHelper.WriteLog("开始同步的项目信息");
                    PrintProperties(project);
                }

                using (GXX_DS_0312Entities dbcontext = new GXX_DS_0312Entities())
                {
                    //更新项目的信息
                    //PageNumber=1001 
                    CustomerFieldTrackExt2 pageNum1001 = new CustomerFieldTrackExt2()
                    {
                        ProjectID = 502,
                        IssueID = project.HiddenTaskID,
                        PageNumber = 1001,
                        Custom_3 = project.ProjectCode,
                        Custom_2 = project.ProjectTtile,
                        Custom_9 = project.ProjectStatus,
                        Custom_7 = project.ProjectLevel,
                        Custom_8 = project.DevWay,//开发方式
                        Custom_6 = project.ProjectType,
                        Custom_11 = project.ProjectGole,
                        Custom_12 = project.ProjectDesc
                    };

                    //PageNumber=1002 
                    CustomerFieldTrackExt2 pageNum1002 = new CustomerFieldTrackExt2()
                    {
                        ProjectID = 502,
                        IssueID = project.HiddenTaskID,
                        PageNumber = 1002,
                        Custom_4 = project.ProductName,
                        Custom_2 = project.PlanStartDate,
                        Custom_3 = project.PlanFinishDate,
                        Custom_1 = project.ProjectManager,
                        Custom_5 = project.ProductManager,
                        Custom_9 = project.ContractMoney,
                        Custom_7 = project.DevManager,
                        Custom_8 = project.TestManager
                    };


                    //PageNumber=1003
                    CustomerFieldTrackExt2 pageNum1003 = new CustomerFieldTrackExt2()
                    {
                        ProjectID = 502,
                        IssueID = project.HiddenTaskID,
                        PageNumber = 1003,
                       Custom_2=project.ProductCode,
                        Custom_9 = project.BelongTo,
                        Custom_4 = project.DevModle,//开发模式
                        Custom_7 = project.ScoreMoney,
                        Custom_5 = project.TransfterProject,
                        Custom_3 = project.Level,//重要程度
                        Custom_6 = project.PatchDeliver
                    };

                    //开发模式,重要程度,合同额（万元）
                    //CustomerFieldTrackExt2 lostFields = new CustomerFieldTrackExt2()
                    //{
                    //    ProjectID = 502,
                    //    IssueID = PIItemID,
                    //    PageNumber = 15,
                    //    Custom_2 = project.DevModle,
                    //    Custom_1=project.Level,
                    //    Custom_4=project.ContractMoney
                    //};

                    BugSelectionInfo projectManager = new BugSelectionInfo()//1000101  --项目经理ID
                    {
                        ProjectID = 502,
                        BugID = project.HiddenTaskID,
                        FieldID = 1000101,
                        FieldSelectionID=project.ProjectManagerID
                    };
                 
                    if (dbcontext.BugSelectionInfo.Any<BugSelectionInfo>(p => p.ProjectID == 502 && p.BugID == project.HiddenTaskID && p.FieldID == 1000101))
                    {
                        //dbcontext.BugSelectionInfo.Attach(projectManager);
                        //dbcontext.Entry<BugSelectionInfo>(projectManager).State = EntityState.Modified;
                        string sql = "update BugSelectionInfo set FieldSelectionID=@FieldSelectionID where ProjectID=@ProjectID and BugID=@BugID and FieldID=@FieldID";
                        SqlParameter[] param= new SqlParameter[]   {
                        new SqlParameter("@ProjectID",502),
                        new SqlParameter("@BugID",project.HiddenTaskID),
                        new SqlParameter("@FieldID",1000101),
                        new SqlParameter("@FieldSelectionID",project.ProjectManagerID)
                        };
                        dbcontext.Database.ExecuteSqlCommand(sql,param);
                    }
                    else
                        dbcontext.Entry<BugSelectionInfo>(projectManager).State = EntityState.Added;

                    BugSelectionInfo productManager = new BugSelectionInfo()//1000105  --产品经理ID
                    {
                        ProjectID = 502,
                        BugID = project.HiddenTaskID,
                        FieldID = 1000105,
                        FieldSelectionID=project.ProductManagerID
                    };

                    if (dbcontext.BugSelectionInfo.Any<BugSelectionInfo>(p => p.ProjectID == 502 && p.BugID == project.HiddenTaskID && p.FieldID == 1000105))
                    {
                        //dbcontext.BugSelectionInfo.Attach(productManager);
                        //dbcontext.Entry<BugSelectionInfo>(productManager).State = EntityState.Modified;
                        string sql = "update BugSelectionInfo set FieldSelectionID=@FieldSelectionID where ProjectID=@ProjectID and BugID=@BugID and FieldID=@FieldID";
                        SqlParameter[] param = new SqlParameter[]   {
                        new SqlParameter("@ProjectID",502),
                        new SqlParameter("@BugID",project.HiddenTaskID),
                        new SqlParameter("@FieldID",1000105),
                        new SqlParameter("@FieldSelectionID",project.ProductManagerID)
                        };
                        dbcontext.Database.ExecuteSqlCommand(sql, param);
                    }
                    else
                        dbcontext.Entry<BugSelectionInfo>(productManager).State = EntityState.Added;

                    BugSelectionInfo devManager = new BugSelectionInfo()//1000107  --开发负责人
                    {
                        ProjectID = 502,
                        BugID = project.HiddenTaskID,
                        FieldID = 1000107,
                        FieldSelectionID=project.DevManagerID
                    };

                    if (dbcontext.BugSelectionInfo.Any<BugSelectionInfo>(p => p.ProjectID == 502 && p.BugID == project.HiddenTaskID && p.FieldID == 1000107))
                    {
                        //dbcontext.BugSelectionInfo.Attach(devManager);
                        //dbcontext.Entry<BugSelectionInfo>(devManager).State = EntityState.Modified;
                        string sql = "update BugSelectionInfo set FieldSelectionID=@FieldSelectionID where ProjectID=@ProjectID and BugID=@BugID and FieldID=@FieldID";
                        SqlParameter[] param = new SqlParameter[]   {
                        new SqlParameter("@ProjectID",502),
                        new SqlParameter("@BugID",project.HiddenTaskID),
                        new SqlParameter("@FieldID",1000107),
                        new SqlParameter("@FieldSelectionID",project.DevManagerID)
                        };
                        dbcontext.Database.ExecuteSqlCommand(sql, param);
                    }
                    else
                        dbcontext.Entry<BugSelectionInfo>(devManager).State = EntityState.Added;

                    BugSelectionInfo TestManager = new BugSelectionInfo()//1000108  --开发负责人
                    {
                        ProjectID = 502,
                        BugID = project.HiddenTaskID,
                        FieldID = 1000108,
                        FieldSelectionID=project.TestManagerID
                    };
                    if (dbcontext.BugSelectionInfo.Any<BugSelectionInfo>(p => p.ProjectID == 502 && p.BugID == project.HiddenTaskID && p.FieldID == 1000108))
                    {
                        //dbcontext.BugSelectionInfo.Attach(TestManager);
                        //dbcontext.Entry<BugSelectionInfo>(TestManager).State = EntityState.Modified;
                        string sql = "update BugSelectionInfo set FieldSelectionID=@FieldSelectionID where ProjectID=@ProjectID and BugID=@BugID and FieldID=@FieldID";
                        SqlParameter[] param = new SqlParameter[]   {
                        new SqlParameter("@ProjectID",502),
                        new SqlParameter("@BugID",project.HiddenTaskID),
                        new SqlParameter("@FieldID",1000108),
                        new SqlParameter("@FieldSelectionID",project.TestManagerID)
                        };
                        dbcontext.Database.ExecuteSqlCommand(sql, param);
                    }
                    else
                        dbcontext.Entry<BugSelectionInfo>(TestManager).State = EntityState.Added;


                    ////项目团队成员多行文本框 pagenumber=15 
                    //CustomerFieldTrackExt2 membertext = new CustomerFieldTrackExt2()
                    //{
                    //    ProjectID = 502,
                    //    IssueID = PIItemID,
                    //    PageNumber = 15,
                    //   // Custom_5 = project.ProjectMembersText,
                    //    Custom_2 = project.DevModle,
                    //    Custom_1 = project.Level,
                    //    Custom_4 = project.ContractMoney
                    //};

                    //项目团队成员多行文本框 pagenumber=15 
                    CustomerFieldTrackExt2 membertext = new CustomerFieldTrackExt2()
                    {
                        ProjectID = 502,
                        IssueID = PIItemID,
                        PageNumber = 11,
                         Custom_11 = project.ProjectMembersText,
                
                    };

                    UpdateMemberList(PIItemID);//比较当前项目的已选成员列表和最新的成员列表，进行资源的更新

                    //PageNumber=1001 
                    if (dbcontext.CustomerFieldTrackExt2.Any<CustomerFieldTrackExt2>(p=>p.ProjectID==502&&p.IssueID== project.HiddenTaskID&&p.PageNumber==1001))
                    {
                        dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1001).State = EntityState.Modified;
                    }
                    else
                        dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1001).State = EntityState.Added;

                    //PageNumber=1002
                    if (dbcontext.CustomerFieldTrackExt2.Any<CustomerFieldTrackExt2>(p => p.ProjectID == 502 && p.IssueID == project.HiddenTaskID && p.PageNumber == 1002))
                    {
                        dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1002).State = EntityState.Modified;
                    }
                    else
                        dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1002).State = EntityState.Added;

                    //PageNumber=1003
                    if (dbcontext.CustomerFieldTrackExt2.Any<CustomerFieldTrackExt2>(p => p.ProjectID == 502 && p.IssueID == project.HiddenTaskID && p.PageNumber == 1003))
                    {
                        dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1003).State = EntityState.Modified;
                    }
                    else
                        dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1003).State = EntityState.Added;

                    //  PageNumber=15 update select members to task property text fields 
                    if (dbcontext.CustomerFieldTrackExt2.Any<CustomerFieldTrackExt2>(p => p.ProjectID == 502 && p.IssueID == PIItemID && p.PageNumber == 15))
                    {
                        dbcontext.Entry<CustomerFieldTrackExt2>(membertext).State = EntityState.Modified;
                    }
                    else
                        dbcontext.Entry<CustomerFieldTrackExt2>(membertext).State = EntityState.Added;

                    
                    return dbcontext.SaveChanges();
                }
            }
            else
            {
                LogHelper.WriteLog("不满足同步条件，不处理");
                return 0;
            }
           
          
            
           
        }

        private static void PrintProperties(Project info)
        {
            foreach (System.Reflection.PropertyInfo item in info.GetType().GetProperties())
            {
                var attrs = item.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                var displayName = "";
                if (attrs.Count() > 0)
                {
                    displayName = ((DisplayNameAttribute)attrs[0]).DisplayName;
                    LogHelper.WriteLog(displayName + "是：" + info.GetType().GetProperty(item.Name).GetValue(info, null));

                }

            }
        }



        /// <summary>
        /// 比较当前项目的已选成员列表和最新的成员列表，进行资源的更新
        /// 调用现有的触发器实现同步更新到缺陷，开发等模块中
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
