using System.Text.Encodings.Web;
using System.Text.Json;
using WestcoastBank.Models.Accounts;

namespace WestcoastBank.Models.Persistance;

public class Storage<T>
{
    private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }; 
    public static void WriteData(List<T> trans, string path)
    {
        try
        {
            string json = JsonSerializer.Serialize(trans, _options);
            File.WriteAllText(path, json);
        }
        catch(IOException ex)
        {
            throw new Exception(ex.Message);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static List<T> ReadData(string path)
    {
        try
        {
            string storedTrx = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(storedTrx) || !string.IsNullOrWhiteSpace(storedTrx))
            {
                return JsonSerializer.Deserialize<List<T>>(storedTrx, _options)!;
            }
            else
            {
                return [];
            }
        }
        catch(IOException ex)
        {
            throw new Exception(ex.Message);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
