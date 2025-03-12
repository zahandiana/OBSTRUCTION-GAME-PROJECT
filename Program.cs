using Microsoft.Extensions.FileProviders;

namespace Backend_Obstruction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Identific locatia in care se afla fisierul obstruct_ui/index.html
            string? path = typeof(Program).Assembly.Location;
            string? obstruct_ui_root = null;
            while(path != null && obstruct_ui_root == null)
            {
                string root_test = System.IO.Path.Join(path, "obstruct_ui");
                string index_test = System.IO.Path.Join(root_test, "index.html");
                if (System.IO.File.Exists(index_test) )
                {
                    obstruct_ui_root = root_test;
                }
                else
                {
                    path = System.IO.Path.GetDirectoryName(path);
                }
            }
            if (obstruct_ui_root == null)
            {
                throw new Exception("Nu am gasit obstruct_ui/index.html");
            }

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(obstruct_ui_root)
            });

            // Configure the HTTP request pipeline.
            app.UseCors(p =>
            {
                p.AllowAnyHeader();
                p.AllowAnyMethod();
                p.AllowAnyOrigin();
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<ReturnIndexByDefault>(System.IO.Path.Join(obstruct_ui_root, "index.html"));

            app.Run();
        }
    }
}