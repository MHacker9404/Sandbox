using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediatorFormDemo
{
    public partial class Form1 : Form
    {
        private MarkerMediator _mediator;
        private Button _addButton;

        public Form1()
        {
            InitializeComponent();

            this._mediator = new MarkerMediator();

            this._addButton = new Button();
            this._addButton.Click += OnAddClick;
            this._addButton.Text = "Add Marker";
            this._addButton.Dock = DockStyle.Bottom;
            this.Controls.Add(this._addButton);
        }

        private void OnAddClick(object? sender, EventArgs e)
        {
            var m = this._mediator.CreateMarker();
            this.Controls.Add(m);
        }
    }
}
