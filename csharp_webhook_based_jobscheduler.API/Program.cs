var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();

app.UseHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

app.Run();
