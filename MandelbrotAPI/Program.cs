

using MandelbrotAPI.Menu;
using MandelbrotAPI.SettingsDef;

if (!File.Exists("Properties/Settings_def.json")) {
    Console.WriteLine("Error no settings def");
}

SettingsManager SM = new SettingsManager("Properties/Settings_def.json");
SM.Load("Properties/Settings.json");

/*
//the API hasn't been loaded;
if (!File.Exists("settings.json")) {

    Console.CursorVisible = false;
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.BufferHeight = 30;
    Console.Clear();


    Menu main = new Menu();

    main.Add("Select Mode",SetMode);
    main.Add("Set remotes",null);

    while (true) {
        main.Open();

    }
}


int? SetMode(string mode) {

    Menu modeMenu = new Menu();
    modeMenu.Add("Local", null);
    modeMenu.Add("Balencer", null);
    modeMenu.Add("Both", null);

    int? res = -1;
    while (res == -1) {
        res = modeMenu.Open();
    }

    return res;
}

*/

#region API
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression(options => {
    options.EnableForHttps = true;
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy => {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseResponseCompression();


app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion