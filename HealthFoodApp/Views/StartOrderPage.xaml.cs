using HealthFoodApp.Services;
using HealthFoodApp.ViewModels;

namespace HealthFoodApp.Views 
{ 
	public partial class StartOrderPage : ContentPage
	{
		public StartOrderPage()
		{
			InitializeComponent();
			BindingContext = new StartOrderItemViewModel();
		}
	}
}