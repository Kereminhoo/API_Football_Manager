using Npgsql;
using FootManager.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddScoped(_ => 
{
    var connection = new NpgsqlConnection(connectionString);
    connection.Open(); 
    return connection;
});


builder.Services.AddScoped<JoueurService>();
builder.Services.AddScoped<MatchService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();