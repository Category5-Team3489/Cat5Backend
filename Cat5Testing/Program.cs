Console.WriteLine("Hello, World!");

Random r = new();

void Recursive50(ref int i)
{
    i++;
    if (r.Next(0, 45) > 0)
    {
        Recursive50(ref i);
    }
}

List<int> list = new List<int>();
for (int i = 0; i < 1000000; i++)
{
    int runs = 0;
    Recursive50(ref runs);
    list.Add(runs);
}
Console.WriteLine(list.Sum() / 1000000f);



/*
int requests = 0;

List<Task> requesters = new();
for (int i = 0; i < 80; i++)
{
    requesters.Add(Task.Run(async () =>
    {
        HttpClient client = new();
        for (int i = 0; i < 1000; i++)
        {
            await client.GetAsync("https://db.team3489.tk/");
            Interlocked.Increment(ref requests);
        }
    }));
}

int prev = 0;
requesters.Add(Task.Run(() =>
{
    while (true)
    {
        int req = requests;
        Console.WriteLine(req - prev);
        prev = req;
        Thread.Sleep(1000);
    }
}));

await Task.WhenAll(requesters);
*/