string trusoKey = await File.ReadAllTextAsync(@"E:\Projects\Visual Studio\Cat5Backend\Cat5DB.Publish\truso.secret");

string publishPath = @"E:\Projects\Visual Studio\Cat5Backend\Cat5DB\bin\Release\net6.0\publish\";

MetaTruso truso = new(trusoKey);

_ = await truso.Delete("Cat5DB/appsettings.json,Cat5DB/Cat5DB");

_ = await truso.Upload(publishPath + "appsettings.json", "Cat5DB/appsettings.json");
_ = await truso.Upload(publishPath + "Cat5DB", "Cat5DB/Cat5DB");

_ = await truso.Execute("Cat5DB/stop.sh");
_ = await truso.Execute("Cat5DB/start.sh");