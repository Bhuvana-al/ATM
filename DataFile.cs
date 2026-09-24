using System.Text.Encodings.Web;
using System.Text.Json;

namespace WestcoastBank;

public class DataFile
{
    private readonly string _path = Environment.CurrentDirectory + "/Data/Transactions.json";

    private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }; 
    public void WriteData(List<Transaction> trans)
    {
        string json = JsonSerializer.Serialize(trans, _options);
        File.WriteAllText(_path, json);
    }
    public List<Transaction> ReadData()
    {
        List<Transaction> txnList = [];
        string TxnString = File.ReadAllText(_path);

        if (!string.IsNullOrEmpty(TxnString) || !string.IsNullOrWhiteSpace(TxnString))
        {
            txnList = JsonSerializer.Deserialize<List<Transaction>>(TxnString, _options)!;
        }
        else
        {
            Console.WriteLine("Tomt");
        }
        return txnList;
    }
}
