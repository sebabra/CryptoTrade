using CrytoTadeUi.ViewModels;

namespace CrytoTadeUi.ViewsPartial;

public partial class PartialCandleChart : ContentView
{
	public PartialCandleChart()
	{
		InitializeComponent();
	}


    public void Initialize(LoadCandlesViewModel loadCandlesViewModel)
    {
        BindingContext = loadCandlesViewModel;
    }
}