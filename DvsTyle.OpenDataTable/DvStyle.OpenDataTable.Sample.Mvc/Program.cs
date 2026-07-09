using System.Text.Json.Serialization;
using DvStyle.OpenDataTable.Binders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Traduit la requête AJAX DataTables (draw/start/length/order/columns/search…) en DataTableServerRequestHeader.
    options.ModelBinderProviders.Insert(0, new DataTableModelBinderProvider());
})
.AddJsonOptions(o =>
{
    // Les colonnes DataTables ciblent les propriétés par leur nom .NET (PascalCase) : on désactive le camelCase.
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
    // Sérialise les enums en texte (ex. "Informatique" au lieu de 0).
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
