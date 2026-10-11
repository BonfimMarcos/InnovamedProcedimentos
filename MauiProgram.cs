using Microsoft.Extensions.Logging;
namespace InnovamedProcedimentos;
public static class MauiProgram {
 public static MauiApp CreateMauiApp() {
  var b=MauiApp.CreateBuilder().UseMauiApp<App>();
#if DEBUG
  b.Logging.AddDebug();
#endif
  return b.Build();
 }
}