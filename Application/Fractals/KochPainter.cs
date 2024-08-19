using di.Infrastructure.Common;
using FractalPainting.Application.Models;
using FractalPainting.Infrastructure.Common;
using Color = FractalPainting.Application.Models.Color;
using Point = FractalPainting.Application.Models.Point;
using Rectangle = FractalPainting.Application.Models.Rectangle;

namespace FractalPainting.Application.Fractals;

public class KochPainter
{
    private readonly Palette palette;
    private readonly IImageSettingsProvider imageSettingsProvider;

    public KochPainter(Palette palette, IImageSettingsProvider imageSettingsProvider)
    {
        this.palette = palette;
        this.imageSettingsProvider = imageSettingsProvider;
    }

    public IReadOnlyCollection<Figure> Paint()
    {
        var imageSettings = imageSettingsProvider.ImageSettings;
        
        var figures = new List<Figure>();
        var bgColor = palette.BackgroundColor;
        figures.Add(new Rectangle(imageSettings.Width, imageSettings.Height, new Point(0, 0),
            new Color(bgColor.R, bgColor.G, bgColor.B)));
        DrawSegment(figures, 0, imageSettings.Height * 0.9f, imageSettings.Width, imageSettings.Height * 0.9f, true);
        return figures;
    }

    private void DrawSegment(List<Figure> figures, float x0, float y0, float x1, float y1, bool primaryColor)
    {
        var len2 = (x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1);
        if (len2 < 4)
        {
            if (y0 < 0 || y1 < 0) return;
            var color = primaryColor ? palette.PrimaryColor : palette.SecondaryColor;
            figures.Add(new Line(new Point((int)x0, (int)y0), new Point((int)x1, (int)y1),
                new Color(color.R, color.G, color.B)));
        }
        else
        {
            var vx = (x1 - x0) / 3;
            var vy = (y1 - y0) / 3;
            DrawSegment(figures, x0, y0, x0 + vx, y0 + vy, primaryColor);
            var k = (float)Math.Sqrt(3) / 2f;
            var px = (x0 + x1) / 2 + vy * k;
            var py = (y0 + y1) / 2 - vx * k;
            DrawSegment(figures, x0 + vx, y0 + vy, px, py, !primaryColor);
            DrawSegment(figures, px, py, x0 + 2 * vx, y0 + 2 * vy, !primaryColor);
            DrawSegment(figures, x0 + 2 * vx, y0 + 2 * vy, x1, y1, primaryColor);
        }
    }
}