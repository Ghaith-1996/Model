using System.IO;
using Bibliotheque.Services;
using Bibliotheque.ViewModels;
using View;

namespace View;

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

        // 📁 Emplacement du fichier XML
        string basePath = FileSystem.AppDataDirectory;
        string cheminBibliotheque = Path.Combine(basePath, "bibliotheque.xml");
        string cheminFavoris = Path.Combine(basePath, "favoris.xml");

        System.Diagnostics.Debug.WriteLine($"[DEBUG] bibliotheque.xml = {cheminBibliotheque}");

        // 🔧 Services
        builder.Services.AddSingleton(sp => new XmlBibliothequeService(cheminBibliotheque));
        builder.Services.AddSingleton(sp => new FavorisService(cheminFavoris));
        builder.Services.AddSingleton<AuthService>();

        // 🧠 ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ListeLivresViewModel>();
        builder.Services.AddTransient<FavorisViewModel>();
        builder.Services.AddTransient<AdminDashboardViewModel>();
        builder.Services.AddTransient<AdminAjoutLivreViewModel>();
        builder.Services.AddTransient<AdminSuppressionLivreViewModel>();
        builder.Services.AddTransient<AdminListeUtilisateursViewModel>();
        builder.Services.AddTransient<DetailsLivreViewModel>();

        // 🎨 Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ListeLivresPage>();
        builder.Services.AddTransient<FavorisPage>();
        builder.Services.AddTransient<AdminDashboardPage>();
        builder.Services.AddTransient<AdminAjoutLivrePage>();
        builder.Services.AddTransient<AdminSuppressionLivrePage>();
        builder.Services.AddTransient<AdminListeUtilisateursPage>();
        builder.Services.AddTransient<DetailsLivrePage>();

        return builder.Build();
    }
}
