namespace InnovamedProcedimentos;
public partial class App:Application {
 public App(){InitializeComponent();MainPage=new NavigationPage(new MainPage()){BarBackgroundColor=Color.FromArgb("#B71C1C"),BarTextColor=Colors.White};}
}