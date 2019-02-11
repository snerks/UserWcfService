#define IsWcfService
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UserWcfServiceLib
{
#if IsWcfService
    [ServiceContract]
#endif
    public interface IUserService
    {
#if IsWcfService
        [OperationContract]
#endif
        int? GetUserIdByName(string userName);
    }
}
