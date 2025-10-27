using UnityEngine;

namespace EditorAttributes
{
    public class TooltipTextAreaAttribute : PropertyAttribute
    {
        public int Height { get; private set; }

        public TooltipTextAreaAttribute()
        {
            Height = 6;
        }
        public TooltipTextAreaAttribute(int height)
        {
            Height = height;
        }
    }

}