using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public enum PlatformType
    {
        None,
        Web,
        Mobile,
        Desktop,
        GameConsole
    }

    public class ProjectManager
    {
        public static PlatformType PlatformType { get; set; }

        private static string projectname = null;
        public static string ProjectName
        {
            get
            {
                return projectname ?? (projectname = Assembly.GetCallingAssembly().GetName().Name);
            }

            set
            {
                projectname = value;
            }
        }

        public static string ProjectVersion { get; set; }

        public static string TargetOperationSystem { get; set; }

        public static void Load(PlatformType platformType = PlatformType.Desktop, string targetOS = null, string projectName = null, string projectVersion = null)
        {
            PlatformType = platformType;

            TargetOperationSystem = targetOS;

            ProjectName = projectName;

            ProjectVersion = projectVersion;
        }
    }
}
