using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using SyncProjectInfo;

namespace testconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Project project = new Project(); 
            //  project= ProjectTransfer.LoadProjectInfo(5422); //Get project info via task id
            //update project to project space tab and update project team resource
            //int result=   ProjectTransfer.UpdateProjectInfo(5422);
            try
            {
                LogHelper.WriteLog("-----------------------------------------------------------------------------");
                LogHelper.WriteLog("当前检测到项目属性编号："+args[0]);


                int result = ProjectTransfer.UpdateProjectInfo(Convert.ToInt32(args[0]));
               // int result = ProjectTransfer.UpdateProjectInfo(5422);
                if (result>0)
                {
                    LogHelper.WriteLog("更新成功");
                    LogHelper.WriteLog("-----------------------------------------------------------------------------");
                }
            else
                {
                    LogHelper.WriteLog("无更新");
                    LogHelper.WriteLog("-----------------------------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {

                LogHelper.Error("调用更新项目信息出错:"+ex.Message, ex);
            }
           
     
        }
    }
}
