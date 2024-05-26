using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MainClient._Model;
using MainClient.Services;
using MainClient.Utilities;

namespace MainClient._ViewModel
{
    class TradeStatusVM : ViewModelBase
    {
        private ObservableCollection<PrinterService> _printerTradeResults =
            new ObservableCollection<PrinterService>();
        public ObservableCollection<PrinterService> PrinterTradeResults
        {
            get => _printerTradeResults;
            set
            {
                if (_printerTradeResults != value)
                {
                    _printerTradeResults = value;
                    OnPropertyChanged(nameof(PrinterTradeResults));
                }
            }
        }

        private ObservableCollection<TradeStatusModel> _acctTradeResults =
            new ObservableCollection<TradeStatusModel>();
        public ObservableCollection<TradeStatusModel> AcctTradeResults
        {
            get => _acctTradeResults;
            set
            {
                if (_acctTradeResults != value)
                {
                    _acctTradeResults = value;
                    OnPropertyChanged(nameof(AcctTradeResults));
                }
            }
        }

        public TradeStatusVM(string accountNumber)
        {
            string acctNum = accountNumber;
            FetchTradeDetails(acctNum);

            PrintCommand = new RelayCommand(ExecutePrint);

            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }

        private void FetchTradeDetails(string acctNum)
        {
            var acctTradeList = TradeStatusModel.GetAcctTransactionsTradeByAcctNum(acctNum);
            if (acctTradeList != null)
            {
                _acctTradeResults.Clear();
                foreach (var accountTrans in acctTradeList)
                {
                    _acctTradeResults.Add(accountTrans);
                }
            }

            var printerTradeList = PrinterService.GetAcctTransactionPrintInfoByAcctNum(acctNum);
            if (printerTradeList != null)
            {
                _printerTradeResults.Clear();
                foreach (var printerTrans in printerTradeList)
                {
                    _printerTradeResults.Add(printerTrans);
                }
            }

            OnPropertyChanged(String.Empty);
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICommand PrintCommand { get; private set; }

        private void ExecutePrint(object parameter)
        {
            FlowDocument docToPrint = CreateFlowDocumentForPrinting();
            PrintDialog printDialog = new PrintDialog();

            // Check if the default printer settings can be obtained
            if (printDialog.PrintQueue != null && printDialog.PrintTicket != null)
            {
                // Set page orientation to Landscape
                printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
            }

            if (printDialog.ShowDialog() == true)
            {
                // If user confirms the print, send the document to the printer
                IDocumentPaginatorSource paginatorSource = docToPrint as IDocumentPaginatorSource;
                printDialog.PrintDocument(
                    paginatorSource.DocumentPaginator,
                    "Transaction Printing"
                );
            }
        }

        private FlowDocument CreateFlowDocumentForPrinting()
        {
            // Define withdrawal types to sort transactions.
            var buyTypes = new HashSet<string> { "BUY" };

            // Create a new FlowDocument
            FlowDocument flowDoc = new FlowDocument
            {
                PagePadding = new Thickness(50),
                ColumnGap = 0,
                ColumnWidth = double.MaxValue,
                PageWidth = 1122,
                PageHeight = 793,
                FontFamily = new FontFamily("Arial")
            };

            // Load the logo image
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Images/JosHoBankLogoBlue.png");
            logo.EndInit();

            // Create an Image control
            Image logoImage = new Image
            {
                Source = logo,
                Width = 110,
                Height = 60,
                Stretch = Stretch.Uniform
            };

            Paragraph bankInfoParagraph = new Paragraph();
            Run bankNameRun = new Run("JosHo Bank")
            {
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };

            // Add the bank name Run to the paragraph
            bankInfoParagraph.Inlines.Add(bankNameRun);

            // Line break after the bank name
            bankInfoParagraph.Inlines.Add(new LineBreak());

            // Create a normal font size Run for the address
            Run bankAddressRun = new Run("101 Main Street\nSan Francisco CA 94119\n1-800-555-5555")
            {
                FontSize = 12,
                FontWeight = FontWeights.Normal
            };

            // Add the address Run to the paragraph
            bankInfoParagraph.Inlines.Add(bankAddressRun);

            Paragraph clientInfoParagraph = new Paragraph();

            if (PrinterTradeResults.Any())
            {
                var transaction = PrinterTradeResults[0];

                // Check if Address2 is present
                if (string.IsNullOrWhiteSpace(transaction.PrinterContactAddres2))
                {
                    // Add a transparent rectangle to simulate the height of a line of text, effectively pushing the text down
                    Rectangle rect = new Rectangle
                    {
                        Height = 24,
                        Width = 1,
                        Fill = Brushes.Transparent
                    }; // Height to match the font size
                    InlineUIContainer inlineContainer = new InlineUIContainer(rect);
                    clientInfoParagraph.Inlines.Add(inlineContainer);
                }

                // Add the contact name
                clientInfoParagraph.Inlines.Add(
                    new Run(transaction.PrinterContactName) { FontSize = 12 }
                );
                clientInfoParagraph.Inlines.Add(new LineBreak());

                // Add the first address line
                clientInfoParagraph.Inlines.Add(
                    new Run(transaction.PrinterContactAddress) { FontSize = 12 }
                );
                clientInfoParagraph.Inlines.Add(new LineBreak());

                if (!string.IsNullOrWhiteSpace(transaction.PrinterContactAddres2))
                {
                    // Add Address2 if it exists
                    clientInfoParagraph.Inlines.Add(
                        new Run(transaction.PrinterContactAddres2) { FontSize = 12 }
                    );
                    clientInfoParagraph.Inlines.Add(new LineBreak());
                }

                // Add the city, state, and postal code
                clientInfoParagraph.Inlines.Add(
                    new Run(
                        $"{transaction.PrinterCity}, {transaction.PrinterState} {transaction.PrinterPostalCode}"
                    )
                    {
                        FontSize = 12
                    }
                );
            }

            // Account statement info
            Paragraph accountInfoParagraph = new Paragraph();

            if (PrinterTradeResults.Any())
            {
                var transaction = PrinterTradeResults[0];

                Rectangle rect = new Rectangle
                {
                    Height = 24,
                    Width = 1,
                    Fill = Brushes.Transparent
                }; // Height to match the font size
                InlineUIContainer inlineContainer = new InlineUIContainer(rect);
                accountInfoParagraph.Inlines.Add(inlineContainer);

                // Create a Run for the account type and make it bold and larger
                Run accountTypeRun = new Run(transaction.PrinterAcctType + " Trade Statement")
                {
                    FontSize = 12,
                    FontWeight = FontWeights.Bold
                };
                accountInfoParagraph.Inlines.Add(accountTypeRun);

                // Add a line break after the account type
                accountInfoParagraph.Inlines.Add(new LineBreak());

                string accountNumberFormatted;
                string acctNum = transaction.PrinterAcctNum;

                // Check the length of the account number and format it
                if (acctNum.Length == 14)
                {
                    accountNumberFormatted = acctNum.Insert(4, "-").Insert(10, "-");
                }
                else if (acctNum.Length == 10)
                {
                    accountNumberFormatted = acctNum.Insert(5, "-");
                }
                else
                {
                    accountNumberFormatted = acctNum;
                }

                // Construct the rest of the account details and keep the font size normal
                string details =
                    "Statement Period: "
                    + StartDate.ToString("MM/dd/yyyy")
                    + " to "
                    + EndDate.ToString("MM/dd/yyyy")
                    + "\nAccount No: "
                    + accountNumberFormatted;

                // Create a Run for the rest of the details
                Run detailsRun = new Run(details) { FontSize = 12, };

                accountInfoParagraph.Inlines.Add(detailsRun);
            }

            // Create a table to layout the header including the logo and the bank info
            Table headerTable = new Table();
            headerTable.CellSpacing = 0;

            TableColumn logoColumn = new TableColumn { Width = new GridLength(100) };
            TableColumn textColumn = new TableColumn { Width = GridLength.Auto }; // Let the content size the column
            headerTable.Columns.Add(logoColumn);
            headerTable.Columns.Add(textColumn);

            TableRowGroup headerGroup = new TableRowGroup();
            headerTable.RowGroups.Add(headerGroup);

            TableRow headerRow = new TableRow();
            headerGroup.Rows.Add(headerRow);

            // Add the logo to the first cell
            BlockUIContainer logoContainer = new BlockUIContainer(logoImage);
            TableCell logoCell = new TableCell(logoContainer)
            {
                BorderThickness = new Thickness(0)
            };
            headerRow.Cells.Add(logoCell);

            // Add the bank info to the second cell
            TableCell bankInfoCell = new TableCell(bankInfoParagraph)
            {
                BorderThickness = new Thickness(0),
                Padding = new Thickness(0, 0, 0, 0)
            };
            headerRow.Cells.Add(bankInfoCell);

            flowDoc.Blocks.Add(headerTable);

            Table infoTable = new Table();
            infoTable.CellSpacing = 0;

            infoTable.Columns.Add(
                new TableColumn
                {
                    Width = new GridLength(
                        flowDoc.PageWidth / 2 - flowDoc.PagePadding.Left - flowDoc.PagePadding.Right
                    )
                }
            );
            infoTable.Columns.Add(
                new TableColumn
                {
                    Width = new GridLength(
                        flowDoc.PageWidth / 2 - flowDoc.PagePadding.Left - flowDoc.PagePadding.Right
                    )
                }
            );

            TableRowGroup infoGroup = new TableRowGroup();
            infoTable.RowGroups.Add(infoGroup);

            TableRow infoRow = new TableRow();
            infoGroup.Rows.Add(infoRow);

            TableCell clientInfoCell = new TableCell(clientInfoParagraph)
            {
                BorderThickness = new Thickness(0),
                Padding = new Thickness(100, 0, 0, 0)
            };
            infoRow.Cells.Add(clientInfoCell);

            TableCell accountInfoCell = new TableCell(accountInfoParagraph)
            {
                BorderThickness = new Thickness(0),
                Padding = new Thickness(0, 0, 0, 0),
                TextAlignment = TextAlignment.Right
            };
            infoRow.Cells.Add(accountInfoCell);

            flowDoc.Blocks.Add(infoTable);

            // Add a line separator
            Line line = new Line
            {
                X1 = 0,
                X2 = flowDoc.PageWidth - flowDoc.PagePadding.Left - flowDoc.PagePadding.Right,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            BlockUIContainer lineContainer = new BlockUIContainer(line);
            flowDoc.Blocks.Add(lineContainer);

            // Create a table to hold the DataGrid content
            Table dgTable = new Table();
            flowDoc.Blocks.Add(dgTable); // Add the table to the document

            // Set fixed column widths based on content
            // Width must equal 1010
            int[] columnWidths = { 107, 362, 107, 108, 108, 108, 108 };
            foreach (int width in columnWidths)
            {
                dgTable.Columns.Add(new TableColumn { Width = new GridLength(width) });
            }

            // Create and add a header row to the table
            TableRow headerDataGridRow = new TableRow();
            dgTable.RowGroups.Add(new TableRowGroup());
            dgTable.RowGroups[0].Rows.Add(headerDataGridRow);

            // Set the font size and weight for the header
            headerDataGridRow.FontSize = 12;
            headerDataGridRow.FontWeight = FontWeights.Bold;

            string[] headers =
            {
                "Date",
                "Asset",
                "Buy",
                "Sell",
                "Quantity",
                "Market Price",
                "Fees"
            };

            // Add headers to the header row

            foreach (string header in headers)
            {
                TableCell headerCell = new TableCell(new Paragraph(new Run(header)))
                {
                    TextAlignment = TextAlignment.Center,
                    Background = (Brush)new BrushConverter().ConvertFrom("#009edc"),
                    Foreground = Brushes.White
                };
                headerDataGridRow.Cells.Add(headerCell);
            }

            int rowIndex = 0;

            foreach (
                TradeStatusModel transaction in AcctTradeResults.Where(
                    t => t.TradeDate >= StartDate && t.TradeDate <= EndDate
                )
            )
            {
                TableRow row = new TableRow();

                // Add date cell
                TableCell dateCell = new TableCell(
                    new Paragraph(new Run(transaction.TradeDate?.ToString("MM/dd/yyyy") ?? "N/A"))
                );

                // Add symbol cell
                TableCell symbolCell = new TableCell(
                    new Paragraph(new Run(transaction.TradeName))
                );

                // Add symbol cell
                TableCell quantityCell = new TableCell(
                    new Paragraph(new Run(transaction.TradeQuantity.ToString()))
                    {
                        TextAlignment = TextAlignment.Right
                    }
                );

                // Add market price cell
                TableCell priceCell = new TableCell(
                    new Paragraph(new Run(transaction.TradePrice?.ToString("C") ?? "—"))
                    {
                        TextAlignment = TextAlignment.Right
                    }
                );

                // Initialize cells for Buy and Sell
                string amountString = transaction.TradeAmount?.ToString("C") ?? "N/A";
                string buyAmount = "—";
                string sellAmount = "—";

                // Check if the transaction is a buy
                if (buyTypes.Contains(transaction.TradeAction))
                {
                    buyAmount = amountString;
                }
                else
                {
                    sellAmount = amountString;
                }

                // Add buy cell
                TableCell buyCell = new TableCell(
                    new Paragraph(new Run(buyAmount)) { TextAlignment = TextAlignment.Right }
                );

                // Add sell cell
                TableCell sellCell = new TableCell(
                    new Paragraph(new Run(sellAmount)) { TextAlignment = TextAlignment.Right }
                );

                // Add fees cell
                TableCell feesCell = new TableCell(
                    new Paragraph(new Run(transaction.TradeFees?.ToString("C") ?? "—"))
                    {
                        TextAlignment = TextAlignment.Right
                    }
                );

                // Set background color based on row index (even or odd)
                if (rowIndex % 2 == 0) // Even row index, zero-based
                {
                    row.Background = Brushes.White;
                }
                else // Odd row index
                {
                    row.Background = Brushes.LightGray;
                }

                row.Cells.Add(dateCell);
                row.Cells.Add(symbolCell);
                row.Cells.Add(buyCell);
                row.Cells.Add(sellCell);
                row.Cells.Add(quantityCell);
                row.Cells.Add(priceCell);
                row.Cells.Add(feesCell);

                // Add the row to the table's row groups
                dgTable.RowGroups[0].Rows.Add(row);

                // Increment the row index for the next iteration
                rowIndex++;
            }
            return flowDoc;
        }
    }
}
