using Energizet.VideoStriming.DB;
using Energizet.VideoStriming.DB.DB;
using Energizet.VideoStriming.Download;
using Energizet.VideoStriming.Upload;
using Energizet.VideoStriming.Upload.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddDbContext<EntitiesContext>(provider =>
	provider.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")),
	ServiceLifetime.Singleton
);

builder.Services.AddScoped<IUploadFileController, UploadFileController>((provider) => new UploadFileController($"files/"));
builder.Services.AddScoped<IUploadDBController, UploadDBController>((provider) => new UploadDBController((EntitiesContext)provider.GetService(typeof(EntitiesContext))!));
builder.Services.AddScoped<IVideoInfoController, VideoInfoController>((provider) => new VideoInfoController((EntitiesContext)provider.GetService(typeof(EntitiesContext))!, 1 * 1024 * 1024));
builder.Services.AddScoped<IDownloadController, DownloadFileController>((provider) => new DownloadFileController($"files/", 1 * 1024 * 1024));

FileHelper.Init("energizet@yandex.ru", "ec7907f9df02db5a1c51f6c91d4d9cb4");

var app = builder.Build();
app.UseCors(builder =>
{
	builder
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
