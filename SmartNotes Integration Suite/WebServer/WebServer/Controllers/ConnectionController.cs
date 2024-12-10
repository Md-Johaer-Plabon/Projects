using Microsoft.AspNetCore.Mvc;
using System.Text;

[Route("api")]
[ApiController]
public class ConnectionController : ControllerBase
{
    [HttpGet("samsung-notes")]
    public async Task<IActionResult> InitAddIn()
    {
        var appServiceClient = new AppServiceClient();
        List<Tuple<string, string, string>> response = await appServiceClient.SendRequestToInitAddInPage();
        //List<Tuple<string, string, string>> response = appServiceClient.DemoList();
        Response.Headers.Append("Access-Control-Allow-Origin", "*");

        return Ok(response);
    }

    [HttpPost("pages")]
    public async Task<IActionResult> RequestForNotePages()
    {
        string uuid = "";

        using (StreamReader streamReader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            try
            {
                uuid = await streamReader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception to read body stream of HTTP request. Message: " + ex.Message);
            }
        }

        var appServiceClient = new AppServiceClient();
        List<string> response = await appServiceClient.SendRequestToGetNotePages(uuid);
        Response.Headers.Append("Access-Control-Allow-Origin", "*");

        return Ok(response);
    }
}