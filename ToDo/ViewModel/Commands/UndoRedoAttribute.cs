using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.ViewModel.Commands
{
    [AttributeUsage(AttributeTargets.Property)]
    class UndoRedoAttribute : Attribute { }
}
