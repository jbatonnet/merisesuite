using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

namespace MeriseSuite.History
{
    public class ActionStack
    {
        public int Count
        {
            get
            {
                return historyStack.Count;
            }
        }

        private Stack<Action> historyStack = new Stack<Action>();
        private Stack<Action> executionStack = new Stack<Action>();

        public void Execute(Action action)
        {
            action.Execute();

            historyStack.Push(action);
            executionStack.Clear();

            // TODO: Simplify();
        }
        public void Clear()
        {
            historyStack.Clear();
            executionStack.Clear();
        }

        public void Execute()
        {
            if (executionStack.Count == 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            Action action = executionStack.Pop();
            action.Execute();
            historyStack.Push(action);
        }
        public void Rollback()
        {
            if (historyStack.Count == 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            Action action = historyStack.Pop();
            action.Rollback();
            executionStack.Push(action);
        }
    }
}
