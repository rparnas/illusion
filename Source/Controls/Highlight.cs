namespace Illusion.Controls;

internal class Highlight
{
  public static readonly Highlight[] StandardHighlights = new []
  {
    new Highlight("Red",    196, 002, 051),
    new Highlight("Orange", 255, 120, 019),
    new Highlight("Yellow", 255, 211, 000),
    new Highlight("Green",  000, 163, 104),
    new Highlight("Blue",   000, 136, 191),
    new Highlight("Violet", 131, 063, 135),
  };

  public readonly Brush Brush;
  public readonly Color Color;
  public readonly string Name;

  public Highlight(string name, int r, int g, int b)
  {
    var color = Color.FromArgb(r, g, b);

    Brush = new SolidBrush(color);
    Color = color;
    Name = name;
  }
}
