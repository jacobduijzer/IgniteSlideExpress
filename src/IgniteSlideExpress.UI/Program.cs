using IgniteSlideExpress.Core;
using IgniteSlideExpress.UI;
using IgniteSlideExpress.UI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IgniteSlideExpress.Core.ITimer, IgniteSlideExpress.Core.Timer>();
builder.Services.AddSingleton<SessionViewModel>();
builder.Services.AddSingleton<PresentationViewModel>();
builder.Services.AddSingleton<ISessionRepository, SessionRepository>();
builder.Services.AddSingleton<IPdf2Slides, Pdf2Slides>();
builder.Services.AddSingleton<UploadFileProcessor>();
builder.Services.AddSingleton<StartupPresentationImporter>();

var app = builder.Build();

await app.Services.GetRequiredService<StartupPresentationImporter>().Import();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
