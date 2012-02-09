using System.Collections.Generic;

namespace AspMvc.Models
{
    public class ViewModel
    {
        public bool IsChromeBrowser { get; set; }
        public List<EntryModel> Results { get; set; }
    }
}