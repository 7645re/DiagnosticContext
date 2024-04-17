using RequestSpammer;

var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;
var endpointUrls = new CircularBuffer<string>(4, cancellationToken)
{
    "http://localhost:5082/ControllerOne/MethodOne",
    "http://localhost:5082/ControllerOne/MethodTwo",
    "http://localhost:5082/ControllerTwo/MethodOne",
    "http://localhost:5082/ControllerTwo/MethodTwo"
};

var tasks = new List<Task>();
for (var i = 0; i < 8; i++)
{
    var task = Task.Run(async () =>
    {
        using var httpClient = new HttpClient();
        while (!cancellationToken.IsCancellationRequested)
        {
            foreach (var url in endpointUrls)
            {
                Console.WriteLine($"Sending request to: {url} from Task: {Task.CurrentId}");
                await httpClient.GetAsync(url, cancellationToken);
            }
            await Task.Delay(new Random().Next(500, 2000), cancellationToken);
        }
        
        cancellationTokenSource.Cancel();
    });
    
    tasks.Add(task);
}

await Task.WhenAll(tasks);