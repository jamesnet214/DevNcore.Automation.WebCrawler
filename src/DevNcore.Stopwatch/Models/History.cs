using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace DevNcore.Stopwatch.Models
{
    [AddINotifyPropertyChangedInterface]
    public class History
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public string Memo { get; set; }
    }
}
