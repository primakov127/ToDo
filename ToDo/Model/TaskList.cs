using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Abstract;

namespace ToDo.Model
{
    [Serializable]
    public class TaskList : CustomNotifyPropertyChanged
    {
        #region Fields

        private string listName;
        private ObservableCollection<Task> tasks;

        #endregion

        #region Constructors

        public TaskList()
        {

        }

        public TaskList(string nameParam)
        {
            this.listName = nameParam;
            this.tasks = new ObservableCollection<Task>();
        }

        #endregion

        #region Dependency property

        public string ListName
        {
            get => this.listName;
            set => Set(ref this.listName, value);
        }
        public ObservableCollection<Task> Tasks
        {
            get => this.tasks;
            set => Set(ref this.tasks, value);
        }

        #endregion

    }
}
