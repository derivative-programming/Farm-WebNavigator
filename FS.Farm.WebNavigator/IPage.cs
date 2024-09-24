using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator
{
    public interface IPage
    {
        PageView BuildPageView(Guid sessionCode, Guid contextCode);

        PagePointer ProcessCommand(Guid sessionCode, Guid contextCode, string commandText, string postData = "");
    }
}
