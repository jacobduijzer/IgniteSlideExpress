using IgniteSlideExpress.Core;
using IgniteSlideExpress.UI.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IgniteSlideExpress.Core.ITimer, IgniteSlideExpress.Core.Timer>();
builder.Services.AddSingleton<PresentationViewModel>();
builder.Services.AddSingleton<ISessionRepository, SessionRepository>();
builder.Services.AddSingleton<IPdf2Slides, Pdf2Slides>();
builder.Services.AddSingleton<CreateTalkHandler>();
builder.Services.AddSingleton<GetSessionHandler>();
builder.Services.AddSingleton<TalkPositionHandler>();
builder.Services.AddSingleton<RemoveTalkHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();