using System;
using System.Collections.Generic;
using FleetAssist.UI.Web.Common.View;

namespace FleetAssist.UI.Web.Areas.Customer
{
	public static class Views
	{
		public static ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.CreateCustomerVM> CreateCustomer { get; } = new ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.CreateCustomerVM>(@"~\Areas\Customer\Views\CreateCustomer.cshtml");
		public static ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.CustomerContactCardVM> CustomerContactCard { get; } = new ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.CustomerContactCardVM>(@"~\Areas\Customer\Views\CustomerContactCard.cshtml");
		public static ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.ManageCustomerInformationVM> ManageCustomerInformation { get; } = new ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.ManageCustomerInformationVM>(@"~\Areas\Customer\Views\ManageCustomerInformation.cshtml");
		public static ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.ManageCustomerSettingsVM> ManageCustomerSettings { get; } = new ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.ManageCustomerSettingsVM>(@"~\Areas\Customer\Views\ManageCustomerSettings.cshtml");
		public static ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.ManageCustomerSwitchBoardTypeVM> ManageCustomerSwitchBoardType { get; } = new ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.ManageCustomerSwitchBoardTypeVM>(@"~\Areas\Customer\Views\ManageCustomerSwitchBoardType.cshtml");
		public static ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.SearchCustomerVM> SearchCustomer { get; } = new ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.SearchCustomerVM>(@"~\Areas\Customer\Views\SearchCustomer.cshtml");
		public static ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.ManageCustomerBaseVM> _ManageCustomerBase { get; } = new ViewLocation<FleetAssist.UI.Web.Areas.Customer.ViewModels.ManageCustomerBaseVM>(@"~\Areas\Customer\Views\_ManageCustomerBase.cshtml");
	}
}
namespace FleetAssist.UI.Web.Common.Navigation
{
	public static class Views
	{
		public static ViewLocation MainNavigationBar { get; } = new ViewLocation(@"~\Common\Navigation\MainNavigationBar.cshtml");
		public static ViewLocation<FleetAssist.UI.Web.Common.Navigation.MainNavigationPageVM> MainNavigationPage { get; } = new ViewLocation<FleetAssist.UI.Web.Common.Navigation.MainNavigationPageVM>(@"~\Common\Navigation\MainNavigationPage.cshtml");
	}
}
