using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Exchange.Class;

namespace Exchange.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double rate; double amount;
        string result;
        string currencyName = "";
        int currencyId = 0;
        string operation = "ask";
        IEnumerable<Rate> selectedCurrency = new List<Rate>();
        List<Currency> currencyList = new List<Currency>();
        List<Rate> ratesList = new List<Rate>();
        JsonDeserializer dataManager = new JsonDeserializer();



        public MainWindow()
        {
            dataManager.GetJson();
            dataManager.Deserialize();
            InitializeComponent();
            LoadData();
        }

        private void EnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckAndConfirm();
            }
        }
        private void EscapeKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAndConfirm();
        }

        private void CheckAndConfirm()
        {
            if (sellRadioBtn.IsChecked == false && buyRadioBtn.IsChecked == false)
                MessageBox.Show("Wybierz operację kupna lub sprzedaży.", "Nieprawidłowa operacja", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (amountBox.Text == "" || amountBox.Text == null)
                MessageBox.Show("Podaj kwotę do zamiany.", "Nie podano kwoty", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (double.TryParse(amountBox.Text.ToString(), out double result) == false)
                MessageBox.Show("Niewłaściwy format kwoty.", "Niewłaściwy format", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (amountBox.Text.Count() > 10)
                MessageBox.Show("Podana wartość jest zbyt długa.", "Niewłaściwa ilość znaków", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (double.TryParse(amountBox.Text.ToString(), out result) && result <= 0)
                MessageBox.Show("Kwota musi być większa niż 0.", "Niewłaściwa kwota", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                try
                {
                    rate = Convert.ToDouble(currencyBox1.Text);
                    amount = Convert.ToDouble(amountBox.Text.ToString());
                    this.result = RateCalculator.Calculate(rate, amount).ToString("#.##");
                    exchangedBox.Text = $"{this.result}zł";
                }
                catch (Exception)
                {
                    MessageBox.Show("Nieznany błąd. Aplikacja zostanie zamknięta.", "Nieznany błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();

                }
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void sellRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            SelectBidOrAsk();
            operation = "ask";
        }

        private void buyRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            SelectBidOrAsk();
            operation = "bid";
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sellRadioBtn.IsChecked = false;
            buyRadioBtn.IsChecked = false;
            currencyBox1.Text = "";
        }
        private void SelectBidOrAsk()
        {
            currencyName = currencyComboBox1.Text;
            selectedCurrency = ratesList.Where(r => r.currency == currencyName);

            if (operation == "ask")
            {
                rate = selectedCurrency.Select(r => r.ask).FirstOrDefault();
            }
            else
            {
                rate = selectedCurrency.Select(r => r.bid).FirstOrDefault();
            }

            currencyBox1.Text = Convert.ToString(rate);
        }
        // zrobić klasę odpowiadająca za działania na danych(metody LoadData,GetJson...)
        private void LoadData()
        {
            ratesList = dataManager.GetResponseData[0].rates;

            foreach (var r in ratesList)
            {
                currencyList.Add(new Currency { Id = currencyId, Code = r.code.ToString(), Name = r.currency.ToString() }); ;
                currencyId++;
            };

            currencyComboBox1.ItemsSource = currencyList;
            currencyComboBox1.DisplayMemberPath = "Name";
            currencyComboBox1.SelectedValuePath = "Id";
            currencyComboBox1.SelectedValue = "3";
        }
    }


}

