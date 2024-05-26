using MainClient.Utilities;
using MainClient._Model;
using MainClient.Services;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System;
using System.Windows;

namespace MainClient._ViewModel
{
    class TradeSellVM : ViewModelBase
    {
        private ObservableCollection<PositionsModel> _accountHoldings;
        public ObservableCollection<PositionsModel> AccountHoldings
        {
            get => _accountHoldings;
            set
            {
                _accountHoldings = value;
                OnPropertyChanged(nameof(AccountHoldings));
            }
        }

        private PositionsModel _selectedStock;
        public PositionsModel SelectedStock
        {
            get => _selectedStock;
            set
            {
                _selectedStock = value;
                OnPropertyChanged(nameof(SelectedStock));
                UpdateSelectedCultureBasedOnExchange(_selectedStock?.PositionsExchange);
                UpdateStockInfo();
            }
        }

        // Selected culture properties
        private CultureInfo _selectedCultureInfo;
        public CultureInfo SelectedCultureInfo
        {
            get => _selectedCultureInfo;
            set
            {
                _selectedCultureInfo = value;
                OnPropertyChanged(nameof(SelectedCultureInfo));
            }
        }

        public string DisplayStockInfo =>
            $"{SelectedStock?.PositionsExchange}: {SelectedStock?.PositionsStockTicker}";
        public string StockName { get; set; }
        public string StockExchange { get; set; }
        public string SellTradeCurrency { get; set; }
        public string SellStockTicker { get; set; }
        public decimal? StockAvailable { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? UsdPrice { get; set; }
        public decimal? LocalPrice { get; set; }
        public decimal? TotalTradePrice { get; set; }
        public decimal? SellPrice { get; set; }

        // Bid price properties
        private string _sellPriceText;
        public string SellPriceText
        {
            get => _sellPriceText;
            set
            {
                _sellPriceText = value;
                SellPrice = TryParseDecimal(value);
                CalculateTotalTradePrice();
            }
        }

        private decimal? TryParseDecimal(string value)
        {
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }
            return null;
        }

        public decimal? SellTradeQuantity { get; set; }

        // Trade quantity properties
        private string _sellTradeQuantityText;
        public string SellTradeQuantityText
        {
            get => _sellTradeQuantityText;
            set
            {
                if (_sellTradeQuantityText != value)
                {
                    _sellTradeQuantityText = value;
                    SellTradeQuantity = TryParseDecimal(value);
                    OnPropertyChanged(nameof(SellTradeQuantity));
                    CalculateTotalTradePrice();
                }
            }
        }

        public ICommand SellTradeCommand { get; set; }

        public TradeSellVM()
        {
            LoadAccountHoldings();
            SellTradeCommand = new RelayCommand(ExecuteTradeSell);
        }

        private void LoadAccountHoldings()
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            var holdings = PositionsModel.GetAcctHoldingsByAcctNum(accountNumber);
            AccountHoldings = new ObservableCollection<PositionsModel>(holdings);
            if (AccountHoldings.Any())
            {
                SelectedStock = AccountHoldings.First();
            }
        }

        public void UpdateSelectedCultureBasedOnExchange(string stockExchange)
        {
            switch (stockExchange)
            {
                case "Nasdaq":
                case "New York Stock Exchange":
                case "NYSE American":
                    SelectedCultureInfo = new CultureInfo("en-US"); // United States Dollar ($)
                    break;
                case "Toronto Stock Exchange":
                    SelectedCultureInfo = new CultureInfo("en-CA"); // Canadian Dollar ($)
                    break;
                case "London Stock Exchange":
                    SelectedCultureInfo = new CultureInfo("en-GB"); // British Pound (£)
                    break;
                case "Australian Securities Exchange":
                    SelectedCultureInfo = new CultureInfo("en-AU"); // Australian Dollar ($)
                    break;
                case "European New Exchange Technology":
                    // Custom culture for the Euro with no space and symbol before the number
                    var customEuroCulture = (CultureInfo)
                        CultureInfo.GetCultureInfo("fr-FR").Clone();
                    customEuroCulture.NumberFormat.CurrencyPositivePattern = 0;
                    customEuroCulture.NumberFormat.CurrencyNegativePattern = 0;
                    SelectedCultureInfo = customEuroCulture;
                    break;
                case "Hong Kong Stock Exchange":
                    SelectedCultureInfo = new CultureInfo("zh-HK"); // Hong Kong Dollar ($)
                    break;
                default:
                    SelectedCultureInfo = CultureInfo.CurrentCulture; // Fallback to current culture
                    break;
            }
            UpdateStockInfo();
        }

        private void UpdateStockInfo()
        {
            if (SelectedStock != null)
            {
                SellStockTicker = SelectedStock.PositionsStockTicker;
                StockAvailable = SelectedStock.PositionsQuantity;
                StockName = SelectedStock.PositionsStockName;
                StockExchange = SelectedStock.PositionsExchange;
                CurrentPrice = SelectedStock.PositionsCurrentPrice;
                UsdPrice = SelectedStock.PositionsUsdPrice;
                LocalPrice = SelectedStock.PositionsCurrentPrice;
                SellTradeCurrency = SelectedStock.PositionsCurrency;
                OnPropertyChanged(nameof(SellStockTicker));
                OnPropertyChanged(nameof(StockAvailable));
                OnPropertyChanged(nameof(StockName));
                OnPropertyChanged(nameof(StockExchange));
                OnPropertyChanged(nameof(CurrentPrice));
                OnPropertyChanged(nameof(UsdPrice));
                OnPropertyChanged(nameof(LocalPrice));
                OnPropertyChanged(nameof(SellTradeCurrency));
                OnPropertyChanged(nameof(DisplayStockInfo));
                CalculateTotalTradePrice();
            }
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }
        
        private void ExecuteTradeSell(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            if (SellPrice != null)
            {
                TradeService tradeService = new TradeService();
                tradeService.InsertAcctTransactionTradeSellByAcctNum(
                    accountNumber,
                    repId,
                    SellPrice,
                    SellTradeQuantity,
                    SellStockTicker
                );

                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void CalculateTotalTradePrice()
        {
            if (SellTradeQuantity > 0 && SellPrice > 0)
            {
                TotalTradePrice = SellTradeQuantity * SellPrice;
                OnPropertyChanged(nameof(TotalTradePrice));
            }
        }
    }
}
