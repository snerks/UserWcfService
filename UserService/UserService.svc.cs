using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using UserWcfServiceLib;

namespace UserService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu 
    // to change the class name "UserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, 
    // please select UserService.svc or UserService.svc.cs at the Solution Explorer and start debugging.
    //[ServiceContract]
    public class UserService : IUserService
    {
        //[OperationContract]
        public int? GetUserIdByName(string userName)
        {
            if (userName == @"DOMAIN\j.smith")
            {
                return 22;
            }

            return null;
        }
    }
}
