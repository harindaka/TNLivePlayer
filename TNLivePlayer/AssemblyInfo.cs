using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace TNLivePlayer
{
    internal class AssemblyInfo
    {
        static AssemblyInfo _current;
        FileVersionInfo _info;

        private AssemblyInfo()
            : base()
        {
            _info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
        }

        internal static AssemblyInfo Current
        {
            get { return _current; }
        }

        internal string AssemblyProduct
        {
            get { return _info.ProductName; }
        }

        internal string AssemblyVersion
        {
            get { return _info.ProductVersion; }
        }

        internal static void Initialize()
        {
            if (_current == null)
                _current = new AssemblyInfo();
        }
    }
}
