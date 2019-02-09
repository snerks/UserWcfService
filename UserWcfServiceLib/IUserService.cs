using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserWcfServiceLib
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        int GetUserIdByName(string userName);
    }
}
