using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToDo.ViewModel.Commands;

namespace ToDo.Abstract
{
    public class ViewModel : INotifyPropertyChanged
    {
        static bool isUndoProcess = false;
        static bool isRedoProcess = false;

        // Stacks which keep history
        static Stack<(object Obj, string Prop, object OldValue)> undoHistory
        = new Stack<(object Obj, string Prop, object OldValue)>();

        static Stack<(object Obj, string Prop, object OldValue)> redoHistory
            = new Stack<(object Obj, string Prop, object OldValue)>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            SaveHistory(this, propertyName, field);
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        static void Undo()
        {
            if (undoHistory.Count == 0) return;
            var undo = undoHistory.Pop();
            UndoCommand.RaiseCanExecuteChanged();
            try
            {
                isUndoProcess = true;
                undo.Obj.GetType().GetProperty(undo.Prop).SetValue(undo.Obj, undo.OldValue);
            }
            finally
            {
                isUndoProcess = false;
            }
        }

        static void Redo()
        {
            if (redoHistory.Count == 0) return;
            var redo = redoHistory.Pop();
            RedoCommand.RaiseCanExecuteChanged();
            try
            {
                isRedoProcess = true;
                redo.Obj.GetType().GetProperty(redo.Prop).SetValue(redo.Obj, redo.OldValue);
            }
            finally
            {
                isRedoProcess = false;
            }
        }

        static void SaveHistory(object obj, string propertyName, object value)
        {
            if (obj.GetType()
               .GetProperty(propertyName)
               .GetCustomAttributes(typeof(UndoRedoAttribute), true)
               .Length == 0) return;

            if (isUndoProcess)
            {
                redoHistory.Push((obj, propertyName, value));
                RedoCommand.RaiseCanExecuteChanged();
            }
            else if (isRedoProcess)
            {
                undoHistory.Push((obj, propertyName, value));
                UndoCommand.RaiseCanExecuteChanged();
            }
            else
            {
                undoHistory.Push((obj, propertyName, value));
                UndoCommand.RaiseCanExecuteChanged();
                redoHistory.Clear();
                RedoCommand.RaiseCanExecuteChanged();
            }
        }

        public static void ClearHistory()
        {
            undoHistory.Clear();
            UndoCommand.RaiseCanExecuteChanged();
            redoHistory.Clear();
            RedoCommand.RaiseCanExecuteChanged();
        }


        #region Commands

        public static HistoryCommand UndoCommand { get; }
        = new HistoryCommand(_ => Undo(), _ => undoHistory.Count > 0);
        public static HistoryCommand RedoCommand { get; }
            = new HistoryCommand(_ => Redo(), _ => redoHistory.Count > 0);
        public static HistoryCommand ClearHistoryCommand { get; }
            = new HistoryCommand(_ => ClearHistory());

        #endregion

    }
}
