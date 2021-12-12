
//var content = 
string publishPath = @"E:\Projects\Visual Studio\Cat5Backend\Cat5DB\bin\Release\net6.0\publish";
var appsettingsJson = new ByteArrayContent(File.ReadAllBytes($@"{publishPath}\appsettings.json"));
var cat5DB = new ByteArrayContent(File.ReadAllBytes($@"{publishPath}\Cat5DB"));

string[] truso = File.ReadAllLines(@"E:\Projects\Visual Studio\Cat5Backend\Cat5DB.Publish\truso.secret");
string trusoKey = truso[0];
HttpClient client = new();
//string deleteRes = await client.GetStringAsync($"https://db.team3489.tk:8443/delete?key={trusoKey}");
//Console.WriteLine($"Delete Request: {deleteRes}");
var res = await client.PostAsync($"https://db.team3489.tk:8443/upload?key={trusoKey}&file=test.json", appsettingsJson);
Console.WriteLine("Done");