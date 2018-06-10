using System.Threading.Tasks;

namespace PriceMonitoring.Inerfaces.Services
{
    public interface IDataReader
    {
        Task<string> GetJsonFromUrl(string url);
    
        byte[] ReadImageBytes(string imageName, string rootUrl);
    }
}