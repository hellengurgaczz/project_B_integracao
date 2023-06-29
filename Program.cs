using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using project_B_integracao.Models;
using System;
using System.Text;
using project_B_integracao.Controllers;
using project_B_integracao.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>
(
    options => options.UseInMemoryDatabase("database")
);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IModel>(serviceProvider =>
{
    var factory = new ConnectionFactory() { HostName = "localhost" };
    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();
    return channel;
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Consumer
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
     var context = serviceProvider.GetRequiredService<DataContext>();
    var channel = serviceProvider.GetRequiredService<IModel>();

    string queueName = "folhas-calculadas";
    channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var content = Encoding.UTF8.GetString(body);
        Console.WriteLine("Mensagem recebida: {0}", content);

        var folha = JsonConvert.DeserializeObject<Folha>(content);

        var folhasController = new FolhaController(context);
        var result = folhasController.Create(folha);
       
    };

    channel.BasicConsume(queueName, autoAck: true, consumer: consumer);

    Console.ReadLine();
}

app.Run();
