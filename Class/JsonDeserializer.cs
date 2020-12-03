using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Windows;
using Exchange.Class;

namespace Exchange.Views
{
    public class JsonDeserializer
    {
        string jsonValue = "";
        IEnumerable<Rate> selectedCurrency = new List<Rate>();
        List<Currency> currenciesList = new List<Currency>();
        List<Rate> ratesList = new List<Rate>();
        TableData[] responseData;

        public JsonDeserializer(TableData[] responseData)
        {
            this.responseData = responseData;
        }
        public JsonDeserializer()
        {

        }

        public TableData[] GetResponseData
        {
             get {return responseData; }
               
        }
        internal void GetJson()
        {
            string url = "http://api.nbp.pl/api/exchangerates/tables/c/?format=json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    jsonValue = reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    MessageBox.Show($"Nie można pobrać danych z NBP. (Status Code: {httpResponse.StatusCode})\nProgram zostanie zamknięty.", "Nie można pobrać danych", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Coś poszło nie tak...\n Program zostanie zamknięty.", "Nieznany błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

        }
        internal TableData[] Deserialize()
        {
            responseData = JsonConvert.DeserializeObject<TableData[]>(jsonValue);
            return responseData;
        }


    }


}

