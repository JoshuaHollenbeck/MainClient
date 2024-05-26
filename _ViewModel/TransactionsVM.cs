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
    class TransactionsVM : ViewModelBase
    {
        private ObservableCollection<PrinterService> _printerTransactionResults =
            new ObservableCollection<PrinterService>();
        public ObservableCollection<PrinterService> PrinterTransactionResults
        {
            get => _printerTransactionResults;
            set
            {
                if (_printerTransactionResults != value)
                {
                    _printerTransactionResults = value;
                    OnPropertyChanged(nameof(PrinterTransactionResults));
                }
            }
        }

        private ObservableCollection<TransactionsModel> _acctTransactionResults =
            new ObservableCollection<TransactionsModel>();
        public ObservableCollection<TransactionsModel> AcctTransactionResults
        {
            get => _acctTransactionResults;
            set
            {
                if (_acctTransactionResults != value)
                {
                    _acctTransactionResults = value;
                    OnPropertyChanged(nameof(AcctTransactionResults));
                }
            }
        }

        public TransactionsVM(string accountNumber)
        {
            string acctNum = accountNumber;
            FetchTransactionDetails(acctNum);
            PrintCommand = new RelayCommand(ExecutePrint);

            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }

        private void FetchTransactionDetails(string acctNum)
        {
            var acctTransactionList = TransactionsModel.GetAcctTransactionsByAcctNum(acctNum);
            if (acctTransactionList != null)
            {
                _acctTransactionResults.Clear();
                foreach (var accountTrans in acctTransactionList)
                {
                    _acctTransactionResults.Add(accountTrans);
                }
            }

            var printerTransactionList = PrinterService.GetAcctTransactionPrintInfoByAcctNum(
                acctNum
            );
            if (printerTransactionList != null)
            {
                _printerTransactionResults.Clear();
                foreach (var printerTrans in printerTransactionList)
                {
                    _printerTransactionResults.Add(printerTrans);
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
            var withdrawalTypes = new HashSet<string>
            {
                "CASH-W",
                "BUY",
                "CHCK",
                "DEBIT",
                "CREDT",
                "ACH-W",
                "WIRE-W",
                "ATM-W",
                "JRNL-W"
            };

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

            if (PrinterTransactionResults.Any())
            {
                var transaction = PrinterTransactionResults[0];

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

            if (PrinterTransactionResults.Any())
            {
                var transaction = PrinterTransactionResults[0];

                Rectangle rect = new Rectangle
                {
                    Height = 24,
                    Width = 1,
                    Fill = Brushes.Transparent
                }; // Height to match the font size
                InlineUIContainer inlineContainer = new InlineUIContainer(rect);
                accountInfoParagraph.Inlines.Add(inlineContainer);

                // Create a Run for the account type and make it bold and larger
                Run accountTypeRun = new Run(transaction.PrinterAcctType + " Account Statement")
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
            int[] columnWidths = { 161, 300, 160, 160, 80, 149 };
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
                "Description",
                "Withdrawals",
                "Deposits",
                "Fees",
                "Balance"
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
                TransactionsModel transaction in AcctTransactionResults.Where(
                    t => t.TransactionsDate >= StartDate && t.TransactionsDate <= EndDate
                )
            )
            {
                TableRow row = new TableRow();

                // Add date cell
                TableCell dateCell = new TableCell(
                    new Paragraph(
                        new Run(transaction.TransactionsDate?.ToString("MM/dd/yyyy") ?? "N/A")
                    )
                );

                // Add description cell
                TableCell descriptionCell = new TableCell(
                    new Paragraph(new Run(transaction.TransactionsActionLong))
                );

                // Initialize cells for Withdrawals and Deposits
                string amountString = transaction.TransactionsAmount?.ToString("C") ?? "N/A";
                string withdrawalAmount = "—";
                string depositAmount = "—";

                // Check if the transaction is a withdrawal
                if (withdrawalTypes.Contains(transaction.TransactionsAction))
                {
                    withdrawalAmount = amountString;
                }
                else
                {
                    depositAmount = amountString;
                }

                // Add withdrawal cell
                TableCell withdrawalCell = new TableCell(
                    new Paragraph(new Run(withdrawalAmount)) { TextAlignment = TextAlignment.Right }
                );

                // Add deposit cell
                TableCell depositCell = new TableCell(
                    new Paragraph(new Run(depositAmount)) { TextAlignment = TextAlignment.Right }
                );

                // Add fees cell
                TableCell feesCell = new TableCell(
                    new Paragraph(new Run(transaction.TransactionsTradeFees?.ToString("C") ?? "—"))
                    {
                        TextAlignment = TextAlignment.Right
                    }
                );

                // Add balance cell
                TableCell balanceCell = new TableCell(
                    new Paragraph(
                        new Run(transaction.TransactionsPostBalance?.ToString("C") ?? "—")
                    )
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
                row.Cells.Add(descriptionCell);
                row.Cells.Add(withdrawalCell);
                row.Cells.Add(depositCell);
                row.Cells.Add(feesCell);
                row.Cells.Add(balanceCell);

                // Add the row to the table's row groups
                dgTable.RowGroups[0].Rows.Add(row);

                // Increment the row index for the next iteration
                rowIndex++;
            }
            return flowDoc;
        }
    }
}
