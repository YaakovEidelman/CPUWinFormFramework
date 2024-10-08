﻿using System.Data;

namespace CPUWinFormFramework
{
    public class WinFormsUtility
    {
        public static void SetListBindingWithSource(ComboBox lst, DataTable dtDisplay, BindingSource bs, string valuemember)
        {
            lst.DataSource = dtDisplay;
            lst.ValueMember = valuemember + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            lst.DataBindings.Add("SelectedValue", bs, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }
        public static void SetListBinding(ComboBox lst, DataTable dtDisplay, DataTable? dtBinding, string valuemember)
        {
            lst.DataSource = dtDisplay;
            lst.ValueMember = valuemember + "Id";
            lst.DisplayMember = lst.Name.Substring(3);
            if (dtBinding != null)
            {
                lst.DataBindings.Add("SelectedValue", dtBinding, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
        public static void SetControlBinding(Control ctrl, BindingSource bindsource)
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
                case "chk":
                    propertyname = "Checked";
                    break;
            }

            if (propertyname != "" && columnname != "")
            {
                ctrl.DataBindings.Add(propertyname, bindsource, columnname, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void FormatGridSearchResults(DataGridView grid, string tablename)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DoFormatGrid(grid, tablename);
        }

        public static void FormatGridForEdit(DataGridView grid, string tablename)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            DoFormatGrid(grid, tablename);
        }
        private static void DoFormatGrid(DataGridView grid, string tablename)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.RowHeadersWidth = 25;
            string pkname = tablename + "Id";
            foreach (DataGridViewColumn c in grid.Columns)
            {
                if (c.Name.EndsWith("Id"))
                {
                    c.Visible = false;
                }
            }
            if (grid.Columns.Count > 0 && grid.Columns.Contains(pkname))
            {
                grid.Columns[pkname].Visible = false;
            }
        }

        public static int GetIdFromGrid(DataGridView grid, int rowindex, string colname)
        {
            int id = 0;
            if (rowindex < grid.Rows.Count && grid.Columns.Contains(colname) && grid.Rows[rowindex].Cells[colname].Value != DBNull.Value)
            {
                if (grid.Rows[rowindex].Cells[colname].Value is int)
                {
                    id = (int)grid.Rows[rowindex].Cells[colname].Value;
                }
            }
            return id;
        }

        public static int GetIdFromComboBox(ComboBox lst)
        {
            int value = 0;
            if (lst.SelectedValue != null && lst.SelectedValue is int)
            {
                value = (int)lst.SelectedValue;
            }
            return value;
        }
        public static void AddComboBoxToGrid(DataGridView grid, DataTable datasource, string tablename, string displaymember)
        {
            DataGridViewComboBoxColumn c = new();
            c.DataSource = datasource;
            c.DisplayMember = displaymember;
            c.ValueMember = tablename + "Id";
            c.DataPropertyName = c.ValueMember;
            c.HeaderText = tablename;
            grid.Columns.Insert(0, c);
        }
        public static void AddDeleteButtonToGrid(DataGridView grid, string deletecolname)
        {
            grid.Columns.Add(new DataGridViewButtonColumn() { UseColumnTextForButtonValue = true, Name = deletecolname, Text = "X", HeaderText = "Delete" });
        }

        public static bool IsFormOpen(Type frmType, int pkvalue = 0)
        {
            bool exists = false;
            foreach (Form frm in Application.OpenForms)
            {
                int frmpkvalue = 0;
                if (frm.Tag != null && frm.Tag is int)
                {
                    frmpkvalue = (int)frm.Tag;
                }
                if (frm.GetType() == frmType && frmpkvalue == pkvalue)
                {
                    frm.Activate();
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        public static void SetupNav(ToolStrip ts)
        {
            ts.Items.Clear();
            foreach (Form f in Application.OpenForms)
            {
                if (f.IsMdiContainer == false)
                {
                    ToolStripButton btn = new ToolStripButton();
                    btn.Text = f.Text;
                    btn.Tag = f;
                    btn.Click += Btn_Click;
                    ts.Items.Add(btn);
                    ts.Items.Add(new ToolStripSeparator());
                }
            }
        }



        private static void Btn_Click(object? sender, EventArgs e)
        {
            if (sender != null && sender is ToolStripButton)
            {
                ToolStripButton btn = (ToolStripButton)sender;
                if (btn.Tag != null && btn.Tag is Form)
                {
                    ((Form)btn.Tag).Activate();
                }
            }
        }





        public static void ReplaceLettersWithBlanks(TextBox t, bool AllowDecimal = true)
        {
            if (t.Text.Length > 0)
            {
                string currenttext = t.Text;
                string finaltext = new string(currenttext.Where(c => char.IsDigit(c) || (AllowDecimal && c == '.')).ToArray());

                if(AllowDecimal && finaltext.Count(c => c == '.') > 1)
                {
                    MessageBox.Show("You cannot enter more than one decimal into this field.", Application.ProductName);
                    int i = finaltext.IndexOf('.');
                    finaltext = finaltext.Substring(0, i + 1) + finaltext.Substring(i + 1).Replace(".", "");
                }

                else if (finaltext != currenttext)
                {
                    MessageBox.Show("You must enter a valid number.", Application.ProductName);
                }

                if(finaltext.StartsWith("."))
                {
                    finaltext = "0" + finaltext;
                }
                t.Text = finaltext;
                t.SelectionStart = t.TextLength;
            }
        }


        //End Class
    }
}