using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ToDo.Model;
using ToDo.ViewModel.Commands;

namespace ToDo.ViewModel
{
    public class MainWindowViewModel : ToDo.Abstract.ViewModel
    {
        #region Fields

        private ObservableCollection<TaskList> taskLists;
        private TaskList selectedTaskList;
        private Task selectedTask;
        private Task newTask;

        #endregion

        #region Commands

        public Command AddListCommand { get; set; }
        public Command AddTaskCommand { get; set; }
        public Command ChangeLanguageCommand { get; set; }
        public Command TodayTasksCommand { get; set; }
        public Command DayTasksCommand { get; set; }
        public Command RemoveTaskListCommand { get; set; }
        public Command RemoveTaskCommand { get; set; }
        public Command ChangeThemeCommand { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            taskLists = new ObservableCollection<TaskList>();
            newTask = new Task();

            AddListCommand = new Command(AddNewList, CanAddList);
            AddTaskCommand = new Command(AddNewTask, CanAddTask);
            ChangeLanguageCommand = new Command(ChangeLanguage, x => true);
            TodayTasksCommand = new Command(ShowTodayTasks, x => true);
            DayTasksCommand = new Command(ShowDayTasks, x => true);
            RemoveTaskListCommand = new Command(DeleteTaskList, x => true);
            RemoveTaskCommand = new Command(DeleteTask, x => true);
            ChangeThemeCommand = new Command(ChangeTheme, x => true);

            JsonFileDeserialization();
            newTask.Priority = Priority.Normal;
            newTask.Repeat = Repeat.None;

            if (taskLists.Count != 0)
                SelectedTaskList = TaskLists[0];

        }

        ~MainWindowViewModel()
        {
            jsonFileSerialization();
        }

        #region Dependency property

        [UndoRedo]
        public ObservableCollection<TaskList> TaskLists
        {
            get => taskLists;
            set => Set(ref taskLists, value);
        }

        [UndoRedo]
        public TaskList SelectedTaskList
        {
            get => selectedTaskList;
            set => Set(ref selectedTaskList, value);
        }

        public Task SelectedTask
        {
            get => selectedTask;
            set => Set(ref selectedTask, value);
        }

        public Task NewTask
        {
            get => newTask;
            set => Set(ref newTask, value);
        }

        #endregion

        public void AddNewList(object param)
        {
            string listName = (param as TextBox).Text;
            TaskLists = new ObservableCollection<TaskList>(taskLists);
            TaskLists.Add(new TaskList(listName));
            (param as TextBox).Text = "";
        }

        public bool CanAddList(object param)
        {
            if (param == null) return false;
            string text = (param as TextBox).Text;
            return !String.IsNullOrEmpty(text);
        }

        public bool CanAddTask(object param)
        {
            if (param == null || SelectedTaskList == null) return false;
            
            return !String.IsNullOrEmpty(NewTask.Text);
        }

        public void AddNewTask(object param)
        {
            SelectedTaskList.Tasks.Add(new Task()
            {
                Text = NewTask.Text,
                Date = NewTask.Date,
                Repeat = NewTask.Repeat,
                Priority = NewTask.Priority
            });
            NewTask.Text = "";
        }

        public void DeleteTaskList(object param)
        {
            TaskLists = new ObservableCollection<TaskList>(taskLists);
            taskLists.Remove(selectedTaskList);
        }

        public void DeleteTask(object param)
        {
            selectedTaskList.Tasks.Remove(selectedTask);
        }

        public void ChangeLanguage(object param)
        {
            if (param == null)
                return;

            string lang = "";
            if ((bool)param)
            {
                lang = ".ru-RU";
            }

            ChangeResources(String.Format("Resources/lang{0}.xaml", lang), "Resources/lang.");
        }

        public void ChangeTheme(object param)
        {
            if (param == null)
                return;

            string lang = "Resources/Themes/";
            if ((bool)param)
            {
                lang = lang + "dark.xaml";
            }
            else
            {
                lang = lang + "origin.xaml";
            }

            ChangeResources(lang, "Resources/Themes/");
        }

        public void ShowTodayTasks(object param)
        {
            SelectedTaskList = null;
            SelectedTaskList = new TaskList("Today")
            {
                Tasks = new ObservableCollection<Task>(DayTasks(DateTime.Now.Date))
            };
        }

        public void ShowDayTasks(object param)
        {
            if (param == null) return;
            DateTime date = (DateTime)param;

            SelectedTaskList = null;
            SelectedTaskList = new TaskList("Selected Date")
            {
                Tasks = new ObservableCollection<Task>(DayTasks(date))
            };
        }

        public IEnumerable<Task> DayTasks(DateTime date)
        {
            return TaskLists.SelectMany(x => x.Tasks.Where(y => y.Date == date));
        }

        public void ChangeResources(string newUri, string oldUri)
        {
            ResourceDictionary newDict = new ResourceDictionary();
            newDict.Source = new Uri(newUri, UriKind.Relative);

            ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                          where d.Source != null && d.Source.OriginalString.StartsWith(oldUri)
                                          select d).First();

            if (oldDict != null)
            {
                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                Application.Current.Resources.MergedDictionaries.Insert(ind, newDict);
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(newDict);
            }
        }

        private void JsonFileDeserialization()
        {
            using (FileStream cout = new FileStream("data.json", FileMode.OpenOrCreate))
            {
                using (StreamReader reader = new StreamReader(cout))
                {
                    string data = reader.ReadToEnd();
                    if (String.IsNullOrEmpty(data))
                        return;
                    taskLists = JsonSerializer.Deserialize<ObservableCollection<TaskList>>(data, new JsonSerializerOptions { AllowTrailingCommas = true});
                }
            }
        }

        private void jsonFileSerialization()
        {
            using (FileStream cout = new FileStream("data.json", FileMode.Create))
            {
                string data = JsonSerializer.Serialize<ObservableCollection<TaskList>>(taskLists);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                cout.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
