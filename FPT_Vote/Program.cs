using FPT_Vote.ExcelDataHub;
using FPT_Vote.IServices;
using FPT_Vote.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IExcelSignalService, ExcelSignalService>();

builder.Services.AddResponseCompression(options => options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" }));

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
