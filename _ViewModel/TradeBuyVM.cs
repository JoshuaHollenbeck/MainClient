using MainClient.Utilities;
using MainClient.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Globalization;
using System.Windows;
using System;

namespace MainClient._ViewModel
{
    class TradeBuyVM : ViewModelBase
    {
        private ObservableCollection<string> _stockExchanges;
        public ObservableCollection<string> StockExchanges
        {
            get => _stockExchanges;
            set
            {
                _stockExchanges = value;
                OnPropertyChanged(nameof(StockExchanges));
            }
        }

        private string _selectedStockExchange;
        public string SelectedStockExchange
        {
            get => _selectedStockExchange;
            set
            {
                _selectedStockExchange = value;
                OnPropertyChanged(nameof(SelectedStockExchange));
                UpdateSelectedCultureBasedOnExchange(value);
                // Filter stocks based on the selected exchange
                FilterStocksByExchange();
            }
        }

        private ObservableCollection<TradeService.TradeInfo> _stockInfos;
        public ObservableCollection<TradeService.TradeInfo> StockInfos
        {
            get => _stockInfos;
            set
            {
                _stockInfos = value;
                OnPropertyChanged(nameof(StockInfos));
            }
        }

        // Selected stock info properties
        private TradeService.TradeInfo _selectedStockInfo;
        public TradeService.TradeInfo SelectedStockInfo
        {
            get => _selectedStockInfo;
            set
            {
                _selectedStockInfo = value;
                OnPropertyChanged(nameof(SelectedStockInfo));
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

        public decimal? BuyTradeQuantity { get; set; }

        // Trade quantity properties
        private string _buyTradeQuantityText;
        public string BuyTradeQuantityText
        {
            get => _buyTradeQuantityText;
            set
            {
                if (_buyTradeQuantityText != value)
                {
                    _buyTradeQuantityText = value;
                    BuyTradeQuantity = TryParseDecimal(value);
                    OnPropertyChanged(nameof(BuyTradeQuantity));
                    CalculateTotalTradePrice();
                }
            }
        }

        public decimal? BidPrice { get; set; }

        // Bid price properties
        private string _bidPriceText;
        public string BidPriceText
        {
            get => _bidPriceText;
            set
            {
                _bidPriceText = value;
                BidPrice = TryParseDecimal(value);
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

        private void CalculateTotalTradePrice()
        {
            if (BuyTradeQuantity > 0 && BidPrice > 0)
            {
                TotalTradePrice = (BuyTradeQuantity * BidPrice) + (StockFee ?? 0);
                OnPropertyChanged(nameof(TotalTradePrice));
            }
        }

        public string StockName { get; set; }
        public string BuyTradeCurrency { get; set; }
        public string BuyStockTicker { get; set; }
        public decimal? StockFee { get; set; }
        public decimal? UsdPrice { get; set; }
        public decimal? LocalPrice { get; set; }
        public string StockDisplay { get; set; }
        public decimal? TotalTradePrice { get; set; }

        public ICommand AddTradeCommand { get; set; }

        public TradeBuyVM()
        {
            StockInfos = new ObservableCollection<TradeService.TradeInfo>();
            StockExchanges = new ObservableCollection<string>();
            LoadStockInfos();
            AddTradeCommand = new RelayCommand(ExecuteTradeBuy);
        }

        private void LoadStockInfos()
        {
            var tradeInfos = TradeService.LookUpStockInfo();
            foreach (var tradeInfo in tradeInfos)
            {
                StockInfos.Add(tradeInfo);
                if (!StockExchanges.Contains(tradeInfo.StockExchangeName))
                {
                    StockExchanges.Add(tradeInfo.StockExchangeName);
                }
            }
        }

        private void FilterStocksByExchange()
        {
            var filteredStocks = TradeService
                .LookUpStockInfo()
                .Where(info => info.StockExchangeName == SelectedStockExchange);
            StockInfos.Clear();
            foreach (var stock in filteredStocks)
            {
                StockInfos.Add(stock);
            }

            if (!filteredStocks.Any(info => info == SelectedStockInfo))
            {
                SelectedStockInfo = filteredStocks.FirstOrDefault();
            }
        }

        private void UpdateStockInfo()
        {
            if (SelectedStockInfo != null)
            {
                StockName = SelectedStockInfo?.StockName;
                StockDisplay = SelectedStockInfo?.DisplayStockInfo;
                StockFee = SelectedStockInfo?.StockFee;
                BuyStockTicker = SelectedStockInfo?.StockAbbr;
                BuyTradeCurrency = SelectedStockInfo?.StockCurrency;
                UsdPrice = SelectedStockInfo?.UsdPrice;
                LocalPrice = SelectedStockInfo?.LocalPrice;
                OnPropertyChanged(nameof(StockName));
                OnPropertyChanged(nameof(StockDisplay));
                OnPropertyChanged(nameof(BuyStockTicker));
                OnPropertyChanged(nameof(BuyTradeCurrency));
                OnPropertyChanged(nameof(StockFee));
                OnPropertyChanged(nameof(UsdPrice));
                OnPropertyChanged(nameof(LocalPrice));
                CalculateTotalTradePrice();
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

        public Action<string> CloseAndLoadAccountAction { get; set; }

        private void ExecuteTradeBuy(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            if (BidPrice != null)
            {
                TradeService tradeService = new TradeService();
                tradeService.InsertAcctTransactionTradeBuyByAcctNum(
                    accountNumber,
                    repId,
                    BidPrice,
                    BuyTradeQuantity,
                    BuyStockTicker
                );

                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }
    }
}
