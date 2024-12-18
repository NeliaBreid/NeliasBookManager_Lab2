using System.Configuration;
using System.Data;
using System.Windows;
using Labb2DataAcess.Services;
using Microsoft.Extensions.DependencyInjection;
using NeliasBookManager.Infrastructure.Data;
using NeliasBookManager.presentation.Viewmodel;

namespace NeliasBookManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { //TODO: Lägg loadStores här som async, eller iaf kalla på den här?
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

        }
    }
}
