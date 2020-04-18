using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToDo.Abstract;

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
    public class Task : CustomNotifyPropertyChanged
    {
        #region Fields

        private string text;
        private string description;
        private DateTime? date;
        private Repeat? repeat;
        private Priority? priority;
        private bool isCompleted;

        #endregion

        public Task()
        {
            this.description = "";
            this.isCompleted = false;
        }

        #region Dependency property

        public string Text
        {
            get => this.text;
            set => Set(ref this.text, value);
        }

        public string Description
        {
            get => this.description;
            set => Set(ref this.description, value);
        }

        public DateTime? Date
        {
            get => this.date;
            set => Set(ref this.date, value);
        }

        public Repeat? Repeat
        {
            get => this.repeat;
            set => Set(ref this.repeat, value);
        }

        public Priority? Priority
        {
            get => this.priority;
            set => Set(ref this.priority, value);
        }

        public bool IsCompleted
        {
            get => this.isCompleted;
            set => Set(ref this.isCompleted, value);
        }

        #endregion

    }
}
