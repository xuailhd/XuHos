using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace XuHos.UI.WinSvr.JobService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        //系统用于标志此服务名称(唯一性)
        public static string ServiceName;
        //向用户标志服务的显示名称(可以重复)
        public static string DisplayName;
        //服务的说明(描述)
        public static string Description;


        public ProjectInstaller()
        {
      
            InitializeComponent();


        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            

            

        }

        private void ProjectInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
          
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
        }

        private void serviceProcessInstaller1_BeforeInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
