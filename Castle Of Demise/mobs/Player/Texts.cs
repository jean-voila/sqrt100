
public partial class Player
{
    private string Title(string name)
    {
        var equals = new string('=', ((22 - name.Length) / 2) + 1);
        return $"|{equals} {name.ToUpper()} {equals}|\n";
    }

    private string Red(string text)
    {
        return $"[color=red]{text}[/color]";
    }
}
