using FleetAssist.UI.Web.Common.T4;
using System.Web.Routing;

namespace FleetAssist.UI.Web.Areas.Site.Controllers
{
	public static class FindUs
	{
		public static ActionRoute Index()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.FindUsController", "Index", "FindUs", "Site", new RouteValueDictionary());
		}
	}
	public static class Home
	{
		public static ActionRoute Index()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.HomeController", "Index", "Home", "Site", new RouteValueDictionary());
		}
		public static ActionRoute Robots()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.HomeController", "Robots", "Home", "Site", new RouteValueDictionary());
		}
		public static ActionRoute SiteMap()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.HomeController", "SiteMap", "Home", "Site", new RouteValueDictionary());
		}
	}
	public static class Jobs
	{
		public static ActionRoute Index()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.JobsController", "Index", "Jobs", "Site", new RouteValueDictionary());
		}
	}
	public static class JoinNetwork
	{
		public static ActionRoute Index()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.JoinNetworkController", "Index", "JoinNetwork", "Site", new RouteValueDictionary());
		}
		public static ActionRoute Index(FleetAssist.Cmd.Domain.NewGarageEnquiry.NewGarageEnquiry.Create.CreateNewGarageEnquiryDTO CreateNewGarageEnquiry)
		{
			var routeValues = new RouteValueDictionary()
			{
				{ "CreateNewGarageEnquiry", CreateNewGarageEnquiry},
			};
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.JoinNetworkController", "Index", "JoinNetwork", "Site", routeValues);
		}
		public static ActionRoute ThankYou()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.JoinNetworkController", "ThankYou", "JoinNetwork", "Site", new RouteValueDictionary());
		}
	}
	public static class PrivacyPolicy
	{
		public static ActionRoute Index()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.PrivacyPolicyController", "Index", "PrivacyPolicy", "Site", new RouteValueDictionary());
		}
	}
	public static class Services
	{
		public static ActionRoute Index()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "Index", "Services", "Site", new RouteValueDictionary());
		}
		public static ActionRoute Atlas()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "Atlas", "Services", "Site", new RouteValueDictionary());
		}
		public static ActionRoute Consultancy()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "Consultancy", "Services", "Site", new RouteValueDictionary());
		}
		public static ActionRoute DowntimeManagement()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "DowntimeManagement", "Services", "Site", new RouteValueDictionary());
		}
		public static ActionRoute FleetAdmin()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "FleetAdmin", "Services", "Site", new RouteValueDictionary());
		}
		public static ActionRoute NetworkManagement()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "NetworkManagement", "Services", "Site", new RouteValueDictionary());
		}
		public static ActionRoute Rental()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "Rental", "Services", "Site", new RouteValueDictionary());
		}
		public static ActionRoute ServiceBooking()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "ServiceBooking", "Services", "Site", new RouteValueDictionary());
		}
		public static ActionRoute TechnicalAuthorisation()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.ServicesController", "TechnicalAuthorisation", "Services", "Site", new RouteValueDictionary());
		}
	}
	public static class Team
	{
		public static ActionRoute Index()
		{
			return new ActionRoute("FleetAssist.UI.Web.Areas.Site.Controllers.TeamController", "Index", "Team", "Site", new RouteValueDictionary());
		}
	}
}