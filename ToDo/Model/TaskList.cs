using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Model
{
    [Serializable]
    public class TaskList
    {
        public string ListName { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }

        public TaskList()
        {

        }

        public TaskList(string nameParam)
        {
            this.ListName = nameParam;
            this.Tasks = new ObservableCollection<Task>();
        }
    }
}
