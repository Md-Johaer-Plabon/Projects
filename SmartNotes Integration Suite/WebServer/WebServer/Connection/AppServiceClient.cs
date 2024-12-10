using Newtonsoft.Json;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Storage;

public class AppServiceClient
{
    private readonly AppServiceConnection _connection;

    public AppServiceClient()
    {
        _connection = new AppServiceConnection
        {
            AppServiceName = "samsungnotes.addin.apiservice",
            PackageFamilyName = "SAMSUNGELECTRONICSCoLtd.SamsungNotes_wyx1vj98g3asy"
        };
    }

    public List<Tuple<string, string, string>> DemoList()
    {
        List<Tuple<string, string, string>> list = new List<Tuple<string, string, string>>();
        string path = Path.Combine("D:\\DemoThumbnail", "Images");
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] files = info.GetFiles();

        foreach (FileInfo imageFile in files)
        {
            
            string base64 = ImageToBase64(imageFile.FullName);
            Tuple<string, string, string> dataList = new(base64, "image", "123");
            list.Add(dataList);
        }
        return list;
    }

    private string ImageToBase64(string filePath)
    {
        byte[] imageBytes = File.ReadAllBytes(filePath);

        return Convert.ToBase64String(imageBytes);
    }

    public async Task<List<string>> SendRequestToGetNotePages(string uuid)
    {
        var status = await _connection?.OpenAsync();
        string responseData = "App Response Failed!";
        List<string> dataList = [];

        if (status != AppServiceConnectionStatus.Success)
        {
            return null;
        }

        var request = new ValueSet { { "RequestType", "NotePages" }, { "uuid", uuid } };
        AppServiceResponse response = await _connection?.SendMessageAsync(request);

        if (response?.Status == AppServiceResponseStatus.Success)
        {
            try
            {
                responseData = (response?.Message["NotePagesList"]) as string;
                dataList = JsonConvert.DeserializeObject<List<string>>(responseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        return dataList;
    }

    public async Task<List<Tuple<string, string, string>>> SendRequestToInitAddInPage()
    {
        var status = await _connection?.OpenAsync();
        string responseData = "App Response Failed!";
        List<Tuple<string, string, string>> dataList = [];

        if (status != AppServiceConnectionStatus.Success)
        {
            return null;
        }

        var request = new ValueSet { { "RequestType", "Initialize" } };
        AppServiceResponse response = await _connection?.SendMessageAsync(request);

        if (response?.Status == AppServiceResponseStatus.Success)
        {
            try
            {
                responseData = (response?.Message["AddInInitialization"]) as string;
                dataList = JsonConvert.DeserializeObject<List<Tuple<string, string, string>>>(responseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        return dataList;
    }
}