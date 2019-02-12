using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace CityConditionComparision.Pages
{
    public class DataModel : PageModel
    {
        public string Message { get; set; }
        public string JSONresponse;

        public string firstSearchText;
        public string secondSearchText;

        public string firstCityName;
        public string firstCityCountry;
        public string firstCityTemperature;
        public string firstCityPressure;
        public string firstCityIcon;
        public string firstCityMapURL;

        public string secondCityName;
        public string secondCityCountry;
        public string secondCityTemperature;
        public string secondCityPressure;
        public string secondCityIcon;
        public string secondCityMapURL;



        public void OnGet()
        {
            string userEntry = Request.QueryString.Value;
            firstSearchText = userEntry.Split("=")[1].Split("&")[0];
            secondSearchText = userEntry.Split("=")[2];

            string firstCityDetails = FetchDetails(firstSearchText);
            string secondCityDetails = FetchDetails(secondSearchText);

            firstCityName = firstCityDetails.Split(",")[0];
            firstCityCountry = firstCityDetails.Split(",")[1];
            firstCityTemperature = firstCityDetails.Split(",")[2];
            firstCityPressure = firstCityDetails.Split(",")[3];
            firstCityIcon = firstCityDetails.Split(",")[4];

            firstCityMapURL = "https://www.google.com/maps/place/" + firstSearchText;

            secondCityName = secondCityDetails.Split(",")[0];
            secondCityCountry = secondCityDetails.Split(",")[1];
            secondCityTemperature = secondCityDetails.Split(",")[2];
            secondCityPressure = secondCityDetails.Split(",")[3];
            secondCityIcon = secondCityDetails.Split(",")[4];

            secondCityMapURL = "https://www.google.com/maps/place/" + secondSearchText;

        }

        public string FetchDetails(string searchtext)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?q=" + searchtext + "&appid=c2675cc27e2d55338119291273f23807");
            try
            {
                request.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1521.3 Safari/537.36";
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    JSONresponse = reader.ReadToEnd();

                    dynamic parsing = JObject.Parse(JSONresponse);
                    try
                    {
                        string cityName = parsing.name;
                        string cityCountry = parsing.sys.country;
                        string cityTemperature = parsing.main.temp;
                        string cityPressure = parsing.main.pressure;
                        string cityIcon = "http://openweathermap.org/img/w/" + parsing.weather[0].icon + ".png";

                        return cityName + "," + cityCountry + "," + cityTemperature + "," + cityPressure + "," + cityIcon;
                    }
                    catch (Exception e)
                    {
                        Message = "Oops! Something went wrong.";
                        return null;
                    }
                }
            }
            catch (WebException ex)
            {
                Message = ex.ToString();
                return null;
            }
        }
    }
}
