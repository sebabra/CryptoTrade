namespace CrytoTadeUi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF1cWWhBYVJ/WmFZfVpgdVRMYVhbRX9PMyBoS35RckVrWH1fcXVRR2lZUEZw\r\n");
            MainPage = new AppShell();
        }
    }
}
