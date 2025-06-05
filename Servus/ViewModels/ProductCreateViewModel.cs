using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Servus.ViewModels
{
    public class ProductCreateViewModel : INotifyPropertyChanged
    {
        private readonly ProductDbContext _context;

        // Product Properties
        private string _productName = string.Empty;
        private string _productCode = string.Empty;
        private ProductCategory? _selectedCategory;
        private Price? _selectedPrice;
        private ProductStatus _selectedStatus = ProductStatus.Active;
        private int _displayOrder = 1;

        // Image Properties
        private bool _isUrlSelected = true;
        private bool _isFileSelected = false;
        private string? _imageUrl;
        private byte[]? _imageData;
        private string? _selectedFileName;
        private ImageSource? _imageSource;

        public ProductCreateViewModel()
        {
            // Geçici olarak - normalde Dependency Injection ile gelecek
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "servus.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            _context = new ProductDbContext(optionsBuilder.Options, DatabaseProvider.SQLite);

            InitializeCommands();
            InitializeCollections();
            LoadStatusList();
        }

        #region Properties

        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }

        public string ProductCode
        {
            get => _productCode;
            set => SetProperty(ref _productCode, value);
        }

        public ProductCategory? SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public Price? SelectedPrice
        {
            get => _selectedPrice;
            set => SetProperty(ref _selectedPrice, value);
        }

        public ProductStatus SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        public int DisplayOrder
        {
            get => _displayOrder;
            set => SetProperty(ref _displayOrder, value);
        }

        public bool IsUrlSelected
        {
            get => _isUrlSelected;
            set
            {
                SetProperty(ref _isUrlSelected, value);
                if (value) IsFileSelected = false;
            }
        }

        public bool IsFileSelected
        {
            get => _isFileSelected;
            set
            {
                SetProperty(ref _isFileSelected, value);
                if (value) IsUrlSelected = false;
            }
        }

        public string? ImageUrl
        {
            get => _imageUrl;
            set
            {
                SetProperty(ref _imageUrl, value);
                if (!string.IsNullOrEmpty(value) && IsUrlSelected)
                {
                    ImageSource = ImageSource.FromUri(new Uri(value));
                    OnPropertyChanged(nameof(HasImage));
                }
            }
        }

        public string? SelectedFileName
        {
            get => _selectedFileName;
            set
            {
                SetProperty(ref _selectedFileName, value);
                OnPropertyChanged(nameof(HasSelectedFile));
            }
        }

        public ImageSource? ImageSource
        {
            get => _imageSource;
            set
            {
                SetProperty(ref _imageSource, value);
                OnPropertyChanged(nameof(HasImage));
            }
        }

        public bool HasImage => ImageSource != null;
        public bool HasSelectedFile => !string.IsNullOrEmpty(SelectedFileName);

        #endregion

        #region Collections

        public ObservableCollection<ProductCategory> Categories { get; set; } = new();
        public ObservableCollection<Price> Prices { get; set; } = new();
        public ObservableCollection<Price> FirstChoicePrices { get; set; } = new();
        public ObservableCollection<Price> SecondChoicePrices { get; set; } = new();
        public ObservableCollection<ProductStatusItem> StatusList { get; set; } = new();
        public ObservableCollection<ProductChoiceViewModel> FirstChoices { get; set; } = new();
        public ObservableCollection<ProductChoiceViewModel> SecondChoices { get; set; } = new();

        #endregion

        #region Commands

        public ICommand SaveProductCommand { get; private set; } = null!;
        public ICommand CancelCommand { get; private set; } = null!;
        public ICommand RemoveFirstChoiceCommand { get; private set; } = null!;
        public ICommand RemoveSecondChoiceCommand { get; private set; } = null!;

        #endregion

        #region Initialization

        private void InitializeCommands()
        {
            SaveProductCommand = new Command(async () => await SaveProductAsync());
            CancelCommand = new Command(async () => await CancelAsync());
            RemoveFirstChoiceCommand = new Command<ProductChoiceViewModel>(RemoveFirstChoice);
            RemoveSecondChoiceCommand = new Command<ProductChoiceViewModel>(RemoveSecondChoice);
        }

        private void InitializeCollections()
        {
            Categories = new ObservableCollection<ProductCategory>();
            Prices = new ObservableCollection<Price>();
            FirstChoicePrices = new ObservableCollection<Price>();
            SecondChoicePrices = new ObservableCollection<Price>();
            StatusList = new ObservableCollection<ProductStatusItem>();
            FirstChoices = new ObservableCollection<ProductChoiceViewModel>();
            SecondChoices = new ObservableCollection<ProductChoiceViewModel>();
        }

        private void LoadStatusList()
        {
            StatusList.Clear();
            foreach (ProductStatus status in Enum.GetValues<ProductStatus>())
            {
                StatusList.Add(new ProductStatusItem
                {
                    Status = status,
                    Description = status.GetDescription()
                });
            }
            SelectedStatus = ProductStatus.Active;
        }

        #endregion

        #region Data Loading

        public async Task LoadDataAsync()
        {
            try
            {
                await LoadCategoriesAsync();
                await LoadPricesAsync();
            }
            catch (Exception ex)
            {
                await Application.Current?.MainPage?.DisplayAlert("Hata", $"Veri yüklenirken hata oluştu: {ex.Message}", "Tamam")!;
            }
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            Categories.Clear();
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private async Task LoadPricesAsync()
        {
            var prices = await _context.Prices
                .Where(p => p.IsActive)
                .ToListAsync();

            // Genel fiyatlar
            Prices.Clear();
            foreach (var price in prices)
            {
                Prices.Add(price);
            }

            // Birinci seçenek fiyatları
            FirstChoicePrices.Clear();
            foreach (var price in prices.Where(p => p.FirstChoice))
            {
                FirstChoicePrices.Add(price);
            }

            // İkinci seçenek fiyatları
            SecondChoicePrices.Clear();
            foreach (var price in prices.Where(p => p.SecondChoice))
            {
                SecondChoicePrices.Add(price);
            }
        }

        #endregion

        #region Image Handling

        public async Task HandleImageSelectedAsync(FileResult fileResult)
        {
            try
            {
                using var stream = await fileResult.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                _imageData = memoryStream.ToArray();
                SelectedFileName = fileResult.FileName;
                ImageSource = ImageSource.FromStream(() => new MemoryStream(_imageData));
            }
            catch (Exception ex)
            {
                await Application.Current?.MainPage?.DisplayAlert("Hata", $"Resim işlenirken hata oluştu: {ex.Message}", "Tamam")!;
            }
        }

        #endregion

        #region Choice Management

        public void AddFirstChoice()
        {
            FirstChoices.Add(new ProductChoiceViewModel
            {
                DisplayOrder = FirstChoices.Count + 1,
                AvailablePrices = new ObservableCollection<Price>(FirstChoicePrices)
            });
        }

        public void AddSecondChoice()
        {
            SecondChoices.Add(new ProductChoiceViewModel
            {
                DisplayOrder = SecondChoices.Count + 1,
                AvailablePrices = new ObservableCollection<Price>(SecondChoicePrices)
            });
        }

        private void RemoveFirstChoice(ProductChoiceViewModel choice)
        {
            FirstChoices.Remove(choice);
            // Sıralamaları yeniden düzenle
            for (int i = 0; i < FirstChoices.Count; i++)
            {
                FirstChoices[i].DisplayOrder = i + 1;
            }
        }

        private void RemoveSecondChoice(ProductChoiceViewModel choice)
        {
            SecondChoices.Remove(choice);
            // Sıralamaları yeniden düzenle
            for (int i = 0; i < SecondChoices.Count; i++)
            {
                SecondChoices[i].DisplayOrder = i + 1;
            }
        }

        #endregion

        #region Save Product

        private async Task SaveProductAsync()
        {
            try
            {
                if (!await ValidateInputAsync())
                    return;

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Ürünü oluştur
                    var product = new Product
                    {
                        ProductName = ProductName,
                        ProductCode = ProductCode,
                        Status = SelectedStatus,
                        CategoryId = SelectedCategory!.Id,
                        PriceId = SelectedPrice!.IND,
                        DisplayOrder = DisplayOrder,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "User" // Burası authentication'dan gelecek
                    };

                    // Resim işleme
                    if (IsUrlSelected && !string.IsNullOrEmpty(ImageUrl))
                    {
                        product.IsImageAvailable = true;
                        product.IsImageType = false; // URL
                        product.ImageUrl = ImageUrl;
                    }
                    else if (IsFileSelected && _imageData != null)
                    {
                        product.IsImageAvailable = true;
                        product.IsImageType = true; // Byte array
                        product.Image = _imageData;
                    }

                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();

                    // Birinci seçenekleri kaydet
                    foreach (var choice in FirstChoices.Where(c => !string.IsNullOrEmpty(c.ChoiceName) && c.SelectedPrice != null))
                    {
                        var firstChoice = new ProductFirstChoice
                        {
                            ProductId = product.IND,
                            ChoiceName = choice.ChoiceName,
                            ProductPriceId = choice.SelectedPrice.IND,
                            DisplayOrder = choice.DisplayOrder,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = "User"
                        };
                        await _context.ProductFirstChoices.AddAsync(firstChoice);
                    }

                    // İkinci seçenekleri kaydet
                    foreach (var choice in SecondChoices.Where(c => !string.IsNullOrEmpty(c.ChoiceName) && c.SelectedPrice != null))
                    {
                        var secondChoice = new ProductSecondChoice
                        {
                            ProductId = product.IND,
                            ChoiceName = choice.ChoiceName,
                            ProductPriceId = choice.SelectedPrice.IND,
                            DisplayOrder = choice.DisplayOrder,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = "User"
                        };
                        await _context.ProductSecondChoices.AddAsync(secondChoice);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    await Application.Current?.MainPage?.DisplayAlert("Başarılı", "Ürün başarıyla kaydedildi!", "Tamam")!;
                    await CancelAsync(); // Formu temizle ve geri dön
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                await Application.Current?.MainPage?.DisplayAlert("Hata", $"Ürün kaydedilirken hata oluştu: {ex.Message}", "Tamam")!;
            }
        }

        private async Task<bool> ValidateInputAsync()
        {
            if (string.IsNullOrEmpty(ProductName))
            {
                await Application.Current?.MainPage?.DisplayAlert("Hata", "Ürün adı boş olamaz", "Tamam")!;
                return false;
            }

            if (string.IsNullOrEmpty(ProductCode))
            {
                await Application.Current?.MainPage?.DisplayAlert("Hata", "Ürün kodu boş olamaz", "Tamam")!;
                return false;
            }

            if (SelectedCategory == null)
            {
                await Application.Current?.MainPage?.DisplayAlert("Hata", "Kategori seçilmelidir", "Tamam")!;
                return false;
            }

            if (SelectedPrice == null)
            {
                await Application.Current?.MainPage?.DisplayAlert("Hata", "Fiyat seçilmelidir", "Tamam")!;
                return false;
            }

            // Ürün kodu benzersizlik kontrolü
            var existingProduct = await _context.Products
                .AnyAsync(p => p.ProductCode == ProductCode);

            if (existingProduct)
            {
                await Application.Current?.MainPage?.DisplayAlert("Hata", "Bu ürün kodu zaten kullanılıyor", "Tamam")!;
                return false;
            }

            return true;
        }

        #endregion

        private async Task CancelAsync()
        {
            // Formu temizle
            ProductName = string.Empty;
            ProductCode = string.Empty;
            SelectedCategory = null;
            SelectedPrice = null;
            SelectedStatus = ProductStatus.Active;
            DisplayOrder = 1;
            ImageUrl = null;
            _imageData = null;
            SelectedFileName = null;
            ImageSource = null;
            FirstChoices.Clear();
            SecondChoices.Clear();

            // Geri dön
            await Shell.Current.GoToAsync("..");
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }

    // Helper Classes
    public class ProductStatusItem
    {
        public ProductStatus Status { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class ProductChoiceViewModel : INotifyPropertyChanged
    {
        private string _choiceName = string.Empty;
        private Price? _selectedPrice;
        private int _displayOrder = 1;

        public string ChoiceName
        {
            get => _choiceName;
            set => SetProperty(ref _choiceName, value);
        }

        public Price? SelectedPrice
        {
            get => _selectedPrice;
            set => SetProperty(ref _selectedPrice, value);
        }

        public int DisplayOrder
        {
            get => _displayOrder;
            set => SetProperty(ref _displayOrder, value);
        }

        public ObservableCollection<Price> AvailablePrices { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}