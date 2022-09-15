using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUMCustomizations.Helper.Model.Interface
{
    public interface IFTPConfig
    {
        string FtpHost { get; set; }
        string FtpUserName { get; set; }
        string FtpPassword { get; set; }
        string FtpPort { get; set; }
    }
}
