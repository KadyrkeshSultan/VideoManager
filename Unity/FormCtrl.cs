using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using VIBlend.WinForms.Controls;

namespace Unity
{
    public static class FormCtrl
    {
        
        public static void ClearForm(Control control)
        {
            foreach (Control control1 in (ArrangedElementCollection)control.Controls)
            {
                if (control1 is TextBox)
                    ((TextBoxBase)control1).Clear();
                if (control1.HasChildren)
                    FormCtrl.ClearForm(control1);
                if (control1 is CheckBox)
                    ((CheckBox)control1).Checked = false;
                if (control1 is RadioButton)
                    ((RadioButton)control1).Checked = false;
                if (control1 is MaskedTextBox)
                    ((TextBoxBase)control1).Clear();
                if (control1 is RichTextBox)
                    ((TextBoxBase)control1).Clear();
                if (control1 is ComboBox)
                    ((ListControl)control1).SelectedIndex = -1;
                if (control1 is vComboBox)
                    ((vComboBox)control1).SelectedIndex = -1;
                if (control1 is ListBox)
                    ((ListControl)control1).SelectedIndex = -1;
                if (control1 is vListBox)
                    ((vListBox)control1).SelectedIndex = -1;
            }
        }

        
        public static string GetPropertyFromObject(string propertyName, object obj)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);
            if (property != (PropertyInfo)null)
            {
                object obj1 = property.GetValue(obj, (object[])null);
                if (obj1 != null)
                    return obj1.ToString();
            }
            return string.Empty;
        }

        
        public static void SetComboItem(ComboBox cbo, string item)
        {
            try
            {
                cbo.SelectedIndex = cbo.Items.IndexOf((object)item);
            }
            catch
            {
            }
        }

        
        public static void SetComboItem(vComboBox cbo, string item)
        {
            try
            {
                cbo.SelectedIndex = cbo.Items.IndexOf(new ListItem()
                {
                    Text = item
                });
            }
            catch
            {
            }
        }
    }
}
