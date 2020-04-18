using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDo.Model;

namespace ToDo.ViewModel
{
    class TaskViewModel : ToDo.Abstract.ViewModel
    {
        private Task task;

        public TaskViewModel()
        {
            Task task = new Task();
        }

        #region Dependency property

        public string Text
        {
            get => task.text;
        }

        #endregion
    }
}
