using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using IotCoreApp.Model;
using IotCoreApp.Model.MessageData;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IotCoreApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        static string deviceId= "YourDeviceId";
        static string deviceKey = "YourDeviceKey";
        static string connectionString = "YourAzureName.azure-devices.net";

        ObservableCollection<string> weatherLog = new ObservableCollection<string>();
        DeviceClient deviceClient;


        public MainPage()
        {
            InitializeComponent();

            lvMessages.ItemsSource = weatherLog;

            UpdateDeviceInfo();
            StartLoop();


        }

        private async void UpdateDeviceInfo()
        {

            try
            {

                deviceClient = DeviceClient.Create(connectionString, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey), TransportType.Http1);
                //Device Info object
                var deviceInfo = new DeviceType()
                {
                    ObjectType = "DeviceInfo",
                    Version = "1.0",
                    IsSimulatedDevice = false,
                    DeviceProperties = new DeviceProperties()
                    {
                        HubEnabledState = true,
                        Latitude = 48.827514,
                        Longitude = 2.238925,
                        DeviceID = deviceId
                    },
                    Commands = new List<Command>()
                    {
                        new Command()
                        {
                            Name = "SetHumidity",
                            Parameters = new List<Parameter>() {
                                new Parameter() {
                                    Name = "humidity",
                                    Type = "int"
                                }
                            }
                        },
                        new Command()
                        {
                            Name = "SetTemperature",
                            Parameters = new List<Parameter>() {
                                new Parameter() {
                                    Name = "temperature",
                                    Type = "int"
                                }
                            }
                        }
                    }
                };


                var messageString = JsonConvert.SerializeObject(deviceInfo);
                var interactiveMessage = new Message(Encoding.ASCII.GetBytes(messageString));
                interactiveMessage.Properties["messageType"] = "interactive";
                interactiveMessage.MessageId = Guid.NewGuid().ToString();

                await deviceClient.SendEventAsync(interactiveMessage);
            }
            catch (Exception)
            {

            }
        }

        private async void StartLoop()
        {
            int delay;
            while (true)
            {
                await SendDataToAzureIOTSuite();


                delay = Convert.ToInt32(txtBoxDelay.Text);
                //Delay
                await Task.Delay(delay * 1000);
            }
        }

        private async Task SendDataToAzureIOTSuite()
        {
            try
            {

                WeatherObject weather = await GetWeather(txtBoxCity.Text);
                string messageBody = DateTime.Now + " Temperature: " + Math.Round(weather.main.tempCelsius, 2)
                       + ", Humidity: " + weather.main.humidity + "%"
                       + ", External Temperature: " + Math.Round(weather.main.tempCelsius, 2);

                weatherLog.Add(messageBody);

                //We add rand value to see variation on graph
                Random rand = new Random();
                double randHumidity = rand.Next(0, 5) * 0.1;
                double randTemperature = rand.Next(0, 5) * 0.1;

                RemoteMonitorTelemetryData monitorData = new RemoteMonitorTelemetryData()
                {
                    DeviceId = deviceId,
                    ExternalTemperature = Math.Round(weather.main.tempCelsius, 2) + randTemperature,
                    Humidity = weather.main.humidity + randHumidity,
                    Temperature = Math.Round(weather.main.tempCelsius, 2) + randTemperature
                };
                var messageString = JsonConvert.SerializeObject(monitorData);
                var interactiveMessage = new Message(Encoding.ASCII.GetBytes(messageString));
                interactiveMessage.Properties["messageType"] = "interactive";
                interactiveMessage.MessageId = Guid.NewGuid().ToString();

                await deviceClient.SendEventAsync(interactiveMessage);


            }
            catch (Exception)
            {

            }
        }

        private async Task<WeatherObject> GetWeather(string cityName)
        {
            WeatherObject weather = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = await client.GetAsync(string.Format("weather?q={0},fr&appid=028742d8a62796df26d97018bd777514", cityName));
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    weather = JsonConvert.DeserializeObject<WeatherObject>(result);
                }
            }

            return weather;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SendDataToAzureIOTSuite();
        }
    }
}
