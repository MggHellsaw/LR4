using LR4.Models;
using System.Text.Json;

namespace LR4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.Map("/Library", async context =>
                {
                    await context.Response.WriteAsync("Welcome to the Library!");
                });

                endpoints.Map("/Library/Books", async context =>
                {
                    var booksJson = File.ReadAllText("books.json");
                    var books = JsonSerializer.Deserialize<List<Book>>(booksJson);
                    var response = JsonSerializer.Serialize(books);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(response);
                });

                endpoints.Map("/Library/Profile/{id:int:range(0,5)?}", async context =>
                {
                    var id = context.Request.RouteValues["id"]?.ToString();
                    if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int userId))
                    {
                        var profilesJson = File.ReadAllText("Users.json");
                        var profiles = JsonSerializer.Deserialize<List<User>>(profilesJson);
                        if (userId >= 0 && userId < profiles.Count)
                        {
                            var profile = JsonSerializer.Serialize(profiles[userId]);
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(profile);
                        }
                    }
                    else
                    {
                        var profileJson = File.ReadAllText("CurrentUser.json");
                        var profile = JsonSerializer.Serialize(JsonSerializer.Deserialize<User>(profileJson));
                        await context.Response.WriteAsync(profile);
                    }
                });
            });
        }
    }
}
