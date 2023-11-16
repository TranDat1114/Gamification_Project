using FPT_Vote.ExcelDataHub;
using FPT_Vote.IServices;
using FPT_Vote.Services;
using FPT_Vote.SignalHub;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IExcelSignalService, ExcelSignalService>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapHub<ExcelDataHub>("/exceldatahub");

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
