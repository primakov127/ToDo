using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Model
{
    public enum Repeat
    {
        None = 1,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public enum Priority
    {
        Highest = 1,
        Normal,
        Lowest
    }

    [Serializable]
    public class Task : INotifyPropertyChanged
    {
        public string text;

        public Task()
        {
            this.Description = "";
            this.IsCompleted = false;
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public Repeat? Repeat { get; set; }
        public Priority? Priority { get; set; }
        public bool IsCompleted { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
