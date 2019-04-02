using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ePS_Robot.公共函数
{
    class Fun_Css
    {
        public void DataGridViewWidth(DataGridView dataGridView1)
        {
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            dataGridView1.MultiSelect = false;
            dataGridView1.RowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 246, 242);
            dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("宋体", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                //第一列冻结
                if (i == 0)
                {
                    dataGridView1.Columns[i].Frozen = true;
                }
                if (i == 1 && (dataGridView1.Columns[i].Name == "RowID" || dataGridView1.Columns[i].Name == " "))
                {
                    dataGridView1.Columns[i].Frozen = true;
                }
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //对datagridview中指定列的列宽进行设定
                if (dataGridView1.Columns[i].HeaderCell.Value.ToString() == " ")
                {
                    dataGridView1.Columns[i].Width = 50;
                    dataGridView1.Columns[i].Resizable = DataGridViewTriState.False;
                }
            }
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
        }

    }
}
