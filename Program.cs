using di.Infrastructure.Common;
using FractalPainting.Application;
using FractalPainting.Application.Actions;
using FractalPainting.Application.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<Palette>();

services.AddSingleton<IObjectSerializer, XmlObjectSerializer>();
services.AddSingleton<IBlobStorage, FileBlobStorage>();
services.AddSingleton<SettingsManager>();

services.AddSingleton(provider => provider.GetService<SettingsManager>()!.Load());
services.AddSingleton<IImageSettingsProvider>(provider => provider.GetService<AppSettings>()!);

services.AddSingleton<IDragonPainterFactory, DragonPainterFactory>();

services.AddSingleton<KochPainter>();

services.AddSingleton<IApiAction, DragonFractalAction>();
services.AddSingleton<IApiAction, KochFractalAction>();
services.AddSingleton<IApiAction, UpdateImageSettingsAction>();
services.AddSingleton<IApiAction, GetImageSettingsAction>();
services.AddSingleton<IApiAction, UpdatePaletteSettingsAction>();
services.AddSingleton<IApiAction, GetPaletteSettingsAction>();

services.AddSingleton<App>();

var sp = services.BuildServiceProvider();

var app = sp.GetRequiredService<App>();

await app.Run();