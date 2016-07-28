namespace Core.Time
{

    public interface IDateTime
    {

        IDateTime Add(IDateTime dateTime);
        IDateTime Minus(IDateTime dateTime);

    }

}