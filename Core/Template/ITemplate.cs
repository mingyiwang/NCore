namespace Core.Template
{
    public interface ITemplate
    {
        string Generate(string input);
        string Generate(string input, TemplateContext context);
    }

}
