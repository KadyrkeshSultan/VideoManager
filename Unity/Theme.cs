using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using VIBlend.Utilities;

namespace Unity
{
    public static class Theme
    {
        public static List<string> GetThemeList()
        {
            List<string> stringList = new List<string>();
            foreach (string name in Enum.GetNames(typeof(VIBLEND_THEME)))
                stringList.Add(name);
            return stringList;
        }

        
        private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return ((IEnumerable<Type>)assembly.GetTypes()).Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }

        
        public static void Set(VIBLEND_THEME theme)
        {
            List<Type> list = ((IEnumerable<Type>)GetTypesInNamespace(Assembly.LoadFile(Application.StartupPath + "\\VIBlend.WinForms.Controls.dll"), "VIBlend.WinForms.Controls")).ToList();
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm != null)
                {
                    foreach (object control in GetControls(openForm, list))
                    {
                        try
                        {
                            control.GetType().GetProperty("VIBlendTheme").SetValue(control, theme, null);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        
        public static void Set(VIBLEND_THEME theme, Form form)
        {
            List<Type> list = ((IEnumerable<Type>)GetTypesInNamespace(Assembly.LoadFile(Application.StartupPath + "\\VIBlend.WinForms.Controls.dll"), "VIBlend.WinForms.Controls")).ToList();
            if (form == null)
                return;
            foreach (object control in GetControls(form, list))
            {
                try
                {
                    control.GetType().GetProperty("VIBlendTheme").SetValue(control, theme, null);
                }
                catch
                {
                }
            }
        }

        
        private static IEnumerable<object> GetControls(Control form, List<Type> typelist)
        {
            //      // ISSUE: object of a compiler-generated type is created
            //      return (IEnumerable<object>)new Theme.\u003CGetControls\u003Ed__3(-2)
            //    {
            //  \u003C\u003E3__form = form,
            //  \u003C\u003E3__typelist = typelist
            //};

            return null;
        }
    }
}
