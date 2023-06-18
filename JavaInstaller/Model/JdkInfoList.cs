using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaInstaller.Model
{
    public class JdkInfoList : ObservableCollection<jdkInfo>
    {
        public JdkInfoList() 
        {

        }
    }

    public class jdkInfo
    {
        public string product { get; set; }
        public string jdkver { get; set; }
        public string version { get; set; }
        public string path { get; set; }
        public Boolean pathChk { get; set; }

        public string pathChkMessage { get; set; }
    }
}
