using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MenuEngine.src
{
    public class Element : IDisposable
    {

        public Element? Parent { get; set; }
        public readonly List<Element> children;

        public Element(Element? parent = null)
        {
            AddEventListeners();

            children = new();

            Parent = parent ?? Project.Instance.RootElement;
            Parent?.children.Add(this);
        }

        private void AddEventListeners()
        {
            if (IsMethodOverridden(Update))
                Engine.OnUpdateEvent += Update;

            if (IsMethodOverridden(Draw))
                Engine.OnDrawEvent += Draw;
        }

        private void RemoveEventListeners()
        {
            if (IsMethodOverridden(Update))
                Engine.OnUpdateEvent -= Update;

            if (IsMethodOverridden(Draw))
                Engine.OnDrawEvent -= Draw;
        }

        public void Dispose()
        {
            RemoveEventListeners();

            // Dispose appears to remove the element from the list, so we just keep removing the first element until there are none left.
            while (children.Any())
                children[0].Dispose();

            Parent?.children.Remove(this);
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }

        private MethodInfo GetMethodInfo(Delegate del)
        {
            return del.Method;
        }

        private bool IsMethodOverridden(Delegate del)
        {
            MethodInfo methodInfo = GetMethodInfo(del);

            return methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;
        }
    }
}
