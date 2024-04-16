using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace PicTools
{
    public class FontFamilyEx 
    {
        public FontFamily Value { get; set; }
        public int lang { get; set; }

        public FontFamilyEx() { }
        public FontFamilyEx(FontFamily fontFamily)
        {
            this.Value = fontFamily;
        }

        public FontFamilyEx(string fontFamily)
        {
            this.Value = new FontFamily(fontFamily);
        }

        public override string ToString()
        {
            if (Value.FamilyNames.Count > 0)
            {
               KeyValuePair<XmlLanguage,string> name = Value.FamilyNames.Where(N => N.Key.ToString() == "zh-cn").FirstOrDefault();
                if(name.Key != null)
                {
                    lang = 1;
                    return name.Value.ToString();
                }
            }
            lang = 0;
            return Value.ToString();
        }
    }
}
