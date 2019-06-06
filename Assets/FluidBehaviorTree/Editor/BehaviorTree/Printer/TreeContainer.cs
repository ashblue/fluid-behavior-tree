using System.Collections.Generic;
using UnityEngine.Events;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public interface IEventSizeChange {
        void Invoke (float newChildWidth, float newChildHeight);
        void AddListener (UnityAction<float, float> action);
    }
    public class EventSizeChange : UnityEvent<float, float>, IEventSizeChange {}
    
    public interface ITreeBox {
        float LocalPositionX { get; }
        float LocalPositionY { get; }
        float GlobalPositionX { get; }
        float GlobalPositionY { get; }

        float Width { get; }
        float Height { get; }
        IEventSizeChange EventSizeChange { get; }

        void SetSize (float width, float height);
        void SetLocalPosition (float x, float y);
        void SetGlobalPosition (float x, float y);
   }
    
    public interface ITreeContainer : ITreeBox {
        void AddBox (ITreeBox container);
    }

    public class TreeBox : ITreeBox {
        public float LocalPositionX { get; private set; }
        public float LocalPositionY { get; private set; }
        
        public float GlobalPositionX { get; private set; }
        public float GlobalPositionY { get; private set; }
        
        public float Width { get; private set; }
        public float Height { get; private set; }
        public IEventSizeChange EventSizeChange { get; } = new EventSizeChange();

        public void SetSize (float width, float height) {
            Width = width;
            Height = height;
            
            EventSizeChange.Invoke(Width, Height);
        }

        public void SetLocalPosition (float x, float y) {
            LocalPositionX = x;
            LocalPositionY = y;
        }

        public void SetGlobalPosition (float x, float y) {
            GlobalPositionX = x;
            GlobalPositionY = y;
        }
    }
    
    public class TreeContainerHorizontal : ITreeContainer {
        protected readonly List<ITreeBox> _childContainers = new List<ITreeBox>();
        
        public float LocalPositionX { get; private set; }
        public float LocalPositionY { get; private set; }
        public float GlobalPositionX { get; private set; }
        public float GlobalPositionY { get; private set; }
        public float Width { get; protected set; }
        public float Height { get; protected set; }
        public IEventSizeChange EventSizeChange { get; } = new EventSizeChange();
        public List<ITreeBox> ChildContainers => _childContainers;

        public void SetLocalPosition (float x, float y) {
            LocalPositionX = x;
            LocalPositionY = y;
        }

        public virtual void AddBox (ITreeBox child) {
            CalculateChild(child);

            child.EventSizeChange.AddListener((newChildWidth, newChildHeight) => {
                Width = 0;
                Height = 0;
                foreach (var c in _childContainers) {
                    CalculateChild(c);
                }
                
                EventSizeChange.Invoke(Width, Height);
            });
            EventSizeChange.Invoke(Width, Height);
            
            _childContainers.Add(child);
        }

        private void CalculateChild (ITreeBox child) {
            child.SetLocalPosition(Width, 0);
            child.SetGlobalPosition(GlobalPositionX + child.LocalPositionX, GlobalPositionY + child.LocalPositionY);

            Width += child.Width;
            if (child.Height > Height) Height = child.Height;
        }

        public void SetSize (float width, float height) {
            throw new System.NotImplementedException();
        }

        public void SetGlobalPosition (float x, float y) {
            GlobalPositionX = x;
            GlobalPositionY = y;
            
            foreach (var child in ChildContainers) {
                child.SetGlobalPosition(child.GlobalPositionX + x, child.GlobalPositionY + y);
            }
        }
    }

    public class TreeContainerVertical : TreeContainerHorizontal {
        public override void AddBox (ITreeBox child) {
            CalculateChild(child);

            child.EventSizeChange.AddListener((newChildWidth, newChildHeight) => {
                Width = 0;
                Height = 0;
                foreach (var c in _childContainers) {
                    CalculateChild(c);
                }

                EventSizeChange.Invoke(Width, Height);
            });
            EventSizeChange.Invoke(Width, Height);
            
            _childContainers.Add(child);
        }

        private void CalculateChild (ITreeBox child) {
            child.SetLocalPosition(0, Height);
            child.SetGlobalPosition(GlobalPositionX + child.LocalPositionX, GlobalPositionY + child.LocalPositionY);

            Height += child.Height;
            if (child.Width > Width) Width = child.Width;
        }
    }
}
