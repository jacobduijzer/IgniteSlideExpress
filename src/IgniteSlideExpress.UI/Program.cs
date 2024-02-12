using IgniteSlideExpress.Application;
using IgniteSlideExpress.Domain;
using IgniteSlideExpress.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ISessionRepository, SessionRepository>();
builder.Services.AddSingleton<IPdf2Images, Pdf2Images>();
builder.Services.AddSingleton<CreateTalkHandler>();
builder.Services.AddSingleton<GetSessionHandler>();
builder.Services.AddSingleton<TalkPositionHandler>();
builder.Services.AddSingleton(new PresentationPlayer(new IgniteSlideExpress.Infrastructure.Timer()));

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