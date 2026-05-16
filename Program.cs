using Npgsql;
using FootManager.Services;
using Microsoft.AspNetCore.Authentication.Cookies; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";         
        options.AccessDeniedPath = "/Login";   
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddScoped(_ => 
{
    var connection = new NpgsqlConnection(connectionString);
    connection.Open(); 
    return connection;
});


builder.Services.AddScoped<JoueurService>();
builder.Services.AddScoped<MatchService>();
builder.Services.AddScoped<EquipeService>();
builder.Services.AddScoped<UserService>();

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


app.UseAuthentication(); 
app.UseAuthorization();  

app.MapRazorPages();

app.Run();