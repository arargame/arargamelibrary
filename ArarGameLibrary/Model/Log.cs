using ArarGameLibrary.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Model
{
    public class Log
    {
        public static void Create(string description,
            LogManagement.LogType logType = LogManagement.LogType.Error,
            LogManagement.LogStorageType logStorageType = LogManagement.LogStorageType.Cache,
            string entityId = null,
            string version = null)
        {

#if DEBUG

            logType = LogManagement.LogType.Debug;

#endif


            LogManagement.Log.Create(description,
                                        logType,
                                        logStorageType,
                                        entityId,
                                        LogManagement.LogPlatformType.Desktop,
                                        ProjectManager.ProjectName,
                                        ProjectManager.ProjectVersion,
                                        3);
        }
    }
}
