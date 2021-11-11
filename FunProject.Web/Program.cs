using FunProject.Domain;
using FunProject.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInrustractureLayerServices();
builder.Services.AddPersistanceLayerServices();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var sampleData = serviceScope.ServiceProvider.GetService<ISampleData>();
    sampleData?.SeedDataAsync();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();