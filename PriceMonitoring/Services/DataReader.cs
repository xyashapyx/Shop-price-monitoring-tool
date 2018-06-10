using System.IO;
using System.Net;
using System.Threading.Tasks;
using PriceMonitoring.Inerfaces.Services;

namespace PriceMonitoring.Services
{
    public class DataReader : IDataReader
    {
        public async Task<string> GetJsonFromUrl(string url)
        {
            string jsonResponse = string.Empty;
            ServicePointManager
                    .ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
            var request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                jsonResponse = await reader.ReadToEndAsync();
            }
            return jsonResponse;
        }

        public byte[] ReadImageBytes(string imageName, string rootUrl)
        {
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(rootUrl + imageName);
            return imageBytes;
        }
    }
}