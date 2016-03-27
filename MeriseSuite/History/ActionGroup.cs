using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeriseSuite.History
{
    public class ActionGroup : Action
    {
        public List<Action> Actions { get; private set; }

        public ActionGroup(ModelVisualizer visualizer) : base(visualizer)
        {
            Actions = new List<Action>();
        }

        public void Add(Action action)
        {
            Actions.Add(action);
        }
        public void Remove(Action action)
        {
            Actions.Remove(action);
        }

        public override void Execute()
        {
            for (int i = 0; i < Actions.Count; i++)
                Actions[i].Execute();
        }
        public override void Rollback()
        {
            for (int i = Actions.Count - 1; i >= 0; i--)
                Actions[i].Rollback();
        }
    }
}