using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms.Design;

namespace OpetraViews.Controls
{
    /// <summary>Represents: FileExplorerTreeViewDesigner</summary>
    public class FileExplorerTreeViewDesigner : ScrollableControlDesigner
    {
        /// <summary>Initializes a new instance of the FileExplorerTreeViewDesigner class</summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileExplorerTreeViewDesigner()
        {
        }

        /// <summary>The FilterProp field</summary>
        readonly List<string> FilterProp = new List<string>()
        {
            "BackgroundImage",
            "BackgroundImageLayout",
            "ImeMode",
        };
        /// <summary>The _excludeBrowsableEvents field</summary>
        private readonly List<string> _excludeBrowsableEvents = new List<string>()
        {
            "BindingContextChanged",
            "CausesValidationChanged",
            "ImeModeChanged",
        };
        /// <summary>
        /// Drops the BackgroundImage property
        /// </summary>
        /// <param name = "properties">properties to remove BackGroundImage from</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            foreach (var p in FilterProp)
            {
                if (properties.Contains(p) == true)
                    properties.Remove(p);
            }
        }

        /// <summary>Performs pre filter events</summary>
        /// <param name = "events">The events collection</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected override void PreFilterEvents(IDictionary events)
        {
            base.PreFilterEvents(events);
            foreach (var p in _excludeBrowsableEvents)
            {
                if (events.Contains(p) == true)
                    events.Remove(p);
            }
        }
    }
}