﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbForWS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public static SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\WebServices.mdf;Integrated Security=True;Connect Timeout=30");
        //Thread AppExit = new Thread(CloseAndExit);

        static bool CreateDB()
        {
            //true - enabled
            bool c = false;

            try
            {
                SqlConnection connection = new SqlConnection(@"server=(localdb)\MSSQLLocalDB");
                using (connection)
                {
                    connection.Open();
                    string sql = string.Format(@"CREATE DATABASE [WebServices] ON PRIMARY (NAME=WebServices,FILENAME = '{0}\WebServices.mdf')LOG ON (NAME=WebServices_log,FILENAME = '{0}\WebServices_log.ldf')", @"D:\College\C#\DbForWS\DbForWS\bin\Debug\");
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();

                    //create table timetables
                    command = new SqlCommand(@"create table TimeTables(Id int, LessonName nvarchar(50), LessonTeacher nvarchar (50), Room nvarchar(50), Week int);", connection);
                    command.ExecuteNonQuery();

                    //create table usertable
                    command = new SqlCommand(@"create table UserTable(Id int, FirstName nvarchar(50), LastName nvarchar(50), AccountNumber nvarchar (50), Password nvarchar(50), Token nvarchar(MAX), Email nvarchar(50), Dob date, GroupName nvarchar (50));", connection);
                    command.ExecuteNonQuery();

                    //create table events
                    command = new SqlCommand(@"create table Events (Id int, Name nvarchar (50), Description nvarchar(max), Day int, Month int, Year int);", connection);
                    command.ExecuteNonQuery();

                    //create table TableOfStatements
                    command = new SqlCommand(@"create table TableOfStatements(AccountNumber nvarchar(50), ApplicationDate date, TypeOfStatement nvarchar (50), Verification bit);", connection);
                    command.ExecuteNonQuery();
                }
        }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                c = true;
            }

            return c;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            dataGridView1.ReadOnly = true;
            dataGridView1.Width = this.Width;
            dataGridView1.Height = this.Height - 24;
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            CreateDB();

            using (SqlCommand comm = new SqlCommand("SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_TYPE LIKE '%TABLE%'", connection))
            {
                //try
                //{
                connection.Open();
                var reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    toolStripComboBox1.Items.Add(reader.GetString(0));
                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK);
                //    this.Enabled = true;
                //    this.Cursor = Cursors.Default;
                //    return;
                //}


            }
            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;



            DataSet ds = new DataSet();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from " + toolStripComboBox1.SelectedItem.ToString(), connection);
            //try
            //{
            dataAdapter.Fill(ds, toolStripComboBox1.SelectedItem.ToString());
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Enabled = true;
            //    this.Cursor = Cursors.Default;
            //    connection.Close();
            //    return;
            //}

            //try
            //{
            dataGridView1.DataSource = ds.Tables[toolStripComboBox1.SelectedItem.ToString()].DefaultView;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Enabled = true;
            //    this.Cursor = Cursors.Default;
            //    connection.Close();
            //    return;
            //}

            this.Enabled = true;
            this.Cursor = Cursors.Default;
        }
    }
}
