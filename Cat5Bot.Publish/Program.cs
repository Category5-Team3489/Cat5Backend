string trusoKey = await File.ReadAllTextAsync(@"E:\Projects\Visual Studio\Cat5Backend\Cat5DB.Publish\truso.secret");

string publishPath = @"E:\Projects\Visual Studio\Cat5Backend\Cat5Bot\bin\Release\net6.0\publish\linux-x64\";

MetaTruso truso = new(trusoKey);

//_ = await truso.Delete("Cat5Bot/Cat5Bot");

_ = await truso.Upload(publishPath + "Cat5Bot", "Cat5Bot/Cat5Bot");

_ = await truso.Execute("Cat5Bot/stop.sh");
_ = await truso.Execute("Cat5Bot/start.sh");