namespace Servus
{
    public partial class MainPage : ContentPage
    {
        private ProductCreateViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new ProductCreateViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadDataAsync();
        }

        private async void OnPickImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Resim Seçiniz",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    await _viewModel.HandleImageSelectedAsync(result);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Resim seçilirken hata oluştu: {ex.Message}", "Tamam");
            }
        }

        private void OnAddFirstChoiceClicked(object sender, EventArgs e)
        {
            _viewModel.AddFirstChoice();
        }

        private void OnAddSecondChoiceClicked(object sender, EventArgs e)
        {
            _viewModel.AddSecondChoice();
        }
    }
}
