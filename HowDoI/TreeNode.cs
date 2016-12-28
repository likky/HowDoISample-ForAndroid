using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace CSHowDoISamples
{
    public class TreeNode
    {
        private string name;
        private bool visible;
        private string className;
        private string description;
        private List<TreeNode> child;

        public TreeNode(string name, string className, string description, List<TreeNode> child)
        {
            this.name = name;
            this.child = child;
            this.visible = true;
            this.className = className;
            this.description = description;
        }

        public TreeNode(XElement sampleElement)
        {
            visible = true;
            child = new List<TreeNode>();
            name = sampleElement.Attribute("Name").Value;
            foreach (XElement element in sampleElement.Elements())
            {
                child.Add(new TreeNode(element.Attribute("Name").Value, element.Attribute("Class").Value, element.Element("Description").Value, null));
            }
        }

        public string Name
        {
            get { return name; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public List<TreeNode> Child
        {
            get { return child; }
        }

        public string ClassName
        {
            get { return className; }
        }

        public Type ClassType
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetType("CSHowDoISamples." + className);
            }
        }
    }
}