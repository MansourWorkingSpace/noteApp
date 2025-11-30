using Microsoft.Extensions.Logging;
using MauiApp4.Infrastructure.Persistence;
using MauiApp4.Services.Persistence;
using MauiApp4.Models.Business;
using MauiApp4.Services.Web;
using MauiApp4.Infrastructure.WebServices;

namespace MauiApp4
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Register persistence and business services for simple Clean Architecture wiring
            builder.Services.AddSingleton<DaoContext>();
            builder.Services.AddSingleton<INoteDao, NoteDao>();
            // Web API service (JSONPlaceholder demo)
            builder.Services.AddSingleton<INotesApiService, NotesApiService>();
            builder.Services.AddSingleton<NoteService>();
            builder.Services.AddSingleton<IUserDao, UserDao>();
            builder.Services.AddSingleton<UserService>();

            // ViewModels are instantiated directly in views; DI registrations removed as unused.

            var app = builder.Build();

            // Make DI container available via App.Services for pages/code-behind convenience
            MauiApp4.App.Services = app.Services;

            // Initialize SQLite DB (create file and tables) on startup.
            var ctx = app.Services.GetRequiredService<DaoContext>();
            ctx.Initialize();

            return app;
        }
    }
}
