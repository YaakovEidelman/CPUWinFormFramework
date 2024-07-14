using System.Data;

namespace CPUWinFormFramwork
{
    public class WinFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable dtDisplay, DataTable dtBinding, string valuemember)
        {
            lst.DataSource = dtDisplay;
            lst.ValueMember = valuemember;
            lst.DisplayMember = lst.Name.Substring(3);
            lst.DataBindings.Add("SelectedValue", dtBinding, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }
        public static void SetControlBinding(Control ctrl, DataTable dt)
        {
            string propertyname = "";
            string controlname = ctrl.Name.ToLower();
            string controltype = controlname.Substring(0, 3);
            string columnname = controlname.Substring(3);

            switch (controltype)
            {
                case "txt":
                case "lbl":
                    propertyname = "Text";
                    break;
                case "dtp":
                    propertyname = "Value";
                    break;
            }

            if (propertyname != "" && columnname != "")
            {
                ctrl.DataBindings.Add(propertyname, dt, columnname, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void FormatGridSearchResults(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
