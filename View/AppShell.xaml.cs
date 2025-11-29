using View;

namespace View;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Routes utilisées par Shell.GoToAsync(...)
        Routing.RegisterRoute("ListeLivresPage", typeof(ListeLivresPage));
        Routing.RegisterRoute("FavorisPage", typeof(FavorisPage));
        Routing.RegisterRoute("AdminDashboardPage", typeof(AdminDashboardPage));
        Routing.RegisterRoute("AdminAjoutLivrePage", typeof(AdminAjoutLivrePage));
        Routing.RegisterRoute("AdminSuppressionLivrePage", typeof(AdminSuppressionLivrePage));
        Routing.RegisterRoute("AdminListeUtilisateursPage", typeof(AdminListeUtilisateursPage));
        Routing.RegisterRoute("DetailsLivrePage", typeof(DetailsLivrePage));
    }
}
