using System.Web.Mvc;
using System.Web.Routing;

namespace BuildScreen
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteTable.Routes.MapMvcAttributeRoutes();
		}
	}
}
