﻿using CustomerCare.STR;
using SMLOGX.Core;
using System;
using System.Windows.Forms;

namespace CustomerCare
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            txtPassword.txtValue.PasswordChar = '*';
            txtPassword.txtValue.KeyPress += new KeyPressEventHandler(txtPassword_KeyPress);
            txtUsername.txtValue.KeyPress += new KeyPressEventHandler(txtUsername_KeyPress);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            App.ExitAll();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Database.DBName = "CustomerCare";
            Database.Open();
            string[] user = { txtUsername.Value, txtPassword.Value };
            object userID = 0;
            Database.User.Table = "tbl_users";
            Database.User.Username = user[0];
            Database.User.Password = user[1];

            if (Database.User.Login(ref userID))
            {
                Temp.staff_id = int.Parse(Database.QueryScalar("SELECT staff_id FROM tbl_users WHERE user_id = " + userID) + "");
                Temp.staff_name = Database.QueryScalar("SELECT name_en FROM tbl_mststaff WHERE staff_id = " + Temp.staff_id) + "";
                new frmMain().Show();
                Temp.frm_login = this;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please try to login again!");
            }
        }

        private bool showPass = false;

        private void btnShowHide_Click(object sender, EventArgs e)
        {
            showPass = !showPass;
            txtPassword.txtValue.PasswordChar = showPass ? '\0' : '*';
            btnShowHide.Text = showPass ? "H" : "V";
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            App.Overlay(new frmDBSetting(), new GUI.Components.Overlay());
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txtPassword.txtValue.Focus();
            if (e.KeyChar == 27)
            {
                txtPassword.txtValue.Text = "";
                txtUsername.txtValue.Text = "";
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnLogin_Click(null, null);
            if (e.KeyChar == 27)
            {
                txtPassword.txtValue.Text = "";
                txtUsername.txtValue.Text = "";
            }
        }
    }
}