using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using UserWcfServiceLib;

namespace UserWebApp.Controllers
{
    public class WcfProxy<T>
    {
        public ChannelFactory<T> Channel { get; set; }

        public WcfProxy()
        {
            //Channel = new ChannelFactory<T>("http://localhost:58314/UserService.svc");
            // Channel = new ChannelFactory<T>("*");

            //        ////Create a binding of the type exposed by service
            //        ////BasicHttpBinding binding = new BasicHttpBinding();
            //        ////var binding = new BasicHttpsBinding();
            //        //var binding = new BasicHttpBinding();

            //        ////Create EndPoint address
            //        //var endpointAddress = new EndpointAddress("http://localhost:58314/UserService.svc");

            //        ////Pass Binding and EndPoint address to ChannelFactory
            //        //channelFactory = new ChannelFactory<IUserService>(binding, endpointAddress);

            //Channel = new ChannelFactory<T>(
            //    new BasicHttpBinding(),
            //    new EndpointAddress("http://localhost:58314/UserService.svc"));

            //Channel = new ChannelFactory<T>("BasicHttpBinding_IUserService");
            Channel = new ChannelFactory<T>("*");
        }

        public T CreateChannel()
        {
            return Channel.CreateChannel();
        }

        public void Execute(Action<T> action)
        {
            T proxy = CreateChannel();
            var communicationObject = (ICommunicationObject)proxy;

            try
            {
                action(proxy);

                communicationObject.Close();
            }
            finally
            {
                communicationObject.Abort();
            }
        }

        public TResult Execute<TResult>(Func<T, TResult> function)
        {
            T proxy = CreateChannel();
            var communicationObject = (ICommunicationObject)proxy;

            try
            {
                var result = function(proxy);

                communicationObject.Close();

                return result;
            }
            finally
            {
                communicationObject.Abort();
            }
        }
    }

    public class UserServiceProxy : IUserService
    {
        public int GetUserIdByName(string userName)
        {
            var userServiceProxy = new WcfProxy<IUserService>();

            //// for a void method
            //WcfProxy.Execute(prxy => prxy.Method());

            // for non void method.
            var result = userServiceProxy.Execute(prxy => prxy.GetUserIdByName(userName));

            return result;
        }

        //public int GetUserIdByName(string userName)
        //{
        //    var channelFactory = GetUserServiceChannelFactory();

        //    try
        //    {
        //        ////Create a binding of the type exposed by service
        //        ////BasicHttpBinding binding = new BasicHttpBinding();
        //        ////var binding = new BasicHttpsBinding();
        //        //var binding = new BasicHttpBinding();

        //        ////Create EndPoint address
        //        //var endpointAddress = new EndpointAddress("http://localhost:58314/UserService.svc");

        //        ////Pass Binding and EndPoint address to ChannelFactory
        //        //channelFactory = new ChannelFactory<IUserService>(binding, endpointAddress);

        //        var userServiceChannel = channelFactory.CreateChannel();

        //        var result = userServiceChannel.GetUserIdByName(userName);

        //        return result;
        //    }
        //    catch (TimeoutException)
        //    {
        //        if (channelFactory != null)
        //            channelFactory.Abort();

        //        throw;
        //    }
        //    catch (FaultException)
        //    {
        //        if (channelFactory != null)
        //            channelFactory.Abort();

        //        throw;
        //    }
        //    catch (CommunicationException)
        //    {
        //        if (channelFactory != null)
        //            channelFactory.Abort();

        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        if (channelFactory != null)
        //            channelFactory.Abort();

        //        throw;
        //    }
        //}

        private IUserService GetUserServiceChannel()
        {
            //var binding = new BasicHttpsBinding();
            var binding = new BasicHttpBinding();

            var endpointAddress = new EndpointAddress("http://localhost:58314/UserService.svc");
            var channelFactory = new ChannelFactory<IUserService>(binding, endpointAddress);

            IUserService channel = channelFactory.CreateChannel();

            return channel;
        }

        private ChannelFactory<IUserService> GetUserServiceChannelFactory()
        {
            //var binding = new BasicHttpsBinding();
            var binding = new BasicHttpBinding();
            var endpointAddress = new EndpointAddress("http://localhost:58314/UserService.svc");
            var channelFactory = new ChannelFactory<IUserService>(binding, endpointAddress);

            return channelFactory;
        }
    }

    public class HomeController : Controller
    {
        public IUserService UserService { get; }

        public HomeController() : this(new UserServiceProxy())
        {
        }

        public HomeController(IUserService userService)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public ActionResult Index()
        {
            var result = UserService.GetUserIdByName(@"DOMAIN\j.smith");

            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}