namespace Machine.Fakes.Specs.TestClasses
{
    public class WithStringInConstructor
    {
        public string Content { get; private set; }

        public WithStringInConstructor(string content)
        {
            Content = content;
        }
    }
}