using System.Collections.Generic;

namespace FubuMvc._tests
{
    public class ViewModel
    {
        public IList<string> Tests { get; set; } 
    }

    public class PublicGetHandler
    {
        public PublicGetHandler()
        {
        }

        public ViewModel Execute()
        {
            return new ViewModel();
        }
    }
}