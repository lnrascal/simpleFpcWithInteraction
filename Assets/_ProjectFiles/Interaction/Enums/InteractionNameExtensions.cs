public static class InteractionNameExtensions
{
    public static string ToDisplayString(this InteractionName name)
    {
        return name switch
        {
            InteractionName.PickUp => "Pick Up",
            InteractionName.Press => "Press",
            InteractionName.Speak => "Speak",
            _ => name.ToString()
        };
    }
}