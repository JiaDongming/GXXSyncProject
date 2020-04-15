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

                        project.ProjectSpaceTitle = (from c in dbcontext.SubProject where c.ProjectID == 502 && c.SubProjectID == project.ProjectSpaceID select c.Title).SingleOrDefault();
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
                      

                        project.DevWay = (from a in customFields2 where a.PageNumber == 14 select a.Custom_2).SingleOrDefault();// 开发方式

                     

                        project.ContractMoney = (from a in customFields2 where a.PageNumber == 15 select a.Custom_4).SingleOrDefault();// 合同额（万）
                        project.DevModle = (from a in customFields2 where a.PageNumber == 15 select a.Custom_2).SingleOrDefault();// 开发模式

                     

                        project.ScoreMoney = (from a in customFields2 where a.PageNumber == 14 select a.Custom_1).SingleOrDefault();//贡献值（万）
                        project.ProjectType = (from a in customFields2 where a.PageNumber == 13 select a.Custom_5).SingleOrDefault();//项目类型

                     
                        #region 人员获取
                        //project.ProjectManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_4).SingleOrDefault();//项目经理
                        project.ProjectManager = (from a in customFields select a.Custom_2).SingleOrDefault();//项目经理文本
                        project.ProjectManagerID = (from id in bugselectionInfo where id.FieldID == 2 select id.FieldSelectionID).FirstOrDefault();//项目经理编号 FieldID=2

                        project.ProductManager = (from a in customFields2 where a.PageNumber == 6 select a.Custom_3).SingleOrDefault();// 产品经理(PM)
                        project.ProductManagerID = (from id in bugselectionInfo where id.FieldID == 1603 select id.FieldSelectionID).FirstOrDefault();//产品经理编号 FieldID=1603

                        project.RequirementAnalyst = (from a in customFields2 where a.PageNumber == 15 select a.Custom_8).SingleOrDefault();//需求分析师   FieldID=12508 page=15 Custom_8
                        project.RequirementAnalystIDs = (from ids in bugselectionInfo where ids.FieldID == 12508 select ids.FieldSelectionID).ToList();//需求分析师编号

                        project.DevManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_7).SingleOrDefault();//开发负责人
                        project.DevManagerID = (from id in bugselectionInfo where id.FieldID == 12407 select id.FieldSelectionID).FirstOrDefault();//开发负责人编号 FieldID=12407 page=14
                        project.DevManagerIDs = (from ids in bugselectionInfo where ids.FieldID == 12407 select ids.FieldSelectionID).ToList();

                        project.DevEngineer = (from a in customFields2 where a.PageNumber == 16 select a.Custom_1).SingleOrDefault();//开发工程师   FieldID=12601 page=16 Custom_1
                        project.DevEngineerIDs = (from ids in bugselectionInfo where ids.FieldID == 12601 select ids.FieldSelectionID).ToList();//开发工程师编号

                        project.TestManager = (from a in customFields2 where a.PageNumber == 14 select a.Custom_8).SingleOrDefault();//测试负责人
                        project.TestManagerID = (from id in bugselectionInfo where id.FieldID == 12408 select id.FieldSelectionID).FirstOrDefault();//测试负责人编号 FieldID=12408 page=14
                        project.TestManagerIDs = (from ids in bugselectionInfo where ids.FieldID == 12408 select ids.FieldSelectionID).ToList();

                        project.TestEngineer = (from a in customFields2 where a.PageNumber == 16 select a.Custom_2).SingleOrDefault();//测试工程师   FieldID=12602 page=16 Custom_2
                        project.TestEngineerIDs = (from ids in bugselectionInfo where ids.FieldID == 12602  select ids.FieldSelectionID).ToList();//测试工程师编号

                        project.QualityManager = (from a in customFields2 where a.PageNumber == 16 select a.Custom_3).SingleOrDefault();//质量管理员   FieldID=12603 page=16 Custom_3
                        project.QualityManagerIDs = (from ids in bugselectionInfo where ids.FieldID == 12603 select ids.FieldSelectionID).ToList();//质量管理员编号

                        project.ConfigManager = (from a in customFields2 where a.PageNumber == 16 select a.Custom_4).SingleOrDefault();//配置管理员   FieldID=12604 page=16 Custom_4
                        project.ConfigManagerIDs = (from ids in bugselectionInfo where ids.FieldID == 12604 select ids.FieldSelectionID).ToList();//配置管理员编号

                        //（其他成员）项目团队成员下拉列表
                        //project.ProjectMemberList = (from members in dbcontext.BugSelectionInfo where members.ProjectID == 502 && members.BugID == PIItemID && members.FieldID == 12505 select members.FieldSelectionID).ToList<int>();  
                        //if (project.ProjectMemberList.Count > 0)
                        //{
                        //    project.SelectedProjecMembers = (from persons in dbcontext.LogIn join list in project.ProjectMemberList on persons.PersonID equals list select persons).ToList<LogIn>();
                        //    project.ProjectMembersText = string.Join(",", (from member in project.SelectedProjecMembers select member.FName).ToArray());
                        //}
                        project.ProjectMembers = (from a in customFields2 where a.PageNumber == 15 select a.Custom_5).SingleOrDefault();//其他成员   FieldID=12505 page=15 Custom_5
                        project.ProjectMemberList = (from ids in bugselectionInfo where ids.FieldID == 12505 select ids.FieldSelectionID).ToList();//其他成员编号

                        char newLine = '\n';
                        //所有人员
                        if (project.ProjectManager!=null && project.ProjectManager.Length!=0 )
                        {
                            //project.ProjectResourceText = "项目经理：" + project.ProjectManager + newLine;
                            project.ProjectResourceText = "项目经理：" + project.ProjectManager.Split(' ')[0] + newLine;
                            project.ProjectResourceIDs.Add(project.ProjectManagerID);
                        }

                        if (project.ProductManager != null && project.ProductManager.Length != 0)
                        {
                            project.ProjectResourceText =project.ProjectResourceText + "产品经理：" + project.ProductManager.Split(' ')[0] + newLine;
                            project.ProjectResourceIDs.Add(project.ProductManagerID);
                        }

                        if (project.DevManager != null && project.DevManager.Length != 0)
                        {
                            project.ProjectResourceText = project.ProjectResourceText + "开发负责人：" + GetMemberText (project.DevManagerIDs) + newLine;
                            project.ProjectResourceIDs.AddRange(project.DevManagerIDs);
                        }

                        if (project.DevEngineer != null && project.DevEngineer.Length != 0)
                        {
                            project.ProjectResourceText = project.ProjectResourceText + "开发工程师：" + GetMemberText(project.DevEngineerIDs) + newLine;
                            project.ProjectResourceIDs.AddRange(project.DevEngineerIDs);
                        }

                        if (project.TestManager != null && project.TestManager.Length != 0)
                        {
                            project.ProjectResourceText = project.ProjectResourceText + "测试负责人：" + GetMemberText(project.TestManagerIDs) + newLine;
                            project.ProjectResourceIDs.AddRange(project.TestManagerIDs);
                        }

                        if (project.TestEngineer != null && project.TestEngineer.Length != 0)
                        {
                            project.ProjectResourceText = project.ProjectResourceText + "测试工程师：" + GetMemberText(project.TestEngineerIDs) + newLine;
                            project.ProjectResourceIDs.AddRange(project.TestEngineerIDs);
                        }

                        if (project.QualityManager != null && project.QualityManager.Length != 0)
                        {
                            project.ProjectResourceText = project.ProjectResourceText + "质量管理员：" + GetMemberText(project.QualityManagerIDs) + newLine;
                            project.ProjectResourceIDs.AddRange(project.QualityManagerIDs);
                        }

                        if (project.ConfigManager != null && project.ConfigManager.Length != 0)
                        {
                            project.ProjectResourceText = project.ProjectResourceText + "配置管理员：" + GetMemberText(project.ConfigManagerIDs) + newLine;
                            project.ProjectResourceIDs.AddRange(project.ConfigManagerIDs);
                        }

                        if (project.ProjectMembers != null && project.ProjectMembers.Length != 0)
                        {
                            project.ProjectResourceText = project.ProjectResourceText + "其他成员：" + GetMemberText(project.ProjectMemberList) + newLine;
                            project.ProjectResourceIDs.AddRange(project.ProjectMemberList);
                        }

                        #endregion




                        project.TransfterProject = (from a in customFields2 where a.PageNumber == 13 select a.Custom_7).SingleOrDefault();//转产项目
                       project.Level = (from a in customFields2 where a.PageNumber == 15 select a.Custom_1).SingleOrDefault();//重要程度
                        project.PatchDeliver = (from a in customFields2 where a.PageNumber == 13 select a.Custom_8).SingleOrDefault();//批量发货
                        project.ProjectMembers = (from a in customFields2 where a.PageNumber == 15 select a.Custom_5).SingleOrDefault();//项目团队成员(下拉框文本)

                        project.ShangJiID = (from a in customFields2 where a.PageNumber == 15 select a.Custom_6).SingleOrDefault();//商机编号
                        project.EndDate = (from a in customFields2 where a.PageNumber == 15 select a.Custom_7).SingleOrDefault();//结束时间

                      
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

        private static string GetMemberText(List<int> memberIDs)
        {
            if (memberIDs.Count > 0)
            {
                using (GXX_DS_0312Entities dbcontext = new GXX_DS_0312Entities())
                {
                    var SelectedProjecMembers = (from persons in dbcontext.LogIn join list in memberIDs on persons.PersonID equals list select persons).ToList<LogIn>();
                    var ProjectMembersText = string.Join(",", (from member in SelectedProjecMembers select member.FName).ToArray());
                    return ProjectMembersText;
                }
              
            }
            return "";
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
                        Custom_12 = project.ProjectDesc,
                        Custom_4= project.ShangJiID//商机编号
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
                        //Custom_7 = project.DevManager,//开发负责人(单选)
                       // Custom_8 = project.TestManager//测试负责人(单选)
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
                        Custom_6 = project.PatchDeliver,
                        Custom_8 = project.EndDate//结束时间
                    };

                    //PageNumber=1004
                    CustomerFieldTrackExt2 pageNum1004 = new CustomerFieldTrackExt2()
                    {
                        ProjectID = 502,
                        IssueID = project.HiddenTaskID,
                        PageNumber = 1004,
                        Custom_1= project.DevManager,//开发负责人（多选）
                        Custom_2 = project.TestManager,//测试负责人（多选）

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


                    #region 单选时的开发负责人和测试负责人代码
                    //1000107  --开发负责人
                    //BugSelectionInfo devManager = new BugSelectionInfo()//1000107  --开发负责人
                    //{
                    //    ProjectID = 502,
                    //    BugID = project.HiddenTaskID,
                    //    FieldID = 1000107,
                    //    FieldSelectionID = project.DevManagerID
                    //};

                    //if (dbcontext.BugSelectionInfo.Any<BugSelectionInfo>(p => p.ProjectID == 502 && p.BugID == project.HiddenTaskID && p.FieldID == 1000107))
                    //{
                    //    //dbcontext.BugSelectionInfo.Attach(devManager);
                    //    //dbcontext.Entry<BugSelectionInfo>(devManager).State = EntityState.Modified;
                    //    string sql = "update BugSelectionInfo set FieldSelectionID=@FieldSelectionID where ProjectID=@ProjectID and BugID=@BugID and FieldID=@FieldID";
                    //    SqlParameter[] param = new SqlParameter[]   {
                    //    new SqlParameter("@ProjectID",502),
                    //    new SqlParameter("@BugID",project.HiddenTaskID),
                    //    new SqlParameter("@FieldID",1000107),
                    //    new SqlParameter("@FieldSelectionID",project.DevManagerID)
                    //    };
                    //    dbcontext.Database.ExecuteSqlCommand(sql, param);
                    //}
                    //else
                    //    dbcontext.Entry<BugSelectionInfo>(devManager).State = EntityState.Added;

                    //BugSelectionInfo TestManager = new BugSelectionInfo()//1000108  --测试负责人
                    //{
                    //    ProjectID = 502,
                    //    BugID = project.HiddenTaskID,
                    //    FieldID = 1000108,
                    //    FieldSelectionID = project.TestManagerID
                    //};
                    //if (dbcontext.BugSelectionInfo.Any<BugSelectionInfo>(p => p.ProjectID == 502 && p.BugID == project.HiddenTaskID && p.FieldID == 1000108))
                    //{
                    //    //dbcontext.BugSelectionInfo.Attach(TestManager);
                    //    //dbcontext.Entry<BugSelectionInfo>(TestManager).State = EntityState.Modified;
                    //    string sql = "update BugSelectionInfo set FieldSelectionID=@FieldSelectionID where ProjectID=@ProjectID and BugID=@BugID and FieldID=@FieldID";
                    //    SqlParameter[] param = new SqlParameter[]   {
                    //    new SqlParameter("@ProjectID",502),
                    //    new SqlParameter("@BugID",project.HiddenTaskID),
                    //    new SqlParameter("@FieldID",1000108),
                    //    new SqlParameter("@FieldSelectionID",project.TestManagerID)
                    //    };
                    //    dbcontext.Database.ExecuteSqlCommand(sql, param);
                    //}
                    //else
                    //    dbcontext.Entry<BugSelectionInfo>(TestManager).State = EntityState.Added;
                    #endregion

                    #region 多选的开发负责人和测试负责人代码
                    UpdateBugSelection(project, 1000301);//开发负责人
                    UpdateBugSelection(project, 1000302);//测试负责人


                    #endregion


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
                        //Custom_11 = project.ProjectMembersText,
                        Custom_11 = project.ProjectResourceText,

                    };

                    //UpdateMemberList(PIItemID);//比较当前项目的已选成员列表和最新的成员列表，进行资源的更新
                    UpdateSpaceTitle(project); //更新项目space的标题
                    UpdateProjectResource(project);

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

                    //PageNumber=1004
                    if (dbcontext.CustomerFieldTrackExt2.Any<CustomerFieldTrackExt2>(p => p.ProjectID == 502 && p.IssueID == project.HiddenTaskID && p.PageNumber == 1004))
                    {
                        dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1004).State = EntityState.Modified;
                    }
                    else
                        dbcontext.Entry<CustomerFieldTrackExt2>(pageNum1004).State = EntityState.Added;

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

        /// <summary>
        /// 更新BugSelection表： 用于更新项目属性页中多选字段的更新：开发负责人，测试负责人
        /// </summary>
        /// <param name="project">项目信息</param>
        /// <param name="FieldID">要更新的下拉多选字段编号</param>
        private static void UpdateBugSelection(Project project, int FieldID)
        {
            List<int> newMemberList = new List<int>();
            List<int> currentMemberList = new List<int>();
            using (GXX_DS_0312Entities dbcontext = new GXX_DS_0312Entities())
            {
                var bugselectioninfo = (from b in dbcontext.BugSelectionInfo where b.ProjectID == 502 && b.BugID == project.HiddenTaskID select b);
                //最新选择的项目成员列表
                if (FieldID == 1000301) //开发负责人
                {
                    newMemberList = project.DevManagerIDs;
                    currentMemberList = (from c in bugselectioninfo where c.FieldID == 1000301 select c.FieldSelectionID).ToList();
                }
                else if (FieldID == 1000302)
                {
                    newMemberList = project.TestManagerIDs;
                    currentMemberList = (from c in bugselectioninfo where c.FieldID == 1000302 select c.FieldSelectionID).ToList();
                }

                //获取是新增的成员，遍历插入
                List<int> addedList = newMemberList.Except(currentMemberList).ToList();

                List<int> deletedList = currentMemberList.Except(newMemberList).ToList();
                string sql = string.Empty;
                foreach (var item in addedList)
                {
                    sql = "insert into BugSelectionInfo(ProjectID,BugID,FieldID,FieldSelectionID) values (@ProjectID,@BugID,@FieldID,@FieldSelectionID)";
                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@ProjectID",502),
                        new SqlParameter("@BugID",project.HiddenTaskID),
                        new SqlParameter("@FieldID",FieldID),
                        new SqlParameter("@FieldSelectionID",item)
                    };
                    dbcontext.Database.ExecuteSqlCommand(sql, param);
                }

                foreach (var item in deletedList)
                {
                    sql = "delete from BugSelectionInfo where ProjectID=@ProjectID and BugID=@BugID and FieldID=@FieldID and FieldSelectionID= @FieldSelectionID";
                    SqlParameter[] param = new SqlParameter[]
                    {
                          new SqlParameter("@ProjectID",502),
                        new SqlParameter("@BugID",project.HiddenTaskID),
                        new SqlParameter("@FieldID",FieldID),
                        new SqlParameter("@FieldSelectionID",item)
                    };
                    dbcontext.Database.ExecuteSqlCommand(sql, param);
                }




            }


        }

        /// <summary>
        /// 更新项目资源
        /// </summary>
        /// <param name="project">项目信息</param>
        private static void UpdateProjectResource(Project project)
        {
            
                //最新选择的项目成员列表
                List<int> newMemberList = project.ProjectResourceIDs.Distinct<int>().ToList();
                List<int> currentMemberList = new List<int>();
                using (GXX_DS_0312Entities dbcontext = new GXX_DS_0312Entities())
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

        /// <summary>
        /// 更新项目标题（当项目属性任务中标题与当前项目标题不一致时更新）
        /// </summary>
        /// <param name="project">项目信息</param>
        private static void UpdateSpaceTitle(Project project)
        {
            if (project.ProjectTtile == project.ProjectSpaceTitle)
                return;
            using (GXX_DS_0312Entities dbcontext = new GXX_DS_0312Entities())
            {
                string sql = "update SubProject set Title=@ProjectTitle where projectid=@ProjectID and subprojectid=@ProjectSpaceID";
                SqlParameter[] param = new SqlParameter[]   {
                        new SqlParameter("@ProjectTitle",project.ProjectTtile),
                        new SqlParameter("@ProjectID",502),
                        new SqlParameter("@ProjectSpaceID",project.ProjectSpaceID)
                        };
                dbcontext.Database.ExecuteSqlCommand(sql, param);

            }

        }

        /// <summary>
        /// 项目属性打印到日志
        /// </summary>
        /// <param name="info"></param>
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
