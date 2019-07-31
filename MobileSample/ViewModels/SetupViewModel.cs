using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using MobileSample.Models;
using MobileSample.Services;

namespace MobileSample.ViewModels
{
    public class SetupViewModel : BaseViewModel
    {
        private readonly INavigation navigation;
        private readonly DatabaseService databaseService;

        public SetupViewModel(INavigation navigation, DatabaseService databaseService)
        {
            this.navigation = navigation;
            this.databaseService = databaseService;
            Title = MainMenuItem.GetMenus().Where(w => w.Id == MenuItemType.Setup).First().Title;
        }

        public ICommand ResetDatabaseCommand => new Command(async () =>
        {
            bool isAccept = await Application.Current.MainPage.DisplayAlert("Reset Database", "Resetting the Database, Continue?", "Yes", "No");
            if (!isAccept) return;

            using (var databaseContext = databaseService.GetDatabaseContext())
            {
                databaseContext.Employees.RemoveRange(databaseContext.Employees);
                databaseContext.Departments.RemoveRange(databaseContext.Departments);
                await databaseContext.SaveChangesAsync();
            }

            await Application.Current.MainPage.DisplayAlert("Success", "Database is successfully reset", "OK");
            Application.Current.MainPage = new Views.MainPage();
        });

        public ICommand DateTimeCommand => new Command(async () =>
        {
            string text = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            await Application.Current.MainPage.DisplayAlert("Date Time", text, "OK");
        });
    }
}
