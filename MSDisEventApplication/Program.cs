using MSDisEventApplication.Extensions;

namespace MSDisEventApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices(builder.Configuration);
            var app = builder.Build();

            app.Run();
        }
    }
}
